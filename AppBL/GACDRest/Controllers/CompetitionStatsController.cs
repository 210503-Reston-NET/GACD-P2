﻿using GACDBL;
using GACDModels;
using GACDRest.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GACDRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitonStatsController : ControllerBase
    {
        private ICategoryBL _categoryBL;
        private ICompBL _compBL;
        private IUserBL _userBL;
        private IUserStatBL _userStatService;
        private readonly ApiSettings _ApiSettings;

        public CompetitonStatsController(IUserBL userBL, ICategoryBL categoryBL, IUserStatBL _userstat, ICompBL compBL, IOptions<ApiSettings> settings)
        {
            _compBL = compBL;
            _compBL = compBL;
            _userBL = userBL;
            _categoryBL = categoryBL;
            _userStatService = _userstat;
            _ApiSettings = settings.Value;
        }
        /// <summary>
        /// GET /api/CompetitionStats/{id}
        /// Gets the information needed for a user to participate in the competition and update their related statistics
        /// </summary>
        /// <param name="id">Competition Id for the competition</param>
        /// <returns>Output or 404 if competition cannot be found</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CompetitionContent>> Get(int id)
        {
            try
            {
                CompetitionContent c = new CompetitionContent();
                Tuple<string, string, int> compTuple = await _compBL.GetCompStuff(id);
                c.author = compTuple.Item1;
                c.testString = compTuple.Item2;
                Category category = await _categoryBL.GetCategoryById(compTuple.Item3);
                c.categoryId = category.Name;
                c.id = id;
                return c;
            }catch (Exception e)
            {
                Log.Error(e.Message);
                Log.Error("Error with retrieving comp string, returning not found");
                return NotFound();
            }

        }
        /// <summary>
        /// POST /api/CompetitionStats
        /// Posts the user's competition statistics, updates competition standings and user statistics
        /// </summary>
        /// <param name="compInput">DTO for competition input, extension of type test input with competition ID</param>
        /// <returns>200 with rank in the body or 400 if the test input is incorrect or the competition is already 
        /// done.Returns 404 if competition cannot be found.</returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<int>> Post(CompInput compInput)
        {
            TypeTestInput typeTest = compInput;
            Log.Information(typeTest.categoryId.ToString());
            string UserID = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (await _userBL.GetUser(UserID) == null)
            {
                GACDModels.User user = new GACDModels.User();
                user.Auth0Id = UserID;
                await _userBL.AddUser(user);
            }
            if (await _categoryBL.GetCategory(typeTest.categoryId) == null)
            {
                GACDModels.Category category = new GACDModels.Category();
                category.Name = typeTest.categoryId;
                await _categoryBL.AddCategory(category);
            }
            Category category1 = await _categoryBL.GetCategory(typeTest.categoryId);
            GACDModels.User user1 = await _userBL.GetUser(UserID);
            TypeTest testToBeInserted = await _userStatService.SaveTypeTest(typeTest.numberoferrors, typeTest.numberofcharacters, typeTest.timetakenms, typeTest.wpm, typeTest.date);
            CompetitionStat competitionStat = new CompetitionStat();
            bool typeTestFlag = (await _userStatService.AddTestUpdateStat(user1.Id, category1.Id, testToBeInserted) == null);
            if (typeTestFlag) return BadRequest();
            competitionStat.WPM = typeTest.wpm;
            competitionStat.UserId = user1.Id;
            competitionStat.CompetitionId = compInput.compId;
            if ((await _compBL.GetCompetition(compInput.compId)).EndDate < DateTime.Now) return BadRequest();
            int returnValue =  await _compBL.InsertCompStatUpdate(competitionStat, typeTest.numberofcharacters, typeTest.numberoferrors);
            if (returnValue == -1) return NotFound();
            else return returnValue;
        }
        
    }
}

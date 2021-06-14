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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        // GET: api/<CompeititonStatsController>
        [HttpGet("{id}")]
        public async Task<CompetitionContent> Get(int id)
        {
            try
            {
                CompetitionContent c = new CompetitionContent();
                c.testString = await _compBL.GetCompString(id);
                c.id = id;
                return c;
            }catch (Exception)
            {
                Log.Error("Error with retrieving comp string, returning null");
                return null;
            }

        }

        // GET api/<CompeititonStatsController>/5
        
        // POST api/<CompeititonStatsController>
        [HttpPost]
        [Authorize]
        public async Task<int> Post(CompInput compInput)
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
            if (typeTestFlag) return -1;
            competitionStat.WPM = typeTest.wpm;
            competitionStat.UserId = user1.Id;
            competitionStat.CompetitionId = compInput.compId;
            return await _compBL.InsertCompStatUpdate(competitionStat, typeTest.numberofcharacters, typeTest.numberoferrors);
        }

        // PUT api/<CompeititonStatsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CompeititonStatsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GACDBL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Octokit;
using GACDModels;
using GACDRest.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using RestSharp;
using Newtonsoft.Json;
using Serilog;

namespace GACDRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeTestController : ControllerBase
	{
		private readonly ISnippets _snippetsService;
        
        private readonly IUserStatBL _userStatService;
        private readonly IUserBL _userBL;
        private readonly ICategoryBL _categoryBL;
        public TypeTestController(ISnippets snip, IUserStatBL _userstat, IUserBL userBL, ICategoryBL categoryBL)

        {
            _userBL = userBL;
            _snippetsService = snip;
            _userStatService = _userstat;
            _categoryBL = categoryBL;
        }
        /// <summary>
        /// GET /api/TypeTest
        /// Gets a random quote, author, and length from http://api.quotable.io/random
        /// </summary>
        /// <returns>TestMaterial DTO or 500 on internal server error</returns>
        [HttpGet]
        public async Task<TestMaterial> GetQuote()
        {
            return await _snippetsService.GetRandomQuote();
        }
        /// <summary>
        /// GET /api/TypeTest/{id}
        /// Used to get a random quote on -1 or language number from Octokit.Language to search in 
        /// https://raw.githubusercontent.com/ for a random file in that language to get for coding test
        /// </summary>
        /// <param name="id">Category to get test from</param>
        /// <returns> TestMaterial DTO or 500 on internal server error</returns>
        [HttpGet("{id}")]
        public async Task<TestMaterial> CodeSnippet(int id)
        {
            if (id == -1) return await _snippetsService.GetRandomQuote();
            else return await _snippetsService.GetCodeSnippet(id);
        }

        /// <summary>
        /// POST /api/TypeTest
        /// Used to post the results of a type test to the database, adding a category and/or user if necessary and 
        /// updating user stats.
        /// </summary>
        /// <param name="typeTest">Typetest to insert</param>
        /// <returns>400 if request can't be processed, 200 if successful</returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateTypeTest(TypeTestInput typeTest)
        {
            Log.Information(typeTest.categoryId.ToString());
            string UserID = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if(await _userBL.GetUser(UserID) == null)
            {
                GACDModels.User user = new GACDModels.User();
                user.Auth0Id = UserID;
                await _userBL.AddUser(user);
            }
            if(await _categoryBL.GetCategory(typeTest.categoryId) == null)
            {
                GACDModels.Category category = new GACDModels.Category();
                category.Name = typeTest.categoryId;
                await _categoryBL.AddCategory(category);
            }
            Category category1 = await _categoryBL.GetCategory(typeTest.categoryId);
            GACDModels.User user1 = await _userBL.GetUser(UserID);
            TypeTest testToBeInserted = await _userStatService.SaveTypeTest(typeTest.numberoferrors,typeTest.numberofcharacters,typeTest.timetakenms,typeTest.wpm,typeTest.date);
            bool typeTestFlag =  (await _userStatService.AddTestUpdateStat(user1.Id, category1.Id, testToBeInserted) == null);
            if (typeTestFlag) return BadRequest();
            else return Ok();
        }
        
    }
}

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

/// <summary>
/// Summary description for Class1
/// </summary>
namespace GACDRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeTestController : ControllerBase
	{
		private ISnippets _snippetsService;
        
        private IUserStatBL _userStatService;
        private IUserBL _userBL;

        public TypeTestController(ISnippets snip, IUserStatBL _userstat, IUserBL userBL)

        {
            _userBL = userBL;
            _snippetsService = snip;
            _userStatService = _userstat;
        }
        [HttpGet]
        public async Task<TestMaterial> GetQuote(int id)
        {
            return await _snippetsService.GetRandomQuote();
        }
        
        /// <summary>
        /// Method which adds a test to the database
        /// </summary>
        /// <param name="typeTest">Typetest to insert</param>
        /// <returns>400 if request can't be processed, 200 if successful</returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateTypeTest(TypeTestInput typeTest)
        {
            string UserID = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if(await _userBL.GetUser(UserID) == null)
            {
                GACDModels.User user = new GACDModels.User();
                user.Auth0Id = UserID;
                await _userBL.AddUser(user);
            }
            GACDModels.User user1 = await _userBL.GetUser(UserID);
            TypeTest testToBeInserted = typeTest;
            bool typeTestFlag =  (await _userStatService.AddTestUpdateStat(user1.Id, typeTest.CategoryId, testToBeInserted) == null);
            if (typeTestFlag) return BadRequest();
            else return Ok();
        }
	}
}

using GACDBL;
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
    
    public class CompetitionController : ControllerBase{
        private ICompBL _compBL;
        private ICategoryBL _categoryBL;
        private IUserBL _userBL;
        private ISnippets _snippets;
        public CompetitionController(ICompBL compBL, ICategoryBL catBL, IUserBL uBL, ISnippets snippets){
            _compBL = compBL;
            _categoryBL = catBL;
            _userBL = uBL;
            _snippets = snippets;
        }
        
        [HttpPost]
        [Authorize]
        //[Route("CreateCompetition/{Name}/{Start}/{End}/{Category}")]
        public async Task<ActionResult> Post(CompetitionObject cObject)
        {
            Competition c = new Competition();
            string UserID = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if(await _userBL.GetUser(UserID) == null)
            {
                GACDModels.User user = new GACDModels.User();
                user.Auth0Id = UserID;
                await _userBL.AddUser(user);
            }
            if(await _categoryBL.GetCategory(cObject.Category) == null)
            {
                GACDModels.Category category = new GACDModels.Category();
                category.Name = cObject.Category;
                await _categoryBL.AddCategory(category);
            }
            TestMaterial t = await _snippets.GetCodeSnippet(cObject.Category);
            User u = await _userBL.GetUser(UserID);
            Category category1 = await _categoryBL.GetCategory(cObject.Category);
            int compId = await _compBL.AddCompetition(cObject.Start, cObject.End, category1.Id, cObject.Name, u.Id, t.content);
            bool AddCompetitionFlag = compId == -1;
            if (!AddCompetitionFlag) return CreatedAtRoute("Get", new { compId }, compId);
            else return BadRequest();
        }
        [HttpGet]
        public async Task<User> GetCompAsync(){
            //return await _compBL.GetCompetitions();
            return null;
        }

        [HttpGet("{id}")]
        public async Task<User> GetCompAsync(int id){
            //return await _compBL.GetCompetition(id);
            return null;
        }
    }
    


}
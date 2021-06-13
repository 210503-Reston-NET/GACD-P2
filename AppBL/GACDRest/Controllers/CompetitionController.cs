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
        private readonly ApiSettings _ApiSettings;

        public CompetitionController(ICompBL compBL, ICategoryBL catBL, IUserBL uBL, ISnippets snippets, IOptions<ApiSettings> settings)
        {
            _compBL = compBL;
            _categoryBL = catBL;
            _userBL = uBL;
            _snippets = snippets;
        }
        [HttpGet]
        public async Task<IEnumerable<CompetitionObject>> GetAsync()
        {
            try
            {
                List<Competition> competitions = await _compBL.GetAllCompetitions();
                List<CompetitionObject> competitionObjects = new List<CompetitionObject>();
                foreach (Competition c in competitions)
                {
                    Category cat = await _categoryBL.GetCategoryById(c.Id);
                    CompetitionObject competitionObject = new CompetitionObject(c.CompetitionName, c.StartDate, c.EndDate, cat.Id);
                }
            }
            catch (Exception) { Log.Error("unexpected error in Competition get method"); }
            return null;
        }
        [HttpGet("{id}")]
        public async Task<IEnumerable<CompStatOutput>> GetAsync(int id)
        {
            List<CompetitionStat> competitionStats = await _compBL.GetCompetitionStats(id);
            List<CompStatOutput> compStatOutputs = new List<CompStatOutput>();
            foreach (CompetitionStat c in competitionStats)
            {
                CompStatOutput compStatOutput = new CompStatOutput();
                try
                {
                    User u = await _userBL.GetUser(c.UserId);
                    dynamic AppBearerToken = GetApplicationToken();
                    var client = new RestClient($"https://kwikkoder.us.auth0.com/api/v2/users/{u.Auth0Id}");
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("authorization", "Bearer " + AppBearerToken.access_token);
                    IRestResponse restResponse = await client.ExecuteAsync(request);
                    dynamic deResponse = JsonConvert.DeserializeObject(restResponse.Content);

                    compStatOutput.userName = deResponse.username;
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                    Log.Error("Unexpected error occured in LBController");
                }

                compStatOutput.accuracy = c.Accuracy;
                compStatOutput.wpm = c.WPM;
                compStatOutput.rank = c.rank;
            }
            return compStatOutputs;
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
            TestMaterial t;
            if (cObject.Category == -1) t = await _snippets.GetRandomQuote();
            else  t = await _snippets.GetCodeSnippet(cObject.Category);
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

        
        private dynamic GetApplicationToken()
        {
            var client = new RestClient("https://kwikkoder.us.auth0.com/oauth/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", _ApiSettings.authString, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            //Log.Information("Response: {0}",response.Content);
            return JsonConvert.DeserializeObject(response.Content);
        }
    }
    


}
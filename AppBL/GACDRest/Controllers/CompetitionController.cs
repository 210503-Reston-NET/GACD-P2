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
    [Route("api/[controller]")]
    [ApiController]
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
            _ApiSettings = settings.Value;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompetitionObject>>> GetAsync()
        {
            try
            {
                List<Competition> competitions = await _compBL.GetAllCompetitions();
                List<CompetitionObject> competitionObjects = new List<CompetitionObject>();
                foreach (Competition c in competitions)
                {
                    Category cat = await _categoryBL.GetCategoryById(c.CategoryId);
                    CompetitionObject competitionObject = new CompetitionObject(c.CompetitionName, c.StartDate, c.EndDate, cat.Name);
                    competitionObject.compId = c.Id;
                    competitionObjects.Add(competitionObject);
                }
                return competitionObjects;
            }
            catch (Exception) { Log.Error("unexpected error in Competition get method"); }
            return NotFound();
        }
        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult<IEnumerable<CompStatOutput>>> GetAsync(int id)
        {
            List<CompStatOutput> compStatOutputs = new List<CompStatOutput>();
            try { 
                List<CompetitionStat> competitionStats = await _compBL.GetCompetitionStats(id);
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
                        compStatOutput.Name = deResponse.name;
                        compStatOutput.userName = deResponse.username;
                        compStatOutput.CompName = (await _compBL.GetCompetition(id)).CompetitionName;
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.Message);
                        Log.Error("Unexpected error occured in LBController");
                    }
                    
                    compStatOutput.accuracy = c.Accuracy;
                    compStatOutput.wpm = c.WPM;
                    compStatOutput.rank = c.rank;
                    compStatOutputs.Add(compStatOutput);
                }
                return compStatOutputs;
            }           
            catch (Exception)
            {
                Log.Error("Id not found");
                return NotFound();
            }
            return compStatOutputs;
        }
        [HttpPost]
        [Authorize]
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
            if (String.IsNullOrEmpty(cObject.snippet) || String.IsNullOrWhiteSpace(cObject.snippet)) return BadRequest();
            User u = await _userBL.GetUser(UserID);
            Category category1 = await _categoryBL.GetCategory(cObject.Category);
            int compId = await _compBL.AddCompetition(cObject.Start, cObject.End, category1.Id, cObject.Name, u.Id, cObject.snippet, cObject.author);
            bool AddCompetitionFlag = (compId == -1);
            if (!AddCompetitionFlag) { return CreatedAtRoute(
                                        routeName : "Get", 
                                        routeValues: new { id = compId }, 
                                        value: compId); }
            else return BadRequest();
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
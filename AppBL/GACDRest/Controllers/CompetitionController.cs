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
        /// <summary>
        /// GET /api/Competition
        /// Gets a List of competitions in the database
        /// </summary>
        /// <returns>List of competitions or 404 if they can't be found</returns>
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
        /// <summary>
        /// GET /api/Competition/{id}
        /// Gets a list of competition results for a given competition
        /// </summary>
        /// <param name="id">competition Id</param>
        /// <returns>List of competitions or 404 if not found</returns>
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
                        compStatOutput.userId = c.UserId;
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
            
        }
        /// <summary>
        /// PUT /api/Competition/bet
        /// Places a bet on a given competitor for user based on input
        /// </summary>
        /// <param name="compBetInput">DTO for input, Competition Bet Information</param>
        /// <returns>400 if given a bad input (such as betting on a finished competition) or 200 otherwise</returns>
        [HttpPut("bet")]
        [Authorize]
        public async Task<ActionResult> PutBet(CompBetInput compBetInput) {
            string UserID = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if ((await _compBL.GetCompetition(compBetInput.CompId)).EndDate < DateTime.Now) return BadRequest();
            if (await _userBL.GetUser(UserID) == null) return BadRequest();
            else if (await _compBL.PlaceBet(UserID, compBetInput.UserBetOn, compBetInput.CompId, compBetInput.BetAmount) == null) return BadRequest();
            else return Ok();
        }
        /// <summary>
        /// PUT /api/Competition/bet/{id}
        /// Claims the bets for a given user
        /// </summary>
        /// <param name="id">id of user for whose bets you wish to collect</param>
        /// <returns>404 if no such bets can be found, 200 otherwise</returns>
        [HttpPut("bet/{id}")]
        [Authorize]
        public async Task<ActionResult> ClaimBet(int id)
        {
            if ((await _compBL.ClaimBets(id)).Count == 0) return NotFound();
            else return Ok();
        }
        /// <summary>
        /// POST /api/Competition
        /// Posts a new competition to the created
        /// </summary>
        /// <param name="cObject">DTO for input, Competition information</param>
        /// <returns>Returns: 201 with route and then new competition Id in body, otherwise 400 if user tries to enter bad 
        /// data.</returns>
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

        /// <summary>
        /// Private method to get application token for auth0 management 
        /// </summary>
        /// <returns>dynamic object with token for Auth0 call</returns>
        private dynamic GetApplicationToken()
        {
            var client = new RestClient("https://kwikkoder.us.auth0.com/oauth/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", _ApiSettings.authString, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Log.Information("Response: {0}",response.Content);
            return JsonConvert.DeserializeObject(response.Content);
        }
    }
    


}
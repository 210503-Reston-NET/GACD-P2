using GACDBL;
using GACDModels;
using GACDRest.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GACDRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitonStatsController : ControllerBase
    {
        private ICompBL _compBL;
        private IUserBL _userBL;
        private readonly ApiSettings _ApiSettings;

        public CompetitonStatsController(IUserBL userBL, ICompBL compBL, IOptions<ApiSettings> settings)
        {
            _compBL = compBL;
            _compBL = compBL;
            _userBL = userBL;
        }
        // GET: api/<CompeititonStatsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CompeititonStatsController>/5
        [HttpGet("{id}")]
        public async Task<IEnumerable<CompStatOutput>> GetAsync(int id)
        {
            List<CompetitionStat>competitionStats = await _compBL.GetCompetitionStats(id);
            List<CompStatOutput> compStatOutputs = new List<CompStatOutput>();
            foreach(CompetitionStat c in competitionStats)
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

        // POST api/<CompeititonStatsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
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

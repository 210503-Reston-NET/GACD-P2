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
    public class LBController : ControllerBase
    {
        private readonly ApiSettings _ApiSettings;
        private IUserStatBL _userStatBL;
        public LBController(IUserStatBL userStatBL, IOptions<ApiSettings> settings)
        {
            _userStatBL = userStatBL;
            _ApiSettings = settings.Value;
        }
        // GET: api/<LBController>
        /// <summary>
        /// General leaderboard, gets the best users in general to send to client
        /// </summary>
        /// <returns>List of best users in the database sorted by WPM</returns>
        [HttpGet]
        public async Task<IEnumerable<LBUserModel>> GetAsync()
        {

            List<Tuple<User, double, double>> statTuples = await _userStatBL.GetOverallBestUsers();
            List<LBUserModel> lBUserModels = new List<LBUserModel>();
            foreach (Tuple<User, double, double> tuple in statTuples)
            {
                LBUserModel lBUserModel = new LBUserModel();
                lBUserModel.AverageWPM = tuple.Item2;
                lBUserModel.AverageAcc = tuple.Item3;
                //lBUserModel.Name = tuple.Item1.Name;
                //lBUserModel.UserName = tuple.Item1.UserName;
                lBUserModels.Add(lBUserModel);
            }
            return lBUserModels;
        }
        /// <summary>
        /// Gets the best User for a given category
        /// </summary>
        /// <param name="id">category of Id to search</param>
        /// <returns>List of best users in that category</returns>
        // GET api/<LBController>/5
        [HttpGet("{id}")]
        public async Task<IEnumerable<LBUserModel>> GetAsync(int id)
        {
            List<Tuple<User, double, double>> statTuples = await _userStatBL.GetBestUsersForCategory(id);
            List<LBUserModel> lBUserModels = new List<LBUserModel>();
            foreach (Tuple<User, double, double> tuple in statTuples)
            {
                LBUserModel lBUserModel = new LBUserModel();
                try
                {
                    dynamic AppBearerToken = GetApplicationToken();
                    var client = new RestClient($"https://kwikkoder.us.auth0.com/api/v2/users/{tuple.Item1.Auth0Id}");
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("authorization", "Bearer " + AppBearerToken.access_token);
                    IRestResponse restResponse = await client.ExecuteAsync(request);
                    dynamic deResponse = JsonConvert.DeserializeObject(restResponse.Content);

                    lBUserModel.UserName = deResponse.username;
                }
                catch(Exception e)
                {
                    Log.Error(e.Message);
                    Log.Error("Unexpected error occured in LBController");
                }
                lBUserModel.AverageWPM = tuple.Item2;
                lBUserModel.AverageAcc = tuple.Item3;
                //lBUserModel.Name = tuple.Item1.Name;
                //lBUserModel.UserName = tuple.Item1.UserName;
                lBUserModels.Add(lBUserModel);
            }
            return lBUserModels;
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

        // POST api/<LBController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<LBController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<LBController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

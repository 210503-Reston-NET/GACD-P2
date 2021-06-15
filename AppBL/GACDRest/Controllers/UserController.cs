using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GACDModels;
using GACDBL;
using Serilog;
using GACDRest.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using RestSharp;
using Newtonsoft.Json;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GACDRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserBL _userBL;
        private readonly ApiSettings _ApiSettings;
        public UserController(IUserBL userBL, IOptions<ApiSettings> settings)
        {
            _userBL = userBL;
            _ApiSettings = settings.Value;
        }
        // GET: api/<UserController>
        /// <summary>
        /// Gets all the users in the database
        /// </summary>
        /// <returns>List of Users</returns>
        [HttpGet]
        public async Task<IEnumerable<User>> GetAsync()
        {
            return await _userBL.GetUsers();
        }
        /// <summary>
        /// Gets a username with a given user
        /// </summary>
        /// <returns>username / name associated with user</returns>
        // GET api/<UserController>/5
        [Authorize]
        [HttpGet("username")]
        public async Task<ActionResult<UserNameModel>> Get()
        {
            try
            {
                string UserID = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (await _userBL.GetUser(UserID) == null)
                {
                    GACDModels.User user = new GACDModels.User();
                    user.Auth0Id = UserID;
                    await _userBL.AddUser(user);
                }
                User u = await _userBL.GetUser(UserID);
                dynamic AppBearerToken = GetApplicationToken();
                var client = new RestClient($"https://kwikkoder.us.auth0.com/api/v2/users/{u.Auth0Id}");
                var request = new RestRequest(Method.GET);
                request.AddHeader("authorization", "Bearer " + AppBearerToken.access_token);
                IRestResponse restResponse = await client.ExecuteAsync(request);
                dynamic deResponse = JsonConvert.DeserializeObject(restResponse.Content);
                UserNameModel userNameModel = new UserNameModel();
                try
                {
                    userNameModel.UserName = deResponse.username;
                    userNameModel.Name = deResponse.name;
                }
                catch (Exception) { }
                return userNameModel;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                Log.Error("Unexpected error occured in LBController");
                return NotFound();
            }
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


using System.Net.Mime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GACDBL;
using GACDModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Octokit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using GACDRest.DTO;
using RestSharp;
using System.Security.Claims;
using Serilog;
using Newtonsoft.Json;

namespace GACDRest
{
    [Route("api/[controller]")]
    [ApiController]
    public class testController : ControllerBase
    {
        private readonly ApiSettings _ApiSettings;
        private ISnippets _snippetsService;

        public testController(ISnippets snip, IOptions<ApiSettings> settings)
        {
            _snippetsService = snip;
            _ApiSettings = settings.Value;
        }
        [HttpGet]
        [Route("RandomQuote")]
        public async Task<TestMaterial> GetRandomQuote()
        {
            return await _snippetsService.GetRandomQuote();
        }
        [HttpGet]
        [Route("CodeSnippet")]
        public async Task<String> CodeSnippet()
        {
            var l = Language.CSharp;
            return await _snippetsService.GetCodeSnippet(l);
        }
        [HttpGet("CodeSnippet/Secret")]
        [Authorize]
        [EnableCors("AllowOrigin")]
        public async Task<String> CodeSnippetSecret()
        {
            var l = Language.CSharp;
            return await _snippetsService.GetCodeSnippet(l);
        }
        [HttpGet("Test/Secret")]
        [Authorize]
        public async Task<TestUserObject> TestUserSecret()
        {
            //LOTS OF Exceptions handleing needs to be done here
            
            string UserID = "auth0|"+this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            
            Log.Information("UserID?: {0}", UserID);

            dynamic AppBearerToken = GetApplicationToken();

            var client = new RestClient($"https://kwikkoder.us.auth0.com/api/v2/users/{UserID}");
            var request = new RestRequest(Method.GET);
            request.AddHeader("authorization", "Bearer " + AppBearerToken.access_token);
            IRestResponse restResponse = await client.ExecuteAsync(request);            
            dynamic JSONrestResponse = JsonConvert.DeserializeObject(restResponse.Content);

            Log.Information("response from call:{0}",JSONrestResponse);

            //return the wrong thing just to test
            return JSONrestResponse;
            // TestUserObject testUserObject = new TestUserObject();
            // testUserObject.Email = JSONrestResponse.email;
            // testUserObject.Name = JSONrestResponse.name;
            // testUserObject.UserName = JSONrestResponse.username;
            // return testUserObject;
        }

        private dynamic GetApplicationToken(){
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
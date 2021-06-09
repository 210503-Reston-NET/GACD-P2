
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
using System.Text.Json;
using System.Security.Claims;

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
            var client = new RestClient("https://kwikkoder.us.auth0.com/oauth/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", _ApiSettings.authString, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            dynamic JSONresponse = response.Content;
            
            var client1 = new RestClient($"https://kwikkoder.us.auth0.com/api/v2/users/{this.User.FindFirst(ClaimTypes.NameIdentifier).Value}");
            var request1 = new RestRequest(Method.GET);
            request1.AddHeader("authorization", "Bearer " + JSONresponse.access_token);
            IRestResponse restResponse = client1.Execute(request1);
            dynamic JSONrestResponse = restResponse.Content;
            TestUserObject testUserObject = new TestUserObject();
            testUserObject.Email = JSONrestResponse.email;
            testUserObject.Name = JSONrestResponse.name;
            testUserObject.UserName = JSONrestResponse.username;
            return testUserObject;
        }
    }
}
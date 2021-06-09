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

namespace GACDRest
{
    [Route("api/[controller]")]
    [ApiController]
    public class testController : ControllerBase
    {
        private ISnippets _snippetsService;
        private readonly  JwtBearerOptions _jwtOptions;
        public testController(ISnippets snip, IOptionsMonitor<JwtBearerOptions> jwtOptions){
            _snippetsService = snip;
            _jwtOptions = jwtOptions.Get(JwtBearerDefaults.AuthenticationScheme);
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
    }
}
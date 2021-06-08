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

namespace GACDRest
{
    [Route("api/[controller]")]
    [ApiController]
    public class testController : ControllerBase
    {
        private ISnippets _snippetsService;
        public testController(ISnippets snip){
            _snippetsService = snip;
        }
        [HttpGet]
        [Route("RandomQuote")]
        public async Task<TestMaterial> GetRandomQuote()
        {
            return await _snippetsService.GetRandomQuote();
        }
        [HttpGet]
        [Route("CodeSnippet/{Language}")]
        public async Task<String> CodeSnippet(string lang)
        {
            var l = Language.CSharp;
            return await _snippetsService.GetCodeSnippet(l);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GACDBL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<String> GetRandomQuote()
        {
            return await _snippetsService.GetRandomQuote();
        }

    }
}
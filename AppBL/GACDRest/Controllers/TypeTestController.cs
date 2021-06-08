using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GACDBL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Octokit;
using GACDModels;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace GACDRest.Controllers
{

	public class TypeTestController : ControllerBase
	{
		private ISnippets _snippetsService;
        private int language;
        private IUserStatBL _userStatService;

        public TypeTestController(ISnippets snip, int l, IUserStatBL _userstat)

        {
            _snippetsService = snip;
            _userStatService = _userstat;
            language = l;
        }
        [HttpGet]
        public async Task<TestMaterial> GetQuote()
        {
            return await _snippetsService.GetRandomQuote();
        }
        [HttpGet]
        public async Task<String> GetSnippet()
        {
            var snippetLanguage = Language.CSharp;
            switch (language)
            {
                case 1:
                    snippetLanguage = Octokit.Language.CSharp;
                    break;
            }
            return await _snippetsService.GetCodeSnippet(snippetLanguage);
        }
        /// <summary>
        /// Method which adds a test to the database
        /// </summary>
        /// <param name="typeTest">Typetest to insert</param>
        /// <returns>400 if request can't be processed, 200 if successful</returns>
        [HttpPost]
        public async Task<ActionResult> CreateTypeTest(TypeTestInput typeTest)
        {
            TypeTest testToBeInserted = typeTest;
            bool typeTestFlag =  (await _userStatService.AddTestUpdateStat(typeTest.UserId, typeTest.CategoryId, testToBeInserted) == null);
            if (typeTestFlag) return BadRequest();
            else return Ok();
        }
	}
}

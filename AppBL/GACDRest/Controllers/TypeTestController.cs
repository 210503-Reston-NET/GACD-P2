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
        public async Task<String> GetQuote()
        {
            return await _snippetsService.GetRandomQuote()
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
        [HttpPost]
        public async Task<TypeTest> CreateTypeTest(int user, int cgory,  UserStat userStats, int errors, int words, int timeTaken, DateTime date)
        {
            
            TypeTest t = await _userStatService.SaveTypeTest(userStats, errors, words, timeTaken, date);
            await _userStatService.AddTestUpdateStat(user, cgory, t);
            return t;
        }
	}
}

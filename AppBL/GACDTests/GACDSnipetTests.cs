using System;
using Xunit;
using GACDModels;
using GACDBL;
using GACDDL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Serilog;
using Moq;
namespace GACDTests
{

        public class GACDSnipetTest
        {
                private readonly IOptions<ApiSettings> s = Options.Create(new ApiSettings());

                public GACDSnipetTest()
                {

                }
                //ISnippets Coverage
                [Fact]
                public async Task RandomQuoteShouldNotBeNull()
                {
                        ISnippets _snipetService = new Snippets();
                        TestMaterial test1 = await _snipetService.GetRandomQuote();
                        Assert.NotNull(test1);
                }
                [Fact]
                public async Task RandomQuoteShouldReturnRandom()
                {
                        ISnippets _snipetService = new Snippets();
                        TestMaterial test1 = await _snipetService.GetRandomQuote();
                        TestMaterial test2 = await _snipetService.GetRandomQuote();
                        Assert.NotEqual(test1, test2);
                
                }

                
                /*[Fact]
                public async Task RandomCodeShouldNotBeNull()
                {
                        ISnippets _snipetService = new Snippets(s);
                        TestMaterial test1 = await _snipetService.GetCodeSnippet(1);
                        Assert.NotNull(test1);
               
                }
                [Fact]
                public async Task RandomCodeShouldReturnRandom()
                {
                
                }*/

        }
}
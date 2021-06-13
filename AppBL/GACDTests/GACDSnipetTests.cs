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
                private Mock<ISnippets> _snipetService;
                public GACDSnipetTest()
                {
                this._snipetService = new Mock<ISnippets>();
                }

                //ISnippets Coverage
                [Fact]
                public async Task RandomQuoteShouldNotBeNull()
                {
                        var response = _snipetService.Setup(x => 
                        x.GetRandomQuote());
                        Assert.NotNull(response);
                }
                [Fact]
                public async Task RandomQuoteShouldReturnRandom()
                {
                        var response1 =  _snipetService.Setup(x => 
                        x.GetRandomQuote());
                        var response2 = _snipetService.Setup(x => 
                        x.GetRandomQuote());
                        Assert.NotEqual(response1, response2);
                }
                [Fact]
                public async Task RandomCodeShouldNotBeNull()
                {
                var response = _snipetService.Setup(x => 
                x.GetCodeSnippet(1));
                Assert.NotNull(response);
                }
                [Fact]
                public async Task RandomCodeShouldReturnRandom()
                {
                var response1 = _snipetService.Setup(x => 
                x.GetCodeSnippet(1));
                var response2 = _snipetService.Setup(x => 
                x.GetCodeSnippet(1));
                Assert.NotEqual(response1, response2);
                }

        }
}
using System.Threading.Tasks;
using GACDModels;
using Octokit;

namespace GACDBL
{
    public interface ISnippets
    {
        Task<TestMaterial> GetRandomQuote();
        Task<string> GetCodeSnippet(Language language);
        Task<string> GetAuth0String();
    }
}
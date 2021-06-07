using System.Threading.Tasks;
using GACDModels;
using Octokit;

namespace GACDBL
{
    public interface ISnippets
    {
        Task<string> GetRandomQuote();
        Task<string> GetCodeSnippet(Language language);
    }
}
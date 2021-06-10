using System.Threading.Tasks;
using GACDModels;
using Octokit;

namespace GACDBL
{
    public interface ISnippets
    {
        Task<TestMaterial> GetRandomQuote();
        Task<TestMaterial> GetCodeSnippet(int id);
        Task<string> GetAuth0String();
    }
}
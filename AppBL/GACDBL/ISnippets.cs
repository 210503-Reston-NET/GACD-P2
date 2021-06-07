using System.Threading.Tasks;
using GACDModels;

namespace GACDBL
{
    public interface ISnippets
    {
        Task<string> GetRandomQuote();
        Task<string> GetCodeSnippet(string language);
    }
}
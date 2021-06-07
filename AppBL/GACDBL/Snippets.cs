using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Serilog;

namespace GACDBL
{
    public class Snippets : ISnippets
    {
        public Snippets(){

        }

        public async Task<string> GetRandomQuote()
        {
            Log.Debug("Getting Random Quote From API");
            var client = new HttpClient();
            var uri = new Uri("http://api.quotable.io/random");
                        
            dynamic responseContent = null;
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
               responseContent = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
               Log.Debug($"Retrieved Quote {responseContent.content}");
            }else{
                Log.Warning("Getting Quote Failed");
            }
            return responseContent.content;
            //Should we return content, author, length?           
        }

        Task<string> ISnippets.GetCodeSnippet(string language)
        {
            throw new NotImplementedException();
        }
    }
}
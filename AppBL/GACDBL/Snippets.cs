using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Octokit;
using Serilog;

namespace GACDBL
{
    public class Snippets : ISnippets
    {
        private readonly ApiSettings _ApiSettings;
        public Snippets(IOptions<ApiSettings> settings){
            _ApiSettings = settings.Value;
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

        public async Task<string> GetCodeSnippet(Octokit.Language language)
        {
            var client = new GitHubClient(new ProductHeaderValue("Not-sure-why-I-need-this"));
            var tokenAuth = new Credentials(_ApiSettings.githubApiKey); // NOTE: not real token
            client.Credentials = tokenAuth;

            var request = new SearchCodeRequest{
                Language = language, 
            };

            SearchCodeResult searchResult= await client.Search.SearchCode(request);
            
            String htmlUrl = searchResult.Items[0].HtmlUrl;
            //convert html url to raw.githubusercontent
            htmlUrl = htmlUrl.Replace("/blob/", "/");
            htmlUrl = htmlUrl.Replace("https://github.com/", "https://raw.githubusercontent.com/");
            
            //collect text from site
            var httpClient = new HttpClient();
            var uri = new Uri(htmlUrl);

            dynamic responseContent = null;
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
               responseContent = await response.Content.ReadAsStringAsync();
               Log.Debug("Retrieved Code snippet");
            }else{
                Log.Warning("Getting Code Snippet Failed");
            }

            return responseContent;
           
        }
    }
}
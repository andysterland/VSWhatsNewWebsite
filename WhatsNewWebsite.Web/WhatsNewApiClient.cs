using WhatsNewWebsite.DataModel;

namespace WhatsNewWebsite.Web
{
    public class WhatsNewApiClient(HttpClient httpClient)
    {
        public async Task<List<WhatsNewItemSummary>> GetAllSummaries()
        {
            var url = "whatsnewitem/";
            return await GetSummaries(url);
        }
        public async Task<WhatsNewItemSummary> GetSummaryById(string Id)
        {
            return await httpClient.GetFromJsonAsync<WhatsNewItemSummary>($"whatsnewitem/{Id}");
        }

        public async Task<List<WhatsNewItemSummary>> GetSearchResults(string Query)
        {
            var url = $"search/{Query}";
            return await GetSummaries(url);
        }

        private async Task<List<WhatsNewItemSummary>> GetSummaries(string Url)
        {
            List<WhatsNewItemSummary>? itemSummaries = null;
            try
            {
                var response = await httpClient.GetAsync(Url);
                response.EnsureSuccessStatusCode();
                itemSummaries = await response.Content.ReadFromJsonAsync<List<WhatsNewItemSummary>>();
            }
            catch (HttpRequestException e)
            {
                // Log error
                Console.WriteLine($"Error: {e.Message}");
            }


            return itemSummaries ?? new List<WhatsNewItemSummary>(); ;
        }


        public async Task<List<WhatsNewItemSummary>> GetWithVersion(string Version)
        {
            return await httpClient.GetFromJsonAsync<List<WhatsNewItemSummary>>($"whatsnewitem/version/{Version}");
        }

        public async Task<WhatsNewItemArticle> GetArticle(string Id)
        {
            return await httpClient.GetFromJsonAsync<WhatsNewItemArticle>($"article/{Id}");
        }
    }
}

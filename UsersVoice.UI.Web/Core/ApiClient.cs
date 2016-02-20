using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UsersVoice.UI.Web.Models;

namespace UsersVoice.UI.Web.Core
{
    public class ApiClient : IApiClient
    {
        private readonly string _baseUrl;

        public ApiClient(string apiUrl)
        {
            _baseUrl = apiUrl;
        }

        public string GetUrl(string url)
        {
            return string.Format("{0}{1}", _baseUrl, url);
        }

        public async Task<PagedCollection<TModel>> FetchCollection<TModel>(string url)
        {
            var items = await FetchData<PagedCollection<TModel>>(url);
            return items;
        }

        public async Task<TModel> FetchData<TModel>(string url)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(GetUrl(url));
                if(null == response || response.StatusCode != HttpStatusCode.OK)
                    return default(TModel);

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(content))
                    return default(TModel);
                var item = JsonConvert.DeserializeObject<TModel>(content);
                return item;
            }
        }
    }
}
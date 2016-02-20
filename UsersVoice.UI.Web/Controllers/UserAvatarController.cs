using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using UsersVoice.UI.Web.Core;

namespace UsersVoice.UI.Web.Controllers
{
    [RoutePrefix("api/useravatar")]
    public class UserAvatarController : ApiController
    {
        private readonly IApiClient _apiClient;

        public UserAvatarController(IApiClientFactory apiClientFactory)
        {
            if (apiClientFactory == null) throw new ArgumentNullException("apiClientFactory");
            _apiClient = apiClientFactory.Create(new ApiClientContext(Services.Content));
        }

        public async Task<IHttpActionResult> Get(Guid id)
        {
            var url = _apiClient.GetUrl("useravatar/" + id);

            HttpResponseMessage response;

            using (var client = new HttpClient())
                response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

            var streamToReadFrom = await response.Content.ReadAsStreamAsync();

            var content = new StreamContent(streamToReadFrom);
            content.Headers.ContentType = response.Content.Headers.ContentType;

            var imageResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = content
            };

            return ResponseMessage(imageResponse);
        }
    }
}
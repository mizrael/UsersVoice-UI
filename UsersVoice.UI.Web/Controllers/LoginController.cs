using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using UsersVoice.UI.Web.Core;
using UsersVoice.UI.Web.Models;

namespace UsersVoice.UI.Web.Controllers
{
    [RoutePrefix("api/Login")]
    public class LoginController : ApiController
    {
        private readonly IApiClient _apiClient;

        public LoginController(IApiClientFactory apiClientFactory)
        {
            if (apiClientFactory == null) throw new ArgumentNullException("apiClientFactory");
            _apiClient = apiClientFactory.Create(new ApiClientContext(Services.Default));
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(LoginRequestModel model)
        {
            var url = "/users?email=" + model.userName;

            var items = await _apiClient.FetchCollection<User>(url);
            if (null == items || 0 == items.TotalItemsCount)
                return NotFound();
            return Ok(items.Items.FirstOrDefault());

        }
    }
}

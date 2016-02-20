using System;
using System.Threading.Tasks;
using System.Web.Http;
using UsersVoice.UI.Web.Core;
using UsersVoice.UI.Web.Models;

namespace UsersVoice.UI.Web.Controllers
{
    [RoutePrefix("api/IdeaComments")]
    public class IdeaCommentsController : ApiController
    {
        private readonly IApiClient _apiClient;

        public IdeaCommentsController(IApiClientFactory apiClientFactory)
        {
            if (apiClientFactory == null) throw new ArgumentNullException("apiClientFactory");
            _apiClient = apiClientFactory.Create(new ApiClientContext(Services.Default));
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get([FromUri] CommentFilter filter)
        {
            var url = string.Format("/ideaComments?page={0}&pageSize={1}", filter.Page, filter.PageSize);
            if (filter.IdeaId != Guid.Empty)
                url += "&ideaId=" + filter.IdeaId;
            if (filter.AuthorId  != Guid.Empty)
                url += "&authorId=" + filter.AuthorId;

            var items = await _apiClient.FetchCollection<Comment>(url);
            return Ok(items);
        }

    }
}
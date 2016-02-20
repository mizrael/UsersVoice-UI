using System;
using System.Threading.Tasks;
using System.Web.Http;
using UsersVoice.UI.Web.Core;
using UsersVoice.UI.Web.Models;

namespace UsersVoice.UI.Web.Controllers
{
    [RoutePrefix("api/Areas")]
    public class AreasController : ApiController
    {
        private readonly IApiClient _apiClient;

        public AreasController(IApiClientFactory apiClientFactory)
        {
            if (apiClientFactory == null) throw new ArgumentNullException("apiClientFactory");
            _apiClient = apiClientFactory.Create(new ApiClientContext(Services.Default));
        }

        public async Task<IHttpActionResult> Get()
        {
            var url = "/areas?pageSize=10&page=0";

            var items = await _apiClient.FetchCollection<Area>(url);
            return Ok(items);
        }

        [HttpGet, Route("{id}")]
        public async Task<IHttpActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return NotFound();

            var url = "/areas/" + id;
            var item = await _apiClient.FetchData<Area>(url);

            return Ok(item);

        }

        [HttpGet, Route("{areaId}/ideas")]
        public async Task<IHttpActionResult> GetIdeas(Guid areaId, int page, int pageSize)
        {
            if (areaId == Guid.Empty)
                return NotFound();

            var url = string.Format("/ideas?sortBy=CreationDate&sortDirection=DESC&page={0}&pageSize={1}&areaId={2}",
                page, pageSize, areaId);

            var items = await _apiClient.FetchCollection<Idea>(url);
            return Ok(items);
        }

    }
}
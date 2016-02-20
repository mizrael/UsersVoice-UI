using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Coder101.ApiControllers
{
    [RoutePrefix("api/Areas")]
    public class AreasController : ApiController
    {
        [Route("")]
        public async Task<HttpResponseMessage> Get()
        {
            var areas = await Database.GetAreasAsync();
            return Request.CreateResponse(HttpStatusCode.OK, new {areas});
        }
    }

    public class AreaModel
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public static class Database
    {
        private static List<AreaModel> _areas;

        static Database()
        {
            _areas=new List<AreaModel>();

            for (int x = 0; x < 3; x++)
            {
                _areas.Add(new AreaModel(){ID=Guid.NewGuid().ToString(),Title=string.Format("Area {0} Title", x),Description = string.Format("Area {0}Description", x)});
            }

        }

        public static async Task<IEnumerable<AreaModel>> GetAreasAsync()
        {
            return _areas;
        }

        public static async Task AddArea(AreaModel area)
        {
            _areas.Add(area);
        }
    }
}

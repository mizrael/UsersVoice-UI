using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using UsersVoice.UI.Web.Core;
using UsersVoice.UI.Web.Models;

namespace UsersVoice.UI.Web.Controllers
{

    [RoutePrefix("api/Ideas")]
    public class IdeasController : ApiController
    {
        private readonly IApiClient _apiClient;

        public IdeasController(IApiClientFactory apiClientFactory)
        {
            if (apiClientFactory == null) throw new ArgumentNullException("apiClientFactory");
            _apiClient = apiClientFactory.Create(new ApiClientContext(Services.Default));
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get([FromUri]IdeaFilter filter)
        {
            filter = filter ?? new IdeaFilter();

#if DEBUG
          
            var ideas = await Database.GetLatestIdeasAsync(filter.PageSize);
            return Ok(ideas);
#else
            var url = string.Format("/ideas?sortBy={0}&sortDirection={1}&page={2}&pageSize={3}",
              filter.SortBy, filter.SortDirection, filter.Page, filter.PageSize);
            if (filter.AreaId != Guid.Empty)
                url += "&areaId=" + filter.AreaId;

            var items = await _apiClient.FetchCollection<Idea>(url);
            return Ok(items);
#endif
        }

        [HttpGet, Route("{id}")]
        public async Task<IHttpActionResult> GetById(Guid id)
        {
#if DEBUG

            var idea = await Database.GetIdeaAsync(id);
            return Ok(idea);
#else
            if (id == Guid.Empty)
                return NotFound();

            var url = "/ideas/"+id;
            var item = await _apiClient.FetchData<Idea>(url);
            if (null != item)
            {
                url = "/ideaComments?ideaId=" + id;
                var comments = await _apiClient.FetchData<PagedCollection<Comment>>(url);
                item.Comments = (null != comments) ? comments.Items : Enumerable.Empty<Comment>();
            }
            return Ok(item);
#endif
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(Idea idea)
        {
#if DEBUG
            await Database.AddIdea(idea);
#else
            var result = string.Empty;

            using (var client = new HttpClient())
            {
                var data = new CreateIdeaCommand(Guid.NewGuid(), idea.AreaId, idea.AuthorId, idea.Title, idea.Description);
                var responsePost = await client.PostAsJsonAsync(_apiClient.GetUrl("/ideas"), data);

                result = await responsePost.Content.ReadAsStringAsync();
                if (!responsePost.IsSuccessStatusCode)
                    return InternalServerError();
            }
#endif

            return Ok();
        }

        [HttpPost, Route("{id}/comment")]
        public async Task<IHttpActionResult> PostComment(Comment comment)
        {
            comment.CommentId = Guid.NewGuid();
#if DEBUG
            await Database.SaveCommentAsync(comment);
            return Ok();
#else
            var result = string.Empty;
            using (var client = new HttpClient())
            {
                var responsePost = await client.PostAsJsonAsync(_apiClient.GetUrl("/ideaComments"), comment);
                result = await responsePost.Content.ReadAsStringAsync();
                if (!responsePost.IsSuccessStatusCode)
                    return InternalServerError();
            }
            return Ok(result);
#endif
        }

        [HttpGet, Route("{ideaId}/votes/{userId}")]
        public async Task<IHttpActionResult> HasVoted(Guid ideaId, Guid userId)
        {
#if DEBUG
            var result = await Database.HasVoted(ideaId, userId);
            return Ok(result);
#else
            var url = String.Format("/vote/hasVoted?ideaId={0}&userId={1}", ideaId, userId);

            var result = await _apiClient.FetchData<bool>(url);
            return Ok(result);
#endif

        }

        [HttpPost, Route("vote/")]
        public async Task<IHttpActionResult> Vote(Vote newVote)
        {
           
#if DEBUG
            var user = await Database.AddVote(newVote);
            return Ok(user);
#else
            var url = "/vote/";
            if (newVote.Points < 1)
                url += "unvote/";

            using (var client = new HttpClient())
            {
                var responsePost = await client.PostAsJsonAsync(_apiClient.GetUrl(url), newVote);
                var result = await responsePost.Content.ReadAsStringAsync();
                if (!responsePost.IsSuccessStatusCode)
                    return InternalServerError(new Exception(result));
            }

            url = "/users/" + newVote.VoterId;
            var user = await _apiClient.FetchData<User>(url);
            if (null == user)
                return NotFound();
            return Ok(user);
#endif
        }
    }
}
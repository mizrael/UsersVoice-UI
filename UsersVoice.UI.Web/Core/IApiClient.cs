using System.Threading.Tasks;
using UsersVoice.UI.Web.Models;

namespace UsersVoice.UI.Web.Core
{
    public interface IApiClient
    {
        string GetUrl(string url);

        Task<PagedCollection<TModel>> FetchCollection<TModel>(string url);

        Task<TModel> FetchData<TModel>(string url);
    }
}
namespace UsersVoice.UI.Web.Core
{
    public interface IApiClientFactory
    {
        IApiClient Create(ApiClientContext context);
    }
}
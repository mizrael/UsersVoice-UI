namespace UsersVoice.UI.Web.Core
{
    public class ApiClientContext
    {
        public Services Service { get; private set; }

        public ApiClientContext(Services service)
        {
            Service = service;
        }
    }

    public enum Services
    {
        Default,
        Content
    }
}
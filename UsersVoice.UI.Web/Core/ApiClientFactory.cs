using System;
using System.Configuration;

namespace UsersVoice.UI.Web.Core
{
    public class ApiClientFactory : IApiClientFactory
    {
        public IApiClient Create(ApiClientContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            string url;

            switch (context.Service)
            {
                case Services.Content:
                    url = ConfigurationManager.AppSettings["ContentServiceUrl"];
                    break;
                default:
                    url = ConfigurationManager.AppSettings["DefaultServiceUrl"];
                    break;
            }

            return new ApiClient(url);
        }
    }
}
using System.Web.Http;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using UsersVoice.UI.Web.Core;

namespace UsersVoice.UI.Web
{
    public class DependenciesConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new Container();

            container.Register<IApiClientFactory, ApiClientFactory>();

            container.RegisterWebApiControllers(config);
            
            container.Verify();

            config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }
    }
}
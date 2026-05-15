using AutoMapper;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Vidly_MVCProject
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static IMapper Mapper { get; private set; }

        protected void Application_Start()
        {
            var config = new MapperConfigurationExpression();
            config.AddProfile<MappingProfile>();

            var mapperConfig = new MapperConfiguration(config, null);
            Mapper = mapperConfig.CreateMapper();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
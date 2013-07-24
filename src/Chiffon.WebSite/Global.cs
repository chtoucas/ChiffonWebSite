namespace Chiffon.WebSite
{
    using System.Web;
    //using System.Web.Http;
    using System.Web.Mvc;
    //using System.Web.Optimization;
    using System.Web.Routing;
    using Chiffon.WebSite.Activation;

    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            // Remove the header "X-AspNetMvc-Version".
            MvcHandler.DisableMvcResponseHeader = true;
            
            // 6. Authentification. (FIXME)
            //AuthConfig.Start();

            // 1. Areas.
            //AreaRegistration.RegisterAllAreas();
            // 3. Filters.
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            // 4. Routes.
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            // 5. Optimisation.
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
            // 2. WebApi.
            //WebApiConfig.Register(GlobalConfiguration.Configuration);
        }
    }
}
﻿namespace Chiffon.WebSite.Configs
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Autofac;
    using Autofac.Integration.Mvc;
    using Chiffon.Crosscuttings;
    using Chiffon.Services;
    using Chiffon.WebSite.Handlers;

    public static class DependencyConfig
    {
        public static void RegisterDependencies(ChiffonConfig config, RouteCollection routes)
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new CrosscuttingsModule(config));
            builder.RegisterModule(new PersistenceModule());
            builder.RegisterModule(new ServicesModule());

            builder.RegisterControllers(typeof(Global).Assembly);

            RegisterHttpHandlers_(builder, routes);

            IContainer container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        static void RegisterHttpHandlers_(ContainerBuilder builder, RouteCollection routes)
        {
            routes.Add(new Route("PatternImage.ashx", new AutofacRouteHandler<PatternImageHandler>()));
            builder.Register(
                    _ => new PatternImageHandler(_.Resolve<ChiffonConfig>(), _.Resolve<IPatternService>()))
                .AsSelf().SingleInstance(); // XXX: Maybe InstancePerHttpRequest
        }

        // Cf. https://groups.google.com/forum/#!msg/autofac/BkY4s4tusUc/micDCB0YiN8J
        class AutofacRouteHandler<THandler> : IRouteHandler where THandler : IHttpHandler
        {
            public IHttpHandler GetHttpHandler(RequestContext requestContext)
            {
                return AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<THandler>();
            }
        }
    }
}

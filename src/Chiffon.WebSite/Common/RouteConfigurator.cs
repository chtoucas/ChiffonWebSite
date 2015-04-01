namespace Chiffon.Common
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Chiffon.Handlers;
    using Chiffon.Infrastructure;
    using Narvalo;
    using Narvalo.Web;

    // TODO: I18N.
    public sealed class RouteConfigurator
    {
        private readonly RouteCollection _routes;

        public RouteConfigurator(RouteCollection routes)
        {
            Require.NotNull(routes, "routes");

            _routes = routes;
        }

        public void Configure()
        {
            _routes.IgnoreRoute("devil/elmah");
            _routes.IgnoreRoute("devil/glimpse");
            _routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // HomeController.
            _routes.MapRoute(Constants.RouteName.Home.Index, String.Empty,
                new { controller = Constants.ControllerName.Home, action = Constants.ActionName.Home.Index });
            _routes.MapRoute(Constants.RouteName.Home.About, Routes.About,
                new { controller = Constants.ControllerName.Home, action = Constants.ActionName.Home.About });
            _routes.MapRoute(Constants.RouteName.Home.Contact, Routes.Contact,
                new { controller = Constants.ControllerName.Home, action = Constants.ActionName.Home.Contact });
            _routes.MapRoute(Constants.RouteName.Home.ContactSuccess, "contact-confirmation",
                new { controller = Constants.ControllerName.Home, action = Constants.ActionName.Home.ContactSuccess });

            // AccountController.
            _routes.MapRoute(Constants.RouteName.Account.Register, Routes.Register,
                new { controller = Constants.ControllerName.Account, action = Constants.ActionName.Contact.Register });
            _routes.MapRoute(Constants.RouteName.Account.RegisterSuccess, "inscription-confirmation",
                new { controller = Constants.ControllerName.Account, action = Constants.ActionName.Contact.RegisterSuccess });
            _routes.MapRoute(Constants.RouteName.Account.Login, Routes.Login,
                new { controller = Constants.ControllerName.Account, action = Constants.ActionName.Contact.Login });
            _routes.MapRoute(Constants.RouteName.Account.Newsletter, Routes.Newsletter,
                new { controller = Constants.ControllerName.Account, action = Constants.ActionName.Contact.Newsletter });

            // DesignerController.
            _routes.MapRoute(Constants.RouteName.Designer.Index, Routes.Designer,
                new { controller = Constants.ControllerName.Designer, action = Constants.ActionName.Designer.Index },
                new { designerKey = new DesignerKeyConstraint() });
            _routes.MapRoute(Constants.RouteName.Designer.Category, Routes.DesignerCategory,
                new { controller = Constants.ControllerName.Designer, action = Constants.ActionName.Designer.Category },
                new { designerKey = new DesignerKeyConstraint() });
            _routes.MapRoute(Constants.RouteName.Designer.Pattern, Routes.DesignerPattern,
                new { controller = Constants.ControllerName.Designer, action = Constants.ActionName.Designer.Pattern },
                new { designerKey = new DesignerKeyConstraint() });

            // MailController.
            _routes.MapRoute(Constants.RouteName.MailMerge.Welcome, "mail/bienvenue",
                new { controller = Constants.ControllerName.MailMerge, action = Constants.ActionName.MailMerge.Welcome });
            _routes.MapRoute(Constants.RouteName.MailMerge.ForgottenPassword, "mail/mot-de-passe-oublié",
               new { controller = Constants.ControllerName.MailMerge, action = Constants.ActionName.MailMerge.ForgottenPassword });

            _routes.MapChildOnlyActionRoutesFrom(typeof(Global).Assembly);

            RegisterHandlerRoutes_();
        }

        private void RegisterHandlerRoutes_()
        {
            _routes.Add(new Route(Routes.Pattern, new AutofacRouteHandler<PatternImageHandler>()));
            _routes.Add(new Route(Routes.LogOn, new AutofacRouteHandler<LogOnHandler>()));
            _routes.Add(new Route(Routes.LogOff, new AutofacRouteHandler<LogOffHandler>()));
            //_routes.Add(new Route("go", new AutofacRouteHandler<GoHandler>()));
        }
    }
}
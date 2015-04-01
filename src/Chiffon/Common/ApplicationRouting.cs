namespace Chiffon.Common
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Chiffon.Handlers;
    using Chiffon.Internal;
    using Narvalo;
    using Narvalo.Web;

    // TODO: I18N.
    public sealed class ApplicationRouting
    {
        private readonly RouteCollection _routes;

        public ApplicationRouting(RouteCollection routes)
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
            _routes.MapRoute(
                Constants.RouteName.Home.Index,
                String.Empty,
                new { controller = Constants.ControllerName.Home, action = Constants.ActionName.Home.Index });
            _routes.MapRoute(
                Constants.RouteName.Home.About,
                Constants.RoutePath.About,
                new { controller = Constants.ControllerName.Home, action = Constants.ActionName.Home.About });
            _routes.MapRoute(
                Constants.RouteName.Home.Contact,
                Constants.RoutePath.Contact,
                new { controller = Constants.ControllerName.Home, action = Constants.ActionName.Home.Contact });
            _routes.MapRoute(
                Constants.RouteName.Home.ContactSuccess,
                "contact-confirmation",
                new { controller = Constants.ControllerName.Home, action = Constants.ActionName.Home.ContactSuccess });

            // AccountController.
            _routes.MapRoute(
                Constants.RouteName.Account.Register,
                Constants.RoutePath.Register,
                new { controller = Constants.ControllerName.Account, action = Constants.ActionName.Contact.Register });
            _routes.MapRoute(
                Constants.RouteName.Account.RegisterSuccess,
                "inscription-confirmation",
                new { controller = Constants.ControllerName.Account, action = Constants.ActionName.Contact.RegisterSuccess });
            _routes.MapRoute(
                Constants.RouteName.Account.Login,
                Constants.RoutePath.Login,
                new { controller = Constants.ControllerName.Account, action = Constants.ActionName.Contact.Login });
            _routes.MapRoute(
                Constants.RouteName.Account.Newsletter,
                Constants.RoutePath.Newsletter,
                new { controller = Constants.ControllerName.Account, action = Constants.ActionName.Contact.Newsletter });

            // DesignerController.
            _routes.MapRoute(
                Constants.RouteName.Designer.Index,
                Constants.RoutePath.Designer,
                new { controller = Constants.ControllerName.Designer, action = Constants.ActionName.Designer.Index },
                new { designerKey = new DesignerKeyConstraint() });
            _routes.MapRoute(
                Constants.RouteName.Designer.Category,
                Constants.RoutePath.DesignerCategory,
                new { controller = Constants.ControllerName.Designer, action = Constants.ActionName.Designer.Category },
                new { designerKey = new DesignerKeyConstraint() });
            _routes.MapRoute(
                Constants.RouteName.Designer.Pattern,
                Constants.RoutePath.DesignerPattern,
                new { controller = Constants.ControllerName.Designer, action = Constants.ActionName.Designer.Pattern },
                new { designerKey = new DesignerKeyConstraint() });

            // MailController.
            _routes.MapRoute(
                Constants.RouteName.MailMerge.Welcome,
                "mail/bienvenue",
                new { controller = Constants.ControllerName.MailMerge, action = Constants.ActionName.MailMerge.Welcome });
            _routes.MapRoute(
                Constants.RouteName.MailMerge.ForgottenPassword,
                "mail/mot-de-passe-oublié",
               new { controller = Constants.ControllerName.MailMerge, action = Constants.ActionName.MailMerge.ForgottenPassword });

            _routes.MapChildOnlyActionRoutesFrom(typeof(Application).Assembly);

            RegisterHandlerRoutes_();
        }

        private void RegisterHandlerRoutes_()
        {
            _routes.Add(new Route(Constants.RoutePath.Pattern, new AutofacRouteHandler<PatternImageHandler>()));
            _routes.Add(new Route(Constants.RoutePath.LogOn, new AutofacRouteHandler<LogOnHandler>()));
            _routes.Add(new Route(Constants.RoutePath.LogOff, new AutofacRouteHandler<LogOffHandler>()));
        }
    }
}
namespace Chiffon.Common
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Chiffon.Handlers;
    using Narvalo;
    using Narvalo.Web;

    // TODO: I18N.
    public class RouteConfigurator
    {
        readonly RouteCollection _routes;

        public RouteConfigurator(RouteCollection routes)
        {
            Requires.NotNull(routes, "routes");

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
            _routes.MapRoute(Constants.RouteName.Home.About, "informations",
                new { controller = Constants.ControllerName.Home, action = Constants.ActionName.Home.About });
            _routes.MapRoute(Constants.RouteName.Home.Contact, "contact",
                new { controller = Constants.ControllerName.Home, action = Constants.ActionName.Home.Contact });
            _routes.MapRoute(Constants.RouteName.Home.ContactSuccess, "contact-confirmation",
                new { controller = Constants.ControllerName.Home, action = Constants.ActionName.Home.ContactSuccess });

            // AccountController.
            _routes.MapRoute(Constants.RouteName.Account.Register, "inscription",
                new { controller = Constants.ControllerName.Account, action = Constants.ActionName.Contact.Register });
            _routes.MapRoute(Constants.RouteName.Account.RegisterSuccess, "inscription-confirmation",
                new { controller = Constants.ControllerName.Account, action = Constants.ActionName.Contact.RegisterSuccess });
            _routes.MapRoute(Constants.RouteName.Account.Login, "connexion",
                new { controller = Constants.ControllerName.Account, action = Constants.ActionName.Contact.Login });
            _routes.MapRoute(Constants.RouteName.Account.Newsletter, "newsletter",
                new { controller = Constants.ControllerName.Account, action = Constants.ActionName.Contact.Newsletter });

            // DesignerController.
            _routes.MapRoute(Constants.RouteName.Designer.Index, "{designerKey}",
                new { controller = Constants.ControllerName.Designer, action = Constants.ActionName.Designer.Index },
                new { designerKey = new DesignerKeyConstraint() });
            _routes.MapRoute(Constants.RouteName.Designer.Category, "{designerKey}/{categoryKey}",
                new { controller = Constants.ControllerName.Designer, action = Constants.ActionName.Designer.Category },
                new { designerKey = new DesignerKeyConstraint() });
            _routes.MapRoute(Constants.RouteName.Designer.Pattern, "{designerKey}/{categoryKey}/{reference}",
                new { controller = Constants.ControllerName.Designer, action = Constants.ActionName.Designer.Pattern },
                new { designerKey = new DesignerKeyConstraint() });

            // MailController.
            _routes.MapRoute(Constants.RouteName.Mail.Welcome, "mail/bienvenue",
                new { controller = Constants.ControllerName.Mail, action = Constants.ActionName.Mail.Welcome });
            _routes.MapRoute(Constants.RouteName.Mail.ForgottenPassword, "mail/mot-de-passe-oublié",
               new { controller = Constants.ControllerName.Mail, action = Constants.ActionName.Mail.ForgottenPassword });

            _routes.MapChildOnlyActionRoutesFrom(typeof(Global).Assembly);

            RegisterHandlerRoutes_();
        }

        void RegisterHandlerRoutes_()
        {
            _routes.Add(new Route("motif", new AutofacRouteHandler<PatternImageHandler>()));
            _routes.Add(new Route("connecte", new AutofacRouteHandler<LogOnHandler>()));
            _routes.Add(new Route("disconnecte", new AutofacRouteHandler<LogOffHandler>()));
            //_routes.Add(new Route("go", new AutofacRouteHandler<GoHandler>()));
        }
    }
}
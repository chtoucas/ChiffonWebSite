namespace Chiffon
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Chiffon.Handlers;
    using Chiffon.Internal;
    using Narvalo;
    using Narvalo.Web;

    public static class Routes
    {
        // TODO: I18N.
        public static void Register(RouteCollection routes)
        {
            Require.NotNull(routes, "routes");

            routes.IgnoreRoute("devil/elmah");
            routes.IgnoreRoute("devil/glimpse");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // HomeController.
            routes.MapRoute(
                Constants.RouteName.Home.Index,
                String.Empty,
                new { controller = Constants.ControllerName.Home, action = Constants.ActionName.Home.Index });
            routes.MapRoute(
                Constants.RouteName.Home.About,
                Constants.RoutePath.About,
                new { controller = Constants.ControllerName.Home, action = Constants.ActionName.Home.About });
            routes.MapRoute(
                Constants.RouteName.Home.Contact,
                Constants.RoutePath.Contact,
                new { controller = Constants.ControllerName.Home, action = Constants.ActionName.Home.Contact });
            routes.MapRoute(
                Constants.RouteName.Home.ContactSuccess,
                "contact-confirmation",
                new { controller = Constants.ControllerName.Home, action = Constants.ActionName.Home.ContactSuccess });

            // AccountController.
            routes.MapRoute(
                Constants.RouteName.Account.Register,
                Constants.RoutePath.Register,
                new { controller = Constants.ControllerName.Account, action = Constants.ActionName.Account.Register });
            routes.MapRoute(
                Constants.RouteName.Account.RegisterSuccess,
                "inscription-confirmation",
                new { controller = Constants.ControllerName.Account, action = Constants.ActionName.Account.RegisterSuccess });
            routes.MapRoute(
                Constants.RouteName.Account.Login,
                Constants.RoutePath.Login,
                new { controller = Constants.ControllerName.Account, action = Constants.ActionName.Account.Login });

            // DesignerController.
            routes.MapRoute(
                Constants.RouteName.Designer.Index,
                Constants.RoutePath.Designer,
                new { controller = Constants.ControllerName.Designer, action = Constants.ActionName.Designer.Index },
                new { designerKey = new DesignerKeyConstraint_() });
            routes.MapRoute(
                Constants.RouteName.Designer.Category,
                Constants.RoutePath.DesignerCategory,
                new { controller = Constants.ControllerName.Designer, action = Constants.ActionName.Designer.Category },
                new { designerKey = new DesignerKeyConstraint_() });
            routes.MapRoute(
                Constants.RouteName.Designer.Pattern,
                Constants.RoutePath.DesignerPattern,
                new { controller = Constants.ControllerName.Designer, action = Constants.ActionName.Designer.Pattern },
                new { designerKey = new DesignerKeyConstraint_() });

            routes.MapChildOnlyActionRoutesFrom(typeof(ApplicationLifecycle).Assembly);

            // Gestionnaires HTTP.
            routes.Add(new Route(Constants.RoutePath.Pattern, new AutofacRouteHandler<PatternImageHandler>()));
            routes.Add(new Route(Constants.RoutePath.LogOn, new AutofacRouteHandler<LogOnHandler>()));
            routes.Add(new Route(Constants.RoutePath.LogOff, new AutofacRouteHandler<LogOffHandler>()));
            routes.Add(new Route(Constants.RoutePath.Go, new AutofacRouteHandler<GoHandler>()));
        }

        private sealed class DesignerKeyConstraint_ : IRouteConstraint
        {
            public bool Match(
                HttpContextBase httpContext,
                Route route,
                string parameterName,
                RouteValueDictionary values,
                RouteDirection routeDirection)
            {
                Require.NotNull(values, "values");

                var value = (string)values["designerKey"];
                switch (value)
                {
                    case "chicamancha":
                    case "viviane-devaux":
                    case "petroleum-blue":
                    case "laure-roussel":
                        return true;

                    default:
                        return false;
                }
            }
        }
    }
}
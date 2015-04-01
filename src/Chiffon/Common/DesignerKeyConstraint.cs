namespace Chiffon.Common
{
    using System.Web;
    using System.Web.Routing;

    using Narvalo;

    public sealed class DesignerKeyConstraint : IRouteConstraint
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
            switch (value) {
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
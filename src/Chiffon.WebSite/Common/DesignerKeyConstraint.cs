﻿namespace Chiffon.Common
{
    using System.Web;
    using System.Web.Routing;

    public class DesignerKeyConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName,
            RouteValueDictionary values, RouteDirection routeDirection)
        {
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
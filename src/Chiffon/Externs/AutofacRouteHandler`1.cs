// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Chiffon.Infrastructure
{
    using System.Web;
    using System.Web.Routing;

    using Autofac;
    using Autofac.Integration.Mvc;

    // Cf. https://groups.google.com/forum/#!msg/autofac/BkY4s4tusUc/micDCB0YiN8J
    public sealed class AutofacRouteHandler<THandler> : IRouteHandler where THandler : IHttpHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<THandler>();
        }
    }
}

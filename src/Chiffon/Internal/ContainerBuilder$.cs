﻿namespace Chiffon.Internal
{
    using System;
    using System.Reflection;
    using System.Web;

    using Autofac;
    using Autofac.Builder;
    using Autofac.Features.Scanning;
    using Narvalo;

    public static class ContainerBuilderExtensions
    {
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterHandlers(
                this ContainerBuilder @this,
                params Assembly[] handlerAssemblies)
        {
            Require.Object(@this);

            return @this.RegisterAssemblyTypes(handlerAssemblies)
                .Where(_ => typeof(IHttpHandler).IsAssignableFrom(_)
                    && _.Name.EndsWith("Handler", StringComparison.Ordinal));
        }
    }
}

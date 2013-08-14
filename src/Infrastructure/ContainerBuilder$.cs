namespace Chiffon.Infrastructure
{
    using System;
    using System.Reflection;
    using System.Web;
    using Autofac;
    using Autofac.Builder;
    using Autofac.Features.Scanning;

    public static class ContainerBuilderExtensions
    {
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterHandlers(this ContainerBuilder builder, params Assembly[] handlerAssemblies)
        {
            return builder.RegisterAssemblyTypes(handlerAssemblies)
                .Where(_ => typeof(IHttpHandler).IsAssignableFrom(_)
                    && _.Name.EndsWith("Handler", StringComparison.Ordinal));
        }
    }
}

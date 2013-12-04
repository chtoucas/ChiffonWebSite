namespace Chiffon
{
    using Autofac;
    using Autofac.Integration.Mvc;
    using Chiffon.Data;
    using Chiffon.Infrastructure;
    using Narvalo;

    public class DataAutofacModule : Module
    {
        readonly ChiffonConfig _config;

        public DataAutofacModule(ChiffonConfig config)
        {
            Requires.NotNull(config, "config");

            _config = config;
        }

        protected override void Load(ContainerBuilder builder)
        {
            Requires.NotNull(builder, "builder");

            builder.RegisterType<QueryCache>().As<IQueryCache>().InstancePerHttpRequest();

            if (_config.EnableServerCache) {
                builder.Register(ResolveQueries_).As<IQueries>().InstancePerHttpRequest();
            }
            else {
                builder.RegisterType<Queries>().As<IQueries>().SingleInstance();
            }
        }

        static IQueries ResolveQueries_(IComponentContext context)
        {
            return new CachedQueries(
                new Queries(context.Resolve<ChiffonConfig>()),
                context.Resolve<IQueryCache>());
        }
    }
}

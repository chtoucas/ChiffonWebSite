namespace Chiffon
{
    using Autofac;
    using Autofac.Integration.Mvc;
    using Chiffon.Data;
    using Chiffon.Infrastructure;
    using Narvalo;

    public class DataModule : Module
    {
        readonly ChiffonConfig _config;

        public DataModule(ChiffonConfig config)
        {
            Requires.NotNull(config, "config");

            _config = config;
        }

        protected override void Load(ContainerBuilder builder)
        {
            Requires.NotNull(builder, "builder");

            builder.RegisterType<QueryCache>().As<IQueryCache>().InstancePerHttpRequest();

            if (_config.EnableServerCache) {
                builder.Register(ResolveReadQueries_).As<IReadQueries>().InstancePerHttpRequest();
            }
            else {
                builder.Register(ResolveReadQueriesNoCache_).As<IReadQueries>().SingleInstance();
            }

            builder.Register(ResolveWriteQueries_).As<IWriteQueries>().SingleInstance();
        }

        static IReadQueries ResolveReadQueries_(IComponentContext context)
        {
            return new CachedReadQueries(
                new ReadQueries(context.Resolve<ChiffonConfig>().SqlConnectionString),
                context.Resolve<IQueryCache>());
        }

        static IReadQueries ResolveReadQueriesNoCache_(IComponentContext context)
        {
            return new ReadQueries(context.Resolve<ChiffonConfig>().SqlConnectionString);
        }

        static IWriteQueries ResolveWriteQueries_(IComponentContext context)
        {
            return new WriteQueries(context.Resolve<ChiffonConfig>().SqlConnectionString);
        }
    }
}

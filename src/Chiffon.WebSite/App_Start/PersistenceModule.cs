namespace Chiffon
{
    using Autofac;
    using Autofac.Integration.Mvc;
    using Chiffon.Persistence;
    using Chiffon.Infrastructure;
    using Narvalo;

    public class PersistenceModule : Module
    {
        readonly ChiffonConfig _config;

        public PersistenceModule(ChiffonConfig config)
        {
            Requires.NotNull(config, "config");

            _config = config;
        }

        protected override void Load(ContainerBuilder builder)
        {
            Requires.NotNull(builder, "builder");

            builder.RegisterType<DbQueryCache>().As<IDbQueryCache>().InstancePerHttpRequest();

            if (_config.EnableServerCache) {
                builder.Register(ResolveQueries_).As<IDbQueries>().InstancePerHttpRequest();
            }
            else {
                builder.Register(ResolveQueriesNoCache_).As<IDbQueries>().SingleInstance();
            }

            builder.Register(ResolveCommands_).As<IDbCommands>().SingleInstance();
        }

        static IDbQueries ResolveQueries_(IComponentContext context)
        {
            return new CachedDbQueries(
                new DbQueries(context.Resolve<ChiffonConfig>().SqlConnectionString),
                context.Resolve<IDbQueryCache>());
        }

        static IDbQueries ResolveQueriesNoCache_(IComponentContext context)
        {
            return new DbQueries(context.Resolve<ChiffonConfig>().SqlConnectionString);
        }

        static IDbCommands ResolveCommands_(IComponentContext context)
        {
            return new DbCommands(context.Resolve<ChiffonConfig>().SqlConnectionString);
        }
    }
}

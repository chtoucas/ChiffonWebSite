﻿namespace Chiffon
{
    using Autofac;
    using Chiffon.Infrastructure.Persistence;
    using Chiffon.Infrastructure;
    using Narvalo;

    public class PersistenceModule : Module
    {
        private readonly ChiffonConfig _config;

        public PersistenceModule(ChiffonConfig config)
        {
            Require.NotNull(config, "config");

            _config = config;
        }

        protected override void Load(ContainerBuilder builder)
        {
            Promise.NotNull(builder, "The base class guarantees that this parameter is not null.");

            builder.RegisterType<DbQueryCache>().As<IDbQueryCache>().InstancePerRequest();

            if (_config.EnableServerCache) {
                builder.Register(ResolveQueries_).As<IDbQueries>().InstancePerRequest();
            }
            else {
                builder.Register(ResolveQueriesNoCache_).As<IDbQueries>().SingleInstance();
            }

            builder.Register(ResolveCommands_).As<IDbCommands>().SingleInstance();
        }

        private static IDbQueries ResolveQueries_(IComponentContext context)
        {
            return new CachedDbQueries(
                new DbQueries(context.Resolve<ChiffonConfig>().SqlConnectionString),
                context.Resolve<IDbQueryCache>());
        }

        private static IDbQueries ResolveQueriesNoCache_(IComponentContext context)
        {
            return new DbQueries(context.Resolve<ChiffonConfig>().SqlConnectionString);
        }

        private static IDbCommands ResolveCommands_(IComponentContext context)
        {
            return new DbCommands(context.Resolve<ChiffonConfig>().SqlConnectionString);
        }
    }
}

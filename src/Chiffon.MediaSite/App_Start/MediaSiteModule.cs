﻿namespace Chiffon
{
    using Autofac;
    using Autofac.Integration.Mvc;
    using Chiffon.Infrastructure;
    using Chiffon.Persistence;
    using Narvalo;
    using Narvalo.Web;

    public class MediaSiteModule : Module
    {
        readonly ChiffonConfig _config;

        public MediaSiteModule(ChiffonConfig config)
        {
            Require.NotNull(config, "config");

            _config = config;
        }

        protected override void Load(ContainerBuilder builder)
        {
            Require.NotNull(builder, "builder");

            // > Infrastructure <

            builder.Register(_ => _config).AsSelf().SingleInstance();

            builder.RegisterType<DbQueryCache>().As<IDbQueryCache>().InstancePerRequest();

            // > Persistence <

            if (_config.EnableServerCache) {
                builder.Register(ResolveQueries_).As<IDbQueries>().InstancePerRequest();
            }
            else {
                builder.RegisterType<DbQueries>().As<IDbQueries>().SingleInstance();
            }

            builder.RegisterHandlers(typeof(Global).Assembly);
        }

        static IDbQueries ResolveQueries_(IComponentContext context)
        {
            return new CachedDbQueries(
               new DbQueries(context.Resolve<ChiffonConfig>().SqlConnectionString),
               context.Resolve<IDbQueryCache>());
        }
    }
}

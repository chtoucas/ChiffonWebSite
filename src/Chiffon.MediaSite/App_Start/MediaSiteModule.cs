namespace Chiffon
{
    using Autofac;
    using Autofac.Integration.Mvc;
    using Chiffon.Data;
    using Chiffon.Infrastructure;
    using Narvalo;

    public class MediaSiteModule : Module
    {
        readonly ChiffonConfig _config;

        public MediaSiteModule(ChiffonConfig config)
        {
            Requires.NotNull(config, "config");

            _config = config;
        }

        protected override void Load(ContainerBuilder builder)
        {
            Requires.NotNull(builder, "builder");

            // > Infrastructure <

            builder.Register(_ => _config).AsSelf().SingleInstance();

            builder.RegisterType<DbQueryCache>().As<IDbQueryCache>().InstancePerHttpRequest();

            // > Data <

            if (_config.EnableServerCache) {
                builder.Register(ResolveQueries_).As<IDbQueries>().InstancePerHttpRequest();
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

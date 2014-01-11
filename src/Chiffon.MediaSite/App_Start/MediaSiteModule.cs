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

            builder.RegisterType<QueryCache>().As<IQueryCache>().InstancePerHttpRequest();

            // > Data <

            if (_config.EnableServerCache) {
                builder.Register(ResolveQueries_).As<IQueries>().InstancePerHttpRequest();
            }
            else {
                builder.RegisterType<Queries>().As<IQueries>().SingleInstance();
            }

            builder.RegisterHandlers(typeof(Global).Assembly);
        }

        static IQueries ResolveQueries_(IComponentContext context)
        {
            return new CachedQueries(
               new Queries(context.Resolve<ChiffonConfig>().SqlConnectionString),
               context.Resolve<IQueryCache>());
        }
    }
}

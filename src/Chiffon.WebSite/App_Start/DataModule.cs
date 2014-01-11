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
                builder.Register(ResolveQueries_).As<IQueries>().InstancePerHttpRequest();
            }
            else {
                builder.Register(ResolveQueriesNoCache_).As<IQueries>().SingleInstance();
            }

            builder.Register(ResolveCommands_).As<ICommands>().SingleInstance();
        }

        static IQueries ResolveQueries_(IComponentContext context)
        {
            return new CachedQueries(
                new Queries(context.Resolve<ChiffonConfig>().SqlConnectionString),
                context.Resolve<IQueryCache>());
        }

        static IQueries ResolveQueriesNoCache_(IComponentContext context)
        {
            return new Queries(context.Resolve<ChiffonConfig>().SqlConnectionString);
        }

        static ICommands ResolveCommands_(IComponentContext context)
        {
            return new Commands(context.Resolve<ChiffonConfig>().SqlConnectionString);
        }
    }
}

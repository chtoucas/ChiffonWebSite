namespace Chiffon
{
    using Autofac;
    using Autofac.Integration.Mvc;
    using Chiffon.Data;
    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Addressing;
    using Chiffon.Services;
    using Narvalo;
    using Narvalo.Web.Security;

    public class WebSiteModule : Module
    {
        readonly ChiffonConfig _config;

        public WebSiteModule(ChiffonConfig config)
        {
            Requires.NotNull(config, "config");

            _config = config;
        }

        protected override void Load(ContainerBuilder builder)
        {
            Requires.NotNull(builder, "builder");

            // > Infrastructure <

            builder.Register(_ => _config).AsSelf().SingleInstance();

            // IMPORTANT: ChiffonEnvironment est entièrement résolue à l'exécution.
            // Cf. aussi les commentaires dans la classe ChiffonRuntime.
            builder.Register(_ => ChiffonRuntime.Environment).AsSelf().InstancePerHttpRequest();

            builder.RegisterType<DefaultSiteMapFactory>().As<ISiteMapFactory>().SingleInstance();
            // IMPORTANT: ISiteMap est entièrement résolue à l'exécution.
            builder.Register(ResolveSiteMap_).As<ISiteMap>().InstancePerHttpRequest();

            builder.RegisterType<QueryCache>().As<IQueryCache>().InstancePerHttpRequest();

            // > Data <

            if (_config.EnableServerCache) {
                builder.Register(ResolveQueries_).As<IQueries>().InstancePerHttpRequest();
            }
            else {
                builder.RegisterType<Queries>().As<IQueries>().SingleInstance();
            }

            // > Services <

            builder.RegisterType<FormsAuthenticationService>().As<IFormsAuthenticationService>().SingleInstance();
            builder.RegisterType<MemberService>().As<IMemberService>().SingleInstance();
            // NB: On utilise InstancePerHttpRequest car PatternService dépend d'IQueries.
            builder.RegisterType<PatternService>().As<IPatternService>().InstancePerHttpRequest();

            // > MVC <

            builder.RegisterControllers(typeof(Global).Assembly);
            builder.RegisterHandlers(typeof(Global).Assembly);
        }

        static ISiteMap ResolveSiteMap_(IComponentContext context)
        {
            return context.Resolve<ISiteMapFactory>().CreateMap(context.Resolve<ChiffonEnvironment>());
        }

        static IQueries ResolveQueries_(IComponentContext context)
        {
            return new CachedQueries(
                new Queries(context.Resolve<ChiffonConfig>()),
                context.Resolve<IQueryCache>());
        }
    }
}

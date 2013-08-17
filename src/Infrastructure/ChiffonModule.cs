namespace Chiffon.Infrastructure
{
    using Autofac;
    using Autofac.Integration.Mvc;
    using Chiffon.Data;
    using Chiffon.Infrastructure.Addressing;
    using Chiffon.Services;
    using Narvalo;
    using Narvalo.Web.Security;

    public class ChiffonModule : Module
    {
        readonly ChiffonConfig _config;

        public ChiffonModule(ChiffonConfig config)
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

            // > Data <

            builder.RegisterType<EntityQueries>().As<IEntityQueries>().SingleInstance();
            builder.RegisterType<ViewModelQueries>().As<IViewModelQueries>().SingleInstance();
            // FIXME
            //builder.RegisterType<WebQueryCache>().As<IQueryCache>().InstancePerHttpRequest();

            // > Services <

            builder.RegisterType<FormsAuthenticationService>().As<IFormsAuthenticationService>().SingleInstance();
            builder.RegisterType<MemberService>().As<IMemberService>().SingleInstance();

            // > MVC <

            builder.RegisterControllers(typeof(Global).Assembly);
            builder.RegisterHandlers(typeof(Global).Assembly);
        }

        static ISiteMap ResolveSiteMap_(IComponentContext context)
        {
            return context.Resolve<ISiteMapFactory>().CreateMap(context.Resolve<ChiffonEnvironment>());
        }
    }
}

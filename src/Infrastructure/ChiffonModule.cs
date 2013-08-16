namespace Chiffon.Infrastructure
{
    using Autofac;
    using Autofac.Integration.Mvc;
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

            builder.Register(_ => _config).AsSelf().SingleInstance();
            builder.RegisterType<DbHelper>().AsSelf().SingleInstance();

            // IMPORTANT: Cette classe est entièrement résolue à l'exécution.
            // Cf. aussi les commentaires dans la classe ChiffonRuntime.
            builder.Register(_ => ChiffonRuntime.Environment).AsSelf().InstancePerHttpRequest();

            builder.RegisterType<DefaultSiteMapFactory>().As<ISiteMapFactory>().SingleInstance();
            // IMPORTANT: Cette classe est entièrement résolue à l'exécution.
            builder.Register(ResolveSiteMap_).As<ISiteMap>().InstancePerHttpRequest();

            builder.RegisterType<FormsAuthenticationService>().As<IFormsAuthenticationService>().SingleInstance();
            builder.RegisterType<MemberService>().As<IMemberService>().SingleInstance();

            builder.RegisterControllers(typeof(Global).Assembly);
            builder.RegisterHandlers(typeof(Global).Assembly);
        }

        static ISiteMap ResolveSiteMap_(IComponentContext context)
        {
            return context.Resolve<ISiteMapFactory>().CreateMap(context.Resolve<ChiffonEnvironment>());
        }
    }
}

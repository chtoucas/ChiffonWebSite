namespace Chiffon.Modules
{
    using Autofac;
    using Autofac.Integration.Mvc;
    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Addressing;
    using Chiffon.Mail;
    using Narvalo;

    public class InfrastructureModule : Module
    {
        readonly ChiffonConfig _config;

        public InfrastructureModule(ChiffonConfig config)
        {
            Requires.NotNull(config, "config");

            _config = config;
        }

        protected override void Load(ContainerBuilder builder)
        {
            Requires.NotNull(builder, "builder");

            builder.Register(_ => _config).AsSelf().SingleInstance();

            builder.RegisterType<MailMerge>().AsSelf().InstancePerHttpRequest();

            builder.RegisterType<DefaultSiteMapFactory>().As<ISiteMapFactory>().SingleInstance();
            // IMPORTANT: ISiteMap est entièrement résolue à l'exécution.
            builder.Register(ResolveSiteMap_).As<ISiteMap>().InstancePerHttpRequest();

            // IMPORTANT: ChiffonEnvironment est entièrement résolue à l'exécution.
            // Cf. aussi les commentaires dans la classe ChiffonContext.
            builder.Register(_ => ChiffonContext.Current.Environment).AsSelf().InstancePerHttpRequest();
        }

        static ISiteMap ResolveSiteMap_(IComponentContext context)
        {
            return context.Resolve<ISiteMapFactory>().CreateMap(context.Resolve<ChiffonEnvironment>());
        }
    }
}

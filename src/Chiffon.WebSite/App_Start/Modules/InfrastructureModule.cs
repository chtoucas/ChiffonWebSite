namespace Chiffon.Modules
{
    using Autofac;
    using Autofac.Integration.Mvc;
    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Addressing;
    using Chiffon.Infrastructure.Messaging;
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

            // FIXME: Réactiver l'injection de ISmtpClient.
            //builder.RegisterType<SmtpClientProxy>().As<ISmtpClient>().InstancePerHttpRequest();
            builder.RegisterType<MailMerge>().As<IMailMerge>().InstancePerHttpRequest();
            builder.RegisterType<Messenger>().As<IMessenger>().InstancePerHttpRequest();

            builder.RegisterType<DefaultSiteMapFactory>().As<ISiteMapFactory>().SingleInstance();
            // FIXME: Pour les HttpHandlers, je n'arrive pas à voir pour le moment pourquoi
            // on ne récupère pas la bonne valeur de ChiffonEnvironment et donc de SiteMap, même
            // en précisant IsReusable = false. Peut-être en précisant InstancePerHttpRequest()
            // au niveau de RegisterHandlers() dans AspNetMvcModule ?
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

namespace Chiffon
{
    using Autofac;
    using Chiffon.Common;
    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Addressing;
    using Chiffon.Infrastructure.Messaging;
    using Narvalo;

    public class InfrastructureModule : Module
    {
        private readonly ChiffonConfig _config;

        public InfrastructureModule(ChiffonConfig config)
        {
            Require.NotNull(config, "config");

            _config = config;
        }

        protected override void Load(ContainerBuilder builder)
        {
            Promise.NotNull(builder, "The base class guarantees that this parameter is not null.");

            builder.Register(_ => _config).AsSelf().SingleInstance();

            // FIXME: Réactiver l'injection de ISmtpClient.
            //builder.RegisterType<SmtpClientProxy>().As<ISmtpClient>().InstancePerHttpRequest();
            builder.RegisterType<MailMerge>().As<IMailMerge>().InstancePerRequest();
            builder.RegisterType<Messenger>().As<IMessenger>().InstancePerRequest();

            builder.RegisterType<DefaultSiteMapFactory>().As<ISiteMapFactory>().SingleInstance();
            // FIXME: Pour les HttpHandlers, je n'arrive pas à voir pour le moment pourquoi
            // on ne récupère pas la bonne valeur de ChiffonEnvironment et donc de SiteMap, même
            // en précisant IsReusable = false. Peut-être en précisant InstancePerHttpRequest()
            // au niveau de RegisterHandlers() dans AspNetMvcModule ?
            // IMPORTANT: ISiteMap est entièrement résolue à l'exécution.
            builder.Register(ResolveSiteMap_).As<ISiteMap>().InstancePerRequest();

            // IMPORTANT: ChiffonEnvironment est entièrement résolue à l'exécution.
            // Cf. aussi les commentaires dans la classe ChiffonContext.
            builder.Register(_ => ChiffonContext.Current.Environment).AsSelf().InstancePerRequest();
        }

        static ISiteMap ResolveSiteMap_(IComponentContext context)
        {
            return context.Resolve<ISiteMapFactory>().CreateMap(context.Resolve<ChiffonEnvironment>());
        }
    }
}

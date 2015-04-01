namespace Chiffon.Common
{
    using System.Diagnostics.CodeAnalysis;

    using Autofac;
    using Autofac.Integration.Mvc;
    using Chiffon.Internal;
    using Chiffon.Persistence;
    using Chiffon.Services;
    using Narvalo;

    public class ApplicationContainer : Module
    {
        private readonly ChiffonConfig _config;

        public ApplicationContainer(ChiffonConfig config)
        {
            Require.NotNull(config, "config");

            _config = config;
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(_ => _config).AsSelf().SingleInstance();

            // FIXME: Réactiver l'injection de ISmtpClient.
            //builder.RegisterType<SmtpClientProxy>().As<ISmtpClient>().InstancePerHttpRequest();
            builder.RegisterType<MailMerge>().As<IMailMerge>().InstancePerRequest();
            builder.RegisterType<Messenger>().As<IMessenger>().InstancePerRequest();

            builder.RegisterType<SiteMapFactory>().As<ISiteMapFactory>().SingleInstance();
            // FIXME: Pour les HttpHandlers, je n'arrive pas à voir pour le moment pourquoi
            // on ne récupère pas la bonne valeur de ChiffonEnvironment et donc de SiteMap, même
            // en précisant IsReusable = false. Peut-être en précisant InstancePerHttpRequest()
            // au niveau de RegisterHandlers() dans AspNetMvcModule ?
            // IMPORTANT: ISiteMap est entièrement résolue à l'exécution.
            builder.Register(ResolveSiteMap_).As<ISiteMap>().InstancePerRequest();

            // IMPORTANT: ChiffonEnvironment est entièrement résolue à l'exécution.
            // Cf. aussi les commentaires dans la classe ChiffonContext.
            builder.Register(_ => ChiffonContext.Current.Environment).AsSelf().InstancePerRequest();

            builder.RegisterType<DbQueryCache>().As<IDbQueryCache>().InstancePerRequest();

            if (_config.EnableServerCache)
            {
                builder.Register(ResolveQueries_).As<IDbQueries>().InstancePerRequest();
            }
            else
            {
                builder.Register(ResolveQueriesNoCache_).As<IDbQueries>().SingleInstance();
            }

            builder.Register(ResolveCommands_).As<IDbCommands>().SingleInstance();

            // NB: On utilise InstancePerHttpRequest car MemberService dépend d'ISiteMap.
            builder.RegisterType<MemberService>().As<IMemberService>().InstancePerRequest();
            // NB: On utilise InstancePerHttpRequest car PatternService dépend d'IDbQueries.
            builder.RegisterType<PatternService>().As<IPatternService>().InstancePerRequest();

            // Composants Asp.Net MVC.
            builder.RegisterControllers(typeof(Application).Assembly);
            builder.RegisterHandlers(typeof(Application).Assembly);
            // FIXME: Je n'arrive pas à faire fonctionner la ligne suivante...
            //builder.RegisterSource(new ViewRegistrationSource());
        }

        private static IDbCommands ResolveCommands_(IComponentContext context)
        {
            return new DbCommands(context.Resolve<ChiffonConfig>().SqlConnectionString);
        }

        private static IDbQueries ResolveQueries_(IComponentContext context)
        {
            return new CachedDbQueries(
                new DbQueries(context.Resolve<ChiffonConfig>().SqlConnectionString),
                context.Resolve<IDbQueryCache>());
        }

        private static IDbQueries ResolveQueriesNoCache_(IComponentContext context)
        {
            return new DbQueries(context.Resolve<ChiffonConfig>().SqlConnectionString);
        }

        private static ISiteMap ResolveSiteMap_(IComponentContext context)
        {
            return context.Resolve<ISiteMapFactory>().CreateMap(context.Resolve<ChiffonEnvironment>());
        }
    }
}

namespace Chiffon
{
    using System;
    using System.ComponentModel.Composition.Hosting;
    using System.Web;

    using Autofac;
    using Autofac.Integration.Mvc;
    using Chiffon.Common;
    using Chiffon.Internal;
    using Chiffon.Persistence;
    using Chiffon.Services;
    using Narvalo;
    using Serilog;

    public static class Dependencies
    {
        [CLSCompliant(false)]
        public static ILogger GetLogger(ChiffonConfig config)
        {
            Require.NotNull(config, "config");

            ILoggerProvider provider;

            using (var catalog = new AssemblyCatalog(typeof(ApplicationLifecycle).Assembly))
            {
                using (var container = new CompositionContainer(catalog))
                {
                    provider = container.GetExportedValue<ILoggerProvider>(config.LogProfile);
                }
            }

            return provider.GetLogger(config.LogMinimumLevel);
        }

        public static void Load(ContainerBuilder builder, ChiffonConfig config)
        {
            Require.NotNull(builder, "builder");
            Require.NotNull(config, "config");

            builder.Register(_ => config).AsSelf().SingleInstance();

            builder.RegisterType<MailMerge>().As<IMailMerge>().InstancePerRequest();
            builder.RegisterType<Messenger>().As<IMessenger>().InstancePerRequest();

            builder.RegisterType<SiteMapFactory>().As<ISiteMapFactory>().SingleInstance();
            // FIXME: Pour les HttpHandlers, je n'arrive pas à voir pour le moment pourquoi
            // on ne récupère pas la bonne valeur de ChiffonEnvironment et donc de SiteMap, même
            // en précisant IsReusable = false. Peut-être faut-il utiliser InstancePerHttpRequest()
            // au niveau de RegisterHandlers() dans AspNetMvcModule ?
            // IMPORTANT: ISiteMap est entièrement résolue à l'exécution.
            builder.Register(ResolveSiteMap_).As<ISiteMap>().InstancePerRequest();

            // IMPORTANT: ChiffonEnvironment est entièrement résolue à l'exécution.
            // Cf. aussi les commentaires dans la classe ChiffonContext.
            builder.Register(_ => ChiffonContext.Resolve(HttpContext.Current).Environment).AsSelf().InstancePerRequest();

            RegisterPersistenceTypes_(builder, config.EnableServerCache);
            RegisterServiceTypes_(builder);
            RegisterAspNetMvcTypes_(builder);
        }

        private static void RegisterPersistenceTypes_(ContainerBuilder builder, bool enableServerCache)
        {
            builder.RegisterType<DbQueryCache>().As<IDbQueryCache>().InstancePerRequest();

            if (enableServerCache)
            {
                builder.Register(ResolveQueries_).As<IDbQueries>().InstancePerRequest();
            }
            else
            {
                builder.Register(ResolveQueriesNoCache_).As<IDbQueries>().SingleInstance();
            }

            builder.Register(ResolveCommands_).As<IDbCommands>().SingleInstance();
        }

        private static void RegisterServiceTypes_(ContainerBuilder builder)
        {
            // NB: On utilise InstancePerHttpRequest car MemberService dépend d'ISiteMap.
            builder.RegisterType<MemberService>().As<IMemberService>().InstancePerRequest();
            // NB: On utilise InstancePerHttpRequest car PatternService dépend d'IDbQueries.
            builder.RegisterType<PatternService>().As<IPatternService>().InstancePerRequest();
        }

        private static void RegisterAspNetMvcTypes_(ContainerBuilder builder)
        {
            // Composants Asp.Net MVC.
            builder.RegisterControllers(typeof(ApplicationLifecycle).Assembly);
            builder.RegisterHandlers(typeof(ApplicationLifecycle).Assembly);
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

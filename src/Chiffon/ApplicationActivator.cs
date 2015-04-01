using Chiffon;
using WebActivatorEx;

// WARNING: Cet attribut ne peut être utilisé qu'une fois par assemblée.
//[assembly: System.Web.PreApplicationStartMethod(typeof(ApplicationActivator), "PreStart")]
//[assembly: PreApplicationStartMethod(typeof(System.Web.Mvc.PreApplicationStartCode), "Start")]

[assembly: PreApplicationStartMethod(typeof(ApplicationActivator), "PreStart")]
[assembly: PostApplicationStartMethod(typeof(ApplicationActivator), "PostStart")]
[assembly: ApplicationShutdownMethod(typeof(ApplicationActivator), "Shutdown")]

namespace Chiffon
{
    using System.Web.Mvc;
    using System.Web.Routing;

    using Autofac;
    using Autofac.Integration.Mvc;
    using Chiffon.Common;
    using Chiffon.Controllers;
    using Chiffon.Entities;
    using Narvalo.Web.Optimization;
    using Serilog;

    public static class ApplicationActivator
    {
        public static void PreStart()
        {
            // Chargement de la configuration.
            var config = ChiffonConfig.FromConfiguration();

            // Résolution des dépendances (Autofac).
            var builder = new ContainerBuilder();
            builder.RegisterModule(new ApplicationContainer(config));
            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // Configuration du logger (Serilog).
            (new LogConfig(config)).Configure();

            // Modules HTTP.
            ApplicationLifecycleModule.Register();
            InitializeContextModule.Register();

            // Suppression de l'en-tête "X-AspNetMvc-Version".
            MvcHandler.DisableMvcResponseHeader = true;

            // Optimisation du contenu HTML (ASP.NET WebForm).
            WhiteSpaceBusterProvider.Current.PageBuster = new UnsafeWhiteSpaceBuster();
        }

        public static void Start()
        {
            Log.Information("Application starting.");

            new ApplicationRouting(RouteTable.Routes).Configure();
            ModelBinders.Binders.Add(typeof(DesignerKey), new DesignerKeyModelBinder());

            // La gestion des erreurs est déléguée à customErrors pour avoir un contrôle
            // plus fin des codes HTTP de réponse et du message affiché (en anglais ou en français).
            // Si on réactive le filtre HandleError, il faut aussi créer une vue ~/Views/Shared/Error.cshml.
            //GlobalFilters.Filters.Add(new HandleErrorAttribute());

            ResetViewEngines_();
        }

        public static void PostStart()
        {
            // Laissé vide intentionnellement.
        }

        public static void End()
        {
            Log.Information("Application ending.");
        }

        /// <summary>
        /// Méthode exécutée après le déchargement du dernier module HTTP. 
        /// </summary>
        public static void Shutdown()
        {
            // Laissé vide intentionnellement.
        }

        private static void ResetViewEngines_()
        {
            // On ne garde que Razor.
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
        }
    }
}

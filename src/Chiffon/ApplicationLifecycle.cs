using Chiffon;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(ApplicationLifecycle), "PreStart")]
[assembly: PostApplicationStartMethod(typeof(ApplicationLifecycle), "PostStart")]

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

    public static class ApplicationLifecycle
    {
        public static void PreStart()
        {
            // Enregistrement des modules HTTP.
            ApplicationLifecycleModule.Register();
            InitializeContextModule.Register();

            // On ne garde que le moteur Razor.
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            // Suppression de l'en-tête "X-AspNetMvc-Version".
            MvcHandler.DisableMvcResponseHeader = true;

            // Optimisation du contenu HTML (ASP.NET WebForm).
            WhiteSpaceBusterProvider.Current.PageBuster = new UnsafeWhiteSpaceBuster();
        }

        public static void Start()
        {
            // Chargement de la configuration.
            var config = ChiffonConfig.FromConfiguration();

            // Configuration de l'injection de dépendances.
            var builder = new ContainerBuilder();
            Dependencies.Load(builder, config);
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // Configuration de Serilog.
            Log.Logger = Dependencies.GetLogger(config);
        }

        public static void PostStart()
        {
            ModelBinders.Binders.Add(typeof(DesignerKey), new DesignerKeyModelBinder());

            Routes.Register(RouteTable.Routes);

            // La gestion des erreurs est déléguée à customErrors pour avoir un contrôle
            // plus fin des codes HTTP de réponse et du message affiché (en anglais ou en français).
            // Si on réactive le filtre HandleError, il faut aussi créer une vue ~/Views/Shared/Error.cshml.
            //GlobalFilters.Filters.Add(new HandleErrorAttribute());
        }
    }
}

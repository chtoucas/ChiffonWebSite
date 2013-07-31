namespace Chiffon.WebSite
{
    using System;
    using System.Net;
    //using System.Reflection;
    using System.Web;
    //using System.Web.Http;
    using System.Web.Mvc;
    //using System.Web.Optimization;
    using System.Web.Routing;
    using Chiffon.Crosscuttings;
    using Chiffon.WebSite.Configs;
    using Chiffon.WebSite.Resources;
    using Narvalo;
    using Narvalo.Diagnostics;
    using Narvalo.Web;

    /// <summary>
    /// Au cours de son cycle de vie, l'application déclenche des événements que vous pouvez 
    /// gérer et appelle des méthodes spécifiques que vous pouvez substituer. Pour gérer des 
    /// événements ou des méthodes d'application, vous pouvez créer un fichier nommé Global.asax
    /// dans le répertoire racine de votre application.
    /// 
    /// Si vous créez un fichier Global.asax, ASP.NET le compile en une classe dérivée de la 
    /// classe HttpApplication, puis utilise la classe dérivée pour représenter 
    /// l'application.
    /// 
    /// Une instance de HttpApplication ne traite qu'une demande à la fois. Cela simplifie la 
    /// gestion d'événements d'application, puisque vous n'avez pas besoin de verrouiller les
    /// membres non statiques de la classe d'application lorsque vous y accédez. Cela vous permet 
    /// également de stocker des données spécifiques à la demande dans les membres non statiques 
    /// de la classe d'application. Par exemple, vous pouvez définir une propriété dans le fichier 
    /// Global.asax et lui assigner une valeur spécifique à la demande.
    ///
    /// ASP.NET lie automatiquement des événements d'application à des gestionnaires dans 
    /// le fichier Global.asax à l'aide de la convention d'affectation de noms Application_événement, 
    /// par exemple Application_BeginRequest. Cela est comparable à la façon dont les méthodes 
    /// de page ASP.NET sont automatiquement liées à des événements, comme l'événement Page_Load 
    /// de la page.Pour plus d'informations, consultez Vue d'ensemble du cycle de vie des pages ASP.NET.
    ///
    /// Les méthodes Application_Start et Application_End sont des méthodes spéciales qui ne 
    /// représentent pas d'événements HttpApplication. ASP.NET les appelle une fois dans toute 
    /// la durée de vie du domaine d'application, et non pour chaque instance de HttpApplication.
    /// 
    /// Une méthode Application_événement est déclenché au moment approprié du cycle de vie 
    /// de l'application, comme le montre la table des cycles de vie de l'application, 
    /// vue précédemment dans cette rubrique.
    ///
    /// Application_Error peut être déclenché à tout moment du cycle de vie de l'application.
    ///
    /// Application_EndRequest est le seul événement qui sera déclenché à coup sûr à chaque demande, 
    /// parce qu'une demande peut être court-circuitée. Par exemple, si deux modules gèrent 
    /// l'événement Application_BeginRequest et si le premier lève une exception, l'événement 
    /// Application_BeginRequest ne sera pas appelé pour le deuxième module. Toutefois, 
    /// la méthode Application_EndRequest est toujours appelée pour autoriser l'application à nettoyer 
    /// les ressources.
    /// 
    /// Pour les détails concernant HttpApplication, on se reportera à
    /// http://msdn.microsoft.com/fr-fr/library/system.web.httpapplication%28v=vs.100%29.aspx
    /// Pour comprendre le cycle de vie de HttpApplication,
    /// http://msdn.microsoft.com/en-us/library/ms178473%28v=vs.100%29.aspx
    /// </summary>
    public class Global : HttpApplication
    {
        //static readonly string AssemblyVersion_
        //    = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        static ILogger Logger_ = Logger.Create(typeof(Global));

        public Global()
            : base()
        {
            // FIXME: est-ce vraiment nécessaire ?
            Disposed += Application_Disposed;
            Error += Application_Error;
        }

        //        /// <summary>
        //        /// Retourne la Version de l'assemblée Chiffon.WebSite.
        //        /// </summary>
        //        public static string Version
        //        {
        //            get { return AssemblyVersion_; }
        //        }

        //        /// <summary>
        //        /// Retourne true si l'assemblée Chiffon.WebSite a été compilée en mode Debug.
        //        /// </summary>
        //        /// <remarks>
        //        /// Pour savoir si les pages AspNet ont été compilées en mode Debug, il faut plutôt
        //        /// utiliser <c>HttpContext.Current.IsDebuggingEnable</c>.
        //        /// </remarks>
        //        public static bool Debug
        //        {
        //            // WARNING: tout ce qui n'est pas Debug est assimilé à Release.
        //            get
        //            {
        //#if DEBUG
        //                return true;
        //#else
        //                return false;
        //#endif
        //            }
        //        }

        #region Méthodes spéciales appelées une seule fois dans toute la durée de vie du domaine d'application.

        /// <summary>
        /// Appelé lorsque la première ressource (par exemple une page) d'une application ASP.NET
        /// est demandée. La méthode Application_Start n'est appelée qu'une fois au cours du cycle 
        /// de vie d'une application.Vous pouvez utiliser cette méthode pour effectuer des tâches 
        /// de démarrage telles que le chargement de données dans le cache ou l'initialisation de 
        /// valeurs statiques.
        ///
        /// Les données statiques ne doivent être définies qu'au démarrage de l'application.
        /// Ne définissez pas de données d'instance parce qu'elles ne seront disponibles que pour 
        /// la première instance de la classe HttpApplication créée.
        /// 
        /// NB: Pour exécuter un code avant Application_Start, utiliser les facilités offertes par
        /// l'attribut PreApplicationStartMethod. Voir aussi WebActivator.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Start()
        {
            Log_(LoggerLevel.Informational, () => { return SR.Global_Starting; });

            // Filters.
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            // Routes.
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            // WebApi.
            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            // Injection de dépendances.
            DependencyConfig.RegisterDependencies(ChiffonConfig.Create());
        }

        /// <summary>
        /// Appelé une fois dans la durée de vie de l'application avant que celle-ci ne soit déchargée. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_End(object sender, EventArgs e)
        {
            Log_(LoggerLevel.Informational, () => { return SR.Global_Ending; });
        }

        #endregion

        ///// <summary>
        ///// Exécute un code d'initialisation personnalisé une fois que tous les modules ont été créés.
        ///// Demandé une fois pour chaque instance de la classe HttpApplication.
        ///// </summary>
        //public override void Init()
        //{
        //    base.Init();
        //}

        #region Événements

        //protected void Application_BeginRequest(object sender, EventArgs e)
        //{
        //    using (var fakePage = new Page()) {
        //        var ignored = fakePage.Server;
        //        fakePage.Culture = "auto:fr-FR";
        //        fakePage.UICulture = "auto:fr-FR";
        //    }
        //}

        //protected void Application_EndRequest(object sender, EventArgs e)
        //{
        //    // Dispose Request specific Autofac container
        //    ContainerProvider.EndRequestLifetime();
        //    // or
        //    ContainerProvider.DisposeRequestContainer();
        //}

        /// <summary>
        /// Se produit lorsque l'application est supprimée.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Disposed(object sender, EventArgs e)
        {
            Log_(LoggerLevel.Informational, () => { return SR.Global_Disposed; });
        }

        /// <summary>
        /// Se produit lorsqu'une exception non gérée est levée.
        /// NB: Application_Error peut être déclenché à tout moment du cycle de vie de l'application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
            var server = app.Server;

            var ex = server.GetLastError();
            if (ex == null) {
                // En théorie, cela ne devrait jamais se produire.
                Log_(LoggerLevel.Critical, () => SR.Global_UnknownError);
                return;
            }

            var err = HttpApplicationUtility.GetUnhandledErrorType(ex);

            switch (err) {
                case UnhandledErrorType.InvalidViewState:
                    Log_(LoggerLevel.Warning, ex);
                    server.ClearError();
                    SetResponseStatus_(app, HttpStatusCode.ServiceUnavailable);
                    break;

                case UnhandledErrorType.PotentiallyDangerousForm:
                case UnhandledErrorType.PotentiallyDangerousPath:
                    Log_(LoggerLevel.Warning, ex);
                    server.ClearError();
                    SetResponseStatus_(app, HttpStatusCode.NotFound);
                    break;

                case UnhandledErrorType.NotFound:
                    // NB: on laisse IIS prendre en charge ce type d'erreur.
                    Log_(LoggerLevel.Debug, ex);
                    break;

                case UnhandledErrorType.Unknown:
                default:
                    Log_(LoggerLevel.Critical, ex);
                    break;
            }
        }

        #endregion

        static void Log_(LoggerLevel level, Exception ex)
        {
            if (Logger_ != null) {
                Logger_.Log(level, ex);
            }
        }

        static void Log_(LoggerLevel level, Func<string> messageFactory)
        {
            Requires.NotNull(messageFactory, "messageFactory");

            if (Logger_ != null) {
                Logger_.Log(level, messageFactory);
            }
        }

        static void SetResponseStatus_(HttpApplication app, HttpStatusCode statusCode)
        {
            var response = app.Response;
            if (response != null) {
                response.SetStatusCode(statusCode);
            }
        }
    }
}
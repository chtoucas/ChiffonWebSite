namespace Chiffon
{
    using Autofac;
    using Autofac.Integration.Mvc;
    using Chiffon.Infrastructure;
    using Narvalo;

    public class WebAutofacModule : Module
    {
        public WebAutofacModule() { }

        protected override void Load(ContainerBuilder builder)
        {
            Requires.NotNull(builder, "builder");

            // IMPORTANT: ChiffonEnvironment est entièrement résolue à l'exécution.
            // Cf. aussi les commentaires dans la classe ChiffonContext.
            builder.Register(_ => ChiffonContext.Current.Environment).AsSelf().InstancePerHttpRequest();

            // Composants MVC.
            builder.RegisterControllers(typeof(Global).Assembly);
            builder.RegisterHandlers(typeof(Global).Assembly);
        }
    }
}

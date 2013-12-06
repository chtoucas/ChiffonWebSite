namespace Chiffon.Modules
{
    using Autofac;
    using Autofac.Integration.Mvc;
    using Chiffon.Infrastructure;
    using Narvalo;

    public class MvcModule : Module
    {
        public MvcModule() { }

        protected override void Load(ContainerBuilder builder)
        {
            Requires.NotNull(builder, "builder");

            // Composants Asp.Net MVC.
            builder.RegisterControllers(typeof(Global).Assembly);
            builder.RegisterHandlers(typeof(Global).Assembly);
        }
    }
}

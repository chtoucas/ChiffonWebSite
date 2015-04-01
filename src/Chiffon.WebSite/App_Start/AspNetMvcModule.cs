namespace Chiffon
{
    using Autofac;
    using Autofac.Integration.Mvc;
    using Narvalo;

    public class AspNetMvcModule : Module
    {
        public AspNetMvcModule() { }

        protected override void Load(ContainerBuilder builder)
        {
            Promise.NotNull(builder, "The base class guarantees that this parameter is not null.");

            // Composants Asp.Net MVC.
            builder.RegisterControllers(typeof(Global).Assembly);
            builder.RegisterHandlers(typeof(Global).Assembly);
            // FIXME: Je n'arrive pas à faire fonctionner la ligne suivante...
            //builder.RegisterSource(new ViewRegistrationSource());
        }
    }
}

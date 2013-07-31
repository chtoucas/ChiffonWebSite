namespace Chiffon.WebSite.Configs
{
    using System.Web.Mvc;
    using Autofac;
    using Autofac.Integration.Mvc;
    using Chiffon.Crosscuttings;

    public static class DependencyConfig
    {
        public static void RegisterDependencies(ChiffonConfig config)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new AutofacModule(config));
            builder.RegisterControllers(typeof(Global).Assembly);
            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}

namespace Chiffon.WebSite.Configs
{
    using System.Web.Mvc;
    using Autofac;
    using Autofac.Integration.Mvc;
    using Chiffon.CrossCuttings;

    public static class DependencyConfig
    {
        public static void RegisterDependencies(ChiffonConfig config)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new CrossCuttingsModule(config));
            builder.RegisterModule(new InfrastructureModule());
            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}

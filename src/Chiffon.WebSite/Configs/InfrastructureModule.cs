namespace Chiffon.WebSite.Configs
{
    using Autofac;
    using Narvalo;

    internal class InfrastructureModule : Module
    {
        public InfrastructureModule() : base() { }

        protected override void Load(ContainerBuilder builder)
        {
            Requires.NotNull(builder, "builder");

            //builder.Register(
            //    _ => new DefaultSiteMapFactory(_.Resolve<PacrConfigurationSection>().WebSite.BaseUrl)
            //    ).As<ISiteMapFactory>().SingleInstance();
        }
    }

}

namespace Chiffon.Crosscuttings
{
    using Autofac;
    using Narvalo;

    public class AutofacModule : Module
    {
        readonly ChiffonConfig _config;

        public AutofacModule(ChiffonConfig config)
            : base()
        {
            Requires.NotNull(config, "config");

            _config = config;
        }

        protected override void Load(ContainerBuilder builder)
        {
            Requires.NotNull(builder, "builder");

            // Crosscuttings
            builder.Register(_ => _config).As<ChiffonConfig>().SingleInstance();

            //builder.Register(
            //    _ => new DefaultSiteMapFactory(_.Resolve<PacrConfigurationSection>().WebSite.BaseUrl)
            //    ).As<ISiteMapFactory>().SingleInstance();
        }
    }
}

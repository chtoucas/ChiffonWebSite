namespace Chiffon.Crosscuttings
{
    using Autofac;
    using Narvalo;

    public class CrosscuttingsModule : Module
    {
        readonly ChiffonConfig _config;

        public CrosscuttingsModule(ChiffonConfig config)
            : base()
        {
            Requires.NotNull(config, "config");

            _config = config;
        }

        protected override void Load(ContainerBuilder builder)
        {
            Requires.NotNull(builder, "builder");

            // Crosscuttings
            builder.Register(_ => _config).AsSelf().SingleInstance();

            //builder.Register(
            //    _ => new DefaultSiteMapFactory(_.Resolve<PacrConfigurationSection>().WebSite.BaseUrl)
            //    ).As<ISiteMapFactory>().SingleInstance();
        }
    }
}

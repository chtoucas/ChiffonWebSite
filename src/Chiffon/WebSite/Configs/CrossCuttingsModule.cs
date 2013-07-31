namespace Chiffon.WebSite.Configs
{
    using Autofac;
    using Chiffon.CrossCuttings;
    using Narvalo;

    internal class CrossCuttingsModule : Module
    {
        readonly ChiffonConfig _config;

        public CrossCuttingsModule(ChiffonConfig config)
            : base()
        {
            Requires.NotNull(config, "config");

            _config = config;
        }

        protected override void Load(ContainerBuilder builder)
        {
            Requires.NotNull(builder, "builder");

            builder.Register(_ => _config).As<ChiffonConfig>().SingleInstance();
        }
    }
}

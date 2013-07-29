namespace Chiffon.WebSite.Configs
{
    using Autofac;
    using Chiffon.CrossCuttings;
    using Narvalo;
    using Narvalo.Diagnostics;

    internal class CrossCuttingsModule : Module
    {
        public CrossCuttingsModule() : base() { }

        protected override void Load(ContainerBuilder builder)
        {
            Requires.NotNull(builder, "builder");

            //builder.Register(_ => ChiffonConfigurationSection.GetSection())
            //    .As<ChiffonConfigurationSection>().SingleInstance();

            builder.Register(_ => new DefaultLoggerFactory())
                .As<ILoggerFactory>().SingleInstance();
        }
    }
}

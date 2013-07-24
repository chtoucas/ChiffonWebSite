namespace Chiffon.WebSite.Configs
{
    using Autofac;
    using Narvalo;
    using Narvalo.Diagnostics;
    using Narvalo.Log4Net;
    //using Chiffon.CrossCuttings.Configuration;

    internal class CrossCuttingsModule : Module
    {
        public CrossCuttingsModule() : base() { }

        protected override void Load(ContainerBuilder builder)
        {
            Requires.NotNull(builder, "builder");

            //builder.Register(_ => ChiffonConfigurationSection.GetSection())
            //    .As<ChiffonConfigurationSection>().SingleInstance();

            builder.Register(_ => new Log4NetFactory())
                .As<ILoggerFactory>().SingleInstance();
        }
    }
}

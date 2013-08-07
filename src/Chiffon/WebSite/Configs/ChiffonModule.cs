namespace Chiffon.WebSite.Configs
{
    using Autofac;
    using Autofac.Integration.Mvc;
    using Chiffon.Crosscuttings;
    using Chiffon.Infrastructure;
    using Chiffon.Services;
    using Chiffon.WebSite.Handlers;
    using Narvalo;

    public class ChiffonModule : Module
    {
        public ChiffonModule() { }

        protected override void Load(ContainerBuilder builder)
        {
            Requires.NotNull(builder, "builder");

            var config = ChiffonConfig.Create();
            var connectionStrings = new ChiffonConnectionStrings();

            builder.Register(_ => config).AsSelf().SingleInstance();
            builder.Register(c => connectionStrings).AsSelf().SingleInstance();

            builder.Register(c => new SqlHelper(connectionStrings)).AsSelf().SingleInstance();

            builder.RegisterType<PatternService>().As<IPatternService>().SingleInstance();

            builder.RegisterControllers(typeof(Global).Assembly);

            builder.Register(
                    _ => new PatternImageHandler(_.Resolve<ChiffonConfig>(), _.Resolve<IPatternService>()))
                .AsSelf().SingleInstance();
        }
    }
}

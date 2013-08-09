namespace Chiffon.Crosscuttings
{
    using Autofac;
    using Autofac.Integration.Mvc;
    using Chiffon.Handlers;
    using Chiffon.Infrastructure;
    using Narvalo;

    public class ChiffonModule : Module
    {
        public ChiffonModule() { }

        protected override void Load(ContainerBuilder builder)
        {
            Requires.NotNull(builder, "builder");

            var config = ChiffonConfig.FromConfiguration();
            var dbHelper = new DbHelper(config);

            builder.Register(_ => config).AsSelf().SingleInstance();
            builder.Register(_ => dbHelper).AsSelf().SingleInstance();

            builder.RegisterControllers(typeof(Global).Assembly);

            builder.Register(_ => new PatternImageHandler(config, dbHelper)).AsSelf().SingleInstance();
        }
    }
}

namespace Chiffon.Infrastructure
{
    using Autofac;
    using Autofac.Integration.Mvc;
    using Chiffon.Handlers;
    using Chiffon.Infrastructure;
    using Narvalo;

    public class ChiffonModule : Module
    {
        readonly ChiffonConfig _config;

        public ChiffonModule(ChiffonConfig config)
        {
            Requires.NotNull(config, "config");

            _config = config;
        }

        protected override void Load(ContainerBuilder builder)
        {
            Requires.NotNull(builder, "builder");

            var dbHelper = new DbHelper(_config);

            builder.Register(_ => _config).AsSelf().SingleInstance();
            builder.Register(_ => dbHelper).AsSelf().SingleInstance();

            builder.RegisterControllers(typeof(Global).Assembly);

            builder.Register(_ => new PatternImageHandler(_config, dbHelper)).AsSelf().SingleInstance();
        }
    }
}

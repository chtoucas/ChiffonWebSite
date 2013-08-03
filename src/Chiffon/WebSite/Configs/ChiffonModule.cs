﻿namespace Chiffon.WebSite.Configs
{
    using Autofac;
    using Autofac.Integration.Mvc;
    using Chiffon.Crosscuttings;
    using Chiffon.Entities;
    using Chiffon.WebSite.Handlers;
    using Chiffon.WebSite.Services;
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

            builder.Register(_ => _config).AsSelf().SingleInstance();

            builder.RegisterType<InMemoryDataContext>().As<IDataContext>().SingleInstance();
            builder.RegisterType<PatternServiceImpl>().As<IPatternService>().InstancePerHttpRequest();

            builder.RegisterControllers(typeof(Global).Assembly);

            builder.Register(
                    _ => new PatternImageHandler(_.Resolve<ChiffonConfig>(), _.Resolve<IPatternService>()))
                .AsSelf().InstancePerHttpRequest();
        }
    }
}

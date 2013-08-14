namespace Chiffon.Infrastructure
{
    using Autofac;
    using Autofac.Integration.Mvc;
    using Chiffon.Infrastructure.Addressing;
    using Chiffon.Services;
    using Narvalo;
    using Narvalo.Web.Security;

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
            builder.RegisterType<DefaultSiteMapFactory>().As<ISiteMapFactory>().SingleInstance();

            builder.RegisterType<FormsAuthenticationService>().As<IFormsAuthenticationService>().SingleInstance();
            builder.RegisterType<MemberService>().As<IMemberService>().SingleInstance();

            var assembly = typeof(Global).Assembly;

            builder.RegisterControllers(assembly);
            builder.RegisterHandlers(assembly);

            //builder.RegisterType<PatternImageHandler>().As<PatternImageHandler>().SingleInstance();
            //builder.RegisterType<LogOnHandler>().As<LogOnHandler>().SingleInstance();
            //builder.RegisterType<LogOffHandler>().As<LogOffHandler>().SingleInstance();
        }
    }
}

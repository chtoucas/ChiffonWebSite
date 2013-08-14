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

            builder.Register(_ => _config).AsSelf().SingleInstance();
            builder.RegisterType<DbHelper>().AsSelf().SingleInstance();
            builder.RegisterType<DefaultSiteMapFactory>().As<ISiteMapFactory>().SingleInstance();

            builder.RegisterType<FormsAuthenticationService>().As<IFormsAuthenticationService>().SingleInstance();
            builder.RegisterType<MemberService>().As<IMemberService>().SingleInstance();

            builder.RegisterControllers(typeof(Global).Assembly);
            builder.RegisterHandlers(typeof(Global).Assembly);

            //builder.RegisterType<PatternImageHandler>().As<PatternImageHandler>().SingleInstance();
            //builder.RegisterType<LogOnHandler>().As<LogOnHandler>().SingleInstance();
            //builder.RegisterType<LogOffHandler>().As<LogOffHandler>().SingleInstance();
        }
    }
}

namespace Chiffon
{
    using Autofac;
    using Chiffon.Infrastructure;
    using Narvalo;

    public class WebSiteAutofacModule : Module
    {
        readonly ChiffonConfig _config;

        public WebSiteAutofacModule(ChiffonConfig config)
        {
            Requires.NotNull(config, "config");

            _config = config;
        }

        protected override void Load(ContainerBuilder builder)
        {
            Requires.NotNull(builder, "builder");

            builder.RegisterModule(new MvcAutofacModule());
            builder.RegisterModule(new InfrastructureAutofacModule(_config));
            builder.RegisterModule(new DataAutofacModule(_config));
            builder.RegisterModule(new ServicesAutofacModule());
        }
    }
}

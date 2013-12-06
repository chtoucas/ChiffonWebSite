namespace Chiffon.Modules
{
    using Autofac;
    using Autofac.Integration.Mvc;
    using Chiffon.Services;
    using Narvalo;
    using Narvalo.Web.Security;

    public class ServicesModule : Module
    {
        public ServicesModule() { }

        protected override void Load(ContainerBuilder builder)
        {
            Requires.NotNull(builder, "builder");

            builder.RegisterType<FormsAuthenticationService>().As<IFormsAuthenticationService>().SingleInstance();

            builder.RegisterType<MemberService>().As<IMemberService>().SingleInstance();
            // NB: On utilise InstancePerHttpRequest car PatternService dépend d'IQueries.
            builder.RegisterType<PatternService>().As<IPatternService>().InstancePerHttpRequest();
        }
    }
}

namespace Chiffon
{
    using Autofac;
    using Autofac.Integration.Mvc;
    using Chiffon.Services;
    using Narvalo;

    public class ServicesModule : Module
    {
        public ServicesModule() { }

        protected override void Load(ContainerBuilder builder)
        {
            Requires.NotNull(builder, "builder");

            // NB: On utilise InstancePerHttpRequest car MemberService dépend d'ISiteMap.
            builder.RegisterType<MemberService>().As<IMemberService>().InstancePerHttpRequest();
            // NB: On utilise InstancePerHttpRequest car PatternService dépend d'IDbQueries.
            builder.RegisterType<PatternService>().As<IPatternService>().InstancePerHttpRequest();
        }
    }
}

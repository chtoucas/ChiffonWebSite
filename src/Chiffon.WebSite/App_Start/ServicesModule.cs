namespace Chiffon
{
    using Autofac;
    using Chiffon.Services;
    using Narvalo;

    public class ServicesModule : Module
    {
        public ServicesModule() { }

        protected override void Load(ContainerBuilder builder)
        {
            Promise.NotNull(builder, "The base class guarantees that this parameter is not null.");

            // NB: On utilise InstancePerHttpRequest car MemberService dépend d'ISiteMap.
            builder.RegisterType<MemberService>().As<IMemberService>().InstancePerRequest();
            // NB: On utilise InstancePerHttpRequest car PatternService dépend d'IDbQueries.
            builder.RegisterType<PatternService>().As<IPatternService>().InstancePerRequest();
        }
    }
}

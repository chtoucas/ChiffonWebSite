namespace Chiffon.Crosscuttings
{
    using Autofac;
    using Chiffon.Entities;
    using Chiffon.Persistence.InMemory;
    using Narvalo;

    public class PersistenceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Requires.NotNull(builder, "builder");

            builder.RegisterType<MemberRepositoryImpl>().As<IMemberRepository>().SingleInstance();
            builder.RegisterType<PatternRepositoryImpl>().As<IPatternRepository>().SingleInstance();
        }
    }
}

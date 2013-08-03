namespace Chiffon.Crosscuttings
{
    using Autofac;
    using Chiffon.Domain;
    using Chiffon.Persistence.InMemory;
    using Narvalo;

    public class PersistenceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Requires.NotNull(builder, "builder");

            builder.RegisterType<DesignerRepositoryImpl>().As<IDesignerRepository>().SingleInstance();
            builder.RegisterType<PatternRepositoryImpl>().As<IPatternRepository>().SingleInstance();
        }
    }
}

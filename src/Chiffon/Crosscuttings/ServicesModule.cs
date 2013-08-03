namespace Chiffon.Crosscuttings
{
    using Autofac;
    using Chiffon.Services;
    using Narvalo;

    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Requires.NotNull(builder, "builder");

            builder.RegisterType<PatternServiceImpl>().As<IPatternService>().SingleInstance();
        }
    }
}

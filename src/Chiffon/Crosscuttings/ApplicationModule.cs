namespace Chiffon.Crosscuttings
{
    using Autofac;
    using Chiffon.Application;
    using Narvalo;

    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Requires.NotNull(builder, "builder");

            builder.RegisterType<PatternServiceImpl>().As<IPatternService>().SingleInstance();
        }
    }
}

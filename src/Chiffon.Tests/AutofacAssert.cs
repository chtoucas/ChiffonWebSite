namespace Chiffon
{
    using System.Diagnostics.CodeAnalysis;
    using Autofac;
    using Xunit;

    public static class AutofacAssert
    {
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void IsResolved<T>(IComponentContext context)
        {
            // Exceptions thrown by Autofac:
            // - Autofac.Core.Registration.ComponentNotRegisteredException
            // - Autofac.Core.DependencyResolutionException
            Assert.DoesNotThrow(() => context.Resolve<T>());
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void NotSameInstance<T>(IComponentContext context)
        {
            var firstInstance = context.Resolve<T>();
            var secondInstance = context.Resolve<T>();

            Assert.NotSame(firstInstance, secondInstance);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void SameInstance<T>(IComponentContext context)
        {
            var firstInstance = context.Resolve<T>();
            var secondInstance = context.Resolve<T>();

            Assert.Same(firstInstance, secondInstance);
        }
    }
}

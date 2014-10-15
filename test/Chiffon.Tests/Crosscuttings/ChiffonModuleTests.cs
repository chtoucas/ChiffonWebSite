namespace Chiffon.Crosscuttings
{
    using System;
    using Autofac;
    using Chiffon.Infrastructure;
    using Xunit;

    public class ChiffonModuleTests
    {
        IContainer _container;
        bool _disposed = false;

        public ChiffonModuleTests()
        {
            //var builder = new ContainerBuilder();
            //builder.RegisterModule(new ChiffonModule());
            //_container = builder.Build();
        }

        [Fact]
        public void ChiffonConfig_IsResolved()
        {
            // Act & Assert
            AutofacAssert.IsResolved<ChiffonConfig>(_container);
        }

        //[Fact]
        //public void DbHelper_IsResolvedWithSingleton()
        //{
        //    // Act & Assert
        //    AutofacAssert.SameInstance<DbHelper>(_container);
        //}

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed) {
                if (disposing) {
                    if (_container != null) {
                        _container.Dispose();
                    }
                }

                _container = null;
                _disposed = true;
            }
        }
    }
}

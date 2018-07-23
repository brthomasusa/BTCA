using System;

namespace BTCA.Tests
{
    public abstract class BaseTestClass : IDisposable
    {
        protected string ServiceAddress = "http://localhost:5001";
        protected string RootAddress = String.Empty;

        public virtual void Dispose()
        {            
        }
    }
}

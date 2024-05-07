using API.DAL;
using System;

namespace API.Test.Common
{
    public abstract class TestCommandBase : IDisposable
    {
        protected readonly ApiDbContext Context;

        public TestCommandBase()
        {
            Context = EntityContextFactory.Create();
        }

        public void Dispose()
        {
            EntityContextFactory.Destroy(Context);
        }
    }
}

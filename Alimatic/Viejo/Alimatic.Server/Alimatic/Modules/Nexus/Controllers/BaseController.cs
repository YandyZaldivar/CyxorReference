namespace Alimatic.Nexus.Controllers
{
    using Data;

    using Cyxor.Controllers;

    class BaseController : MasterController
    {
        protected NexusDbContext NexusDbContext;

        [ScopeInitializer]
        public virtual void Initialize(NexusDbContext nexusDbContext) => NexusDbContext = nexusDbContext;
    }
}

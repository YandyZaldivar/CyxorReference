namespace Cyxor.Controllers
{
    using Data;

    public abstract class DbContextMasterController : MasterController
    {
        protected MasterDbContext MasterDbContext;

        [ScopeInitializer]
        public virtual void InitializeMasterDbContext(MasterDbContext masterDbContext) => MasterDbContext = masterDbContext;
    }
}

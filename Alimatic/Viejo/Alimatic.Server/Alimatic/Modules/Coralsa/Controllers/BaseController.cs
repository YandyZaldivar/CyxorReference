namespace Alimatic.Coralsa.Controllers
{
    using Data;

    using Cyxor.Controllers;

    class BaseController : MasterController
    {
        protected CoralsaDbContext CoralsaDbContext;

        [ScopeInitializer]
        public virtual void Initialize(CoralsaDbContext coralsaDbContext) => CoralsaDbContext = coralsaDbContext;
    }
}

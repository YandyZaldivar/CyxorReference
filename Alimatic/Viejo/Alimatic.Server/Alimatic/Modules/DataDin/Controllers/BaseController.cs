namespace Alimatic.DataDin.Controllers
{
    using Data;

    using Cyxor.Controllers;

    class BaseController : MasterController
    {
        protected DataDinDbContext DataDinDbContext;

        [ScopeInitializer]
        public virtual void Initialize(DataDinDbContext dataDinDbContext) => DataDinDbContext = dataDinDbContext;
    }
}

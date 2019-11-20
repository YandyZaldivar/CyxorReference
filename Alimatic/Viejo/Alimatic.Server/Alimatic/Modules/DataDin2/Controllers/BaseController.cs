namespace Alimatic.DataDin2.Controllers
{
    using Data;

    using Cyxor.Controllers;

    class BaseController : MasterController
    {
        protected DataDin2DbContext DataDinDbContext;

        [ScopeInitializer]
        public virtual void Initialize(DataDin2DbContext dataDinDbContext) => DataDinDbContext = dataDinDbContext;
    }
}

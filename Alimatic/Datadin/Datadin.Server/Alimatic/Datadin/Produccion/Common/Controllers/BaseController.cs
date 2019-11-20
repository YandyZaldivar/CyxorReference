// { Alimatic.Datadin } - Backend
// Copyright (C) 2018 Alimatic
// Author:  Yandy Zaldivar

namespace Alimatic.Datadin.Produccion.Controllers
{
    using Data;

    using Cyxor.Controllers;

    abstract class BaseController<TDatadinDbContext> : MasterController
        where TDatadinDbContext : DatadinDbContext
    {
        protected TDatadinDbContext DatadinDbContext;

        [ScopeInitializer]
        public virtual void Initialize(TDatadinDbContext datadinDbContext) => DatadinDbContext = datadinDbContext;
    }
}
// { Alimatic.Datadin } - Backend

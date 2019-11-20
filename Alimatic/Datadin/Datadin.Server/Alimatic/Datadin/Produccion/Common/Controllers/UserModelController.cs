// { Alimatic.Datadin } - Backend
// Copyright (C) 2018 Alimatic
// Author:  Yandy Zaldivar

namespace Alimatic.Datadin.Produccion.Controllers
{
    using Data;
    using Models;

    using Cyxor.Controllers;

    abstract class UserModelController<TDatadinDbContext> : Controller<UserModel, TDatadinDbContext>
        where TDatadinDbContext : DatadinDbContext
    {

    }
}
// { Alimatic.Datadin } - Backend

/*
  { Cardyan } - Inventory module
  Copyright (C) 2018 Cardyan AS
*/

namespace Cardyan.Accounting.Controllers
{
    using Data;

    using Cyxor.Controllers;

    public abstract class CardyanDbContextController<TModel> : Controller<TModel, CardyanDbContext>
        where TModel : class
    {

    }
}
/* { Cardyan } - Inventory module */

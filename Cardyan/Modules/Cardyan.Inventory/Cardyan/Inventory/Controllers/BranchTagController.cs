/*
  { Cardyan } - Inventory module
  Copyright (C) 2018 Cardyan AS
*/

namespace Cardyan.Inventory.Controllers
{
    using Data;
    using Models;

    using Cyxor.Controllers;

    //class BranchTagController : Controller<BranchTag, int, int, BranchTagApiModel, CardyanDbContext> { }
    class BranchTagController : CardyanDbContextController<BranchTag> { }
}
/* { Cardyan } - Inventory module */

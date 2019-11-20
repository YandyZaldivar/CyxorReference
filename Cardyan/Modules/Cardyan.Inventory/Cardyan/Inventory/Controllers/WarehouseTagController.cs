/*
  { Cardyan } - Inventory module
  Copyright (C) 2018 Cardyan AS
*/

namespace Cardyan.Inventory.Controllers
{
    using Data;
    using Models;

    using Cyxor.Controllers;

    //class WarehouseTagController : Controller<WarehouseTag, int, int, WarehouseTagApiModel, CardyanDbContext> { }
    class WarehouseTagController : CardyanDbContextController<WarehouseTag> { }
}
/* { Cardyan } - Inventory module */

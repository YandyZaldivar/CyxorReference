/*
  { Cardyan } - Inventory module
  Copyright (C) 2018 Cardyan AS
*/

namespace Cardyan.Inventory.Controllers
{
    using Data;
    using Models;

    using Cyxor.Controllers;

    //class WarehouseLocationTagController : Controller<WarehouseLocationTag, int, int, int, WarehouseLocationTagApiModel, CardyanDbContext> { }
    class WarehouseLocationTagController : CardyanDbContextController<WarehouseLocationTag> { }
}
/* { Cardyan } - Inventory module */

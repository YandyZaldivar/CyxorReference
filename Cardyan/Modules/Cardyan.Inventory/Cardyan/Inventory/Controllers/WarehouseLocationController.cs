/*
  { Cardyan } - Inventory module
  Copyright (C) 2018 Cardyan AS
*/

namespace Cardyan.Inventory.Controllers
{
    using Data;
    using Models;

    using Cyxor.Controllers;

    //class WarehouseLocationController : Controller<WarehouseLocation, int, int, WarehouseLocationApiModel, CardyanDbContext> { }
    class WarehouseLocationController : CardyanDbContextController<WarehouseLocation> { }
}
/* { Cardyan } - Inventory module */

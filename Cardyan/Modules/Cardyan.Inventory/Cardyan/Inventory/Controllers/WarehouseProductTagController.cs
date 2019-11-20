/*
  { Cardyan } - Inventory module
  Copyright (C) 2018 Cardyan AS
*/

namespace Cardyan.Inventory.Controllers
{
    using Data;
    using Models;

    using Cyxor.Controllers;

    //class WarehouseProductTagController : Controller<WarehouseProductTag, int, int, int, WarehouseProductTagApiModel, CardyanDbContext> { }
    class WarehouseProductTagController : CardyanDbContextController<WarehouseProductTag> { }
}
/* { Cardyan } - Inventory module */

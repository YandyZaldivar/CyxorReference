/*
  { Cardyan } - Inventory module
  Copyright (C) 2018 Cardyan AS
*/

namespace Cardyan.Inventory.Controllers
{
    using Data;
    using Models;

    using Cyxor.Controllers;

    //public class PropertyController : Controller<Property, int, PropertyApiModel, CardyanDbContext> { }
    public class PropertyController : CardyanDbContextController<Property> { }
}
/* { Cardyan } - Inventory module */

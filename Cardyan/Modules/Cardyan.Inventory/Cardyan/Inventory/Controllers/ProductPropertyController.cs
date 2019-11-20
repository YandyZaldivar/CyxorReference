/*
  { Cardyan } - Inventory module
  Copyright (C) 2018 Cardyan AS
*/

namespace Cardyan.Inventory.Controllers
{
    using Data;
    using Models;

    using Cyxor.Controllers;

    //class ProductPropertyController : Controller<ProductProperty, int, int, ProductPropertyApiModel, CardyanDbContext> { }
    class ProductPropertyController : CardyanDbContextController<ProductProperty> { }
}
/* { Cardyan } - Inventory module */

/*
  { Cardyan } - Inventory module
  Copyright (C) 2018 Cardyan AS
*/

namespace Cardyan.Inventory.Controllers
{
    using Data;
    using Models;

    using Cyxor.Controllers;

    //public class ImageController : Controller<Image, int, ImageApiModel, CardyanDbContext> { }
    public class ImageController : CardyanDbContextController<Image> { }
}
/* { Cardyan } - Inventory module */

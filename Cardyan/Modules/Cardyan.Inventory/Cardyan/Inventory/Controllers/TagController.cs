/*
  { Cardyan } - Inventory module
  Copyright (C) 2018 Cardyan AS
*/

namespace Cardyan.Inventory.Controllers
{
    using Data;
    using Models;

    using Cyxor.Controllers;

    //public class TagController : Controller<Tag, int, TagApiModel, CardyanDbContext> { }
    public class TagController : CardyanDbContextController<Tag> { }
}
/* { Cardyan } - Inventory module */

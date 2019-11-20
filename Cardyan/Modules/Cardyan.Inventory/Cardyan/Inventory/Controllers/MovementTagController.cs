/*
  { Cardyan } - Inventory module
  Copyright (C) 2018 Cardyan AS
*/

namespace Cardyan.Inventory.Controllers
{
    using Data;
    using Models;

    using Cyxor.Controllers;

    //class MovementTagController : Controller<MovementTag, int, int, MovementTagApiModel, CardyanDbContext> { }
    class MovementTagController : CardyanDbContextController<MovementTag> { }
}
/* { Cardyan } - Inventory module */

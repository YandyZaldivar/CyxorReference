/*
  { Cardyan } - Inventory module
  Copyright (C) 2018 Cardyan AS
*/

namespace Cardyan.Inventory.Controllers
{
    using Data;
    using Models;

    using Cyxor.Controllers;

    //class AssociateTagController : Controller<AssociateTag, int, int, AssociateTagApiModel, CardyanDbContext> { }
    class AssociateTagController : CardyanDbContextController<AssociateTag> { }
}
/* { Cardyan } - Inventory module */

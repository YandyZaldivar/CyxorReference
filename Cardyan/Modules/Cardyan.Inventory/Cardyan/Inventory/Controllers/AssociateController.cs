/*
  { Cardyan } - Inventory module
  Copyright (C) 2018 Cardyan AS
*/

namespace Cardyan.Inventory.Controllers
{
    using Data;
    using Models;

    using Cyxor.Controllers;

    //public class AssociateController : Controller<Associate, int, AssociateApiModel, CardyanDbContext> { }
    public class AssociateController : CardyanDbContextController<Associate> { }
}
/* { Cardyan } - Inventory module */

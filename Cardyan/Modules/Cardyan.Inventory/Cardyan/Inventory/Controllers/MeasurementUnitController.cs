/*
  { Cardyan } - Inventory module
  Copyright (C) 2018 Cardyan AS
*/

namespace Cardyan.Inventory.Controllers
{
    using Data;
    using Models;

    using Cyxor.Controllers;

    //public class MeasurementUnitController : Controller<MeasurementUnit, int, MeasurementUnitApiModel, CardyanDbContext> { }
    public class MeasurementUnitController : CardyanDbContextController<MeasurementUnit> { }
}
/* { Cardyan } - Inventory module */

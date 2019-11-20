/*
  { Cardyan } - Inventory module
  Copyright (C) 2018 Cardyan AS
*/

namespace Cardyan.Inventory.Controllers
{
    using Data;
    using Models;

    using Cyxor.Controllers;
    using Cyxor.Models;
    using System.Threading.Tasks;

    //public class CategoryController : Controller<Category, int, CategoryApiModel, CardyanDbContext>
    public class CategoryController : CardyanDbContextController<Category>
    {
        // TODO: Delete, this was to tests with a delay
        //public override async Task<CategoryApiModel> Read(KeyApiModel<int> keyApiModel)
        //{
        //    await Task.Delay(3500).ConfigureAwait(false);
        //    return await base.Read(keyApiModel).ConfigureAwait(false);
        //}

        //public override async Task<ResponseListApiModel<CategoryApiModel>> List(ListApiModel model)
        //{
        //    await Task.Delay(3500).ConfigureAwait(false);
        //    return await base.List(model).ConfigureAwait(false);
        //}
    }
}
/* { Cardyan } - Inventory module */

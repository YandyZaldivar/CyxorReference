/*
  { Cardyan } - Inventory module
  Copyright (C) 2018 Cardyan AS
*/

using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Cardyan.Inventory.Controllers
{
    using Data;
    using Models;

    using Cyxor.Models;
    using Cyxor.Controllers;

    //class WarehouseProductController : Controller<WarehouseProduct, int, int, WarehouseProductApiModel, CardyanDbContext>
    //class WarehouseProductController : Controller<WarehouseProduct, int, int, WarehouseProduct, CardyanDbContext>
    class WarehouseProductController : CardyanDbContextController<WarehouseProduct>
    {
        //public async Task<WarehouseProductStatisticApiModel> Statistic(KeyApiModel<int, int> apiModel)
        //    => FromModel<WarehouseProductStatisticApiModel, WarehouseProductStatistic>((await DbContext.WarehouseProducts.Include(p => p.Statistic).SingleAsync(p => p.WarehouseId == apiModel.Id1 && p.ProductId == apiModel.Id2).ConfigureAwait(false)).Statistic);

        //public async override Task<KeyApiModel<int, int>> Create(WarehouseProductApiModel apiModel)
        public override Task<WarehouseProduct> Create(WarehouseProduct model)
        {
            model.Statistic = new WarehouseProductStatistic();
            return base.Create(model);

            //DbContext.WarehouseProductStatistics.Add(warehouseProductStatistic);

            //var warehouseProduct = ToModel(apiModel);
            //warehouseProduct.Statistic = warehouseProductStatistic;

            //DbContext.WarehouseProducts.Add(warehouseProduct);

            //await DbContext.SaveChangesAsync().ConfigureAwait(false);
            //return new KeyApiModel<int, int> { Id1 = warehouseProduct.WarehouseId, Id2 = warehouseProduct.ProductId };
        }

        public override async Task Delete(WarehouseProduct model)
        {
            var warehouseProduct = await DbContext.WarehouseProducts.Include(p => p.Statistic).SingleAsync(p => p.WarehouseId == model.Id1 && p.ProductId == model.Id2).ConfigureAwait(false);
            var warehouseProductStatistic = warehouseProduct.Statistic;

            DbContext.WarehouseProducts.Remove(warehouseProduct);
            DbContext.WarehouseProductStatistics.Remove(warehouseProductStatistic);

            await DbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
/* { Cardyan } - Inventory module */

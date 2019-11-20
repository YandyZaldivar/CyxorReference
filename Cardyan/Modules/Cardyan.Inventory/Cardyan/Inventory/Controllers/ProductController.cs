/*
  { Cardyan } - Inventory module
  Copyright (C) 2018 Cardyan AS
*/

using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

namespace Cardyan.Inventory.Controllers
{
    using Data;
    using Models;

    using Cyxor.Models;
    using Cyxor.Controllers;

    //public class ProductController : Controller<Product, int, Product, CardyanDbContext>
    public class ProductController : CardyanDbContextController<Product>
    {
        //public Task<ResponseListApiModel<ImageApiModel>> Images(TKeyListApiModel<int> apiModel)
        //    => List<ImageApiModel, Image>(apiModel, DbContext.Images.Where(p => p.ProductId == apiModel.Id));

        //public async Task<ProductStatisticApiModel> Statistic(KeyApiModel<int> apiModel)
        //    => FromModel<ProductStatisticApiModel, ProductStatistic>((await DbContext.Products.Include(p => p.Statistic).SingleAsync(p => p.Id == apiModel.Id).ConfigureAwait(false)).Statistic);

        public async Task<IEnumerable<KeyValuePair<string, object>>> Properties(KeyApiModel<int> apiModel)
        {
            var productProperties = await DbContext.ProductProperties.Include(p => p.Property).Where(p => p.ProductId == apiModel.Id).ToListAsync().ConfigureAwait(false);
            var dictionary = new Dictionary<string, object>(productProperties.Count);

            foreach (var productProperty in productProperties)
                dictionary[productProperty.Property.Name] = productProperty.Value;

            return dictionary;
        }

        public async override Task<Product> Create(Product model)
        {
            var productStatistic = new ProductStatistic();
            DbContext.ProductStatistics.Add(productStatistic);

            model.Statistic = productStatistic;

            DbContext.Products.Add(model);

            await DbContext.SaveChangesAsync().ConfigureAwait(false);
            return model;
        }

        public override async Task Delete(Product model)
        {
            var product = await DbContext.Products.Include(p => p.Statistic).SingleAsync(p => p.Id == model.Id).ConfigureAwait(false);
            var productStatistic = product.Statistic;

            DbContext.Products.Remove(product);
            DbContext.ProductStatistics.Remove(productStatistic);

            await DbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }

    //public class ProductController : Controller<Product, int, ProductApiModel, CardyanDbContext>
    //{
    //    public Task<ResponseListApiModel<ImageApiModel>> Images(TKeyListApiModel<int> apiModel)
    //        => List<ImageApiModel, Image>(apiModel, DbContext.Images.Where(p => p.ProductId == apiModel.Id));

    //    public async Task<ProductStatisticApiModel> Statistic(KeyApiModel<int> apiModel)
    //        => FromModel<ProductStatisticApiModel, ProductStatistic>((await DbContext.Products.Include(p => p.Statistic).SingleAsync(p => p.Id == apiModel.Id).ConfigureAwait(false)).Statistic);

    //    public async Task<IEnumerable<KeyValuePair<string, object>>> Properties(KeyApiModel<int> apiModel)
    //    {
    //        var productProperties = await DbContext.ProductProperties.Include(p => p.Property).Where(p => p.ProductId == apiModel.Id).ToListAsync().ConfigureAwait(false);
    //        var dictionary = new Dictionary<string, object>(productProperties.Count);

    //        foreach (var productProperty in productProperties)
    //            dictionary[productProperty.Property.Name] = productProperty.Value;

    //        return dictionary;
    //    }

    //    public async override Task<IKeyApiModel<int>> Create(ProductApiModel apiModel)
    //    {
    //        var productStatistic = new ProductStatistic();
    //        DbContext.ProductStatistics.Add(productStatistic);

    //        var product = ToModel(apiModel);
    //        product.Statistic = productStatistic;

    //        DbContext.Products.Add(product);

    //        await DbContext.SaveChangesAsync().ConfigureAwait(false);
    //        return new KeyApiModel<int> { Id = product.Id };
    //    }

    //    public override async Task Delete(KeyApiModel<int> keyApiModel)
    //    {
    //        var product = await DbContext.Products.Include(p => p.Statistic).SingleAsync(p => p.Id == keyApiModel.Id).ConfigureAwait(false);
    //        var productStatistic = product.Statistic;

    //        DbContext.Products.Remove(product);
    //        DbContext.ProductStatistics.Remove(productStatistic);

    //        await DbContext.SaveChangesAsync().ConfigureAwait(false);
    //    }
    //}
}
/* { Cardyan } - Inventory module */

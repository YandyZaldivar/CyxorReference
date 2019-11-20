using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

namespace Cyxor.Controllers
{
    using Models;

    //public abstract class Controller<TModel, TKey, TApiModel, TDbContext> : DbContextController<TDbContext>
    //    where TModel : class, IKeyApiModel<TKey>
    //    where TDbContext : DbContext
    //{
    //    protected virtual TApiModel FromModel(TModel model)
    //        => FromModel<TApiModel, TModel>(model);

    //    protected virtual TModel ToModel(TApiModel apiModel)
    //        => ToModel<TApiModel, TModel>(apiModel);

    //    protected virtual IEnumerable<TApiModel> FromModels(IEnumerable<TModel> models)
    //        => FromModels<TApiModel, TModel>(models);


    //    protected virtual Task<TModel> FindModelAsync(KeyApiModel<TKey> keyApiModel)
    //        => DbContext.FindAsync<TModel>(keyApiModel.Id);

    //    //public virtual async Task<TApiModel> Read(KeyApiModel<TKey> keyApiModel)
    //    //    => FromModel(await FindModelAsync(keyApiModel).ConfigureAwait(false));

    //    //public virtual Task<ResponseListApiModel<TApiModel>> List(ListApiModel model)
    //    //    => List<TApiModel, TModel>(model);

    //    public virtual Task<ResponseListApiModel<TApiModel>> Read(ListApiModel model)
    //        => List<TApiModel, TModel>(model);

    //    public virtual async Task<IKeyApiModel<TKey>> Create(TApiModel apiModel)
    //    {
    //        var model = DbContext.Add(ToModel(apiModel)).Entity;
    //        await DbContext.SaveChangesAsync().ConfigureAwait(false);
    //        return new KeyApiModel<TKey> { Id = model.Id };
    //    }

    //    public virtual async Task Delete(KeyApiModel<TKey> keyApiModel)
    //    {
    //        var model = Activator.CreateInstance<TModel>();
    //        model.Id = keyApiModel.Id;
    //        model = DbContext.Remove(model).Entity;
    //        await DbContext.SaveChangesAsync().ConfigureAwait(false);
    //    }

    //    // TODO: Fix the update for 2 and 3 keys
    //    public virtual async Task Update(TApiModel apiModel)
    //    {
    //        //var model = ToModel(apiModel);

    //        var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(Context.JsonRequest);

    //        var model = ToModel(apiModel);

    //        if (model.Id.Equals(default(TKey)))
    //            throw new InvalidOperationException("The model key is not set");

    //        var entity = DbContext.Attach(model);

    //        foreach (var propertyName in dictionary.Keys)
    //        {
    //            if (!entity.Property(propertyName).Metadata.IsPrimaryKey())
    //                entity.Property(propertyName).IsModified = true;
    //        }

    //        await DbContext.SaveChangesAsync().ConfigureAwait(false);
    //    }
    //}
}

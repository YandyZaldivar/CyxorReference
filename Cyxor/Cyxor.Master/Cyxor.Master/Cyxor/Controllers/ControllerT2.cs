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

    //public abstract class Controller<TModel, TKey1, TKey2, TApiModel, TDbContext> : DbContextController<TDbContext>
    //    where TModel : KeyApiModel<TKey1, TKey2>
    //    where TDbContext : DbContext
    //{
    //    protected virtual TApiModel FromModel(TModel model)
    //        => model != null ? Node.Mapper.Map<TModel, TApiModel>(model, opts =>
    //        opts.ConfigureMap(AutoMapper.MemberList.None)) : default(TApiModel);

    //    protected virtual TModel ToModel(TApiModel apiModel)
    //        => apiModel != null ? Node.Mapper.Map<TApiModel, TModel>(apiModel, opts =>
    //        opts.ConfigureMap(AutoMapper.MemberList.None)) : default(TModel);

    //    protected virtual Task<TModel> FindModelAsync(KeyApiModel<TKey1, TKey2> keyApiModel)
    //        => DbContext.FindAsync<TModel>(keyApiModel.Id1, keyApiModel.Id2);

    //    public virtual async Task<TApiModel> Read(KeyApiModel<TKey1, TKey2> keyApiModel)
    //        => FromModel(await FindModelAsync(keyApiModel).ConfigureAwait(false));

    //    public virtual async Task<KeyApiModel<TKey1, TKey2>> Create(TApiModel apiModel)
    //    {
    //        var model = DbContext.Add(ToModel(apiModel)).Entity;
    //        await DbContext.SaveChangesAsync().ConfigureAwait(false);
    //        return new KeyApiModel<TKey1, TKey2> { Id1 = model.Id1, Id2 = model.Id2 };
    //    }

    //    public virtual async Task Delete(KeyApiModel<TKey1, TKey2> keyApiModel)
    //    {
    //        var model = Activator.CreateInstance<TModel>();
    //        model.Id1 = keyApiModel.Id1;
    //        model.Id2 = keyApiModel.Id2;
    //        model = DbContext.Remove(model).Entity;
    //        await DbContext.SaveChangesAsync().ConfigureAwait(false);
    //    }

    //    public virtual async Task Update(TApiModel apiModel)
    //    {
    //        var model = DbContext.Update(ToModel(apiModel)).Entity;
    //        await DbContext.SaveChangesAsync().ConfigureAwait(false);
    //    }

    //    public virtual Task<ResponseListApiModel<TApiModel>> List(ListApiModel model)
    //        => List<TApiModel, TModel>(model);
    //}
}

//struct CompoundKey<TKey1, TKey2> : IComparable<CompoundKey<TKey1, TKey2>>
//{
//    readonly TKey1 Key1;
//    readonly TKey2 Key2;

//    public CompoundKey(TKey1 key1, TKey2 key2)
//    {
//        Key1 = key1;
//        Key2 = key2;
//    }

//    public int CompareTo(CompoundKey<TKey1, TKey2> other)
//    {
//        var key11 = Key1.ToString();
//        var key21 = other.Key1.ToString();

//        var result = key11.Length.CompareTo(key21.Length);

//        if (result != 0)
//            return result;

//        result = key11.CompareTo(key21);

//        if (result != 0)
//            return result;

//        var key12 = Key2.ToString();
//        var key22 = other.Key2.ToString();

//        result = key12.Length.CompareTo(key22.Length);

//        if (result != 0)
//            return result;

//        return key12.CompareTo(key22);
//    }
//}

//protected virtual async Task<ResponseListApiModel<TApiModel>> List<TApiModel, TModel, TKey1, TKey2>(ListApiModel model, IQueryable<TModel> queryable = null, bool tryDbContext = false)
//    where TModel : KeyApiModel<TKey1, TKey2>
//{
//    var count = 0;
//    var entries = new List<TApiModel>();
//    var items = default(IEnumerable<TModel>);

//    queryable = queryable ?? DbContext.Set<TModel>();
//    //queryable = tryDbContext ? queryable : DbContext.Set<TModel>();

//    if (model.CountOnly)
//        count = await Query(queryable, model).CountAsync().ConfigureAwait(false);
//    else
//    {
//        if (string.IsNullOrEmpty(model.OrderBy))
//        {
//            if (!model.OrderByDescending)
//                items = await Query(queryable.OrderBy(p => new CompoundKey<TKey1, TKey2>(p.Id1, p.Id2)), model).ToListAsync();
//            else
//                items = await Query(queryable.OrderByDescending(p => new CompoundKey<TKey1, TKey2>(p.Id1, p.Id2)), model).ToListAsync();
//        }
//        else
//        {
//            if (!model.OrderByDescending)
//                items = await Query(queryable.OrderBy(model.OrderBy), model).ToListAsync();
//            else
//                items = await Query(queryable.OrderByDescending(model.OrderBy), model).ToListAsync();
//        }

//        foreach (var item in items)
//        {
//            var apiModel = FromModel<TApiModel, TModel>(item);
//            entries.Add(apiModel);
//        }

//        count = entries.Count;
//    }

//    return new ResponseListApiModel<TApiModel>
//    {
//        Items = entries,
//        Count = count,
//        TotalCount = await queryable.CountAsync().ConfigureAwait(false),
//    };
//}
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

    //public abstract class Controller<TModel, TKey1, TKey2, TKey3, TApiModel, TDbContext> : DbContextController<TDbContext>
    //    where TModel : KeyApiModel<TKey1, TKey2, TKey3>
    //    where TDbContext : DbContext
    //{
    //    protected virtual TApiModel FromModel(TModel model)
    //        => model != null ? Node.Mapper.Map<TModel, TApiModel>(model, opts =>
    //        opts.ConfigureMap(AutoMapper.MemberList.Destination)) : default(TApiModel);

    //    protected virtual TModel ToModel(TApiModel apiModel)
    //        => apiModel != null ? Node.Mapper.Map<TApiModel, TModel>(apiModel, opts =>
    //        opts.ConfigureMap(AutoMapper.MemberList.Source)) : default(TModel);

    //    protected virtual Task<TModel> FindModelAsync(KeyApiModel<TKey1, TKey2, TKey3> keyApiModel)
    //        => DbContext.FindAsync<TModel>(keyApiModel.Id1, keyApiModel.Id2, keyApiModel.Id3);

    //    public virtual async Task<TApiModel> Read(KeyApiModel<TKey1, TKey2, TKey3> keyApiModel)
    //        => FromModel(await FindModelAsync(keyApiModel).ConfigureAwait(false));

    //    public virtual async Task<KeyApiModel<TKey1, TKey2>> Create(TApiModel apiModel)
    //    {
    //        var model = DbContext.Add(ToModel(apiModel)).Entity;
    //        await DbContext.SaveChangesAsync().ConfigureAwait(false);
    //        return new KeyApiModel<TKey1, TKey2, TKey3> { Id1 = model.Id1, Id2 = model.Id2, Id3 = model.Id3 };
    //    }

    //    public virtual async Task Delete(KeyApiModel<TKey1, TKey2, TKey3> keyApiModel)
    //    {
    //        var model = Activator.CreateInstance<TModel>();
    //        model.Id1 = keyApiModel.Id1;
    //        model.Id2 = keyApiModel.Id2;
    //        model.Id3 = keyApiModel.Id3;
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

//struct CompoundKey : IComparable<CompoundKey>
//{
//    readonly TKey1 Key1;
//    readonly TKey2 Key2;
//    readonly TKey3 Key3;

//    public CompoundKey(TKey1 key1, TKey2 key2, TKey3 key3)
//    {
//        Key1 = key1;
//        Key2 = key2;
//        Key3 = key3;
//    }

//    public int CompareTo(CompoundKey other)
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

//        result = key12.CompareTo(key22);

//        if (result != 0)
//            return result;

//        var key13 = Key3.ToString();
//        var key23 = other.Key3.ToString();

//        result = key13.Length.CompareTo(key23.Length);

//        if (result != 0)
//            return result;

//        return key13.CompareTo(key23);
//    }
//}

//public virtual async Task<ResponseListApiModel<TApiModel>> List(ListApiModel model)
//{
//    var count = 0;
//    var entries = new List<TApiModel>();
//    var items = default(IEnumerable<TModel>);

//    if (model.CountOnly)
//        count = await Query(DbContext.Set<TModel>(), model).CountAsync().ConfigureAwait(false);
//    else
//    {
//        if (string.IsNullOrEmpty(model.OrderBy))
//        {
//            if (!model.OrderByDescending)
//                items = await Query(DbContext.Set<TModel>().OrderBy(p => new CompoundKey(p.Id1, p.Id2, p.Id3)), model).ToListAsync();
//            else
//                items = await Query(DbContext.Set<TModel>().OrderByDescending(p => new CompoundKey(p.Id1, p.Id2, p.Id3)), model).ToListAsync();
//        }
//        else
//        {
//            if (!model.OrderByDescending)
//                items = await Query(DbContext.Set<TModel>().OrderBy(model.OrderBy), model).ToListAsync();
//            else
//                items = await Query(DbContext.Set<TModel>().OrderByDescending(model.OrderBy), model).ToListAsync();
//        }

//        foreach (var item in items)
//        {
//            var apiModel = FromModel(item);
//            entries.Add(apiModel);
//        }

//        count = entries.Count;
//    }

//    return new ResponseListApiModel<TApiModel>
//    {
//        Items = entries,
//        Count = count,
//        TotalCount = await DbContext.Set<TModel>().CountAsync().ConfigureAwait(false),
//    };
//}
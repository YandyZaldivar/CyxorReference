using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

namespace Cyxor.Controllers
{
    using Models;

    public abstract class Controller<TModel, TDbContext> : DbContextController<TDbContext>
        where TModel : class
        where TDbContext : DbContext
    {
        public virtual Task<ResponseReadApiModel<TModel>> Read(ReadApiModel readApiModel)
            => Read<TModel>(readApiModel);

        public virtual async Task<TModel> Create(TModel model)
        {
            model = DbContext.Add(model).Entity;
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
            return model;
        }

        public virtual async Task Delete(TModel model)
        {
            DbContext.Remove(model);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public virtual async Task Update(TModel model)
        {
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(Context.JsonRequest);

            var entity = DbContext.Attach(model);

            foreach (var propertyName in dictionary.Keys)
            {
                var propertyEntry = entity.Property(propertyName);

                if (!propertyEntry.Metadata.IsPrimaryKey())
                    propertyEntry.IsModified = true;
                else
                {
                    var defaultValue = default(object);

                    if (propertyEntry.Metadata.ClrType.GetTypeInfo().IsValueType)
                        defaultValue = Activator.CreateInstance(propertyEntry.Metadata.ClrType);

                    if (propertyEntry.CurrentValue.Equals(defaultValue))
                        throw new InvalidOperationException("The model key is not properly set");
                }
            }

            await DbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }







    /*
    using Models;

    //public static class IQueryableExtensions
    //{
    //    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string propertyName, IComparer<object> comparer = null)
    //    {
    //        return CallOrderedQueryable(query, "OrderBy", propertyName, comparer);
    //    }

    //    public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string propertyName, IComparer<object> comparer = null)
    //    {
    //        return CallOrderedQueryable(query, "OrderByDescending", propertyName, comparer);
    //    }

    //    public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> query, string propertyName, IComparer<object> comparer = null)
    //    {
    //        return CallOrderedQueryable(query, "ThenBy", propertyName, comparer);
    //    }

    //    public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> query, string propertyName, IComparer<object> comparer = null)
    //    {
    //        return CallOrderedQueryable(query, "ThenByDescending", propertyName, comparer);
    //    }

    //    /// <summary>
    //    /// Builds the Queryable functions using a TSource property name.
    //    /// </summary>
    //    public static IOrderedQueryable<T> CallOrderedQueryable<T>(this IQueryable<T> query, string methodName, string propertyName,
    //            IComparer<object> comparer = null)
    //    {
    //        var param = Expression.Parameter(typeof(T), "x");

    //        var body = propertyName.Split('.').Aggregate<string, Expression>(param, Expression.PropertyOrField);

    //        return comparer != null
    //            ? (IOrderedQueryable<T>)query.Provider.CreateQuery(
    //                Expression.Call(
    //                    typeof(Queryable),
    //                    methodName,
    //                    new[] { typeof(T), body.Type },
    //                    query.Expression,
    //                    Expression.Lambda(body, param),
    //                    Expression.Constant(comparer)
    //                )
    //            )
    //            : (IOrderedQueryable<T>)query.Provider.CreateQuery(
    //                Expression.Call(
    //                    typeof(Queryable),
    //                    methodName,
    //                    new[] { typeof(T), body.Type },
    //                    query.Expression,
    //                    Expression.Lambda(body, param)
    //                )
    //            );
    //    }
    //}
    ////General Discussion




        // Es este

    //public static class IQueryableExtensions
    //{
    //    public static IOrderedQueryable<T> LIKE<T>(this IQueryable<T> source, string propertyName)
    //        => source.OrderBy(ToLambda<T>(propertyName));



    //    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
    //        => source.OrderBy(ToLambda<T>(propertyName));

    //    public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
    //        => source.OrderByDescending(ToLambda<T>(propertyName));

    //    static Expression<Func<T, object>> ToLambda<T>(string propertyName)
    //    {
    //        var parameter = Expression.Parameter(typeof(T));
    //        var property = Expression.Property(parameter, propertyName);
    //        var propAsObject = Expression.Convert(property, typeof(object));
            
    //        return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
    //    }

    //    //public static string GetStringPropertyValue<T>(this T self, string propertyName) where T : class
    //    //{
    //    //    var param = Expression.Parameter(typeof(T), "value");
    //    //    var getter = Expression.Property(param, propertyName);
    //    //    var boxer = Expression.TypeAs(getter, typeof(string));
    //    //    var getPropValue = Expression.Lambda<Func<T, string>>(boxer, param).Compile();
    //    //    return getPropValue(self);
    //    //}
    //}







    public abstract class Controller<TModel, TKey, TApiModel, TDbContext> : DbContextController<TDbContext>
        where TModel : class, IKeyApiModel<TKey>
        where TDbContext : DbContext
    {
        protected virtual TApiModel FromModel(TModel model)
            => FromModel<TApiModel, TModel>(model);

        protected virtual TModel ToModel(TApiModel apiModel)
            => ToModel<TApiModel, TModel>(apiModel);

        protected virtual IEnumerable<TApiModel> FromModels(IEnumerable<TModel> models)
            => FromModels<TApiModel, TModel>(models);

        //protected virtual TApiModel FromModel(TModel model)
        //    => model != null ? Node.Mapper.Map<TModel, TApiModel>(model, opts =>
        //    opts.ConfigureMap(AutoMapper.MemberList.Destination)) : default(TApiModel);

        //protected virtual TModel ToModel(TApiModel apiModel)
        //    => apiModel != null ? Node.Mapper.Map<TApiModel, TModel>(apiModel, opts =>
        //    opts.ConfigureMap(AutoMapper.MemberList.Source)) : default(TModel);

        //protected virtual IEnumerable<TA> FromModels<TA, TM>(IEnumerable<TM> models)
        //    => models != null ? Node.Mapper.Map<IEnumerable<TM>, IEnumerable<TA>>(models, opts =>
        //    opts.ConfigureMap(AutoMapper.MemberList.Destination)) : default(IEnumerable<TA>);


        protected virtual Task<TModel> FindModelAsync(KeyApiModel<TKey> keyApiModel)
            => DbContext.FindAsync<TModel>(keyApiModel.Id);

        public virtual async Task<TApiModel> Read(KeyApiModel<TKey> keyApiModel)
            => FromModel(await FindModelAsync(keyApiModel).ConfigureAwait(false));

        public virtual async Task<IKeyApiModel<TKey>> Create(TApiModel apiModel)
        {
            var model = DbContext.Add(ToModel(apiModel)).Entity;
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
            return new KeyApiModel<TKey> { Id = model.Id };
        }

        public virtual async Task Delete(KeyApiModel<TKey> keyApiModel)
        {
            var model = Activator.CreateInstance<TModel>();
            model.Id = keyApiModel.Id;
            model = DbContext.Remove(model).Entity;
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        //public virtual async Task Update(TApiModel apiModel)
        //{
        //    var model = DbContext.Update(ToModel(apiModel)).Entity;
        //    await DbContext.SaveChangesAsync().ConfigureAwait(false);
        //}

        public virtual async Task Update(TApiModel apiModel)
        {
            //var model = ToModel(apiModel);

            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(Context.JsonRequest);

            var model = ToModel(apiModel);

            if (model.Id.Equals(default(TKey)))
                throw new InvalidOperationException("The model key is not set");

            var entity = DbContext.Attach(model);

            foreach (var propertyName in dictionary.Keys)
            {
                if (!entity.Property(propertyName).Metadata.IsPrimaryKey())
                    entity.Property(propertyName).IsModified = true;
            }

            await DbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public virtual Task<ResponseListApiModel<TApiModel>> List(ListApiModel model)
            => List<TApiModel, TModel, TKey>(model);

        //public virtual async Task<ResponseListApiModel<TApiModel>> List(ListApiModel model)
        //{
        //    var count = 0;
        //    var entries = new List<TApiModel>();
        //    //var items = await Query(DbContext.Set<TModel>().OrderBy(p => p.Id), model).ToListAsync();

        //    var items = default(IEnumerable<TModel>);

        //    //            items = await Query(DbContext.Set<TModel>()
        //    //#if !NET451 && !NETSTANDARD1_3
        //    //                .Where(p => EF.Functions.Like(, "a%"))
        //    //#endif
        //    //            .OrderBy(p => p.Id), model).ToListAsync();

        //    if (model.CountOnly)
        //        count = await Query(DbContext.Set<TModel>().OrderBy(p => p.Id), model).CountAsync().ConfigureAwait(false);
        //    else
        //    {
        //        if (string.IsNullOrEmpty(model.OrderBy))
        //        {
        //            if (!model.OrderByDescending)
        //                items = await Query(DbContext.Set<TModel>().OrderBy(p => p.Id), model).ToListAsync();
        //            else
        //                items = await Query(DbContext.Set<TModel>().OrderByDescending(p => p.Id), model).ToListAsync();
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

        //public virtual async Task<int> Count(ListApiModel model)
    }

    public abstract class Controller<TModel, TKey1, TKey2, TApiModel, TDbContext> : DbContextController<TDbContext>
            where TModel : KeyApiModel<TKey1, TKey2>
            where TDbContext : DbContext
    {
        struct CompoundKey : IComparable<CompoundKey>
        {
            readonly TKey1 Key1;
            readonly TKey2 Key2;

            public CompoundKey(TKey1 key1, TKey2 key2)
            {
                Key1 = key1;
                Key2 = key2;
            }

            public int CompareTo(CompoundKey other)
            {
                var key11 = Key1.ToString();
                var key21 = other.Key1.ToString();

                var result = key11.Length.CompareTo(key21.Length);

                if (result != 0)
                    return result;

                result = key11.CompareTo(key21);

                if (result != 0)
                    return result;

                var key12 = Key2.ToString();
                var key22 = other.Key2.ToString();

                result = key12.Length.CompareTo(key22.Length);

                if (result != 0)
                    return result;

                return key12.CompareTo(key22);
            }
        }

        protected virtual TApiModel FromModel(TModel model)
            => model != null ? Node.Mapper.Map<TModel, TApiModel>(model, opts =>
            opts.ConfigureMap(AutoMapper.MemberList.None)) : default(TApiModel);

        protected virtual TModel ToModel(TApiModel apiModel)
            => apiModel != null ? Node.Mapper.Map<TApiModel, TModel>(apiModel, opts =>
            opts.ConfigureMap(AutoMapper.MemberList.None)) : default(TModel);

        protected virtual Task<TModel> FindModelAsync(KeyApiModel<TKey1, TKey2> keyApiModel)
            => DbContext.FindAsync<TModel>(keyApiModel.Id1, keyApiModel.Id2);

        public virtual async Task<TApiModel> Read(KeyApiModel<TKey1, TKey2> keyApiModel)
            => FromModel(await FindModelAsync(keyApiModel).ConfigureAwait(false));

        public virtual async Task<KeyApiModel<TKey1, TKey2>> Create(TApiModel apiModel)
        {
            var model = DbContext.Add(ToModel(apiModel)).Entity;
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
            return new KeyApiModel<TKey1, TKey2> { Id1 = model.Id1, Id2 = model.Id2 };
        }

        public virtual async Task Delete(KeyApiModel<TKey1, TKey2> keyApiModel)
        {
            var model = Activator.CreateInstance<TModel>();
            model.Id1 = keyApiModel.Id1;
            model.Id2 = keyApiModel.Id2;
            model = DbContext.Remove(model).Entity;
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public virtual async Task Update(TApiModel apiModel)
        {
            var model = DbContext.Update(ToModel(apiModel)).Entity;
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        //public virtual async Task<IEnumerable<TApiModel>> List(ListApiModel model)
        //{
        //    var entries = new List<TApiModel>();
        //    //var items = await Query(DbContext.Set<TModel>().OrderBy(p => new { p.Id1, p.Id2 }), model).ToListAsync();

        //    var items = await Query(DbContext.Set<TModel>().OrderBy(p => new CompoundKey(p.Id1, p.Id2)), model).ToListAsync();

        //    foreach (var item in items)
        //    {
        //        var apiModel = FromModel(item);
        //        entries.Add(apiModel);
        //    }

        //    return entries;
        //}

        public virtual Task<ResponseListApiModel<TApiModel>> List(ListApiModel model)
            => List<TApiModel, TModel, TKey1, TKey2>(model);

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
        //                items = await Query(DbContext.Set<TModel>().OrderBy(p => new CompoundKey(p.Id1, p.Id2)), model).ToListAsync();
        //            else
        //                items = await Query(DbContext.Set<TModel>().OrderByDescending(p => new CompoundKey(p.Id1, p.Id2)), model).ToListAsync();
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
    }

    public abstract class Controller<TModel, TKey1, TKey2, TKey3, TApiModel, TDbContext> : DbContextController<TDbContext>
            where TModel : KeyApiModel<TKey1, TKey2, TKey3>
            where TDbContext : DbContext
    {
        struct CompoundKey : IComparable<CompoundKey>
        {
            readonly TKey1 Key1;
            readonly TKey2 Key2;
            readonly TKey3 Key3;

            public CompoundKey(TKey1 key1, TKey2 key2, TKey3 key3)
            {
                Key1 = key1;
                Key2 = key2;
                Key3 = key3;
            }

            public int CompareTo(CompoundKey other)
            {
                var key11 = Key1.ToString();
                var key21 = other.Key1.ToString();

                var result = key11.Length.CompareTo(key21.Length);

                if (result != 0)
                    return result;

                result = key11.CompareTo(key21);

                if (result != 0)
                    return result;

                var key12 = Key2.ToString();
                var key22 = other.Key2.ToString();

                result = key12.Length.CompareTo(key22.Length);

                if (result != 0)
                    return result;

                result = key12.CompareTo(key22);

                if (result != 0)
                    return result;

                var key13 = Key3.ToString();
                var key23 = other.Key3.ToString();

                result = key13.Length.CompareTo(key23.Length);

                if (result != 0)
                    return result;

                return key13.CompareTo(key23);
            }
        }

        protected virtual TApiModel FromModel(TModel model)
            => model != null ? Node.Mapper.Map<TModel, TApiModel>(model, opts =>
            opts.ConfigureMap(AutoMapper.MemberList.Destination)) : default(TApiModel);

        protected virtual TModel ToModel(TApiModel apiModel)
            => apiModel != null ? Node.Mapper.Map<TApiModel, TModel>(apiModel, opts =>
            opts.ConfigureMap(AutoMapper.MemberList.Source)) : default(TModel);

        protected virtual Task<TModel> FindModelAsync(KeyApiModel<TKey1, TKey2, TKey3> keyApiModel)
            => DbContext.FindAsync<TModel>(keyApiModel.Id1, keyApiModel.Id2, keyApiModel.Id3);

        public virtual async Task<TApiModel> Read(KeyApiModel<TKey1, TKey2, TKey3> keyApiModel)
            => FromModel(await FindModelAsync(keyApiModel).ConfigureAwait(false));

        public virtual async Task<KeyApiModel<TKey1, TKey2>> Create(TApiModel apiModel)
        {
            var model = DbContext.Add(ToModel(apiModel)).Entity;
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
            return new KeyApiModel<TKey1, TKey2, TKey3> { Id1 = model.Id1, Id2 = model.Id2, Id3 = model.Id3 };
        }

        public virtual async Task Delete(KeyApiModel<TKey1, TKey2, TKey3> keyApiModel)
        {
            var model = Activator.CreateInstance<TModel>();
            model.Id1 = keyApiModel.Id1;
            model.Id2 = keyApiModel.Id2;
            model.Id3 = keyApiModel.Id3;
            model = DbContext.Remove(model).Entity;
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public virtual async Task Update(TApiModel apiModel)
        {
            var model = DbContext.Update(ToModel(apiModel)).Entity;
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        //public virtual async Task<IEnumerable<TApiModel>> List(ListApiModel model)
        //{
        //    var entries = new List<TApiModel>();
        //    var items = await Query(DbContext.Set<TModel>().OrderBy(p => new CompoundKey(p.Id1, p.Id2, p.Id3)), model).ToListAsync();

        //    foreach (var item in items)
        //    {
        //        var apiModel = FromModel(item);
        //        entries.Add(apiModel);
        //    }

        //    return entries;
        //}

        public virtual async Task<ResponseListApiModel<TApiModel>> List(ListApiModel model)
        {
            var count = 0;
            var entries = new List<TApiModel>();
            var items = default(IEnumerable<TModel>);

            if (model.CountOnly)
                count = await Query(DbContext.Set<TModel>(), model).CountAsync().ConfigureAwait(false);
            else
            {
                if (string.IsNullOrEmpty(model.OrderBy))
                {
                    if (!model.OrderByDescending)
                        items = await Query(DbContext.Set<TModel>().OrderBy(p => new CompoundKey(p.Id1, p.Id2, p.Id3)), model).ToListAsync();
                    else
                        items = await Query(DbContext.Set<TModel>().OrderByDescending(p => new CompoundKey(p.Id1, p.Id2, p.Id3)), model).ToListAsync();
                }
                else
                {
                    if (!model.OrderByDescending)
                        items = await Query(DbContext.Set<TModel>().OrderBy(model.OrderBy), model).ToListAsync();
                    else
                        items = await Query(DbContext.Set<TModel>().OrderByDescending(model.OrderBy), model).ToListAsync();
                }

                foreach (var item in items)
                {
                    var apiModel = FromModel(item);
                    entries.Add(apiModel);
                }

                count = entries.Count;
            }

            return new ResponseListApiModel<TApiModel>
            {
                Items = entries,
                Count = count,
                TotalCount = await DbContext.Set<TModel>().CountAsync().ConfigureAwait(false),
            };
        }
    }
    */
}

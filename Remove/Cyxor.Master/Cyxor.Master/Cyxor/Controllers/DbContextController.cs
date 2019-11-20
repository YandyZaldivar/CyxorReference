using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

namespace Cyxor.Controllers
{
    using Models;

    public abstract class DbContextController<TDbContext> : DbContextMasterController
        where TDbContext : DbContext
    {
        protected TDbContext DbContext { get; set; }

        [ScopeInitializer]
        public virtual void InitializeDbContext(TDbContext dbContext) => DbContext = dbContext;

        protected virtual async Task<ResponseReadApiModel<TModel>> Read<TModel>(ReadApiModel readApiModel, IQueryable<TModel> queryable = null)
            where TModel : class
        {
            var count = 0;
            var items = default(IEnumerable<TModel>);

            queryable = queryable ?? DbContext.Set<TModel>();

            queryable = Read(queryable, readApiModel);

            if (readApiModel.CountOnly)
                count = await queryable.CountAsync().ConfigureAwait(false);
            else
            {
                items = await queryable.ToListAsync().ConfigureAwait(false);
                count = items.Count();
            }

            return new ResponseReadApiModel<TModel>
            {
                Items = items,
                Count = count,
                //TotalCount = await DbContext.Set<TModel>().CountAsync().ConfigureAwait(false),
                TotalCount = await DbContext.Set<TModel>().CountAsync().ConfigureAwait(false),
            };
        }

        /*
        protected virtual async Task<ResponseListApiModel<TApiModel>> List<TApiModel, TModel>(ListApiModel model, IQueryable<TModel> queryable = null)
            where TModel : class
        {
            var count = 0;
            var entries = new List<TApiModel>();
            //var items = await Query(DbContext.Set<TModel>().OrderBy(p => p.Id), model).ToListAsync();

            var items = default(IEnumerable<TModel>);

            //            items = await Query(DbContext.Set<TModel>()
            //#if !NET451 && !NETSTANDARD1_3
            //                .Where(p => EF.Functions.Like(, "a%"))
            //#endif
            //            .OrderBy(p => p.Id), model).ToListAsync();




            queryable = queryable ?? DbContext.Set<TModel>();

            queryable = Read(queryable, model);

            if (model.CountOnly)
                count = await queryable.CountAsync().ConfigureAwait(false);
            else
            {
                items = await queryable.ToListAsync().ConfigureAwait(false);

                foreach (var item in items)
                {
                    var apiModel = FromModel<TApiModel, TModel>(item);
                    entries.Add(apiModel);
                }

                count = entries.Count;
            }

            return new ResponseListApiModel<TApiModel>
            {
                Items = entries,
                Count = count,
                //TotalCount = await DbContext.Set<TModel>().CountAsync().ConfigureAwait(false),
                TotalCount = await DbContext.Set<TModel>().CountAsync().ConfigureAwait(false),
            };
        }
        */

        //protected virtual async Task<ResponseListApiModel<TApiModel>> List<TApiModel, TModel, TKey>(ListApiModel model, IQueryable<TModel> queryable = null, bool tryDbContext = false)
        //    where TModel : class, IKeyApiModel<TKey>
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

        //    queryable = queryable ?? DbContext.Set<TModel>();
        //    //queryable = tryDbContext ? queryable : DbContext.Set<TModel>();

        //    queryable = Query(queryable, model);

        //    if (model.CountOnly)
        //        count = await queryable.CountAsync().ConfigureAwait(false);
        //    else
        //    {
        //        items = await queryable.ToListAsync().ConfigureAwait(false);

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
        //        //TotalCount = await DbContext.Set<TModel>().CountAsync().ConfigureAwait(false),
        //        TotalCount = await DbContext.Set<TModel>().CountAsync().ConfigureAwait(false),
        //    };
        //}
    }
}

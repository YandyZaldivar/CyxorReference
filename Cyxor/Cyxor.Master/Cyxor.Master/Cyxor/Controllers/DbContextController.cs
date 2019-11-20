using System;
using System.Linq;
using System.Reflection;
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

        protected virtual async Task<ResponseReadApiModel<TModel>> ReadQuery<TModel>(ReadApiModel readApiModel, IQueryable<TModel> query = null)
            where TModel : class
        {
            var count = 0;
            var total = 0;
            var items = default(IEnumerable<TModel>);

            query = query ?? DbContext.Set<TModel>();

            total = await query.CountAsync().ConfigureAwait(false);

            if (readApiModel != null)
                query = Query(query, readApiModel);

            if (readApiModel?.CountOnly ?? false)
                count = await query.CountAsync().ConfigureAwait(false);
            else
            {
                items = await query.ToListAsync().ConfigureAwait(false);
                count = items.Count();
            }

            var response = new ResponseReadApiModel<TModel> { Items = items, Count = count, Total = total };

            // TODO: Review and redesign when ef core implements 'include filters'.
            // This recursive code can be removed to go back to all include behavior.
            if (readApiModel?.Includes != null)
                foreach (var item in response.Items)
                    foreach (var include in readApiModel.Includes)
                    {
                        var navigationEntry = DbContext.Entry(item).Navigation(include.Property);

                        var propertyType = default(Type);

                        if (!navigationEntry.Metadata.IsCollection())
                        {
                            if (include.Criteria != null)
                                throw new InvalidOperationException("Expected a collection navigation property." +
                                    $" '{navigationEntry.Metadata.Name}' is a single navigation property.");

                            propertyType = navigationEntry.Metadata.ClrType;
                        }
                        else
                            propertyType = navigationEntry.Metadata.GetTargetType().ClrType;

                        var navigationQuery = navigationEntry.Query();

                        var method = GetType().GetRuntimeMethods().Where(p => p.Name == nameof(ReadQuery)).Single().MakeGenericMethod(propertyType);

                        var methodResult = method.Invoke(this, new object[] { include.Criteria, navigationQuery });

                        await (methodResult as Task).ConfigureAwait(false);
                    }

            return response;
        }
    }
}

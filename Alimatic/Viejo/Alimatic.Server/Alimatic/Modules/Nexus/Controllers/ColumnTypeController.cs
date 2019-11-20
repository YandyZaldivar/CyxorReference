using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

namespace Alimatic.Nexus.Controllers
{
    using Data;
    using Models;

    using Cyxor.Controllers;

    class ColumnTypeController : BaseController
    {
        //[Action(ApiId.GetAllColumnTypes)]
        public async Task<IEnumerable<ColumnTypeApiModel>> GetAllColumnTypes()
        {
            var entries = new List<ColumnTypeApiModel>();

            foreach (var entry in await NexusDbContext.ColumnTypes.AsNoTracking().ToListAsync())
                entries.Add(new ColumnTypeApiModel { Id = entry.Id, Name = entry.Name });

            return entries;
        }

        //[Command("nexus column-type list", Description = "Get all column-types in the Nexus.")]
        //public Task<ColumnTypesApiModel> GetAllColumnTypes(CommandArgs args) => InvokeAsync<ColumnTypesApiModel>();
    }
}

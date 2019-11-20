using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

namespace Alimatic.Nexus.Controllers
{
    using Data;
    using Models;

    using Cyxor.Controllers;

    class SecurityController : BaseController
    {
        //[Action(ApiId.GetAllSecurities)]
        public async Task<IEnumerable<SecurityApiModel>> GetAllSecurities()
        {
            var entries = new List<SecurityApiModel>();

            foreach (var entry in await NexusDbContext.Securities.AsNoTracking().ToListAsync())
                entries.Add(new SecurityApiModel { Id = entry.Id, Name = entry.Name });

            return entries;
        }

        //[Command("nexus security list", Description = "Get all securities in the Nexus.")]
        //public Task<SecuritiesApiModel> GetAllSecurities(CommandArgs args) => InvokeAsync<SecuritiesApiModel>();
    }
}

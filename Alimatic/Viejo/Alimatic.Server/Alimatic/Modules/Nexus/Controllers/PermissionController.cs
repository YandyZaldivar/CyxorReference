using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

namespace Alimatic.Nexus.Controllers
{
    using Data;
    using Models;

    using Cyxor.Controllers;

    class PermissionController : BaseController
    {
        //[Action(ApiId.GetAllPermissions)]
        public async Task<IEnumerable<PermissionApiModel>> GetAllPermissions()
        {
            var entries = new List<PermissionApiModel>();

            foreach (var entry in await NexusDbContext.Permissions.AsNoTracking().ToListAsync())
                entries.Add(new PermissionApiModel { Id = entry.Id, Name = entry.Name });

            return entries;
        }

        //[Command("nexus permission list", Description = "Get all permissions in the Nexus.")]
        //public Task<PermissionsApiModel> GetAllPermissions(CommandArgs args) => InvokeAsync<PermissionsApiModel>();
    }
}

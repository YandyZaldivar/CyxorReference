using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

namespace Alimatic.Nexus.Controllers
{
    using Data;
    using Models;

    using Cyxor.Controllers;

    class RoleController : BaseController
    {
        #region Get
        //[Action(ApiId.GetRole)]
        async Task<RoleApiModel> GetRole(GetRoleApiModel getRoleApiModel)
        {
            var entry = await NexusDbContext.Roles.AsNoTracking().SingleAsync(p =>
                getRoleApiModel.IsId ? p.Id == getRoleApiModel.Id : p.Name == getRoleApiModel.Name);

            return new RoleApiModel { Id = entry.Id, Name = entry.Name };
        }

        //[Command("nexus role get", Arguments = "$nameOrId",
        //    Description = "Get the Nexus role identified by $nameOrId.")]
        //public Task<RoleApiModel> GetRole(CommandArgs args) => InvokeAsync<RoleApiModel>(new GetRoleApiModel { NameOrId = args["$nameOrId"] });
        #endregion

        #region Add
        //[Action(ApiId.AddRole)]
        async Task<RoleApiModel> AddRole(AddRoleApiModel addRoleApiModel)
        {
            var entry = NexusDbContext.Roles.Add(new Role { Name = addRoleApiModel.Name }).Entity;
            await NexusDbContext.SaveChangesAsync();
            return new RoleApiModel { Id = entry.Id, Name = entry.Name };
        }

        //[Command("nexus role add", Arguments = "$name",
        //    Description = "Add a new Nexus role with the specified $name.")]
        //public Task<RoleApiModel> AddRole(CommandArgs args) => InvokeAsync<RoleApiModel>(new AddRoleApiModel { Name = args["$name"] });
        #endregion

        #region Remove
        //[Action(ApiId.RemoveRole)]
        async Task RemoveRole(RemoveRoleApiModel removeRoleApiModel)
        {
            var entry = removeRoleApiModel.IsId ? new Role { Id = removeRoleApiModel.Id ?? 0 } :
                await NexusDbContext.Roles.SingleAsync(p => p.Name == removeRoleApiModel.Name);

            NexusDbContext.Roles.Remove(entry);
            await NexusDbContext.SaveChangesAsync();
        }

        //[Command("nexus role remove", Arguments = "$nameOrId",
        //    Description = "Remove the Nexus role with the specified $nameOrId.")]
        //public Task RemoveRole(CommandArgs args) => InvokeActionAsync(new RemoveRoleApiModel { NameOrId = args["$nameOrId"] });
        #endregion

        #region Update
        //[Action(ApiId.UpdateRole)]
        //public async Task UpdateRole(UpdateRoleApiModel updateRoleApiModel)
        //{
        //    var entry = updateRoleApiModel.IsId ? new Role { Id = updateRoleApiModel.Id ?? 0 } :
        //        await NexusDbContext.Roles.SingleAsync(p => p.Name == updateRoleApiModel.Name);

        //    entry.Name = updateRoleApiModel.NewName;

        //    NexusDbContext.Roles.Update(entry);
        //    await NexusDbContext.SaveChangesAsync();
        //}

        //[Command("nexus role update", Arguments = "$nameOrId $newName",
        //    Description = "Update the Nexus role with the specified $nameOrId with $newName.")]
        //public Task UpdateRole(CommandArgs args) => InvokeActionAsync(new UpdateRoleApiModel
        //{
        //    NameOrId = args["$nameOrId"],
        //    NewName = args["$newName"]
        //});
        #endregion

        #region GetAll
        //[Action(ApiId.GetAllRoles)]
        async Task<IEnumerable<RoleApiModel>> GetAllRoles()
        {
            var entries = new List<RoleApiModel>();

            foreach (var item in await NexusDbContext.Roles.AsNoTracking().ToListAsync())
                entries.Add(new RoleApiModel { Id = item.Id, Name = item.Name });

            return entries;
        }

        //[Command("nexus role list", Description = "Get all roles in the Nexus.")]
        //public Task<RolesApiModel> GetAllRoles(CommandArgs args) => InvokeAsync<RolesApiModel>();
        #endregion
    }
}

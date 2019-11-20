using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

namespace Alimatic.Nexus.Controllers
{
    using Data;
    using Models;

    using Cyxor.Controllers;

    class UserRoleController : BaseController
    {
        UserRoleApiModel NewUserRoleApiModel(UserRole userRole) =>
            new UserRoleApiModel
            {
                UserId = userRole.UserId,
                RoleId = userRole.RoleId,
            };

        #region Get
        //[Action(ApiId.GetUserRole)]
        public async Task<UserRoleApiModel> GetUserRole(GetUserRoleApiModel getUserRoleApiModel)
        {
            var user = await NexusDbContext.Users.AsNoTracking().SingleAsync(p =>
                getUserRoleApiModel.UserModel.IsId ? p.Id == getUserRoleApiModel.UserModel.Id : p.Name == getUserRoleApiModel.UserModel.Name);

            var role = await NexusDbContext.Roles.AsNoTracking().SingleAsync(p =>
                getUserRoleApiModel.RoleModel.IsId ? p.Id == getUserRoleApiModel.RoleModel.Id : p.Name == getUserRoleApiModel.RoleModel.Name);

            return new UserRoleApiModel { UserId = user.Id, RoleId = role.Id };
        }

        //[Command("nexus user-role get", Arguments = "$user $role",
        //    Description = "Get the Nexus user-role identified by $userNameOrId and $roleNameOrId.")]
        //public Task<UserRoleApiModel> GetUserRole(CommandArgs args) => InvokeAsync<UserRoleApiModel>(new GetUserRoleApiModel
        //{
        //    UserModel = new UserKeyApiModel { NameOrId = args["$user"] },
        //    RoleModel = new RoleKeyApiModel { NameOrId = args["$role"] },
        //});
        #endregion

        #region Add
        //[Action(ApiId.AddUserRole)]
        public async Task<UserRoleApiModel> AddUserRole(AddUserRoleApiModel addUserRoleApiModel)
        {
            var user = await NexusDbContext.Users.AsNoTracking().SingleAsync(p =>
                addUserRoleApiModel.UserModel.IsId ? p.Id == addUserRoleApiModel.UserModel.Id : p.Name == addUserRoleApiModel.UserModel.Name);

            var role = await NexusDbContext.Roles.AsNoTracking().SingleAsync(p =>
                addUserRoleApiModel.RoleModel.IsId ? p.Id == addUserRoleApiModel.RoleModel.Id : p.Name == addUserRoleApiModel.RoleModel.Name);

            var entry = NexusDbContext.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = role.Id }).Entity;

            await NexusDbContext.SaveChangesAsync();

            return new UserRoleApiModel { UserId = entry.UserId, RoleId = entry.RoleId };
        }

        //[Command("nexus user-role add", Arguments = "$user $role",
        //    Description = "Establishes a new Nexus user-role association.")]
        //public Task<UserRoleApiModel> AddUserRole(CommandArgs args) => InvokeAsync<UserRoleApiModel>(new AddUserRoleApiModel
        //{
        //    UserModel = new UserKeyApiModel { NameOrId = args["$user"] },
        //    RoleModel = new RoleKeyApiModel { NameOrId = args["$role"] },
        //});
        #endregion

        #region Remove
        //[Action(ApiId.RemoveUserRole)]
        public async Task<UserRoleApiModel> RemoveUserRole(RemoveUserRoleApiModel removeUserRoleApiModel)
        {
            var user = await NexusDbContext.Users.AsNoTracking().SingleAsync(p =>
                removeUserRoleApiModel.UserModel.IsId ? p.Id == removeUserRoleApiModel.UserModel.Id : p.Name == removeUserRoleApiModel.UserModel.Name);

            var role = await NexusDbContext.Roles.AsNoTracking().SingleAsync(p =>
                removeUserRoleApiModel.RoleModel.IsId ? p.Id == removeUserRoleApiModel.RoleModel.Id : p.Name == removeUserRoleApiModel.RoleModel.Name);

            var userRole = NexusDbContext.UserRoles.Remove(new UserRole { UserId = user.Id, RoleId = role.Id }).Entity;

            await NexusDbContext.SaveChangesAsync();

            return NewUserRoleApiModel(userRole);
        }

        //[Command("nexus user-role remove", Arguments = "$user $role",
        //    Description = "Remove the Nexus user-role association identified by $user and $role.")]
        //public Task<UserRoleApiModel> RemoveUserRole(CommandArgs args) => InvokeAsync<UserRoleApiModel>(new RemoveUserRoleApiModel
        //{
        //    UserModel = new UserKeyApiModel { NameOrId = args["$user"] },
        //    RoleModel = new RoleKeyApiModel { NameOrId = args["$role"] },
        //});
        #endregion

        #region Update
        //[Action(ApiId.UpdateUserRole)]
        public async Task<UserRoleApiModel> UpdateUserRole(UpdateUserRoleApiModel updateUserRoleApiModel)
        {
            var userEntry = await NexusDbContext.Users.AsNoTracking().SingleAsync(p =>
                updateUserRoleApiModel.UserModel.IsId ? p.Id == updateUserRoleApiModel.UserModel.Id : p.Name == updateUserRoleApiModel.UserModel.Name);

            var roleEntry = await NexusDbContext.Roles.AsNoTracking().SingleAsync(p =>
                updateUserRoleApiModel.RoleModel.IsId ? p.Id == updateUserRoleApiModel.RoleModel.Id : p.Name == updateUserRoleApiModel.RoleModel.Name);

            var newUserEntry = await NexusDbContext.Users.AsNoTracking().SingleAsync(p =>
                updateUserRoleApiModel.NewUserModel.IsId ? p.Id == updateUserRoleApiModel.NewUserModel.Id : p.Name == updateUserRoleApiModel.NewUserModel.Name);

            var newRoleEntry = await NexusDbContext.Roles.AsNoTracking().SingleAsync(p =>
                updateUserRoleApiModel.NewRoleModel.IsId ? p.Id == updateUserRoleApiModel.NewRoleModel.Id : p.Name == updateUserRoleApiModel.NewRoleModel.Name);

            var userRole = NexusDbContext.UserRoles.Find(userEntry.Id, roleEntry.Id);

            userRole.UserId = newUserEntry.Id;
            userRole.RoleId = newRoleEntry.Id;

            await NexusDbContext.SaveChangesAsync();

            return NewUserRoleApiModel(userRole);
        }

        //[Command("nexus user-role update", Arguments = "$user $role $new-user $new-role",
        //    Description = "Update the Nexus user-role identified by $userNameOrId and $roleNameOrId with $newUserNameOrId and $newRoleNameOrId.")]
        //public Task<UserRoleApiModel> UpdateUserRole(CommandArgs args) => InvokeAsync<UserRoleApiModel>(new UpdateUserRoleApiModel
        //{
        //    UserModel = new UserKeyApiModel { NameOrId = args["$user"] },
        //    RoleModel = new RoleKeyApiModel { NameOrId = args["$role"] },
        //    NewUserModel = new UserKeyApiModel { NameOrId = args["$new-user"] },
        //    NewRoleModel = new RoleKeyApiModel { NameOrId = args["$new-role"] },
        //});
        #endregion

        #region GetAll
        //[Action(ApiId.GetAllUserRoles)]
        public async Task<IEnumerable<UserRoleApiModel>> GetAllUserRoles()
        {
            var entries = new List<UserRoleApiModel>();

            foreach (var entry in await NexusDbContext.UserRoles.AsNoTracking().ToListAsync())
                entries.Add(new UserRoleApiModel { UserId = entry.UserId, RoleId = entry.RoleId });

            return entries;
        }

        //[Command("nexus user-role list", Description = "Get all user-roles in the Nexus.")]
        //public Task<UserRolesApiModel> GetAllUserRoles(CommandArgs args) => InvokeAsync<UserRolesApiModel>();
        #endregion
    }
}

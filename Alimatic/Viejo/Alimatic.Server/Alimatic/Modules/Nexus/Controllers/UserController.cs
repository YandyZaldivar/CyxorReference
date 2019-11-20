using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

namespace Alimatic.Nexus.Controllers
{
    using Data;
    using Models;

    using Cyxor.Models;
    using Cyxor.Controllers;

    using Accounts.Data;

    class UserController : BaseController
    {
        protected AccountsDbContext AccountsDbContext;

        [ScopeInitializer]
        public virtual void InitializeAccountScope(AccountsDbContext accountsDbContext) => AccountsDbContext = accountsDbContext;

        #region List
        //[Action(ApiId.UserList)]
        public async Task<IEnumerable<UserApiModel>> List()
        {
            var entries = new List<UserApiModel>();

            foreach (var entry in await NexusDbContext.Users.AsNoTracking().ToListAsync())
                entries.Add(new UserApiModel { Id = entry.Id, Name = entry.Name, SecurityId = entry.SecurityId, AccountId = entry.AccountId });

            return entries;
        }

        //[Command("nexus user list", Description = "Get all users in the Nexus")]
        //public Task<UsersApiModel> List(CommandArgs args) => InvokeAsync<UsersApiModel>();
        #endregion

        #region Get
        //[Action(ApiId.UserGet)]
        public async Task<UserApiModel> Get(UserGetApiModel getUserApiModel)
        {
            var entry = await NexusDbContext.Users.AsNoTracking().SingleAsync(p =>
                getUserApiModel.IsId ? p.Id == getUserApiModel.Id : p.Name == getUserApiModel.Name);

            return new UserApiModel { Id = entry.Id, AccountId = entry.AccountId, Name = entry.Name, SecurityId = entry.SecurityId };
        }

        //[Command("nexus user get", Arguments = "$user", Description = "Get the Nexus $user")]
        //public Task<UserApiModel> Get(CommandArgs args) => InvokeAsync<UserApiModel>(new UserGetApiModel { NameOrId = args["$nameOrId"] });
        #endregion

        #region Add
        //[Action(ApiId.UserAdd)]
        public async Task<UserApiModel> Add(UserAddApiModel addUserApiModel)
        {
            var security = default(Security);

            if (addUserApiModel.SecurityModel != null)
                security = addUserApiModel.SecurityModel == null ? null : await NexusDbContext.Securities.AsNoTracking().SingleAsync(p =>
                    addUserApiModel.SecurityModel.IsId ? p.Id == addUserApiModel.SecurityModel.Id : p.Name == addUserApiModel.SecurityModel.Name);

            if (security == null)
                security = await NexusDbContext.Securities.SingleAsync(p => p.Name == nameof(SecurityValue.User));

            var account = default(Account);
            if (addUserApiModel.AccountModel != null)
                account = await AccountsDbContext.Accounts.SingleAsync(p => p.Id == addUserApiModel.AccountModel.Id);

            var entry = NexusDbContext.Users.Add(new User { Name = addUserApiModel.Name, AccountId = account?.Id, SecurityId = security.Id }).Entity;

            await NexusDbContext.SaveChangesAsync();

            return new UserApiModel { Id = entry.Id, Name = entry.Name, SecurityId = entry.SecurityId, AccountId = entry.AccountId };
        }

        //[Command("nexus user add", Arguments = "$name [$account] [$security]",
        //    Description = "Add a new Nexus user with the specified $name, related [$account] and [$security]")]
        //public Task<UserApiModel> Add(CommandArgs args) => InvokeAsync<UserApiModel>(new UserAddApiModel
        //{
        //    Name = args["$name"],
        //    AccountModel = args["$account"] != null ? new AccountKeyApiModel { NameOrId = args["$account"] } : null,
        //    SecurityModel = args["$security"] != null ? new SecurityKeyApiModel { NameOrId = args["$security"] } : null,
        //});
        #endregion

        #region Remove
        //[Action(ApiId.UserRemove)]
        public async Task<UserApiModel> Remove(UserRemoveApiModel removeUserApiModel)
        {
            var entry = removeUserApiModel.IsId ? new User { Id = removeUserApiModel.Id ?? 0 } :
                await NexusDbContext.Users.SingleAsync(p => p.Name == removeUserApiModel.Name);

            NexusDbContext.Users.Remove(entry);
            await NexusDbContext.SaveChangesAsync();

            return new UserApiModel { Id = entry.Id, Name = entry.Name, SecurityId = entry.SecurityId, AccountId = entry.AccountId };
        }

        //[Command("nexus user remove", Arguments = "$user", Description = "Remove the Nexus $user")]
        //public Task<UserApiModel> Remove(CommandArgs args) => InvokeAsync<UserApiModel>(new UserRemoveApiModel { NameOrId = args["$user"] });
        #endregion

        #region Update
        //[Action(ApiId.UserUpdate)]
        public async Task<UserApiModel> Update(UserUpdateApiModel updateUserApiModel)
        {
            var entry = updateUserApiModel.IsId ? new User { Id = updateUserApiModel.Id ?? 0 } :
                await NexusDbContext.Users.SingleAsync(p => p.Name == updateUserApiModel.Name);

            if (updateUserApiModel.NewNameModel != null)
                entry.Name = updateUserApiModel.NewNameModel.Name;

            if (updateUserApiModel.NewAccountModel != null)
            {
                var account = await AccountsDbContext.Accounts.SingleAsync(p => p.Id == updateUserApiModel.NewAccountModel.Id);

                entry.AccountId = account.Id;
            }

            if (updateUserApiModel.NewSecurityModel != null)
            {
                var securityEntry = updateUserApiModel.NewSecurityModel.IsId ? new Security { Id = updateUserApiModel.NewSecurityModel.Id ?? 0 } :
                    await NexusDbContext.Securities.SingleAsync(p => p.Name == updateUserApiModel.NewSecurityModel.Name);

                entry.SecurityId = securityEntry.Id;
            }

            NexusDbContext.Users.Update(entry);
            await NexusDbContext.SaveChangesAsync();

            return new UserApiModel { Id = entry.Id, Name = entry.Name, SecurityId = entry.SecurityId, AccountId = entry.AccountId };
        }

        //[Command("nexus user update", Arguments = "$user [$new-name] [$new-account] [$new-security]",
        //    Description = "Update the Nexus $user with the specified arguments")]
        //public Task<UserApiModel> Update(CommandArgs args) => InvokeAsync<UserApiModel>(new UserUpdateApiModel
        //{
        //    NameOrId = args["$user"],
        //    NewNameModel = args["$new-name"] != null ? new Cyxor.Models.NameApiModel { Name = args["$new-name"] } : null,
        //    NewAccountModel = args["$new-account"] != null ? new AccountKeyApiModel { NameOrId = args["$new-account"] } : null,
        //    NewSecurityModel = args["$new-security"] != null ? new SecurityKeyApiModel { NameOrId = args["$new-security"] } : null
        //});
        #endregion

        #region GetRoles
        //[Action(ApiId.UserGetRoles)]
        async Task<IEnumerable<Nexus.Models.RoleApiModel>> GetRoles(UserGetRolesApiModel getUserRolesApiModel)
        {
            var user = await NexusDbContext.Users.AsNoTracking().Include(p => p.Roles).ThenInclude(p => p.Role).SingleAsync(p =>
                getUserRolesApiModel.IsId ? p.Id == getUserRolesApiModel.Id : p.Name == getUserRolesApiModel.Name);

            var roleEntries = new List<Models.RoleApiModel>(user.Roles.Count);

            foreach (var role in user.Roles)
                roleEntries.Add(new Models.RoleApiModel { Id = role.RoleId, Name = role.Role.Name });

            return roleEntries;
        }

        //[Command("nexus user roles", Arguments = "$user", Description = "Get the Nexus $user roles")]
        //public Task<RolesApiModel> GetRoles(CommandArgs args) => InvokeAsync<RolesApiModel>(new UserGetRolesApiModel { NameOrId = args["$nameOrId"] });
        #endregion
    }
}

/*
  { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/>
  Copyright (C) 2017  Yandy Zaldivar

  This program is free software: you can redistribute it and/or modify
  it under the terms of the GNU Affero General Public License as
  published by the Free Software Foundation, either version 3 of the
  License, or (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU Affero General Public License for more details.

  You should have received a copy of the GNU Affero General Public License
  along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System.Threading.Tasks;

namespace Cyxor.Controllers
{
    using Data;
    using Models;

    public class AccountDbContextController<TModel> : Controller<TModel, MasterDbContext>
        where TModel : class
    { }

    //public class AccountController : Controller<Account, int, AccountApiModel, MasterDbContext>
    public class AccountController : AccountDbContextController<Account>
    {
        public override Task<Account> Create(Account model)
        {
            model.Password = new AuthRequest { I = model.Name, A = model.Password }.PasswordHash;
            return base.Create(model);
        }

        public override Task Update(Account model)
        {
            model.Password = new AuthRequest { I = model.Name, A = model.Password }.PasswordHash;
            return base.Update(model);
        }

        //[Action(typeof(AccountGetRolesApiModel))]
        //public async Task<IEnumerable<RoleApiModel>> GetRoles(AccountGetRolesApiModel model)
        //{
        //    var account = await MasterDbContext.Accounts.AsNoTracking().Include(p => p.Roles).ThenInclude
        //        (p => p.Role).SingleAsync(p => p.Id == model.Id);

        //    var roleEntries = new List<RoleApiModel>(account.Roles.Count);

        //    foreach (var role in account.Roles)
        //        roleEntries.Add(new RoleApiModel { Id = role.RoleId, Name = role.Role.Name });

        //    return roleEntries;
        //}
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */

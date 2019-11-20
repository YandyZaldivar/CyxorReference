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
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

namespace Alimatic.Coralsa.Controllers
{
    using Data;
    using Models;

    using Cyxor.Models;
    using Cyxor.Controllers;

    [Controller(Route = "coralsa user")]
    //public class UserController : Controller<User, int, UserApiModel, CoralsaDbContext>
    public class UserController : Controller<User, CoralsaDbContext>
    {
        public override Task<User> Create(User model)
        {
            model.Password = new AuthRequest { I = model.Email, A = model.Password }.PasswordHash;
            return base.Create(model);
        }

        public override async Task Update(User model)
        {
            var entity = DbContext.Attach(model);

            entity.Property(p => p.AccountId).IsModified = true;
            entity.Property(p => p.Email).IsModified = true;
            entity.Property(p => p.EnterpriseId).IsModified = true;
            entity.Property(p => p.Name).IsModified = true;
            entity.Property(p => p.Permission).IsModified = true;
            entity.Property(p => p.SecurityLevel).IsModified = true;

            if (model.Password != null)
            {
                entity.Entity.Password = new AuthRequest { I = model.Email, A = model.Password }.PasswordHash;
                entity.Property(p => p.Password).IsModified = true;
            }

            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            //apiModel.Password = new AuthRequest { I = apiModel.Email, A = apiModel.Password }.PasswordHash;
            //return base.Update(apiModel);
        }

        public override async Task<ResponseReadApiModel<User>> Read(ReadApiModel model)
        {
            var users = await base.Read(model);

            foreach (var user in users.Items)
                user.Password = null;

            return users;
        }

        public async Task<IEnumerable<ModelApiModel>> ModelList(KeyApiModel<int> keyApiModel)
        {
            var user = await DbContext.Users.Include(p => p.Models).SingleAsync(p => p.Id == keyApiModel.Id);

            var modelEntries = new List<ModelApiModel>(user.Models.Count);

            foreach (var model in user.Models)
                modelEntries.Add(new ModelApiModel { Id = model.ModelId, Description = model.Model.Description });

            return modelEntries;
        }

        public async Task<IEnumerable<Models.RoleApiModel>> RoleList(KeyApiModel<int> keyApiModel)
        {
            var user = await DbContext.Users.Include(p => p.Roles).SingleAsync(p => p.Id == keyApiModel.Id);

            var roleEntries = new List<Models.RoleApiModel>(user.Roles.Count);

            foreach (var role in user.Roles)
                roleEntries.Add(new Models.RoleApiModel { Id = role.RoleId, Name = role.Role.Name });

            return roleEntries;
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */

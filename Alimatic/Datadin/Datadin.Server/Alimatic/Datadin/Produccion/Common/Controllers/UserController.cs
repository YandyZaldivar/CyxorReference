// { Alimatic.Datadin } - Backend
// Copyright (C) 2018 Alimatic
// Author:  Yandy Zaldivar

using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

namespace Alimatic.Datadin.Produccion.Controllers
{
    using Data;
    using Models;

    using Cyxor.Models;
    using Cyxor.Controllers;

    public abstract class UserController<TDatadinDbContext> : Controller<User, TDatadinDbContext>
        where TDatadinDbContext : DatadinDbContext
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
            var users = await base.Read(model).ConfigureAwait(false);

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
// { Alimatic.Datadin } - Backend

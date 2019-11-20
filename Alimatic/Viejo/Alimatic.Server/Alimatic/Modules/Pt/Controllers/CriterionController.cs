﻿/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  Ramón Menéndez
            Yandy Zaldivar
*/

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

namespace Alimatic.Pt
{
    using Data;
    using Models;
    using Cyxor.Networking;
    using Cyxor.Controllers;
    using Cyxor.Serialization;

    public class CriterionController : IDisposable
    {
        PtDbContext DbContext { get; set; }

        public CriterionController()
        {
            DbContext = new PtDbContext();
        }

        //[Action(Route.GetCharges, Roles = new string[] { nameof(Role.Admin), nameof(Role.Worker) })]
        public async Task<Result> ListCriteriaAsync()
        {
            var charges = await InternalListChargesAsync();
            return new Result(model: charges);
        }

        internal Task<List<Charge>> InternalListChargesAsync()
        {
            return DbContext.Charges.ToListAsync();
        }

        //[Action(Route.GetCharge, Roles = new string[] { nameof(Role.Admin), nameof(Role.Worker) })]
        public async Task<Result> GetChargeAsync(IdViewModel idViewModel)
        {
            var charge = await InternalGetChargeAsync(idViewModel);
            return new Result(model: charge);
        }

        internal async Task<Charge> InternalGetChargeAsync(IdViewModel idViewModel)
        {
            return await DbContext.FindAsync<Charge>(idViewModel.Id);
        }

        //[Action(Route.AddCharge, Roles = new string[] { nameof(Role.Admin) })]
        public async Task<Result> AddChargeAsync(Charge charge)
        {
            await InternalAddChargeAsync(charge);
            return Result.Success;
        }

        internal async Task InternalAddChargeAsync(Charge charge)
        {
            DbContext.Add(charge);
            await DbContext.SaveChangesAsync();
        }

        //[Action(Route.UpdateCharge, Roles = new string[] { nameof(Role.Admin) })]
        public async Task<Result> UpdateChargeAsync(Charge charge)
        {
            await InternalUpdateChargeAsync(charge);
            return Result.Success;
        }

        internal async Task InternalUpdateChargeAsync(Charge charge)
        {
            DbContext.Update(charge);
            await DbContext.SaveChangesAsync();
        }

        //[Action(Route.DeleteCharge, Roles = new string[] { nameof(Role.Admin) })]
        public async Task<Result> RemoveChargeAsync(NameOrIdViewModel nameOrIdViewModel)
        {
            await InternalRemoveChargeAsync(nameOrIdViewModel);
            return Result.Success;
        }

        internal async Task InternalRemoveChargeAsync(NameOrIdViewModel nameOrIdViewModel)
        {
            var charge = default(Charge);

            if (int.TryParse(nameOrIdViewModel.NameOrId, out var id))
                charge = await DbContext.FindAsync<Charge>(id);
            else
                charge = await DbContext.Charges.SingleAsync(p => p.Name == nameOrIdViewModel.NameOrId);

            DbContext.Remove(charge);
            await DbContext.SaveChangesAsync();
        }

        void IDisposable.Dispose() => DbContext?.Dispose();
    }
}
/* { Alimatic.Server } */
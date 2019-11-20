/*
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

    public class WorkerController : IDisposable
    {
        PtDbContext DbContext { get; set; }

        public WorkerController()
        {
            DbContext = new PtDbContext();
        }

        //[Action(Route.GetCharges, Roles = new string[] { nameof(Role.Admin), nameof(Role.Worker) })]
        public async Task<Result> ListWorkersAsync()
        {
            var charges = await InternalListWorkersAsync();
            return new Result(model: charges);
        }

        internal Task<List<Charge>> InternalListWorkersAsync()
        {
            return DbContext.Charges.ToListAsync();
        }

        //[Action(Route.GetCharge, Roles = new string[] { nameof(Role.Admin), nameof(Role.Worker) })]
        public async Task<Result> FindWorkerAsync(IdViewModel idViewModel)
        {
            var charge = await InternalFindWorkerAsync(idViewModel);
            return new Result(model: charge);
        }

        internal async Task<Charge> InternalFindWorkerAsync(IdViewModel idViewModel)
        {
            return await DbContext.FindAsync<Charge>(idViewModel.Id);
        }

        //[Action(Route.AddCharge, Roles = new string[] { nameof(Role.Admin) })]
        public async Task<Result> AddWorkerAsync(Charge charge)
        {
            await InternalAddWorkerAsync(charge);
            return Result.Success;
        }

        internal async Task InternalAddWorkerAsync(Charge charge)
        {
            DbContext.Add(charge);
            await DbContext.SaveChangesAsync();
        }

        //[Action(Route.UpdateCharge, Roles = new string[] { nameof(Role.Admin) })]
        public async Task<Result> UpdateWorkerAsync(Charge charge)
        {
            await InternalUpdateWorkerAsync(charge);
            return Result.Success;
        }

        internal async Task InternalUpdateWorkerAsync(Charge charge)
        {
            DbContext.Update(charge);
            await DbContext.SaveChangesAsync();
        }

        //[Action(Route.DeleteCharge, Roles = new string[] { nameof(Role.Admin) })]
        public async Task<Result> RemoveWorkerAsync(NameOrIdViewModel nameOrIdViewModel)
        {
            await InternalRemoveWorkerAsync(nameOrIdViewModel);
            return Result.Success;
        }

        internal async Task InternalRemoveWorkerAsync(NameOrIdViewModel nameOrIdViewModel)
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

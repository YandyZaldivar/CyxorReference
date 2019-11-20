/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using System;
using System.Threading.Tasks;

namespace Alimatic.Pt
{
    using Data;
    using Models;

    using Cyxor.Networking;
    using Cyxor.Controllers;

    public class ActivityController : IDisposable
    {
        PtDbContext DbContext { get; set; }

        public ActivityController()
        {
            DbContext = new PtDbContext();
        }

        //[Action(Route.AddActivity)]
        public Task AddActivity(Packet packet)
        {
            packet.Node.Log("testing");
            return Utilities.Task.CompletedTask;

            //var addActivityViewModel = packet.GetMessage<AddActivityViewModel>();
            //DbContext.Add(addActivityViewModel);
            //await DbContext.SaveChangesAsync();

            //using (var reply = new Packet(packet) { Message = Result.Success })
            //    await reply.SendAsync();
        }

        //[Action(Route.DeleteActivity)]
        public async Task DeleteActivity(Packet packet)
        {
            var activity = packet.GetModel<Activity>();
            DbContext.Add(activity);
            await DbContext.SaveChangesAsync();

            using (var reply = new Packet(packet) { Model = Result.Success })
                await reply.SendAsync();
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
/* { Alimatic.Server } */

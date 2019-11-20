using System;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Cyxor.Controllers
{
    using Models;
    using Cyxor.Networking;

    public class MasterApiController : MasterController
    {
        public async Task Dump()
        {
            await Node.Database.Engine.DumpWriteAsync();
        }

        //#region List
        //[Action(MasterApiId.ApiList, @internal: true)]
        //public IEnumerable<object> ApiList()
        //{
        //    foreach (var name in Enum.GetNames(typeof(MasterApiId)))
        //    {
        //        var member = Enum.Parse(typeof(MasterApiId), name);
        //        var declaration = typeof(MasterApiId).GetField(name).GetCustomAttribute<DescriptionAttribute>().Description;
        //        var commandName = typeof(MasterApiId).GetField(name).GetCustomAttribute<DefaultValueAttribute>().Value as string;
        //        var command = Node.Controllers.Commands.SingleOrDefault(p => p.Name.ToLowerInvariant() == commandName.ToLowerInvariant());

        //        yield return new { Id = (int)member, Name = name, Syntax = command?.Syntax, Description = command?.Description, Declaration = declaration };
        //    }
        //}

        //[Command("api list", Description = "Get the Server API.")]
        //public IEnumerable<object> ApiList(CommandArgs args) => Invoke<IEnumerable<object>>();
        //#endregion
    }
}

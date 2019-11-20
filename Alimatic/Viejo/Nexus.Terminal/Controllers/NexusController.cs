using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Cyxor.Terminal.Controllers
{
    using Cyxor.Controllers;
    using Cyxor.Networking;

    using Alimatic.Nexus.Models;

    class NexusController : Controller
    {
        //[Command("nexus table export", Arguments = "$table $format $file",
        //    Description = "Export the Nexus $table data into $file with the specified $format")]
        //public async Task<Result> TableExport(CommandArgs args)
        //{
        //    var result = Result.Success;

        //    using (var packet = new Packet(Node) { Model = new GetTableDataApiModel { NameOrId = args["$table"] } })
        //    {
        //        if (!(result = await packet.QueryAsync()))
        //            return result;

        //        result.GetModel<TableDataModel>().Export(args["$file"], args["$format"]);
        //    }

        //    return true;
        //}
    }
}

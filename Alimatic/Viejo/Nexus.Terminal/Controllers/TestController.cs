using System.Text;
using System.Threading.Tasks;

namespace Cyxor.Terminal.Controllers
{
    using Cyxor.Controllers;
    using Cyxor.Networking;

    using Alimatic.Nexus.Models;

    class TestController : Controller
    {
        //[Command("test 1", Arguments = "[$args]")]
        //async Task<Result> Test1(CommandArgs args)
        //{
        //    var sb = new StringBuilder();

        //    for (var i = 0; i < int.Parse(args["$args"]); i++)
        //        using (var packet = new Packet(Node) { Code = "test1", Model = args["$args"] })
        //            sb.AppendLine((await packet.QueryAsync()).ToString());

        //    return new Result(comment: sb.ToString());
        //}
    }
}

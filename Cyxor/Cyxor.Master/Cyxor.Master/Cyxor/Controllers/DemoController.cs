using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
namespace Cyxor.Controllers
{
    using Models;

    [Model("action one")]
    class Action1ApiModel { }

    [Model("action two")]
    class Action2ApiModel { }

    [Model("action three")]
    class Action3ApiModel { }

    [Model("action four")]
    class Action4ApiModel
    {
        public int Id { get; set; }
    }

    [Model("action five")]
    class Action5ApiModel : Action4ApiModel { }

    class Record
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public string Product { get; set; }
    }

    class DemoController : BaseController
    {
        List<Record> Records = new List<Record>
        {
            new Record { Id = 1, Price = 10, Product = "Wood" },
            new Record { Id = 2, Price = 20, Product = "Stone" },
            new Record { Id = 3, Price = 30, Product = "Metal" },
        };

        [Action(typeof(Action1ApiModel))]
        void Action1() => Node.Log("Action one invoked");

        [Action(typeof(Action2ApiModel))]
        string Action2() => "Hello from action two";

        [Action(typeof(Action3ApiModel))]
        List<Record> Action3() => Records;

        [Action(typeof(Action4ApiModel))]
        Record Action4(Action4ApiModel model) => Records.Single(p => p.Id == model.Id);

        [Action(typeof(Action5ApiModel))]
        async Task<Record> Action4(Action5ApiModel model)
            => await Records.ToAsyncEnumerable().Single(p => p.Id == model.Id);
    }
}
*/
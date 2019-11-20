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

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json;
using AgileObjects.AgileMapper;

namespace Cyxor.Controllers
{
    using Models;
    using Extensions;
    using Networking;
    using Networking.Config;

    public class JsonToQueryStringApiModel
    {
        public string Json { get; set; }
        public string Route { get; set; }
    }

    [Controller(Route = "utils")]
    public class UtilsController : Controller
    {
        public async Task Delay(DelayApiModel model)
            => await Utilities.Task.Delay(model.Milliseconds).ConfigureAwait(false);
    }

    [Controller(Route = "utils convert")]
    public class UtilsConvertController : Controller
    {
        public string JsonToQueryString(JsonToQueryStringApiModel apiModel)
        {
            var type = Node.Controllers.GetModelType(apiModel.Route);
            var source = JsonConvert.DeserializeObject(apiModel.Json, type);

            return Node.Mapper.Flatten(source).ToQueryString();
        }
    }

    [Controller(Route = "")]
    public class NodeController : Controller
    {
        public string ConsoleEncoding() =>
            $" In: {Console.InputEncoding.EncodingName}, {nameof(Console.InputEncoding.WebName)} => {Console.InputEncoding.WebName}{Environment.NewLine}" +
            $"Out: {Console.OutputEncoding.EncodingName}, {nameof(Console.OutputEncoding.WebName)} => {Console.OutputEncoding.WebName}";

        public void Shutdown(ShutdownApiModel model) //=> Connection.DisconnectionReason = model.Reason;
            => Connection.Result = new Result(ResultCode.ClientDisconnectionRemote, model.Reason);

        //[Action(typeof(ClsApiModel))]
        public void Cls() => Console.Clear();

        //[Action(typeof(PacketQueryApiModel))]
        public Task<Result> PacketQuery(PacketQueryApiModel model)
        {
            if (Connection == null && model?.Address == null)
                return Node.Controllers.ExecuteAsync($"{model.Route} {model.Model}");

            var action = Node.Controllers.FindAction(model.Route);

            if (action == null)
                return Utilities.Task.FromResult(new Result(ResultCode.Error, "Invalid route provided"));

            var obj = default(object);
            if (model.Model != null)
                obj = Utilities.Json.Deserialize(model.Model, action.RequestType);

            using (var packet = Connection != null ? new Packet(Connection) : new Packet(Node)
            {
                Model = obj,
                Address = model.Address,
            })
                return packet.QueryAsync();
        }

        //[Action(typeof(ApiListApiModel))]
        public IEnumerable<string> Routes()
        {
            var sb = new StringBuilder();
            var routes = new List<string>();

            foreach (var route in Node.Controllers.RoutesCache.Single(p => p.Key == "").Value)
            {
                sb.Clear();
                sb.Append(route);
                if (Node.Controllers.RoutesCache.Any(p => p.Key == route))
                    sb.Append($" {ControllerAction.PartialRoute}");
                routes.Add(sb.ToString());
            }

            return routes;
        }

        //[Action(typeof(HelpApiModel))]
        public ControllerAction.Help Help(HelpApiModel model)
        {
            var action = model.Api == null ? CurrentAction : Node.Controllers.FindAction(model.Api);

            if (action == null)
                throw new InvalidOperationException("Can't find the provided API");

            //return Utilities.Json.Serialize(action.ApiHelp, includeComments: true);
            return action.ApiHelp;
        }

        //[Action(typeof(LicenseApiModel))]
        public string License() =>
            "{ Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/>" + Environment.NewLine +
            "Copyright (C) 2017  Yandy Zaldivar" + Environment.NewLine +
            Environment.NewLine +
            "This program is free software: you can redistribute it and/or modify" + Environment.NewLine +
            "it under the terms of the GNU Affero General Public License as" + Environment.NewLine +
            "published by the Free Software Foundation, either version 3 of the" + Environment.NewLine +
            "License, or (at your option) any later version." + Environment.NewLine +
            Environment.NewLine +
            "This program is distributed in the hope that it will be useful," + Environment.NewLine +
            "but WITHOUT ANY WARRANTY; without even the implied warranty of" + Environment.NewLine +
            "MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the" + Environment.NewLine +
            "GNU Affero General Public License for more details." + Environment.NewLine +
            Environment.NewLine +
            "You should have received a copy of the GNU Affero General Public License" + Environment.NewLine +
            "along with this program.  If not, see <http://www.gnu.org/licenses/>.";

        //[Action(typeof(ConnectApiModel))]
        [Action(Route = nameof(Connect), IsFullRoute = true)]
        public virtual Task<Result> Connect(ConnectApiModel model) => Node.ConnectAsync();

        //[Action(typeof(DisconnectApiModel))]
        public virtual Task<Result> Disconnect(DisconnectApiModel model) => // TODO: Add timeout support
            Node.DisconnectAsync(new Result(comment: model.Reason), ShutdownSequence.Graceful);

        //[Action(typeof(ConfigListApiModel))]
        public Result ConfigList() => new Result(model: Node.Config.SerializeToJson());

        //[Action(typeof(ConfigUpdateApiModel))]
        public Task<Result> ConfigUpdate(ConfigUpdateApiModel model)
        {
            var result = new Result(ResultCode.Success, "Config successfully set");

            var jsonValue = default(string);

            if (model.Key == null)
                jsonValue = model.Value;
            else
            {
                var properties = model.Key.Split('.');
                var sb = new StringBuilder();

                sb.Append("{");

                for (var i = 0; i < properties.Length; i++)
                {
                    sb.Append($"{properties[i]}:");

                    if (i + 1 != properties.Length)
                        sb.Append('{');
                }

                if (model.Value.StartsWith("[") && model.Value.EndsWith("]"))
                    sb.Append(model.Value);
                else
                    sb.Append($"\"{model.Value}\"");

                for (var i = 0; i < properties.Length; i++)
                    sb.Append('}');

                jsonValue = sb.ToString();
            }

            Utilities.Json.Deserialize(jsonValue, Node.Config.GetType());

            Node.Config.Load(jsonValue);

            switch (Node.Config.File.Mode)
            {
                case FileConfigMode.None: break;
                case FileConfigMode.Create: Node.Config.Save(); break;
                case FileConfigMode.Open:
                {
                    if (File.Exists(Node.Config.File.Name))
                        Node.Config.Load();
                    else
                        result = new Result(ResultCode.Error, $"Config set failed: {Environment.NewLine}" +
                            $"The specified config file doesn't exists{Environment.NewLine}" +
                            $"{{{Environment.NewLine}" +
                            $"    FileMode = {Node.Config.File.Mode},{Environment.NewLine}" +
                            $"    FileName = {Node.Config.File.Name},{Environment.NewLine}" +
                            $"}}");

                    break;
                }
                case FileConfigMode.OpenOrCreate:
                {
                    if (File.Exists(Node.Config.File.Name))
                        Node.Config.Load();
                    else
                        Node.Config.Save();

                    break;
                }
            }

            Node.Config.Load(jsonValue);

            return Utilities.Task.FromResult(result);
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */

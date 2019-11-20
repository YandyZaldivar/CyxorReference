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
using System.Linq;
using System.Text;
using System.Security;
using System.Threading;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;
using AgileObjects.AgileMapper;

namespace Cyxor.Networking
{
    using Events;
    using Models;
    using Extensions;
    using Controllers;
    using Serialization;

    public abstract partial class Node
    {
        public abstract partial class NodeControllers : NodeProperty
        {
            internal abstract void Internal();

            public static char Separator = ' ';
            public static char[] Separators = new char[] { ' ', '/', '?' };

            // TODO 1: Bug! Controllers/Assembly registration should be lock protected or 
            // move registration to the node configuration to automatically register on connect

            // TODO 2: Set this as a Dictionary and not to a concurrent dictionary (after TODO 1)
            internal ConcurrentDictionary<Type, ControllerInfo> Controllers { get; }

            HashSet<Assembly> Assemblies { get; }

            SortedList<int, ControllerAction> IdActions { get; }
            SortedList<string, ControllerAction> RouteActions { get; }
            SortedDictionary<string, IEnumerable<string>> Routes { get; }

            public Type GetModelType(string route)
            {
                if (RouteActions.TryGetValue(route, out var controllerAction))
                    return controllerAction.RequestType;

                return null;
            }

            public IEnumerable<KeyValuePair<string, IEnumerable<string>>> RoutesCache =>
                Routes as IEnumerable<KeyValuePair<string, IEnumerable<string>>>;

            protected internal NodeControllers(Node node) : base(node)
            {
                IdActions = new SortedList<int, ControllerAction>();

                RouteActions = new SortedList<string, ControllerAction>();

                Routes = new SortedDictionary<string, IEnumerable<string>>();

                Controllers = new ConcurrentDictionary<Type, ControllerInfo>();

                Assemblies = new HashSet<Assembly>();

                var assemblies = new HashSet<Assembly>();

                if (Node.Config.EntryAssembly != null)
                    assemblies.Add(Node.Config.EntryAssembly);

#if !NETSTANDARD1_3
                //assemblies.Add(Assembly.GetEntryAssembly());
                assemblies.Add(Assembly.GetCallingAssembly());
                assemblies.Add(Assembly.GetExecutingAssembly());
#endif

                var type = node.GetType();

                while (type != typeof(object))
                {
                    var assembly = type.GetTypeInfo().Assembly;
                    assemblies.Add(assembly);
                    type = type.GetTypeInfo().BaseType;
                }

                foreach (var assembly in assemblies)
                    Register(assembly, updateCommands: false);

                UpdateCommands();
            }

            internal ControllerAction FindAction(MethodInfo methodInfo)
                => RouteActions.SingleOrDefault(p => p.Value.MethodInfo == methodInfo).Value;

            internal ControllerAction FindAction(string route)
            {
                if (RouteActions.TryGetValue(route, out var action))
                    return action;
                else if (!int.TryParse(route, out var id))
                    if (IdActions.TryGetValue(id, out action))
                        return action;

                return null;
            }

            public void Register(Assembly assembly) => Register(assembly, updateCommands: true);

            void Register(Assembly assembly, bool updateCommands)
            {
                if (assembly == null)
                    return;

                if (Assemblies.Contains(assembly))
                    return;

                var controllersType = from controllerType in assembly.GetTypes()
                                      where controllerType.GetTypeInfo().IsSubclassOf(typeof(Controller)) && !controllerType.GetTypeInfo().IsAbstract
                                      select controllerType;

                foreach (var controllerType in controllersType)
                    Register(controllerType, registerAllControllersInTheSameAssembly: false, updateCommands);

                // TODO: If controller registration fails assembly shouldn't be registered?!?
                Assemblies.Add(assembly);
            }

            public void Register<T>() => Register<T>(registerAllControllersInTheSameAssembly: false);

            public void Register<T>(bool registerAllControllersInTheSameAssembly)
                => Register(typeof(T), registerAllControllersInTheSameAssembly);

            public void Register(Type controllerType)
                => Register(controllerType, registerAllControllersInTheSameAssembly: false);

            public virtual void Register(Type controllerType, bool registerAllControllersInTheSameAssembly)
                => Register(controllerType, registerAllControllersInTheSameAssembly, updateCommands: true);

            void Register(Type controllerType, bool registerAllControllersInTheSameAssembly, bool updateCommands)
            {
                if (Node.IsConnected)
                    throw new InvalidOperationException("You cannot register a controller while the network instance is connected");

                //if (!controllerType.IsDefined(typeof(ControllerAttribute), inherit: false))
                if (!controllerType.GetTypeInfo().IsSubclassOf(typeof(Controller)))
                {
                    Node.Log(LogCategory.Error, $"Cyxor controller types must inherit from a Controller type.");
                    return;
                }

                if (controllerType.GetTypeInfo().IsAbstract)
                {
                    Node.Log(LogCategory.Error, $"Cannot register an abstract controller.");
                    return;
                }

                if (registerAllControllersInTheSameAssembly)
                {
                    Register(controllerType.GetTypeInfo().Assembly);
                    return;
                }

                if (Controllers.ContainsKey(controllerType))
                    return;

                var ctrlerAttr = controllerType.GetTypeInfo().GetCustomAttribute<ControllerAttribute>(inherit: true);

                if (ctrlerAttr != null)
                {
                    switch (ctrlerAttr.Bounds)
                    {
                        case ControllerBounds.None: break;
                        case ControllerBounds.Client when !Node.IsClient: return;
                        case ControllerBounds.Server when !Node.IsServer: return;
                    }
                }

                foreach (var item in Controllers)
                {
                    if (item.Key.GetTypeInfo().IsSubclassOf(controllerType))
                        return;
                    else if (controllerType.GetTypeInfo().IsSubclassOf(item.Key))
                    {
                        Controllers.TryRemove(item.Key, out var value);
                        break;
                    }
                }

                var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

                var methods = controllerType.GetMethods(bindingFlags);

                var controllerInfo = new ControllerInfo(controllerType);

                foreach (var methodInfo in methods)
                {
                    if (methodInfo.DeclaringType == typeof(Controller) || methodInfo.DeclaringType == typeof(object))
                        continue;

                    if (methodInfo.GetCustomAttributes(typeof(ActionAttribute), inherit: true)
                        .SingleOrDefault() is ActionAttribute actionAttr)
                    {
                        if (!actionAttr.Hide)
                        {
                            var controllerAction = new ControllerAction(controllerInfo, methodInfo, actionAttr);
                            controllerInfo.Actions.Add(controllerAction.Id, controllerAction);
                        }
                    }
                    else if (methodInfo.GetCustomAttributes(typeof(ScopeInitializerAttribute), inherit: true)
                        .SingleOrDefault() is ScopeInitializerAttribute scopeInitializerAttr)
                    {
                        controllerInfo.ScopeInitializers.Add(new ScopeInitializer(methodInfo));
                    }
                    else if (methodInfo.IsPublic) //TODO: and if (![ActionIgnoreAttribute])
                    {
                        actionAttr = new ActionAttribute();
                        var controllerAction = new ControllerAction(controllerInfo, methodInfo, actionAttr);

                        if (controllerInfo.Actions.ContainsKey(controllerAction.Id))
                            throw new InvalidOperationException($"Found two action with the same route: {controllerAction.Route}");

                        controllerInfo.Actions.Add(controllerAction.Id, controllerAction);
                    }
                }

                Controllers.TryAdd(controllerType, controllerInfo);

                if (updateCommands)
                    UpdateCommands();
            }

            void UpdateCommands()
            {
                Routes.Clear();
                IdActions.Clear();
                RouteActions.Clear();

                var routesTokens = new List<List<string>>();

                foreach (var controller in Controllers.Values)
                    foreach (var action in controller.Actions.Values)
                    {
                        var existingAction = (from item in RouteActions.Values
                                              where item.Route.StartsWith(action.Route)
                                              || action.Route.StartsWith(item.Route)
                                              select item).SingleOrDefault();

                        if (existingAction != null)
                        {
                            var errorMessage = "Action routes can't match the start of other routes. " +
                                $"The route '{existingAction.Route}' is ambiguous between " +
                                $"'{action.ControllerInfo.Type.Name}.{action.MethodInfo.Name}' and " +
                                $"'{existingAction.ControllerInfo.Type.Name}.{existingAction.MethodInfo.Name}'. " +
                                "Route names are case insensitive.";

                            throw new InvalidOperationException(errorMessage);
                        }

                        IdActions.Add(action.Id, action);
                        RouteActions.Add(action.Route, action);

                        routesTokens.Add(action.Tokens as List<string>);
                    }

                //ActionList.Sort((commandX, commandY) => commandX.Name.CompareTo(commandY.Name));

                for (int j = 0, c = 1; c > 0; j++)
                {
                    c = 0;

                    for (var i = 0; i < routesTokens.Count; i++)
                    {
                        if (routesTokens[i].Count <= j)
                            continue;

                        c++;

                        var key = string.Empty;
                        var keyBuilder = default(StringBuilder);

                        for (var k = 0; k < j; k++)
                        {
                            keyBuilder = keyBuilder ?? new StringBuilder();
                            keyBuilder.AppendFormat(k == 0 ? "{0}" : " {0}", routesTokens[i][k]);
                        }

                        if (keyBuilder != null)
                            key = keyBuilder.ToString();

                        if (!Routes.ContainsKey(key))
                            Routes[key] = new List<string>();

                        if (!Routes[key].Contains(routesTokens[i][j]))
                            (Routes[key] as List<string>).Add(routesTokens[i][j]);
                    }
                }

                foreach (var tokens in Routes.Values)
                    (tokens as List<string>).Sort();
            }

            //protected internal virtual async void Post(PacketReceiveCompletedEventArgs e)
            //{
            //    var handled = false;
            //    var result = Result.Success;

            //    // TODO: Extract the action and method controller from executeAsync
            //    // to Implement security and role check.
            //    using (e.Reference)
            //    {
            //        try
            //        {
            //            if (e.Packet.IsCommand)
            //            {
            //                handled = true;

            //                var value = e.Packet.GetModel<string>();

            //                //if (value != "favicon.ico")
            //                if (value != "favicon.ico" && e.Packet.Box.HttpRequest.Method != HttpMethod.Options)
            //                {
            //                    //if (!e.Connection.IsAuthenticated && value.StartsWith("datadin2") && !value.StartsWith("datadin2/login"))
            //                    //{
            //                    //    var serializer = new Serialization.Serializer();
            //                    //    serializer.Serialize(e.Packet.Box.HttpRequest.Authorization);
            //                    //    var authRequest = serializer.ToObject<AuthRequest>();
            //                    //    result = await Node.Controllers.ExecuteAsync($"datadin2/login?I={authRequest.I}&A={authRequest.A}", e.Packet).ConfigureAwait(false);
            //                    //}

            //                    if (result)
            //                        result = await Node.Controllers.ExecuteAsync(value, e.Packet).ConfigureAwait(false);
            //                }

            //                if (e.Packet.IsHttpMessage)
            //                {
            //                    var statusCode = "200 OK";
            //                    var resultString = string.Empty;
            //                    var resultData = new Serialization.Serializer("favicon.ico");
            //                    var contentType = "application/json; charset=utf-8";
            //                    //var contentType = "application/pdf; charset=utf-8";

            //                    if (!result)
            //                    {
            //                        statusCode = $"{(int)result.Code} { result.Comment}";
            //                        resultData = new Serialization.Serializer(result.ToJson());
            //                        //contentType = "text/plain; charset=utf-8";
            //                    }
            //                    else
            //                    {
            //                        if (e.Packet.ControllerAction?.ActionAttribute?.ContentType != null)
            //                            contentType = e.Packet.ControllerAction?.ActionAttribute?.ContentType;

            //                        if (value != "favicon.ico")
            //                        {
            //                            if (contentType.StartsWith("application/pdf"))
            //                                resultData = new Serialization.Serializer(result.GetModel<byte[]>());
            //                            else
            //                            {
            //                                resultString = result.GetModel<string>() ?? string.Empty;
            //                                resultData = new Serialization.Serializer(resultString);
            //                            }
            //                        }

            //                        if (e.Packet.ControllerAction?.ActionAttribute?.ContentType == null)
            //                            if (resultString.StartsWith("<!DOCTYPE HTML", StringComparison.OrdinalIgnoreCase) ||
            //                                resultString.StartsWith("<HTML", StringComparison.OrdinalIgnoreCase))
            //                                contentType = "text/html; charset=utf-8";
            //                    }

            //                    var contentLength = string.IsNullOrEmpty(resultString) ? resultData.Length : Encoding.UTF8.GetByteCount(resultString);

            //                    //var contentLength = string.IsNullOrEmpty(resultData) ? 0 : Encoding.UTF8.GetByteCount(resultData);

            //                    var response = $"HTTP/1.1 {statusCode}" + Utilities.Http.NewLine +
            //                        //"Content-Type: " + "text/html" + Utilities.Http.NewLine +
            //                        $"Content-Type: {contentType}" + Utilities.Http.NewLine +
            //                        "Content-Length: " + contentLength + Utilities.Http.NewLine +

            //                        $"Access-Control-Allow-Origin: {e.Packet.Box.HttpRequest.Origin}" + Utilities.Http.NewLine +
            //                        "Access-Control-Allow-Credentials: true" + Utilities.Http.NewLine +
            //                        "Access-Control-Allow-Headers: Origin, Content-Type, X-Requested-With, Accept, Authorization" + Utilities.Http.NewLine +
            //                        "Access-Control-Allow-Methods: GET, PUT, POST, DELETE, PATCH, OPTIONS" + Utilities.Http.NewLine +

            //                        "Connection: keep-alive" + Utilities.Http.NewLine + Utilities.Http.NewLine;


            //                    /*
            //                    res.header('Access-Control-Allow-Origin', '*');
            //                    res.header('Access-Control-Allow-Methods', 'GET, POST, PUT, DELETE, PATCH');
            //                    res.header('Access-Control-Allow-Methods', 'GET,PUT,POST,DELETE,PATCH,OPTIONS');
            //                    res.header('Access-Control-Allow-Headers', 'Origin, X-Requested-With, Content-Type, Accept, Authorization');

            //                    context.Response.Headers.Add("Access-Control-Allow-Origin", new[] { (string)context.Request.Headers["Origin"] });
            //                    context.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "Origin, X-Requested-With, Content-Type, Accept, Authorization" });
            //                    context.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "GET, POST, PUT, DELETE, OPTIONS" });
            //                    context.Response.Headers.Add("Access-Control-Allow-Credentials", new[] { "true" });
            //                    */


            //                    //model = response + jsonString;
            //                    //result = new Result(comment: response + resultData);

            //                    var serializer = new Serialization.Serializer();
            //                    serializer.SerializeRaw(response);
            //                    serializer.SerializeRaw(resultData);
            //                    result = new Result(model: serializer);
            //                }

            //                //using (var commandResponse = new Packet(e.Packet) { Model = model })
            //                //    await commandResponse.SendAsync();
            //            }
            //            else
            //            {
            //                foreach (var item in Controllers)
            //                {
            //                    var controllerType = item.Key;
            //                    var controllerInfo = item.Value;

            //                    if (controllerInfo.Actions.TryGetValue(e.Packet.Id, out var action))
            //                    {
            //                        handled = true;

            //                        // TODO: Implement security and role check.
            //                        {
            //                            // if (dont meet role)
            //                            //     return result;
            //                        }

            //                        e.Packet.ReferenceCounted = e;

            //                        if (action.ParametersCount == 0)
            //                            result = await Controller.InvokeAsync(false, Node, e.Connection, action);
            //                        else
            //                        {
            //                            var model = e.Packet.GetModel(action.RequestModel.GetType());
            //                            result = await Controller.InvokeAsync(false, Node, e.Connection, action, model);
            //                        }

            //                        return;
            //                    }
            //                }
            //            }

            //        }
            //        catch (Exception ex)
            //        {
            //            handled = true;

            //            //Node.Log(LogCategory.Error, ex.Message);
            //            result = new Result(ResultCode.Error, comment: ex.Message);
            //            Node.Log(result);
            //        }
            //        finally
            //        {
            //            if (e.Packet.IsQuery)
            //            {
            //                var model = e.Packet.IsHttpMessage ? result.GetModel<Serialization.Serializer>() : (object)result;

            //                using (var reply = new Packet(e.Packet) { Model = model })
            //                    await reply.SendAsync();
            //            }

            //            e.Packet.ReferenceCounted = null;
            //        }
            //    }

            //    if (!handled)
            //    {
            //        //    await e.Connection.Link.DisconnectAsync(new Result(ResultCode.ProtocolErrorType));
            //    }
            //}





            protected internal virtual async void Post(PacketReceiveCompletedEventArgs e)
            {
                var handled = false;
                var result = Result.Success;

                // TODO: Extract the action and method controller from executeAsync
                // to Implement security and role check.
                using (e.Reference)
                {
                    try
                    {
                        if (e.Packet.IsCommand)
                        {
                            handled = true;

                            var value = e.Packet.GetModel<string>();
                            
                            var processValue = true;

                            if (e.Packet.IsHttpMessage)
                                if (value == "favicon.ico" || e.Packet.Box.HttpRequest.Method == HttpMethod.Options)
                                    processValue = false;

                            if (result && processValue)
                                result = await Node.Controllers.ExecuteAsync(value, e.Packet).ConfigureAwait(false);

                            //if (value != "favicon.ico")
                            //if (value != "favicon.ico" && e.Packet.Box.HttpRequest.Method != HttpMethod.Options)
                            //{
                                ////if (!e.Connection.IsAuthenticated && value.StartsWith("datadin2") && !value.StartsWith("datadin2/login"))
                                ////{
                                ////    var serializer = new Serialization.Serializer();
                                ////    serializer.Serialize(e.Packet.Box.HttpRequest.Authorization);
                                ////    var authRequest = serializer.ToObject<AuthRequest>();
                                ////    result = await Node.Controllers.ExecuteAsync($"datadin2/login?I={authRequest.I}&A={authRequest.A}", e.Packet).ConfigureAwait(false);
                                ////}

                                //if (result)
                                //    result = await Node.Controllers.ExecuteAsync(value, e.Packet).ConfigureAwait(false);
                            //}

                            if (e.Packet.IsHttpMessage)
                            {
                                //var httpRequest = e.Packet.Box.HttpRequest;

                                var statusCode = "200 OK";
                                //var resultString = string.Empty;
                                var resultData = default(Serializer);
                                var contentType = "application/json; charset=utf-8";

                                if (e.Packet.ControllerAction?.ActionAttribute?.HttpContentType is string httpContentType)
                                    contentType = $"{httpContentType}; charset=utf-8";

                                if (value == "favicon.ico")
                                    resultData = new Serializer("favicon.ico");
                                else // TODO: Improve the performance by not serializing the result data as json by default
                                    switch (e.Packet.ControllerAction?.ActionAttribute?.HttpContentFormat ?? HttpContentFormat.Json)
                                    {
                                        case HttpContentFormat.Json: resultData = new Serializer(result.ToJson()); break;
                                        case HttpContentFormat.Xml: resultData = new Serializer(result.ToXml()); break;
                                        case HttpContentFormat.String: resultData = new Serializer(JsonConvert.DeserializeObject<string>(result.Description)); break;
                                        //case HttpContentFormat.Binary: resultData = new Serializer(JsonConvert.DeserializeObject<byte[]>(result.Description)); break;
                                        case HttpContentFormat.Binary: resultData = new Serializer(result.GetModel<byte[]>()); break;
                                    }

                                var contentLength = resultData.Int32Length;

                                var response = $"HTTP/1.1 {statusCode}" + Utilities.Http.NewLine +
                                    $"Content-Type: {contentType}" + Utilities.Http.NewLine +
                                    "Content-Length: " + contentLength + Utilities.Http.NewLine +

                                    ////Access-Control-Expose-Headers 
                                    "Access-Control-Allow-Credentials: true" + Utilities.Http.NewLine +
                                    $"Access-Control-Allow-Origin: {e.Packet.Box.HttpRequest.Origin}" + Utilities.Http.NewLine;

                                if (e.Packet.Box.HttpRequest.Method == HttpMethod.Options)
                                    response +=
                                        "Access-Control-Allow-Headers: Origin, Content-Type, X-Requested-With, Accept, Authorization" + Utilities.Http.NewLine +
                                        "Access-Control-Allow-Methods: GET, POST, OPTIONS" + Utilities.Http.NewLine +
                                        "Access-Control-Max-Age: 604800" + Utilities.Http.NewLine;

                                response += "Connection: keep-alive" + Utilities.Http.NewLine + Utilities.Http.NewLine;

                                var serializer = new Serializer();
                                serializer.SerializeRaw(response);
                                serializer.SerializeRaw(resultData);
                                result = new Result(model: serializer);
                            }
                        }
                        else
                        {
                            foreach (var item in Controllers)
                            {
                                var controllerType = item.Key;
                                var controllerInfo = item.Value;

                                if (controllerInfo.Actions.TryGetValue(e.Packet.Id, out var action))
                                {
                                    handled = true;

                                    // TODO: Implement security and role check.
                                    {
                                        // if (dont meet role)
                                        //     return result;
                                    }

                                    e.Packet.ReferenceCounted = e;

                                    if (action.ParametersCount == 0)
                                        result = await Controller.InvokeAsync(false, Node, e.Connection, action);
                                    else
                                    {
                                        var model = e.Packet.GetModel(action.RequestType);
                                        result = await Controller.InvokeAsync(false, Node, e.Connection, action, model);
                                    }

                                    return;
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        handled = true;

                        //Node.Log(LogCategory.Error, ex.Message);
                        result = new Result(ResultCode.Error, comment: ex.Message);
                        Node.Log(result);
                    }
                    finally
                    {
                        if (e.Packet.IsQuery)
                        {
                            //var model = e.Packet.IsHttpMessage ? result.GetModel<Serializer>() : (object)result;
                            var model = e.Packet.IsCommand ? result.GetModel<Serializer>() : (object)result;

                            using (var reply = new Packet(e.Packet) { Model = model })
                                await reply.SendAsync();
                        }

                        e.Packet.ReferenceCounted = null;
                    }
                }

                if (!handled)
                {
                    //    await e.Connection.Link.DisconnectAsync(new Result(ResultCode.ProtocolErrorType));
                }
            }



            public Task<Result> ExecuteAsync(string value) => ExecuteAsync(value, Timeout.Infinite, CancellationToken.None);
            public Task<Result> ExecuteAsync(string value, int millisecondsTimeout) => ExecuteAsync(value, millisecondsTimeout, CancellationToken.None);
            public Task<Result> ExecuteAsync(string value, CancellationToken cancellationToken) => ExecuteAsync(value, Timeout.Infinite, cancellationToken);
            public Task<Result> ExecuteAsync(string value, TimeSpan timeout) => ExecuteAsync(value, (int)timeout.TotalMilliseconds, CancellationToken.None);

            public Task<Result> ExecuteAsync(string value, int millisecondsTimeout, CancellationToken cancellationToken) =>
                ExecuteAsync(Security.Utilities.Converter.ToSecureString(value), millisecondsTimeout, cancellationToken);

            public Task<Result> ExecuteAsync(SecureString value) => ExecuteAsync(value, Timeout.Infinite, CancellationToken.None);
            public Task<Result> ExecuteAsync(SecureString value, int millisecondsTimeout) => ExecuteAsync(value, millisecondsTimeout, CancellationToken.None);
            public Task<Result> ExecuteAsync(SecureString value, CancellationToken cancellationToken) => ExecuteAsync(value, Timeout.Infinite, cancellationToken);
            public Task<Result> ExecuteAsync(SecureString value, TimeSpan timeout) => ExecuteAsync(value, (int)timeout.TotalMilliseconds, CancellationToken.None);

            //public abstract Task<Result> ExecuteAsync(SecureString value, int millisecondsTimeout, CancellationToken cancellationToken);

            public Task<Result> ExecuteAsync(string value, Connection connection) => ExecuteAsync(value, connection, Timeout.Infinite, CancellationToken.None);
            public Task<Result> ExecuteAsync(string value, Connection connection, int millisecondsTimeout) => ExecuteAsync(value, connection, millisecondsTimeout, CancellationToken.None);
            public Task<Result> ExecuteAsync(string value, Connection connection, CancellationToken cancellationToken) => ExecuteAsync(value, connection, Timeout.Infinite, cancellationToken);
            public Task<Result> ExecuteAsync(string value, Connection connection, TimeSpan timeout) => ExecuteAsync(value, connection, (int)timeout.TotalMilliseconds, CancellationToken.None);

            public Task<Result> ExecuteAsync(string value, Connection connection, int millisecondsTimeout, CancellationToken cancellationToken) =>
                ExecuteAsync(value, Node, connection, default, millisecondsTimeout, cancellationToken);

            public Task<Result> ExecuteAsync(SecureString value, Connection connection) => ExecuteAsync(value, connection, Timeout.Infinite, CancellationToken.None);
            public Task<Result> ExecuteAsync(SecureString value, Connection connection, int millisecondsTimeout) => ExecuteAsync(value, connection, millisecondsTimeout, CancellationToken.None);
            public Task<Result> ExecuteAsync(SecureString value, Connection connection, CancellationToken cancellationToken) => ExecuteAsync(value, connection, Timeout.Infinite, cancellationToken);
            public Task<Result> ExecuteAsync(SecureString value, Connection connection, TimeSpan timeout) => ExecuteAsync(value, connection, (int)timeout.TotalMilliseconds, CancellationToken.None);

            public Task<Result> ExecuteAsync(SecureString value, Connection connection, int millisecondsTimeout, CancellationToken cancellationToken) =>
                ExecuteAsync(Security.Utilities.Converter.FromSecureString(value), Node, connection, default, millisecondsTimeout, CancellationToken.None);

            public Task<Result> ExecuteAsync(SecureString value, int millisecondsTimeout, CancellationToken cancellationToken) =>
                ExecuteAsync(Security.Utilities.Converter.FromSecureString(value), Node, default, default, millisecondsTimeout, cancellationToken);

            public Task<Result> ExecuteAsync(string value, Packet packet)
                => ExecuteAsync(value, Node, packet.RootConnection, packet, Timeout.Infinite, CancellationToken.None);

            static async Task<Result> ExecuteAsync(string cmdstr, Node node, Connection connection, Packet packet, int millisecondsTimeout, CancellationToken cancellationToken)
            {
                var disposeScope = false;
                var result = Result.Success;
                var command = default(ControllerAction);
                //var arguments = default(string[]);

                //var name = default(string);
                var argument = default(string);

                try
                {
                    if (connection == null && node.CommandScope == null)
                    {
                        disposeScope = true;

                        node.CommandScope = node.CreateScope();
                    }

                    var index = cmdstr.IndexOf('{');

                    if (index == -1)
                        argument = "{}";
                    else
                    {
                        argument = cmdstr.Substring(index).Trim();
                        cmdstr = cmdstr.Substring(0, index).Trim();
                    }

                    if (node.Context == null)
                        node.Context = new Context { Node = node, Connection = connection, JsonRequest = argument, Packet = packet };

                    if (!TryParse(cmdstr, out var tokens))
                        return result = new Result(ResultCode.CommandParseError);

                    if (tokens[0] == node.AsClient?.Config?.RemoteCommandTokenName)
                        return result = await node.AsClient.Controllers.ExecuteRemoteAsync(cmdstr, millisecondsTimeout, cancellationToken);

                    var parsedCmdstr = cmdstr;
                    var strbuilder = new StringBuilder();

                    if (cmdstr.Contains('"') || cmdstr.Contains('\''))
                    {
                        for (var i = 0; i < tokens.Count; i++)
                            strbuilder.AppendFormat(i == 0 ? "{0}" : " {0}", tokens[i]);

                        parsedCmdstr = strbuilder.ToString();
                    }

                    foreach (var separator in Separators)
                        if (separator != Separator)
                            parsedCmdstr = parsedCmdstr.Replace(separator, Separator);

                    var commands = from item in node.Controllers.RouteActions.Values
                                   where parsedCmdstr.StartsWith(item.Route, StringComparison.CurrentCultureIgnoreCase) ||
                                   item.Route.StartsWith(parsedCmdstr, StringComparison.CurrentCultureIgnoreCase)
                                   //where cmd.Name.StartsWith(parsedCmdstr, StringComparison.CurrentCultureIgnoreCase)
                                   select item;

                    if (commands.Count() > 1 || (commands.SingleOrDefault()?.Tokens?.Count() ?? 0) > tokens.Count)
                    {
                        var endToken = new string[] { };
                        var subRoute = new string[] { ControllerAction.PartialRoute };
                        var subCommandName = new StringBuilder();
                        var subCommandNames = new List<string>();

                        var tokenizedSubCommands = commands
                            //.Where(p => p.Name.StartsWith(parsedCmdstr + " "))
                            .Select(p => p.Tokens.Take(tokens.Count + 1)
                                .Concat(p.Tokens.Count() > tokens.Count + 1 ? subRoute : endToken));

                        foreach (var tokenizedSubCommand in tokenizedSubCommands)
                        {
                            var i = 0;
                            subCommandName.Clear();
                            foreach (var token in tokenizedSubCommand)
                                if (!(i == tokenizedSubCommand.Count() && !string.IsNullOrEmpty(token)))
                                    subCommandName.AppendFormat(i++ == 0 ? "{0}" : " {0}", token);

                            subCommandNames.Add(subCommandName.ToString());
                        }

                        subCommandNames = new List<string>(subCommandNames.Distinct());

                        return result = new Result(
                            model: subCommandNames,
                            backingSerializer: node.Config.CommandBackingSerializer,
                            comment: $"Command have {subCommandNames.Count} subcommand{(subCommandNames.Count > 1 ? "s" : string.Empty)}:");
                    }

                    command = commands.SingleOrDefault();

                    node.Context.Action = command;

                    if (command == null)
                    {
                        // TODO: detailed parsing
                        // TODO: Detailed parsed of commands looking for tokens starts like wow: command have subcommands

                        return result = new Result(ResultCode.NotFound);
                    }

                    if (packet != null)
                        packet.ControllerAction = command;

                    if (connection != null)
                        if (connection.Security < command.Security)
                            return result = new Result(ResultCode.CommandSecurityViolation);

                    var commandTokensCount = command.Tokens.Count();

                    if (tokens.Count > commandTokensCount)
                        if (index == -1)
                        {
                            var sb = new StringBuilder();

                            for (var i = commandTokensCount; i < tokens.Count; i++)
                                sb.Append(i == commandTokensCount ? tokens[i] : $" {tokens[i]}");

                            argument = sb.ToString();
                        }
                        else
                            return result = new Result(ResultCode.CommandArgumentsMismatch,
                            model: Utilities.Json.Serialize(new ControllerAction.Help(command)));

                    /*
                    tokens.RemoveRange(0, command.Tokens.Count());
                    arguments = tokens.ToArray();

                    if (arguments.Length < command.MinArguments || arguments.Length > command.MaxArguments)
                        //return result = new Result(
                        //    code: ResultCode.CommandArgumentsMismatch,
                        //    model: new { command.Syntax, command.Description, command.Model },
                        //    backingSerializer: JsonBackingSerializer.Instance);
                        return result = new Result(
                            code: ResultCode.CommandArgumentsMismatch,
                    //model: Newtonsoft.Json.JsonConvert.SerializeObject(new { command.Syntax, command.Description, command.Model }));
                    //model: ConfigJsonSerializer.Instance.Serialize(new { command.Syntax, command.Description, command.Modl2 }, excludeDefaultValues: false));
                    model: ConfigJsonSerializer.Instance.Serialize(new Command.Help(command), excludeDefaultValues: false));
                    */

                    //var commandArgs = new CommandArgs(node, connection, command, arguments, cancellationToken);

                    var model = default(object);

                    try
                    {
                        model = Utilities.Json.Deserialize(argument, command.RequestType);
                    }
                    catch (Exception ex)
                    {
                        var xx = ex.ToString();

                        try
                        {
                            if (argument == "{}")
                                model = Activator.CreateInstance(command.RequestType);
                            else
                            {
                                //model = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(argument
                                //    .Split('&').Select(p => p.Split('=')).ToDictionary(p => p[0], p => (object)p[1])), command.RequestType);

                                model = node.Mapper.Unflatten(QueryString.Parse(argument)).To(command.RequestType);
                            }
                        }
                        catch (Exception)
                        {
                            try
                            {
                                var type = command.RequestType;

                                if (type == typeof(string))
                                    model = argument;
                                else
                                    model = type.GetMethod(nameof(int.Parse)).Invoke(default, new object[] { argument });
                            }
                            catch { }
                        }

                        if (model == null)
                            return result = new Result(ResultCode.Error, "Invalid model string format",
                                model: Utilities.Json.Serialize(new ControllerAction.Help(command)));
                    }

                    var commandExecutingEventArgs = node.Events.Post(new CommandExecutingEventArgs(node, connection, command, argument));

                    await commandExecutingEventArgs.ConfigureAwait(false);

                    if (commandExecutingEventArgs.Cancel)
                        return result = new Result(ResultCode.CommandCanceled, commandExecutingEventArgs.CancelReason);

                    if (commandExecutingEventArgs.Handled)
                        return result = commandExecutingEventArgs.Result;

                    if (command.MethodInfo == null)
                        throw new InvalidOperationException("The command has not defined an action or method to execute");
                    else if (command.ParametersCount == 0)
                        result = await Controller.InvokeAsync(true, node, connection, command);
                    else
                        result = await Controller.InvokeAsync(true, node, connection, command, model);

                    if (result)
                        if (string.IsNullOrEmpty(result.Comment))
                            result = new Result(result, comment: "Command successfully executed");

                    return result;
                }
                catch (Exception exc)
                {
                    // TODO: Review innerResults
                    result = new Result(ResultCode.Exception, exception: exc);

                    //while (exc.InnerException != null)
                    //{
                    //    result = new Result(result, new Result(ResultCode.Exception, exception: exc.InnerException));
                    //    exc = exc.InnerException;
                    //}

                    return result;
                }
                finally
                {
                    if (disposeScope)
                    {
                        node.CommandScope?.Dispose();
                        node.CommandScope = null;
                        node.Context = null;
                    }

                    //node.Log(result, isCommandResult: true);
                    node.Events.Post(new CommandExecuteCompletedEventArgs(node, connection, command, argument, result));
                }
            }

            public static bool TryParse(string value, out List<string> tokens, char[] separator = null, bool removeEmptyEntries = true, bool trimEntries = false)
            {
                tokens = new List<string>();

                if (value.Count(item => item == '"') % 2 != 0)
                    return false;

                if (value.Count(item => item == '\'') % 2 != 0)
                    return false;

                for (var pos = 0; pos < value.Length;)
                {
                    var index1 = value.IndexOf('"', pos);
                    var index2 = value.IndexOf('\'', pos);
                    var minIndex = Math.Min(index1, index2);
                    var index = minIndex < 0 ? Math.Max(index1, index2) : minIndex;
                    var quote = index == index1 ? '"' : '\'';

                    if (index == -1)
                        index = value.Length;

                    if (index > 0)
                    {
                        tokens.AddRange(value.Substring(pos, index - pos).Split(separator ?? Separators, removeEmptyEntries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None));

                        if (index < value.Length && !removeEmptyEntries)
                            tokens.RemoveAt(tokens.Count - 1);
                    }

                    if (index < value.Length)
                    {
                        pos = index + 1;

                        do { index = value.IndexOf(quote, ++index); }
                        while (++index < value.Length && value[index] == quote);

                        if (index - pos - 1 > 0)
                            tokens.Add(value.Substring(pos, index - pos - 1).Replace("\"\"", "\"").Replace("''", "'"));
                    }

                    pos = index + 1;
                }

                if (trimEntries)
                    for (var i = 0; i < tokens.Count; i++)
                        tokens[i] = tokens[i].Trim();

                return true;
            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */

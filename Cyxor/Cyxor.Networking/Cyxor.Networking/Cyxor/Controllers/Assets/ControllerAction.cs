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
using System.Reflection;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

using Newtonsoft.Json;
using Newtonsoft.Json.Schema.Generation;

namespace Cyxor.Networking
{
    using Models;
    using Filters;
    using Extensions;
    using Controllers;

    public sealed class ControllerAction
    {
        public class Help
        {
            public class Api
            {
                public static ConcurrentDictionary<string, object> Models = new ConcurrentDictionary<string, object>();

                [JsonIgnore]
                public readonly Type Type;

                public readonly string TypeName;

                object model;
                public object Model
                {
                    get
                    {
                        if (model != null)
                            return model;

                        //#if !NET35
                        //                        var xx = new NJsonSchema.Generation.JsonSchemaGeneratorSettings()
                        //                        {
                        //                            DefaultEnumHandling = NJsonSchema.EnumHandling.String,
                        //                            //AllowReferencesWithProperties = true,
                        //                            //GenerateKnownTypes = false,
                        //                        };

                        //                        var schemaTask = NJsonSchema.JsonSchema4.FromTypeAsync(Type, xx);
                        //                        schemaTask.Wait();
                        //                        model = JsonConvert.DeserializeObject(schemaTask.Result.ToJson());
                        //#endif

                        var qw = new JSchemaGenerator();
                        qw.SchemaIdGenerationHandling = SchemaIdGenerationHandling.TypeName;
                        qw.DefaultRequired = Required.Default;
                        //qw.DefaultRequired = Required.AllowNull;
                        var schema = qw.Generate(Type);
                        var jObject = (JsonConvert.DeserializeObject(schema.ToString()) as Newtonsoft.Json.Linq.JObject);

                        

                        jObject.Remove("definitions");
                        model = jObject;
                        //Newtonsoft.Json.Linq.JObject jk;
                        //jk.Remove

                        return model;
                    }
                }

                internal Api(Type type)
                {
                    model = null;
                    Type = type;
                    TypeName = type.Name;
                }
            }

            public int Id { get; set; }
            public string Route { get; set; }
            public string Description { get; set; }
            public Api Request { get; set; }
            public Api Response { get; set; }

            internal Help(ControllerAction action)
            {
                Id = action.Id;
                Route = action.Route;
                Description = action.Description;
                Request = new Api(action.RequestType);
                Response = new Api(action.ResponseType);
            }
        }

        internal static string PartialRoute = "[...]";

        public Help ApiHelp { get; }
        public int ParametersCount { get; }
        public MethodInfo MethodInfo { get; }
        public IEnumerable<string> Tokens { get; }
        internal Node.ControllerInfo ControllerInfo { get; }


        public Type RequestType { get; }
        public Type ResponseType { get; }
        object RequestModel { get; }
        object ResponseModel { get; }


        public int Id { get; }
        public string Route { get; }
        public int MaximumBytes { get; }
        public bool ValidateModel { get; }
        public int Security { get; internal set; }
        public string Description { get; }
        public string[] Roles { get; internal set; }

        public ModelAttribute ModelAttribute { get; }
        public ActionAttribute ActionAttribute { get; }

        //public List<Attribute> Filters { get; }
        public List<ActionFilterAttribute> Filters { get; }

        string ParseRoute(string route)
        {
            var tokens = new List<string>();
            var tokenSB = new StringBuilder();

            var newToken = true;

            for (var i = 0; i < route.Length; i++)
            {
                if (newToken)
                {
                    newToken = false;
                    tokenSB.Append(route[i]);
                }
                else
                {
                    if (char.IsDigit(route[i]) || char.IsLower(route[i]))
                        tokenSB.Append(route[i]);
                    else if (char.IsUpper(route[i]))
                    {
                        if (!char.IsUpper(route[i - 1]))
                        {
                            tokens.Add(tokenSB.ToString().ToLowerInvariant());
                            newToken = true;
                            tokenSB.Clear();
                        }

                        tokenSB.Append(route[i]);
                    }
                }
            }

            if (tokenSB.Length > 0)
            {
                tokens.Add(tokenSB.ToString().ToLowerInvariant());
                tokenSB.Clear();
            }

            for (var i = 0; i < tokens.Count; i++)
                tokenSB.Append(i > 0 ? $" {tokens[i]}" : tokens[i]);

            return tokenSB.ToString();
        }

        internal ControllerAction(Node.ControllerInfo controllerInfo, MethodInfo methodInfo, ActionAttribute actionAttribute)
        {
            MethodInfo = methodInfo;
            ControllerInfo = controllerInfo;
            ActionAttribute = actionAttribute;
            ModelAttribute = ActionAttribute.ModelAttribute;
            ParametersCount = MethodInfo.GetParameters().Length;

            // TODO: Also verify route from ModuleAttribute
            var nameSpace = ControllerInfo.Type.Namespace;
            var nameSpaceTokens = nameSpace.Split(new char[] { '.' });

            if (nameSpaceTokens.Last() == nameof(Controllers))
                nameSpaceTokens = nameSpaceTokens.Take(nameSpaceTokens.Length - 1).ToArray();

            var nameSpaceRoute = new StringBuilder();

            for (var i = 0; i < nameSpaceTokens.Length; i++)
            {
                nameSpaceRoute.Append(ParseRoute(nameSpaceTokens[i]));

                if (i + 1 != nameSpaceTokens.Length)
                    nameSpaceRoute.Append(' ');
            }

            //var nameSpaceRoute = nameSpaceTokens.Last() == nameof(Controllers) ?
            //    nameSpaceTokens[nameSpaceTokens.Length - 2] : nameSpaceTokens.Last();
            var moduleName = ControllerInfo.Attribute?.Module?.Name;

            var moduleRoute = moduleName != null ? ParseRoute(moduleName) : nameSpaceRoute.ToString();

            var controllerName = ControllerInfo.Type.Name;
            var controllerRoute = ControllerInfo.Attribute?.Route?.ToLowerInvariant() ?? ParseRoute(
                controllerName.Substring(0, controllerName.LastIndexOf(nameof(Controller))));
            var actionRoute = ActionAttribute.Route?.ToLowerInvariant() ?? ParseRoute(methodInfo.Name);

            var moduleControllerRoute = ControllerInfo.Attribute?.Route != null ?
                controllerRoute : $"{moduleRoute} {controllerRoute}";

            if (ActionAttribute.Route == null)
            {
                var handled = false;

                if (methodInfo.GetParameters().FirstOrDefault() is ParameterInfo pi)
                {
                    if (pi?.ParameterType?.GetTypeInfo()?.IsDefined(typeof(ModelAttribute), inherit: true) ?? false)
                    {
                        ActionAttribute.ModelType = pi.ParameterType;
                        handled = true;
                    }
                }

                if (!handled)
                {
                    //var route = GetRouteTokens(methodInfo.Name);

                    //if (!string.IsNullOrEmpty(ControllerInfo.Attribute?.Route))
                    //    route = $"{ControllerInfo.Attribute.Route} {route}";
                    //else
                    //    route = $"{ControllerInfo.Type.Name.Replace(nameof(Controller), string.Empty)} {route}";



                    //ActionAttribute.Route = string.IsNullOrEmpty(controllerRoute) ? actionRoute : $"{controllerRoute} {actionRoute}";
                    ActionAttribute.Route = string.IsNullOrEmpty(moduleControllerRoute) ? actionRoute : $"{moduleControllerRoute} {actionRoute}";

                    ActionAttribute.IsFullRoute = true;
                }
            }

            Route = ActionAttribute.Route;

            if (!ActionAttribute.IsFullRoute)
                //Route = string.IsNullOrEmpty(controllerRoute) ? Route : $"{controllerRoute} {Route}";
                Route = string.IsNullOrEmpty(moduleControllerRoute) ? Route : $"{moduleControllerRoute} {Route}";

            Roles = ActionAttribute.Roles;
            Security = ActionAttribute.Security;
            Id = Utilities.HashCode.GetFrom(Route);
            Description = ActionAttribute.Description;
            MaximumBytes = ActionAttribute.MaximumBytes;
            ValidateModel = ActionAttribute.ValidateModel;

            if (!Node.NodeControllers.TryParse(Route, out var tokens))
                throw new InvalidOperationException("Route tokens parsing error.");

            Tokens = tokens;
            var requestModelType = MethodInfo.GetParameters().FirstOrDefault()?.ParameterType ?? typeof(EmptyApiModel);
            /*
            var modelTypes = new List<Type>();

            if (requestModelType.Name.StartsWith(nameof(Nullable)))
            //if (requestModelType.Name.StartsWith("Nullable"))
                modelTypes = new List<Type>();

            var model = GetDefaultModel(requestModelType, modelTypes);
            modelTypes.Add(model.GetType());

            RequestModel = GetDefaultModel(model, modelTypes);

            modelTypes.Clear();
            model = GetDefaultModel(MethodInfo.ReturnType, modelTypes);
            modelTypes.Add(model.GetType());

            ResponseModel = GetDefaultModel(model, modelTypes);
            */


            RequestType = requestModelType;
            ResponseType = MethodInfo.ReturnType;

#if !NET35


            /*
            var xx = new NJsonSchema.Generation.JsonSchemaGeneratorSettings()
            {
                DefaultEnumHandling = NJsonSchema.EnumHandling.String,
                 AllowReferencesWithProperties = true,
                  GenerateKnownTypes = false,
            };

            RequestType = requestModelType;
            var schemaTask = NJsonSchema.JsonSchema4.FromTypeAsync(RequestType, xx);
            schemaTask.Wait();
            RequestModel = Newtonsoft.Json.JsonConvert.DeserializeObject(schemaTask.Result.ToJson());

            ResponseType = MethodInfo.ReturnType;
            schemaTask = NJsonSchema.JsonSchema4.FromTypeAsync(ResponseType, xx);
            schemaTask.Wait();
            ResponseModel = Newtonsoft.Json.JsonConvert.DeserializeObject(schemaTask.Result.ToJson());

            //Type ParseType(Type type)
            //{
            //    if (type == typeof(Task<>))
            //}
            */
#endif


            ApiHelp = new Help(this);

            Filters = MethodInfo.GetCustomAttributes<ActionFilterAttribute>(inherit: true)
                .Concat(ControllerInfo.Type.GetTypeInfo().GetCustomAttributes<ActionFilterAttribute>(inherit: true))
                //?.Where(p => p is IActionFilter || p is IAsyncActionFilter).OrderBy(p =>
                ?.OrderBy(p =>
                {
                    if (p.Filter is IOrderedFilter orderedFilter)
                        return orderedFilter.Group;

                    return int.MaxValue;
                }).ThenBy(p =>
                {
                    if (p.Filter is IOrderedFilter orderedFilter)
                        return orderedFilter.Order;

                    return int.MaxValue;
                })
            ?.ToList() ?? new List<ActionFilterAttribute>();
        }

        public override string ToString() => Route;

        object GetDefaultModel(object model, List<Type> modelTypes)
        {
            foreach (var property in model.GetType().GetProperties())
            {
                var propertyType = property.PropertyType;

                if (!propertyType.GetTypeInfo().IsValueType
                    && propertyType != typeof(string)
                    && !modelTypes.Contains(propertyType)
                    && !propertyType.IsInterfaceImplemented(typeof(IEnumerable))
                    && !property.IsDefined(typeof(Newtonsoft.Json.JsonIgnoreAttribute), inherit: false))
                {
                    if (property.GetSetMethod() == null)
                        continue;

                    modelTypes.Add(propertyType);

                    var propertyModel = GetDefaultModel(propertyType, modelTypes);
                    property.SetValue(model, propertyModel, null);

                    GetDefaultModel(propertyModel, modelTypes);
                }
            }

            return model;
        }

        object GetDefaultModel(Type type, List<Type> modelTypes)
        {
            var taskType = typeof(Task<>);
            var argumentType = typeof(Result);
            var nullableType = typeof(Nullable<>);
            //var type = MethodInfo.ReturnType;

            if (type.GetTypeInfo().IsGenericType && type.GetGenericArguments().Count() == 1)
            {
                argumentType = type.GetGenericArguments().SingleOrDefault();
                taskType = typeof(Task<>).MakeGenericType(argumentType);
            }

            if (type == taskType)
                type = argumentType;
            else if (type == typeof(Task) || type == typeof(void))
                type = typeof(Result);

            if (type == typeof(string))
                return string.Empty;

            if (type.Name == typeof(Nullable<>).Name)
                type = type.GetGenericArguments().Single();

            //if (type.GetTypeInfo().IsInterface || type.IsArray || type.IsInterfaceImplemented(typeof(IEnumerable)))
            if (type.IsArray || type.IsInterfaceImplemented(typeof(IEnumerable)))
            {
                var array = default(Array);
                var elementType = default(Type);

                if (type.IsArray)
                {
                    elementType = type.GetElementType();
                    array = Array.CreateInstance(elementType, length: 1);
                }
                else if (type.IsInterfaceImplemented(typeof(IEnumerable)))
                {
                    if (type.GetTypeInfo().IsGenericType)
                    {
                        var genericArgumentsType = Utilities.Reflection.GetGenericArguments(type);
                        elementType = genericArgumentsType[0];

                        if (genericArgumentsType.Length == 1)
                            array = Array.CreateInstance(elementType, length: 1);
                        // TODO: Implement for dictionaries
                        else if (genericArgumentsType.Length == 2)
                            array = Array.CreateInstance(elementType, length: 1);
                    }
                    else
                    {
                        elementType = typeof(object);
                        array = Array.CreateInstance(elementType, length: 1);
                    }
                }
                else
                    type = typeof(Result);

                var qq = default(object);

                if (elementType == typeof(string))
                    qq = string.Empty;
                else
                {
                    qq = Activator.CreateInstance(elementType);

                    if (!modelTypes.Contains(elementType))
                    {
                        modelTypes.Add(elementType);
                        qq = GetDefaultModel(qq, modelTypes);
                    }
                }

                array.SetValue(qq, 0);
                return array;
            }

            if (type.GetTypeInfo().IsInterface)
            {
                var properties = type.GetProperties();
                var dictionary = new Dictionary<string, object>();

                foreach (var property in properties)
                {
                    //if (!modelTypes.Contains(property.PropertyType))
                    //    modelTypes.Add(property.PropertyType);

                    dictionary[property.Name] = GetDefaultModel(property.PropertyType, modelTypes);
                }

                var jsondict = Newtonsoft.Json.JsonConvert.SerializeObject(dictionary);

                return Newtonsoft.Json.JsonConvert.DeserializeObject(jsondict);
            }

            //if (type.GetTypeInfo().IsGenericType)
            //    type = type.GetTypeInfo().MakeGenericType(new Type[] { typeof(object) });

            return Activator.CreateInstance(type);
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */

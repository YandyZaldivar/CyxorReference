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
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#if !NET35 || !NET40
using System.ComponentModel.DataAnnotations;
#endif

using Newtonsoft.Json;

namespace Cyxor.Controllers
{
    using Models;
    using Extensions;
    using Networking;
    using Networking.Filters;

    using Utilities = Networking.Utilities;

#if !NET35 || !NET40
    using Serialization;
#endif

    public abstract class Controller
    {
        protected internal virtual Module Module { get; set; }
        protected internal virtual Node Node { get; set; }
        protected internal virtual Connection Connection { get; set; }

        internal virtual ControllerAction CurrentAction { get; set; }

        protected internal virtual Context Context => Connection?.Context ?? Node.Context;

        //protected internal virtual MethodInfo OnInit { get; }
        //protected internal virtual object[] RequestParameters { get; }
        //protected internal virtual IEnumerable<ParameterInfo> RequestParametersInfo { get; }

        public Controller()
        {
            //// TODO: Change the initialize method for attributes on random methods!
            //if ((OnInit = GetType().GetMethod("Initialize", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)) != null)
            //{
            //    var requestParametersInfo = new List<ParameterInfo>();

            //    foreach (var parameter in OnInit.GetParameters())
            //        requestParametersInfo.Add(parameter);

            //    RequestParametersInfo = new ReadOnlyCollection<ParameterInfo>(requestParametersInfo);
            //    RequestParameters = new object[RequestParametersInfo.Count()];
            //}
        }

        protected virtual async Task<Result> OnActionExecutingAsync(ControllerAction action, params object[] parameters)
        {
            var result = Result.Success;

            for (var i = 0; i < action.Filters.Count; i++)
            {
                if (action.Filters[i].AsyncActionFilter != null)
                {
                    if (!(result = await action.Filters[i].AsyncActionFilter.OnActionExecutingAsync(Context).ConfigureAwait(false)))
                        return result;
                }
                else if (!(result = action.Filters[i].ActionFilter.OnActionExecuting(Context)))
                    return result;
            }

            return result;
        }

        protected virtual async Task<Result> OnActionExecutedAsync(ControllerAction action, params object[] parameters)
        {
            var result = Result.Success;

            for (var i = action.Filters.Count - 1; i >= 0; i--)
            {
                if (action.Filters[i].AsyncActionFilter != null)
                {
                    if (!(result = await action.Filters[i].AsyncActionFilter.OnActionExecutedAsync(Context).ConfigureAwait(false)))
                        return result;
                }
                else if (!(result = action.Filters[i].ActionFilter.OnActionExecuted(Context)))
                   return result;
            }

            return result;
        }


        #region void
        public void InvokeAction([CallerMemberName] string callerName = null)
            => InvokeAction(GetType(), callerName, parameters: null);

        public void InvokeAction(string methodName, params object[] parameters)
            => InvokeAction(GetType(), methodName, parameters);

        public void InvokeAction(MethodInfo methodInfo, params object[] parameters)
            => Invoke(Node, Connection, GetType(), methodInfo, parameters);

        public void InvokeAction<T>(string methodName, params object[] parameters)
            => InvokeAction(typeof(T), methodName, parameters);

        public void InvokeAction(object parameter, [CallerMemberName] string callerName = null)
            => InvokeAction(GetType(), callerName, parameter);

        public void InvokeAction<T>(object parameter, [CallerMemberName] string callerName = null)
            => InvokeAction(typeof(T), callerName, parameter);

        public void InvokeAction(Type controllerType, string methodName, params object[] parameters)
            => Invoke(Node, Connection, controllerType, methodName, parameters);
        #endregion

        #region Task
        public Task InvokeActionAsync([CallerMemberName] string callerName = null)
            => InvokeActionAsync(GetType(), callerName, parameters: null);

        public Task InvokeActionAsync(string methodName, params object[] parameters)
            => InvokeActionAsync(GetType(), methodName, parameters);

        public Task InvokeActionAsync(MethodInfo methodInfo, params object[] parameters)
            => Invoke(Node, Connection, GetType(), methodInfo, parameters) as Task;

        public Task InvokeActionAsync<T>(string methodName, params object[] parameters)
            => InvokeActionAsync(typeof(T), methodName, parameters);

        public Task InvokeActionAsync(object parameter, [CallerMemberName] string callerName = null)
            => InvokeActionAsync(GetType(), callerName, parameter);

        public Task InvokeActionAsync<T>(object parameter, [CallerMemberName] string callerName = null)
            => InvokeActionAsync(typeof(T), callerName, parameter);

        public Task InvokeActionAsync(Type controllerType, string methodName, params object[] parameters)
            => Invoke(Node, Connection, controllerType, methodName, parameters) as Task;
        #endregion

        #region TResult
        public TResult Invoke<TResult>([CallerMemberName] string callerName = null)
            => Invoke<TResult>(GetType(), callerName, parameters: null);

        public TResult Invoke<TResult>(string methodName, params object[] parameters)
            => Invoke<TResult>(GetType(), methodName, parameters);

        public TResult Invoke<TResult>(MethodInfo methodInfo, params object[] parameters)
            => (TResult)Invoke(Node, Connection, GetType(), methodInfo, parameters);

        public TResult Invoke<TController, TResult>(string methodName, params object[] parameters)
            => Invoke<TResult>(typeof(TController), methodName, parameters);

        public TResult Invoke<TResult>(object parameter, [CallerMemberName] string callerName = null)
            => Invoke<TResult>(GetType(), callerName, parameter);

        public TResult Invoke<TController, TResult>(object parameter, [CallerMemberName] string callerName = null)
            => Invoke<TResult>(typeof(TController), callerName, parameter);

        public TResult Invoke<TResult>(Type controllerType, string methodName, params object[] parameters)
            => (TResult)Invoke(Node, Connection, controllerType, methodName, parameters);
        #endregion

        #region Task<TResult>
        public Task<TResult> InvokeAsync<TResult>([CallerMemberName] string callerName = null)
            => InvokeAsync<TResult>(GetType(), callerName, parameters: null);

        public Task<TResult> InvokeAsync<TResult>(string methodName, params object[] parameters)
            => InvokeAsync<TResult>(GetType(), methodName, parameters);

        public Task<TResult> InvokeAsync<TResult>(MethodInfo methodInfo, params object[] parameters)
            => Invoke(Node, Connection, GetType(), methodInfo, parameters) as Task<TResult>;

        public Task<TResult> InvokeAsync<TController, TResult>([CallerMemberName] string callerName = null)
            => InvokeAsync<TResult>(typeof(TController), callerName, parameters: null);

        public Task<TResult> InvokeAsync<TController, TResult>(string methodName, params object[] parameters)
            => InvokeAsync<TResult>(typeof(TController), methodName, parameters);

        public Task<TResult> InvokeAsync<TResult>(object parameter, [CallerMemberName] string callerName = null)
            => InvokeAsync<TResult>(GetType(), callerName, parameter);

        public Task<TResult> InvokeAsync<TController, TResult>(object parameter, [CallerMemberName] string callerName = null)
            => InvokeAsync<TResult>(typeof(TController), callerName, parameter);

        public Task<TResult> InvokeAsync<TResult>(Type controllerType, string methodName, params object[] parameters)
            => Invoke(Node, Connection, controllerType, methodName, parameters) as Task<TResult>;
        #endregion Task<TResult>

        static object Invoke(Node node, Connection connection, Type controllerType, string methodName, params object[] parameters)
        {
            var methods = controllerType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(p => p.Name == methodName && p.IsDefined(typeof(ActionAttribute), inherit: true));

            if (methods.Count() == 1)
                return Invoke(node, connection, controllerType, methods.Single(), parameters);
            else if (methods.Count() == 0)
                //return new Result(ResultCode.Error, $"Controller method '{methodName}' not found.");
                throw new InvalidOperationException($"Controller method '{methodName}' not found.");
            else
                //return new Result(ResultCode.Error, $"Ambiguity in Controller method '{methodName}'.");
                throw new InvalidOperationException($"Ambiguity in Controller method '{methodName}'.");
        }

        internal static object Invoke(Node node, Connection connection, Type controllerType, MethodInfo methodInfo, params object[] parameters)
        {
            var result = Result.Success;
            var action = node.Controllers.FindAction(methodInfo);
            var controller = GetController(node, connection, action);

            //TODO: Uncomment to turn on auto validation
            if (action.ActionAttribute.ValidateModel && (parameters?.Length ?? 0) > 0)
                foreach (var parameter in parameters)
                    if (!(result = Utilities.Models.Validate(node, parameter)))
                    {
                        var validationErrors = result.GetModel<IEnumerable<ValidationError>>();
                        return CreateResultCopy(node, result, model: validationErrors, isCommand: true);
                    }

            return methodInfo.Invoke(controller, parameters);
        }

        static Result CreateResult(Node node, object model, bool isCommand = false) => new Result(model: model, backingSerializer: isCommand ? model.GetType() != typeof(byte[]) ? node.Config.CommandBackingSerializer : null : null);

        static Result CreateResultCopy(Node node, Result pResult, object model = default, bool isCommand = false)
        {
            return new Result(pResult, model: model, backingSerializer: isCommand ? (model?.GetType() ?? default) != typeof(byte[]) ? node.Config.CommandBackingSerializer : null : null);
        }

        internal static async Task<Result> InvokeAsync(bool isCommand, Node node, Connection connection, ControllerAction action, params object[] parameters)
        {
            var result = Result.Success;
            var scope = connection?.Scope ?? node.CommandScope;

            var controller = GetController(node, connection, action);

            if (!(result = await controller.OnActionExecutingAsync(action, parameters).ConfigureAwait(false)))
                return result;

            var value = default(object);

            //Result CreateResult(object model) => new Result(model: model, backingSerializer: IsCommand ? model.GetType() != typeof(string) && model.GetType() != typeof(byte[]) ? node.Config.CommandBackingSerializer : null : null);
            //Result CreateResultCopy(Result pResult, object model = default) => new Result(pResult, model: model, backingSerializer: IsCommand ? (model?.GetType() ?? default) != typeof(string) && (model?.GetType() ?? default) != typeof(byte[]) ? node.Config.CommandBackingSerializer : null : null);

            

            try
            {
                value = Invoke(node, connection, action.ControllerInfo.Type, action.MethodInfo, parameters);

                var context = connection?.Context ?? node.Context;
                //node.Log("getter");
                if (context == null)
                    context = connection?.Context ?? node.Context;

                if (context.Result.HasValue)
                    value = CreateResultCopy(node, context.Result.Value, value, isCommand: isCommand);
            }
            catch (Exception ex) when (ex is TargetInvocationException)
            {
                value = CreateResultCopy(node, new Result(exception: ex.InnerException ?? ex.InnerException), isCommand: isCommand);
            }

            // TODO: Make better solution
            //if (value is Result xResult)
            //    value = CreateResultCopy(xResult);

            if (value != null)
            {
                if (value is Result vResult)
                    result = vResult;
                else if (value is Task<Result> vTaskResult)
                    result = await vTaskResult.ConfigureAwait(false);
                else if (value is Task vTask)
                {
                    result = Result.Success;
                    await vTask.ConfigureAwait(false);

                    if (value.GetType().GetTypeInfo().IsGenericType)
                    {
                        var taskResult = value.GetType().GetProperty(nameof(Task<object>.Result)).GetGetMethod().Invoke(value, parameters: null);

                        if (taskResult != null)
                        {
                            if (taskResult.GetType().Name == typeof(View<>).Name)
                                taskResult = await RenderViewAsync(taskResult).ConfigureAwait(false);

                            result = CreateResult(node, taskResult, isCommand: isCommand);
                        }
                    }
                }
                else
                {
                    if (value.GetType().Name == typeof(View<>).Name)
                        value = await RenderViewAsync(value).ConfigureAwait(false);

                    result = CreateResult(node, value, isCommand: isCommand);
                }

                async Task<string> RenderViewAsync(object view)
                {
                    try
                    {
                        var service = scope.GetService<IViewEngine>();
                        return await service.RenderAsync(view, action.MethodInfo.Name).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        return
                            "<!DOCTYPE html>" +
                            "<html>" +
                            "<body>" +
                            "<h2>View error</h2><br />" +
                            $"<h4>{ex.Message}</h4><br />" +
                            $"<p>{ex.ToString()}</p>" +
                            "<footer>" +
                            "</footer>" +
                            "</body>" +
                            "</html>";
                    }
                }
            }

            await controller.OnActionExecutedAsync(action, parameters).ConfigureAwait(false);

            return result;
        }

        static Controller GetController(Node node, Connection connection, ControllerAction action)
        {
            var controllerInstances = connection?.Link?.ControllerInstances ?? node.ControllerInstances;

#pragma warning disable IDE0018 // Inline variable declaration
            var controller = default(Controller);
#pragma warning restore IDE0018 // Inline variable declaration

            while (!controllerInstances.TryGetValue(action.ControllerInfo.Type, out controller))
            {
                controller = Activator.CreateInstance(action.ControllerInfo.Type) as Controller;
                controller.Connection = connection;
                controller.Node = node;

                if (controllerInstances.TryAdd(action.ControllerInfo.Type, controller))
                    break;
            }

            controller.CurrentAction = action;

            var scope = connection?.Scope ?? node.CommandScope;

            foreach (var scopeInitializer in action.ControllerInfo.ScopeInitializers)
                scopeInitializer.Invoke(controller, scope);

            return controller;
        }

        //internal static async Task<object> InvokeAsync(bool IsCommand, Node node, Connection connection, Type controllerType, MethodInfo methodInfo, params object[] parameters)
        //{
        //    var value = Invoke(node, connection, controllerType, methodInfo, parameters);

        //    if (value is Task task)
        //    {
        //        await task.ConfigureAwait(false);

        //        if (value.GetType().GetTypeInfo().IsGenericType)
        //            value = value.GetType().GetProperty(nameof(Task<object>.Result)).GetGetMethod().Invoke(value, parameters: null);
        //    }

        //    return value;
        //}

        public static Result Validate(object instance)
#if NET35 || NET40
            => Result.Success;
#else
        {
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(instance, new ValidationContext(instance), validationResults, true))
                return new Result(ResultCode.Error, model: validationResults);

            return Result.Success;
        }
#endif
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */

/*
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.ObjectPool;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Cyxor.Networking
{
    using Controllers;

    public class RazorViewToStringRenderer : IViewEngine
    {
        private IRazorViewEngine _viewEngine;
        private ITempDataProvider _tempDataProvider;
        private IServiceProvider _serviceProvider;

        public RazorViewToStringRenderer(
            IRazorViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }

        public static void ConfigureServices(IServiceCollection services, string customApplicationBasePath = null)
        {
            string applicationName;
            IFileProvider fileProvider;
            if (!string.IsNullOrEmpty(customApplicationBasePath))
            {
                applicationName = Path.GetFileName(customApplicationBasePath);
                fileProvider = new PhysicalFileProvider(customApplicationBasePath);
            }
            else
            {
                //var sd = AppContext.BaseDirectory;
                var sd = Directory.GetCurrentDirectory();// .GetFullPath(Assembly.GetEntryAssembly().Location);
                //var sd = @"D:\Documents\Visual Studio 2017\Projects\Cyxor\Solution\Alimatic\Alimatic.Server\bin\Debug\net451";

                applicationName = Assembly.GetEntryAssembly().GetName().Name;
                fileProvider = new PhysicalFileProvider(sd);
            }

            services.AddSingleton<IHostingEnvironment>(new HostingEnvironment
            {
                ApplicationName = applicationName,
                WebRootFileProvider = fileProvider,
            });
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.FileProviders.Clear();
                options.FileProviders.Add(fileProvider);
            });
            var diagnosticSource = new DiagnosticListener("Microsoft.AspNetCore");
            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<DiagnosticSource>(diagnosticSource);
            services.AddLogging();
            services.AddMvc();
            services.AddTransient<RazorViewToStringRenderer>();
        }


        public async Task<string> RenderAsync(object viewModel, string viewName = null)
        {
            var model = viewModel.GetType().GetProperty(nameof(View<object>.Model)).GetGetMethod().Invoke(viewModel, parameters: null);
            var name = (string)viewModel.GetType().GetProperty(nameof(View<object>.Name)).GetGetMethod().Invoke(viewModel, parameters: null);
            name = name ?? viewName;


            var actionContext = GetActionContext();

            var viewEngineResult = _viewEngine.FindView(actionContext, name, false);

            if (!viewEngineResult.Success)
            {
                throw new InvalidOperationException(string.Format("Couldn't find view '{0}'", name));
            }

            var view = viewEngineResult.View;

            using (var output = new StringWriter())
            {
                var viewContext = new ViewContext(
                    actionContext,
                    view,
                    new ViewDataDictionary(
                        metadataProvider: new EmptyModelMetadataProvider(),
                        modelState: new ModelStateDictionary())
                    {
                        Model = model
                    },
                    new TempDataDictionary(
                        actionContext.HttpContext,
                        _tempDataProvider),
                    output,
                    new HtmlHelperOptions());

                await view.RenderAsync(viewContext);

                return output.ToString();
            }
        }

        private ActionContext GetActionContext()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.RequestServices = _serviceProvider;
            return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        }

    }
}
*/
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

    public class RazorViewEngine : IViewEngine
    {
        IRazorViewEngine ViewEngine;
        IServiceProvider ServiceProvider;
        ITempDataProvider TempDataProvider;

        public static void ConfigureServices(IServiceCollection services, string applicationBasePath = null)
        {
            var applicationName = default(string);
            var fileProvider = default(IFileProvider);

            if (!string.IsNullOrEmpty(applicationBasePath))
                applicationName = Path.GetFileName(applicationBasePath);
            else
            {
                applicationBasePath = Directory.GetCurrentDirectory();
                applicationName = Assembly.GetEntryAssembly().GetName().Name;
            }

            fileProvider = new PhysicalFileProvider(applicationBasePath);

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

            var diagnosticSource = new DiagnosticListener($"{nameof(Microsoft)}.{nameof(Microsoft.AspNetCore)}");

            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<DiagnosticSource>(diagnosticSource);
            services.AddLogging();
            services.AddMvc();

            services.AddTransient<RazorViewEngine>();
        }

        public RazorViewEngine(IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider)
        {
            ViewEngine = viewEngine;
            ServiceProvider = serviceProvider;
            TempDataProvider = tempDataProvider;
        }

        public async Task<string> RenderAsync(object viewModel, string viewName = null)
        {
            var model = viewModel.GetType().GetProperty(nameof(View<object>.Model)).GetGetMethod().Invoke(viewModel, parameters: null);
            var name = (string)viewModel.GetType().GetProperty(nameof(View<object>.Name)).GetGetMethod().Invoke(viewModel, parameters: null);
            name = name ?? viewName;

            var actionContext = new ActionContext(
                new DefaultHttpContext { RequestServices = ServiceProvider },
                new RouteData(),
                new ActionDescriptor());

            //var viewEngineResulteee = ViewEngine.GetView("Alimatic/Modules/DataDin/Views/", name, false);
            //var viewEngineResult = ViewEngine.GetView(null, $"/Alimatic/Modules/DataDin/Views/{name}.cshtml", true);

            var viewEngineResult = ViewEngine.FindView(actionContext, name, false);

            if (!viewEngineResult.Success)
                throw new InvalidOperationException($"Couldn't find view '{name}'");

            var view = viewEngineResult.View;

            using (var output = new StringWriter())
            {
                var viewDataDictionary = new ViewDataDictionary(
                    metadataProvider: new EmptyModelMetadataProvider(),
                    modelState: new ModelStateDictionary())
                { Model = model };

                var viewContext = new ViewContext(
                    actionContext,
                    view,
                    viewDataDictionary,
                    new TempDataDictionary(actionContext.HttpContext, TempDataProvider),
                    output,
                    new HtmlHelperOptions());

                await view.RenderAsync(viewContext);

                return output.ToString();
            }
        }
    }
}

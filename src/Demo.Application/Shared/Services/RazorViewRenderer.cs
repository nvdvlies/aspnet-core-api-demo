using System;
using System.IO;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Demo.Application.Shared.Services
{
    public class RazorViewRenderer : IRazorViewRenderer
    {
        private readonly ILogger<RazorViewRenderer> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IRazorViewEngine _viewEngine;

        public RazorViewRenderer(
            ILogger<RazorViewRenderer> logger,
            IRazorViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }

        public async Task<string> RenderViewAsync<TModel>(string viewName, TModel model)
        {
            _logger.LogInformation($"Executing {nameof(RazorViewRenderer)}.{nameof(RenderViewAsync)}");
            _logger.LogInformation("Rendering razor view '{viewName}' to html", viewName);

            var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            var viewEngineResult = _viewEngine.GetView(null, viewName, true);

            if (!viewEngineResult.Success)
            {
                throw new Exception($"Unable to find razor view '{viewName}'");
            }

            await using var output = new StringWriter();
            var viewContext = new ViewContext(
                actionContext,
                viewEngineResult.View,
                new ViewDataDictionary<TModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model
                },
                new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                output,
                new HtmlHelperOptions());

            await viewEngineResult.View.RenderAsync(viewContext);

            _logger.LogInformation($"Executed {nameof(RazorViewRenderer)}.{nameof(RenderViewAsync)}");

            return output.ToString();
        }
    }
}

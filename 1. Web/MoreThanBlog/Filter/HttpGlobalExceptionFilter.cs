using System;
using System.Threading.Tasks;
using Core.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace MoreThanBlog.Filter
{
    public class HttpGlobalExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<HttpGlobalExceptionFilter> _logger;

        public HttpGlobalExceptionFilter(ILogger<HttpGlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            // Ignore cancel action
            if (context.Exception is OperationCanceledException || context.Exception is ObjectDisposedException)
            {
                context.ExceptionHandled = true;

                return;
            }

            var errorModel = ExceptionContextHelper.GetErrorModel(context);

            // Response Result

            context.Result = new ContentResult
            {
                ContentType = "application/json",
                Content = errorModel.ToJsonString(),
                StatusCode = errorModel.StatusCode
            };

            context.ExceptionHandled = true;

            // Keep base Exception

            base.OnException(context);
        }
    }
}
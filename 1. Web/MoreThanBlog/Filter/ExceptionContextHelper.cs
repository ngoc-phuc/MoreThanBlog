using System;
using Core.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MoreThanBlog.Filter
{
    public static class ExceptionContextHelper
    {
        public static MoreThanBlogErrorModel GetErrorModel(ExceptionContext context)
        {
            MoreThanBlogErrorModel errorModel;

            switch (context.Exception)
            {
                case Core.Errors.MoreThanBlogException exception:
                    errorModel = new MoreThanBlogErrorModel(exception);
                    break;

                case UnauthorizedAccessException _:
                    errorModel = new MoreThanBlogErrorModel(StatusCodes.Status401Unauthorized,
                        nameof(ErrorCode.Unauthorized), ErrorCode.Unauthorized);
                    break;

                default:
                    var message = context.Exception.Message;

                    errorModel = new MoreThanBlogErrorModel(StatusCodes.Status500InternalServerError,
                        nameof(ErrorCode.Unknown), message);

                    // Add additional data
                    errorModel.AdditionalData.Add("stackTrace", context.Exception.StackTrace);

                    errorModel.AdditionalData.Add("innerException", context.Exception.InnerException?.Message);

                    errorModel.AdditionalData.Add("note",
                        "The message is exception message and additional data such as 'stackTrace', 'internalException' and 'note' only have in [Development Environment].");

                    break;
            }

            context.HttpContext.Response.StatusCode = errorModel.StatusCode;

            return errorModel;
        }
    }
}
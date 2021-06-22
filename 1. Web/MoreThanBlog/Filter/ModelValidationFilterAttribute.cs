using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using Core.Constants;
using Core.Errors;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace MoreThanBlog.Filter
{
    public class ModelValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
            {
                return;
            }

            // Log Error
            var keyValueInvalidDictionary = GetModelStateInvalidInfo(context);

            // Response Result
            var apiErrorViewModel =
                new MoreThanBlogErrorModel(StatusCodes.Status400BadRequest, nameof(ErrorCode.BadRequest), ErrorCode.BadRequest)
                {
                    AdditionalData = keyValueInvalidDictionary
                };

            context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            context.Result = new ContentResult
            {
                ContentType = "application/json",
                StatusCode = context.HttpContext.Response.StatusCode,
                Content = JsonConvert.SerializeObject(apiErrorViewModel, JsonSetting.JsonSerializerSettings)
            };
        }

        public static Dictionary<string, object> GetModelStateInvalidInfo(ActionExecutingContext context)
        {
            var keyValueInvalidDictionary = new Dictionary<string, object>();

            foreach (var keyValueState in context.ModelState)
            {
                var error = string.Join(", ", keyValueState.Value.Errors.Select(x => x.ErrorMessage));

                keyValueInvalidDictionary.Add(keyValueState.Key, error);
            }

            return keyValueInvalidDictionary;
        }
    }
}
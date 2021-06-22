using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace Core.Errors
{
    public class MoreThanBlogErrorModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        /// <summary>
        ///     Unique error code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///     Message/Description of error
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     Http response code
        /// </summary>
        public int StatusCode { get; set; }

        [JsonExtensionData]
        public Dictionary<string, object> AdditionalData { get; set; } = new Dictionary<string, object>();

        //public SystemErrorModel()
        //{
        //}

        //public SystemErrorModel(string code) : this()
        //{
        //    Code = code;
        //}

        //public SystemErrorModel(string code, string message) : this(code)
        //{
        //    Message = message;
        //}

        public MoreThanBlogErrorModel(int statusCode, string code, string message)
        {
            Code = code;
            StatusCode = statusCode;
            Message = string.IsNullOrWhiteSpace(message) ? GetDefaultMessageForStatusCode(statusCode) : message;
        }

        public MoreThanBlogErrorModel(MoreThanBlogException exception) : this(exception.StatusCode, exception.Code, exception.Message)
        {
            Id = exception.Id;

            AdditionalData = exception.AdditionalData;
        }

        private static string GetDefaultMessageForStatusCode(int statusCode)
        {
            switch (statusCode)
            {
                case StatusCodes.Status200OK:
                case StatusCodes.Status201Created:
                case StatusCodes.Status202Accepted:
                    return string.Empty;
                case StatusCodes.Status400BadRequest:
                    return ErrorCode.BadRequest;
                case StatusCodes.Status401Unauthorized:
                    return ErrorCode.Unauthorized;
                case StatusCodes.Status403Forbidden:
                    return ErrorCode.Forbidden;
                case StatusCodes.Status404NotFound:
                    return ErrorCode.ResourceNotFound;
                case StatusCodes.Status408RequestTimeout:
                    return ErrorCode.RequestTimeout;
                case StatusCodes.Status422UnprocessableEntity:
                    return ErrorCode.UnprocessableEntity;
                case 500:
                    return ErrorCode.SystemError;
                default:
                    return ErrorCode.AnUnexpectedErrorOccurred;
            }
        }
    }
}

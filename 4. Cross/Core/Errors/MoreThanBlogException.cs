using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Core.Errors
{
    public class MoreThanBlogException : Exception
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        public string Code { get; }

        public int StatusCode { get; set; }

        [JsonExtensionData]
        public Dictionary<string, object> AdditionalData { get; set; } = new Dictionary<string, object>();

        public MoreThanBlogErrorModel ErrorModel => ToExceptionModel();

        private MoreThanBlogErrorModel _errorModel;

        public MoreThanBlogException(string code, string message = "", int statusCode = StatusCodes.Status400BadRequest) : base(message)
        {
            Code = code;
            StatusCode = statusCode;
        }

        public MoreThanBlogException(MoreThanBlogErrorModel errorModel)
        {
            _errorModel = errorModel;
        }

        private MoreThanBlogErrorModel ToExceptionModel()
        {
            if (_errorModel != null)
            {
                return _errorModel;
            }

            _errorModel = new MoreThanBlogErrorModel(this);

            return _errorModel;
        }
    }
}
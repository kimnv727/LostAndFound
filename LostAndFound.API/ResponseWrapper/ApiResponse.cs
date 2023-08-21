using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using LostAndFound.Infrastructure.DTOs.Common;

namespace LostAndFound.API.ResponseWrapper
{
    public class ApiResponse
    {
        public IReadOnlyList<string> Errors { get; }
        public int StatusCode { get; }
        public bool IsError { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; }

        public ApiResponse(int statusCode, bool isError, string message)
        {
            StatusCode = statusCode;
            IsError = isError;
            Message = /*message ??*/ DefaultMessageForStatusCode(statusCode);

            Errors = new List<string>()
            {
                message
            };
        }

        private static string DefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                200 => "Ok",
                201 => "Created",
                202 => "Accepted",
                204 => "No Content",
                400 => "Bad request",
                401 => "Unauthorized",
                403 => "Not Permitted",
                404 => "Not found",
                500 => "Internal server error",
                _ => null
            };
        }
    }

    public class ApiOkResponse<T> : ApiResponse
    {
        public virtual T Result { get; }

        public ApiOkResponse(T result, string message) : base(200, false, message)
        {
            Result = result;
        }
    }

    public class ApiPaginatedOkResponse<T> : ApiOkResponse<IEnumerable<T>>
    {
        public Pagination Pagination { get; set; }

        public ApiPaginatedOkResponse(IEnumerable<T> result, Pagination pagination, string message) : base(result,
            message)
        {
            Pagination = pagination;
        }
    }

    public class ApiNotFoundResponse : ApiResponse
    {
        public IReadOnlyList<string> Errors { get; }
        public ApiNotFoundResponse(string message) : base(404, true, message)
        {
            Errors = new List<string>()
            {
                message
            };
        }
    }

    public class ApiBadRequestResponse : ApiResponse
    {
        public IReadOnlyDictionary<string, List<string>> Errors { get; }

        public ApiBadRequestResponse(ModelStateDictionary modelState, string message) : base(400, true, message)
        {
            Errors = modelState.ToDictionary(e => e.Key, e => e.Value.Errors.Select(e => e.ErrorMessage).ToList());
        }

        public ApiBadRequestResponse(string message) : base(400, true, message)
        {
        }

        public ApiBadRequestResponse(string propName, string message) : base(400, true, null)
        {
            Errors = new Dictionary<string, List<string>>
            {
                {propName, new List<string> {message}}
            };
        }
    }

    public class ApiCreatedResponse<T> : ApiResponse
    {
        public T Result { get; }

        public ApiCreatedResponse(T result, string message) : base(201, false, message)
        {
            Result = result;
        }
    }

    public class ApiInternalServerErrorResponse : ApiResponse
    {
        public ApiInternalServerErrorResponse(string message) : base(500, true, message)
        {
        }
    }

    public class ApiUnauthorizedResponse : ApiResponse
    {
        public ApiUnauthorizedResponse(string message) : base(401, true, message)
        {
        }
    }
}
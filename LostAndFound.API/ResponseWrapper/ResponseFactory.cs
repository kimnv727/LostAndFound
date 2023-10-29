using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using LostAndFound.Infrastructure.DTOs.Common;

namespace LostAndFound.API.ResponseWrapper
{
    public static class ResponseFactory
    {
        public static ActionResult PaginatedOk<T>(T result, string message = null) =>
            new OkObjectResult(new ApiOkResponse<T>(result, message));

        public static ActionResult PaginatedOk<T>(PaginatedResponse<T> result, string message = null) =>
            new OkObjectResult(new ApiPaginatedOkResponse<T>(result, result.Pagination, message));

        public static ActionResult Accepted(string message = null) => new AcceptedResult();

        public static ActionResult NotFound(string message = null) =>
            new NotFoundObjectResult(new ApiNotFoundResponse(message));

        public static ActionResult NoContent() => new NoContentResult();

        public static ActionResult CreatedAt<T>(string actionName, string controllerName, object routeValues, T obj,
            string message = null) => new CreatedAtActionResult(actionName, controllerName.Replace("Controller", ""),
            routeValues, new ApiCreatedResponse<T>(obj, message));

        public static ActionResult BadRequest(ModelStateDictionary modelState, string message = null) =>
            new BadRequestObjectResult(new ApiBadRequestResponse(modelState, message));

        public static ActionResult BadRequest(string propName, string message) =>
            new BadRequestObjectResult(new ApiBadRequestResponse(propName, message));

        public static ActionResult BadRequest(string message) =>
            new BadRequestObjectResult(new ApiBadRequestResponse(message));
    }
}
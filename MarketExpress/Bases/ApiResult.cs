using MarketExpress.Core.Results;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MarketExpress.Api.Bases
{
    public class ApiResult<T> : IActionResult
    {
        private readonly ApiResponse<T> _apiResponse;

        public ApiResult(ApiResponse<T> apiResponse)
        {
            _apiResponse = apiResponse;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = new ObjectResult(_apiResponse)
            {
                StatusCode = _apiResponse.StatusCode


            };

            await objectResult.ExecuteResultAsync(context);
        }


        public static ApiResult<T> Ok(T data, string message = "Operation successful.")
        {
            return new ApiResult<T>(ApiResponse<T>.SuccessResponse((int)HttpStatusCode.OK, data, message));
        }

        public static ApiResult<T> Created(T data, string message = "Resource created successfully.")
        {
            return new ApiResult<T>(ApiResponse<T>.SuccessResponse((int)HttpStatusCode.Created, data, message));
        }

        public static ApiResult<T> NoContent(string message = "Operation successful.")
        {
            return new ApiResult<T>(ApiResponse<T>.SuccessResponse((int)HttpStatusCode.NoContent, default, message));
        }

        public static ApiResult<T> BadRequest(string message = "Bad Request.", List<string>? errors = null)
        {
            return new ApiResult<T>(ApiResponse<T>.BadRequestResponse(message, errors));
        }

        public static ApiResult<T> NotFound(string message = "Resource not found.")
        {
            return new ApiResult<T>(ApiResponse<T>.NotFoundResponse(message));
        }
        public static ApiResult<T> Conflict(string message = "Confilct.")
        {
            return new ApiResult<T>(ApiResponse<T>.ConflictResponse(message));
        }
        public static ApiResult<T> InternalServerError(string message = "Internal Server Error.")
        {
            return new ApiResult<T>(ApiResponse<T>.InternalServerErrorResponse(message));
        }
        public static ApiResult<T> Unauthorized(string message = "Invalid user ID")
        {
            return new ApiResult<T>(ApiResponse<T>.UnauthorizedResponse(message));
        }
    }
    public class ApiResult : IActionResult
    {
        private readonly ApiResponse _apiResponse;

        public ApiResult(ApiResponse apiResponse)
        {
            _apiResponse = apiResponse;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = new ObjectResult(_apiResponse)
            {
                StatusCode = _apiResponse.StatusCode


            };

            await objectResult.ExecuteResultAsync(context);
        }


        public static ApiResult Ok( string message = "Operation successful.")
        {
            return new ApiResult(ApiResponse.SuccessResponse((int)HttpStatusCode.OK, message));
        }

        public static ApiResult Created( string message = "Resource created successfully.")
        {
            return new ApiResult(ApiResponse.SuccessResponse((int)HttpStatusCode.Created, message));
        }

        public static ApiResult NoContent(string message = "Operation successful.")
        {
            return new ApiResult(ApiResponse.SuccessResponse((int)HttpStatusCode.NoContent, message));
        }

        public static ApiResult BadRequest(string message = "Bad Request.", List<string>? errors = null)
        {
            return new ApiResult(ApiResponse.BadRequestResponse(message, errors));
        }

        public static ApiResult NotFound(string message = "Resource not found.")
        {
            return new ApiResult(ApiResponse.NotFoundResponse(message));
        }
        public static ApiResult Conflict(string message = "Confilct.")
        {
            return new ApiResult(ApiResponse.ConflictResponse(message));
        }
        public static ApiResult InternalServerError(string message = "Internal Server Error.")
        {
            return new ApiResult(ApiResponse.InternalServerErrorResponse(message));
        }
        public static ApiResult Unauthorized(string message = "Invalid user ID")
        {
            return new ApiResult(ApiResponse.UnauthorizedResponse(message));
        }
    }
}

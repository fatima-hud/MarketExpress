using MarketExpress.Core.Results;
using System.ComponentModel;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MarketExpress.Api.Bases
{
    public class ApiResponse<T>
    {

        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public ApiResponse(int statusCode, bool success, string message = "", T? data = default, List<string>? errors = null)
        {
            StatusCode = statusCode;
            Success = success;
            Message = message;
            Data = data;
            Errors = errors ?? new List<string>();
        }

        public static ApiResponse<T> SuccessResponse(int statusCode, T data, string message = "Operation successful.")
        {
            if (statusCode == (int)HttpStatusCode.NoContent)
            {
                return new ApiResponse<T>(statusCode, true, message, default, null);
            }
            return new ApiResponse<T>(statusCode, true, message, data, null);
        }

        public static ApiResponse<T> ErrorResponse(int statusCode, string message = "An error occurred.", List<string>? errors = null)
        {
            return new ApiResponse<T>(statusCode, false, message, default, errors);
        }

        public static ApiResponse<T> BadRequestResponse(string message = "Bad Request.", List<string>? errors = null)
        {
            return ErrorResponse((int)HttpStatusCode.BadRequest, message, errors);
        }

        public static ApiResponse<T> NotFoundResponse(string message = "Resource not found.")
        {
            return ErrorResponse((int)HttpStatusCode.NotFound, message);
        }
        public static ApiResponse<T> ConflictResponse(string message = "Conflict.")
        {
            return ErrorResponse((int)HttpStatusCode.Conflict, message);
        }

        public static ApiResponse<T> InternalServerErrorResponse(string message = "Internal Server Error.")
        {
            return ErrorResponse((int)HttpStatusCode.InternalServerError, message);
        }
        public static ApiResponse<T> UnauthorizedResponse(string message = "Invalid user ID")
        {
            return ErrorResponse((int)HttpStatusCode.Unauthorized, message);
        }

    }
    public class ApiResponse
    {

        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public object? Data { get; set; } = null;

        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new List<string>();

        public ApiResponse(int statusCode,object? Data=null, bool success=true, string message = "", List<string>? errors = null)
        {
            StatusCode = statusCode;
            Success = success;
            Message = message;
            Errors = errors ?? new List<string>();
        }

        public static ApiResponse SuccessResponse(int statusCode,object? data=default , string message = "Operation successful.")
        {
            if (statusCode == (int)HttpStatusCode.NoContent)
            {
                return new ApiResponse(statusCode, data, true, message, null);
            }
            return new ApiResponse(statusCode, data, true, message, null);
        }

        public static ApiResponse ErrorResponse(int statusCode, string message = "An error occurred.", List<string>? errors = null)
        {
            return new ApiResponse(statusCode, default, false, message, errors);
        }

        public static ApiResponse BadRequestResponse(string message = "Bad Request.", List<string>? errors = null)
        {
            return ErrorResponse((int)HttpStatusCode.BadRequest, message, errors);
        }

        public static ApiResponse NotFoundResponse(string message = "Resource not found.")
        {
            return ErrorResponse((int)HttpStatusCode.NotFound, message);
        }
        public static ApiResponse ConflictResponse(string message = "Conflict.")
        {
            return ErrorResponse((int)HttpStatusCode.Conflict, message);
        }

        public static ApiResponse InternalServerErrorResponse(string message = "Internal Server Error.")
        {
            return ErrorResponse((int)HttpStatusCode.InternalServerError, message);
        }
        public static ApiResponse UnauthorizedResponse(string message = "Invalid user ID")
        {
            return ErrorResponse((int)HttpStatusCode.Unauthorized, message);
        }
    }
}


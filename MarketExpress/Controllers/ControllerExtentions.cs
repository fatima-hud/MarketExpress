
using MarketExpress.Api.Bases;
using MarketExpress.Core.Results;
using Microsoft.AspNetCore.Mvc;
namespace MarketExpress.Api.Extention
{
    public static class ControllerExtensions
    {

        public static ApiResult<TValue> ToApiResult<TValue>(this ControllerBase controller, Result<TValue> result)

        {
            if (result.IsSuccess)
            {
              return  ApiResult<TValue>.Ok(result.Value!);
            }
            return result.FailureType switch
            {
                ResultFailureType.NotFound => ApiResult<TValue>.NotFound(result.Message),
                ResultFailureType.BadRequest => ApiResult<TValue>.BadRequest(result.Message, result.Errors),
                ResultFailureType.Conflict => ApiResult<TValue>.Conflict(result.Message),
                ResultFailureType.Unauthorized => ApiResult<TValue>.BadRequest(result.Message),
                ResultFailureType.Forbidden => ApiResult<TValue>.BadRequest(result.Message),
                _ => ApiResult<TValue>.InternalServerError(result.Message)
            };

        }
        public static ApiResult ToApiResult(this ControllerBase controller, Result result)
        {
            if (result.IsSuccess)
            {
               return ApiResult.Ok(result.Message!);
            }
            return result.FailureType switch
            {
                ResultFailureType.NotFound => ApiResult.NotFound(result.Message),
                ResultFailureType.BadRequest => ApiResult.BadRequest(result.Message, result.Errors),
                ResultFailureType.Conflict => ApiResult.Conflict(result.Message),
                ResultFailureType.Unauthorized => ApiResult.BadRequest(result.Message),
                ResultFailureType.Forbidden => ApiResult.BadRequest(result.Message),
                _ => ApiResult.InternalServerError(result.Message)
            };
        }

    }
}
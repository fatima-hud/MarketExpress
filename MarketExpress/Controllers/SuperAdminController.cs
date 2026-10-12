using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Dto;
using MarketExpress.Application.Dto.Order.Response;
using MarketExpress.Application.Dto.RequestStore.Request;
using MarketExpress.Application.Dto.RequestStore.Response;
using MarketExpress.Application.Dto.StoreCategory.Response;
using MarketExpress.Application.Dto.User;
using MarketExpress.Application.Services.StoreCategories;
using MarketExpress.Application.Services.SuperAdmins;
using MarketExpress.Application.Services.Users;
using MarketExpress.Core.DtoHelping;
using MarketExpress.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SuperAdminController : ControllerBase
    {
        private readonly ISuperAdminService _superAdminService;
        private readonly IStoreCategoryService _storeCategoryService;
        private readonly IUserService _userService;
        private readonly ILogger<SuperAdminController> _logger;
        public SuperAdminController(ISuperAdminService superAdminService, IStoreCategoryService storeCategoryService, IUserService userService, ILogger<SuperAdminController> logger)
        {
            _superAdminService = superAdminService;
            _storeCategoryService = storeCategoryService;
            _userService = userService;
            _logger = logger;
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("{userId}")]
        public async Task<ApiResult<bool>> RevokeToken(string userId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<bool>.BadRequest(errors);

                }
                var result = await _superAdminService.RevokeTokenAsync(userId);
                if (!result.IsSuccess)
                {
                    return this.ToApiResult(result);//"Token is invalid"

                }
                return ApiResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while blocked user");
                return ApiResult<bool>.InternalServerError("حدث خطا داخلي");
            }

        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("{userId}")]
        public async Task<ApiResult<bool>> UnbanUserAsync(string userId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<bool>.BadRequest(errors);

                }
                var result = await _superAdminService.UnbanUserAsync(userId);
                if (!result.IsSuccess)
                {
                    return this.ToApiResult(result);//"Token is invalid"

                }
                return ApiResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while unblocked user");
                return ApiResult<bool>.InternalServerError("حدث خطا داخلي");
            }

        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("{storeId}")]
        public async Task<ApiResult<List<ResponseGetStoreCategoryDto>>> GetAllStoreCategoryAsync(int storeId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<List<ResponseGetStoreCategoryDto>>.BadRequest(errors);
                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<List<ResponseGetStoreCategoryDto>>.Unauthorized();
                }
                var res = await _storeCategoryService.GetAllStoreCategoryAsync(storeId);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<List<ResponseGetStoreCategoryDto>>.Ok(res.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Get All Store Category");
                return ApiResult<List<ResponseGetStoreCategoryDto>>.InternalServerError("حدث خطا داخلي");
            }


        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<ApiResult> AddRole(AddRoleDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult.BadRequest(errors);
                }
                var SadminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(SadminId))
                {
                    return ApiResult.Unauthorized();
                }

                var res = await _superAdminService.AddRoleAsync(SadminId, dto);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult.Created();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while add role");
                return ApiResult.InternalServerError("حدث خطا داخلي");
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete]
        public async Task<ApiResult> RemoveRole(AddRoleDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult.BadRequest(errors);
                }
                var SadminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(SadminId))
                {
                    return ApiResult.Unauthorized();
                }

                var res = await _superAdminService.RemoveRoleAsync(SadminId, dto);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while remove role");
                return ApiResult.InternalServerError("حدث خطا داخلي");
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPatch("{requestId}")]
        public async Task<ApiResult<ResponseApprovedStoreRequestDto>> RequestApproved(int requestId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<ResponseApprovedStoreRequestDto>.BadRequest(errors);

                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<ResponseApprovedStoreRequestDto>.Unauthorized();
                }
                var res = await _superAdminService.ApprovedStoreRequestAsync(requestId);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<ResponseApprovedStoreRequestDto>.Ok(res.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while approved request");
                return ApiResult<ResponseApprovedStoreRequestDto>.InternalServerError("حدث خطا داخلي");
            }

        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpPatch]
        public async Task<ApiResult<ResponseRejectedStoreRequestDto>> RequestRejected([FromBody] RequestRejectdDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<ResponseRejectedStoreRequestDto>.BadRequest(errors);

                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<ResponseRejectedStoreRequestDto>.Unauthorized();
                }
                var res = await _superAdminService.RejectedStoreRequestAsync(request);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<ResponseRejectedStoreRequestDto>.Ok(res.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while reject request");
                return ApiResult<ResponseRejectedStoreRequestDto>.InternalServerError("حدث خطا داخلي");
            }

        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public async Task<ApiResult<List<ResponsePendingStoreDto>>> GetAllRequestStoreByFilter([FromQuery] enStoreStatus storeStatus, int pageNumber, int pageSize)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<List<ResponsePendingStoreDto>>.BadRequest(errors);

                }

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<List<ResponsePendingStoreDto>>.Unauthorized();
                }
                var res = await _superAdminService.GetAllRequestStoreByFilter(storeStatus, pageNumber, pageSize);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<List<ResponsePendingStoreDto>>.Ok(res.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while get all request store");
                return ApiResult<List<ResponsePendingStoreDto>>.InternalServerError("حدث خطا داخلي");
            }

        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public async Task<ApiResult<List<ResponseGetActiveUserDto>>> ActiveUsers()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<List<ResponseGetActiveUserDto>>.BadRequest(errors);

                }

                var result = await _userService.GetActiveUsersAsync();
                if (!result.IsSuccess)
                {
                    return this.ToApiResult(result);

                }
                return ApiResult<List<ResponseGetActiveUserDto>>.Ok(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while get active users");
                return ApiResult<List<ResponseGetActiveUserDto>>.InternalServerError("حدث خطا داخلي");
            }

        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public async Task<ApiResult<List<ResponseUserDto>>> BannedUsers()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<List<ResponseUserDto>>.BadRequest(errors);

                }

                var result = await _userService.GetBannedUsersAsync();
                if (!result.IsSuccess)
                {
                    return this.ToApiResult(result);

                }
                return ApiResult<List<ResponseUserDto>>.Ok(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while banned users");
                return ApiResult<List<ResponseUserDto>>.InternalServerError("حدث خطا داخلي");
            }

        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete]
        public async Task<ApiResult> DeleteUser(string userId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult.BadRequest(errors);

                }
                var result = await _userService.DeleteUserAsync(userId);
                if (!result.IsSuccess)
                {
                    return this.ToApiResult(result);

                }
                return ApiResult.Ok("تم الحذف بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while delete user");
                return ApiResult.InternalServerError("حدث خطا داخلي");
            }

        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public async Task<ApiResult<List<ResponseAddOrderDto>>> GetAllFilterOrders(enOrderStatus orderStatus, int pageNumber, int pageSize)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<List<ResponseAddOrderDto>>.BadRequest(errors);

                }

                var result = await _superAdminService.GetAllFilterOrders(orderStatus, pageNumber, pageSize);
                if (!result.IsSuccess)
                {
                    return this.ToApiResult(result);

                }
                return ApiResult<List<ResponseAddOrderDto>>.Ok(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while All Filter Orders");
                return ApiResult<List<ResponseAddOrderDto>>.InternalServerError("حدث خطا داخلي");
            }






        }
        /*  [Authorize(Roles = "SuperAdmin")]
          [HttpGet("{userId}")]
          public async Task<ApiResult<List<ResponseGetRolesByUserDto>>> GetRolesByUserAsync(string userId)
          {
              try
              {
                  if (!ModelState.IsValid)
                  {
                      var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                      return ApiResult<List<ResponseGetRolesByUserDto>>.BadRequest(errors);

                  }
                  var superAdminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                  if (string.IsNullOrEmpty(superAdminId))
                  {
                      return ApiResult<List<ResponseGetRolesByUserDto>>.Unauthorized();
                  }
                  var res = await _superAdminService.GetAllRolesByUserAsync(userId);
                  if (!res.IsSuccess)
                  {
                      return this.ToApiResult(res);
                  }
                  return ApiResult<List<ResponseGetRolesByUserDto>>.Ok(res.Value);
              }
              catch (Exception ex)
              {
                  _logger.LogError(ex, "Error occurred while get roles");
                  return ApiResult<List<ResponseGetRolesByUserDto>>.InternalServerError("حدث خطا داخلي");
              }

          }*/

    }
}

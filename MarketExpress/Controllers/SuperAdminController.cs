using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Abstracts;
using MarketExpress.Application.Dto;
using MarketExpress.Application.Dto.Comment.Response;
using MarketExpress.Application.Dto.RequestStore.Request;
using MarketExpress.Application.Dto.RequestStore.Response;
using MarketExpress.Application.Dto.StoreCategory.Response;
using MarketExpress.Application.Implementations;
using MarketExpress.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SuperAdminController : ControllerBase
    {
        private readonly ISuperAdminService _superAdminService;
        private readonly IStoreCategoryService _storeCategoryService;




        public SuperAdminController(ISuperAdminService superAdminService, IStoreCategoryService storeCategoryService)
        {
            _superAdminService = superAdminService;
            _storeCategoryService = storeCategoryService;
        }
        /*
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("{userId}")]
        public async Task<ApiResult<bool>> RevokeToken(string userId)
        {

            if (!ModelState.IsValid)
            {
                return ApiResult<bool>.BadRequest(ModelState.ToString());
            }
            var result = await _superAdminService.RevokeTokenAsync(userId);
            if (!result.IsSuccess)
            {
                return this.ToApiResult(result);//"Token is invalid"

            }
            return ApiResult<bool>.Ok(true);

        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("{storeId}")]
        public async Task<ApiResult<List<ResponseGetStoreCategoryDto>>> GetAllStoreCategoryAsync(int storeId)
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
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
         public async Task<ApiResult> AddRole(AddRoleDto dto)
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
        
        [Authorize(Roles = "SuperAdmin")]
        [HttpPatch("{requestId}")]
        public async Task<ApiResult<ResponseApprovedStoreRequestDto>> RequestApproved(int requestId)
        {
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
        [Authorize(Roles = "SuperAdmin")]
        [HttpPatch]
        public async Task<ApiResult<ResponseRejectedStoreRequestDto>> RequestRejected([FromBody] RequestRejectdDto request)
        {
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
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public async Task<ApiResult<List<ResponsePendingStoreDto>>> GetAllRequestStoreByFilter([FromQuery]enStoreStatus storeStatus,int pageNumber,int pageSize)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<List<ResponsePendingStoreDto>>.Unauthorized();
            }
            var res=await _superAdminService.GetAllRequestStoreByFilter(storeStatus,pageNumber,pageSize);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<List<ResponsePendingStoreDto>>.Ok(res.Value);

        }



        */

    }
}

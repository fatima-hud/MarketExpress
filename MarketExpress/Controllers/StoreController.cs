using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Abstacts;
using MarketExpress.Application.Dto.Request;
using MarketExpress.Application.Dto.RequestStore.Request;
using MarketExpress.Application.Dto.RequestStore.Response;
using MarketExpress.Application.Dto.Store.Response;
using MarketExpress.Application.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;

        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }
       
     
        [Authorize(Roles ="SuperAdmin")]
        [HttpPut ("SoftDelete/{id}")]

        public async Task<ApiResult> DeleteAsync(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult.Unauthorized();
            }
            if (!ModelState.IsValid)
            {
                return ApiResult.BadRequest(ModelState.ToString());
            }
            var res = await _storeService.SoftDeleteAsync(id);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult.Ok();
        }
     

        [HttpGet("GetAllStores")]
        public async Task<ApiResult<List<ResponseStoreDto>>> GetAllStores()
        {
           
            var res = await _storeService.GetAllStoresAsync();
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<List<ResponseStoreDto>>.Ok(res.Value);
        }
        [Authorize(Roles ="Admin")]
        [HttpGet("GetStoresByAdmin")]
        public async Task<ApiResult<ResponseOwnerStoreDto>> GetStoresByAdmin()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<ResponseOwnerStoreDto>.Unauthorized();
            }
            var res = await _storeService.GetStoreByAdminAsync(userId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<ResponseOwnerStoreDto>.Ok(res.Value);
        }







    }
    }


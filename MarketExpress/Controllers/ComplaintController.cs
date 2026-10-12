using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Dto.Comment.Response;
using MarketExpress.Application.Dto.Complaint.Request;
using MarketExpress.Application.Dto.Complaint.Response;
using MarketExpress.Application.Services.Complaints;
using MarketExpress.Core.DtoHelping.Complaint;
using MarketExpress.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ComplaintController : ControllerBase
    {
        private readonly IComplaintService _complaintService;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ILogger<ComplaintController> _logger;
        public ComplaintController(IComplaintService complaintService, IHubContext<ChatHub> hubContext, ILogger<ComplaintController> logger)
        {
            _complaintService = complaintService;
            _hubContext = hubContext;
            _logger = logger;
        }
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<ApiResult<ResponseAddComplaintDto>> AddComplaint([FromBody] RequestAddComplaintDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<ResponseAddComplaintDto>.BadRequest(errors);

                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<ResponseAddComplaintDto>.Unauthorized();
                }
                var result = await _complaintService.AddAsync(userId, request);
                if (!result.IsSuccess)
                    return this.ToApiResult(result);


                return ApiResult<ResponseAddComplaintDto>.Created(result.Value);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a complaint.");
                return ApiResult<ResponseAddComplaintDto>.InternalServerError("حدث خطا داخلي ");
            }
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<List<ResponseGetComplaintByStoreDto>>> GetAllComplaints()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<List<ResponseGetComplaintByStoreDto>>.Unauthorized();
                }
                var result = await _complaintService.GetAllByStoreAsync(userId);
                if (!result.IsSuccess)
                    return this.ToApiResult(result);
                return ApiResult<List<ResponseGetComplaintByStoreDto>>.Ok(result.Value);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all complaints.");
                return ApiResult<List<ResponseGetComplaintByStoreDto>>.InternalServerError("حدث خطا داخلي ");
            }
        }
        [HttpGet]
        [Authorize]
        public async Task<ApiResult<List<ResponseGetComplaintByUserDto>>> GetAllComplaintsByUser()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<List<ResponseGetComplaintByUserDto>>.Unauthorized();
                }
                var result = await _complaintService.GetAllByUserAsync(userId);
                if (!result.IsSuccess)
                    return this.ToApiResult(result);
                return ApiResult<List<ResponseGetComplaintByUserDto>>.Ok(result.Value);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all complaints by user.");
                return ApiResult<List<ResponseGetComplaintByUserDto>>.InternalServerError("حدث خطا داخلي ");
            }
        }

    }
}

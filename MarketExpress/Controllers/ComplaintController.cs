using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Abstacts;
using MarketExpress.Application.Dto.Complaint.Request;
using MarketExpress.Application.Dto.Complaint.Response;
using MarketExpress.Application.Dto.Response;
using MarketExpress.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintController : ControllerBase
    {
        private readonly IComplaintService _complaintService;
        private readonly IHubContext<ChatHub> _hubContext;

        public ComplaintController(IComplaintService complaintService, IHubContext<ChatHub> hubContext)
        {
            _complaintService = complaintService;
            _hubContext = hubContext;
        }
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<ApiResult<ResponseAddComplaintDto>> AddComplaint([FromBody] RequestAddComplaintDto request)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult<ResponseAddComplaintDto>.BadRequest(ModelState.ToString());
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
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<List<ResponseGetComplaintDto>>> GetAllComplaints()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<List<ResponseGetComplaintDto>>.Unauthorized();
            }
            var result = await _complaintService.GetAllAsync(userId);
            if (!result.IsSuccess)
                return this.ToApiResult(result);
            return ApiResult<List<ResponseGetComplaintDto>>.Ok(result.Value);
        }
    }
}

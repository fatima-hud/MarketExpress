using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Abstracts;
using MarketExpress.Application.Dto.Message.Request;
using MarketExpress.Application.Dto.Message.Response;
using MarketExpress.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IHubContext<ChatHub> _hubContext;

        public MessageController(IMessageService messageService, IHubContext<ChatHub> hubContext)
        {
            _messageService = messageService;
            _hubContext = hubContext;
        }
      /*  [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<ApiResult<ResponseMessageDto>> SendMessage([FromBody] RequestAddMessageDto request)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult<ResponseMessageDto>.BadRequest(ModelState.ToString());
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<ResponseMessageDto>.Unauthorized();
            }
            var result = await _messageService.SendMessageAsync(userId, request);
            if (!result.IsSuccess)
                return ApiResult<ResponseMessageDto>.BadRequest(result.Message);
            await _hubContext.Clients.Group(request.ComplaintId.ToString())
                                      .SendAsync("ReceiveMessage", result.Value.SenderId, result.Value.MessageText);
            return ApiResult<ResponseMessageDto>.Created(result.Value);
        }


        [Authorize(Roles = "Admin,User")]
        [HttpDelete]
        public async Task<ApiResult> RemoveMessage([FromBody] RequestRemoveMessageDto request)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult.BadRequest(ModelState.ToString());
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult.Unauthorized();
            }
            var result = await _messageService.RemoveMessageAsync(userId, request);
            if (!result.IsSuccess)
                return this.ToApiResult(result);

            await _hubContext.Clients.Group(request.ComplaintId.ToString())
    .SendAsync("MessageRemoved", new
    {
        MessageId = request.MessageId
    });
            return ApiResult.Ok("Message removed successfully");
        }


        [Authorize(Roles = "Admin,User")]
        [HttpPut("UpdateMessage")]
        public async Task<ApiResult<ResponseUpdateMessageDto>> UpdateMessage([FromBody] RequestUpdateMessageDto request)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult<ResponseUpdateMessageDto>.BadRequest(ModelState.ToString());
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<ResponseUpdateMessageDto>.Unauthorized();
            }
            var result = await _messageService.UpdateMessageAsync(userId, request);
            if (!result.IsSuccess)
                return this.ToApiResult(result);


              await _hubContext.Clients.Group(request.ComplaintId.ToString())
                                        .SendAsync("MessageUpdated", result.Value.MessageId, result.Value.MessageText);
              await _hubContext.Clients.Group(request.ComplaintId.ToString())
                    .SendAsync("ReceiveMessage", userId, request.MessageText, result.Value.MessageId);


            await _hubContext.Clients.Group(request.ComplaintId.ToString())
      .SendAsync("MessageUpdated", new
      {
          MessageId = result.Value.MessageId,
          MessageText = result.Value.MessageText
      });
            return ApiResult<ResponseUpdateMessageDto>.Ok(result.Value);


        }
        [Authorize(Roles = "Admin,User")]
        [HttpPut("ReadMessage")]
        public async Task<ApiResult<List<ResponseUpdateMessageDto>>> ReadMessage(RequestUpdateReadMessageDto request)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult<List<ResponseUpdateMessageDto>>.BadRequest(ModelState.ToString());
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<List<ResponseUpdateMessageDto>>.Unauthorized();
            }
            var result = await _messageService.UpdateReadMessageAsync(userId, request);
            if (!result.IsSuccess)
                return this.ToApiResult(result);
            await _hubContext.Clients.Group(request.ComplaintId.ToString())
            .SendAsync("MessageReadStatusChanged", new
            {
                complaintId = request.ComplaintId,
                messages = result.Value
            }
                );
            await _hubContext.Clients.Group(request.ComplaintId.ToString())
              .SendAsync("ReceiveMessage", userId, result.Value);


            return ApiResult<List<ResponseUpdateMessageDto>>.Ok(result.Value);



        }
        [Authorize(Roles = "Admin,User")]
        [HttpGet("{complaintId}")]
        public async Task<ApiResult<List<ResponseGetMessageDto>>> GetMessagesByComplaintId(int complaintId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<List<ResponseGetMessageDto>>.Unauthorized();
            }
            var result = await _messageService.GetMessagesByComplaintIdAsync(userId, complaintId);
            if (!result.IsSuccess)
                return this.ToApiResult(result);
            return ApiResult<List<ResponseGetMessageDto>>.Ok(result.Value);

        }*/
    }
}
using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Dto.Complaint.Response;
using MarketExpress.Application.Dto.Message.Request;
using MarketExpress.Application.Dto.Message.Response;
using MarketExpress.Application.Services.Messages;
using MarketExpress.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ILogger<MessageController> _logger;

        public MessageController(IMessageService messageService, IHubContext<ChatHub> hubContext, ILogger<MessageController> logger)
        {
            _messageService = messageService;
            _hubContext = hubContext;
            _logger = logger;
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


          }*/
        [Authorize(Roles = "Admin,User")]
        [HttpPut]
        public async Task<ApiResult<int>> ReadMessage(RequestUpdateReadMessageDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<int>.BadRequest(errors);

                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<int>.Unauthorized();
                }
                var res = await _messageService.UpdateReadMessageAsync(userId, request);
                if (!res.IsSuccess)
                    return this.ToApiResult(res);

                await _hubContext.Clients.Group(request.ComplaintId.ToString())
                  .SendAsync("ReceiveMessage", userId, res.Value);


                return ApiResult<int>.Ok(res.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, " An error occurred while updating read message.");
                return ApiResult<int>.InternalServerError("حدث خطا داخلي ");
            }



        }
        [Authorize(Roles = "Admin,User")]
        [HttpGet("{complaintId}")]
        public async Task<ApiResult<List<ResponseGetMessageDto>>> GetMessagesByComplaintId(int complaintId)
        {
            try
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
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting messages by complaint ID.");
                return ApiResult<List<ResponseGetMessageDto>>.InternalServerError("حدث خطا داخلي ");
            }

        }
    }
}
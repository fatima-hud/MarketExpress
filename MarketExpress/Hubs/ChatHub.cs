using MarketExpress.Application.Dto.Message.Request;
using MarketExpress.Application.Dto.Message.Response;
using MarketExpress.Application.Services.Messages;
using MarketExpress.Core.Entity;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace MarketExpress.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;
        private readonly IComplaintRepository _complaintRepository;
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(IMessageService messageService, IComplaintRepository complaintRepository, ILogger<ChatHub> logger)
        {
            _messageService = messageService;
            _complaintRepository = complaintRepository;
            _logger = logger;
        }

        public async Task SendMessageAsync(RequestAddMessageDto request)
        {
            Console.WriteLine("دخل SendMessageAsync");
            var senderId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(senderId))
            {
                throw new HubException("المستخدم غير مصرح له (Unauthorized) أو التوكن غير صحيح.");
            }

            var complaint = await GetAuthorizedComplaintAsync(senderId, request.ComplaintId);
            var res = await _messageService.SendMessageAsync(senderId, request);
            if (!res.IsSuccess)
            {
                throw new HubException(res.Message);
            }


            var savedMessage = res.Value;

            var ret = new ResponseGetMessageDto
            {
                Id = savedMessage.Id,
                SenderId = savedMessage.SenderId,
                SenderName = savedMessage.SenderName,
                SenderImageUrl = savedMessage.SenderImageUrl,

                MessageText = savedMessage.MessageText,
                SentAt = savedMessage.SentAt,
                IsRead = savedMessage.IsRead


            };

            await Clients.Group($"complaint-{request.ComplaintId}").SendAsync("ReceiveMessage", ret);

        }
        public async Task EditMessageAsync(RequestUpdateMessageDto request)
        {
            var senderId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(senderId))
            {
                throw new HubException("المستخدم غير مصرح له (Unauthorized) أو التوكن غير صحيح.");
            }
            var complaint = await GetAuthorizedComplaintAsync(senderId, request.ComplaintId);

            var res = await _messageService.UpdateMessageAsync(senderId, request);
            if (!res.IsSuccess)
            {
                throw new HubException(res.Message);
            }
            var savedMessage = res.Value;



            await Clients.Group($"complaint-{request.ComplaintId}").SendAsync("MessageEdited", savedMessage);




        }
        public async Task MarkMessagesAsRead(RequestUpdateReadMessageDto request)
        {
            try
            {
                var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                    throw new HubException("المستخدم غير مصرح له (Unauthorized) أو التوكن غير صحيح.");
                var complaint = await GetAuthorizedComplaintAsync(userId, request.ComplaintId);


                var res = await _messageService.UpdateReadMessageAsync(userId, request);

                if (!res.IsSuccess)
                    throw new HubException(res.Message);


                var senderId = complaint.StoreOwnerId == userId ? complaint.CustomerId : complaint.StoreOwnerId;
                await Clients.Group($"complaint-{request.ComplaintId}").SendAsync("MessagesMarkedAsRead", new
                {
                    complaintId = request.ComplaintId,
                    readBy = userId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(
          ex,
          "Error marking messages as read. ComplaintId: {ComplaintId}",
          request.ComplaintId);

                throw new HubException("حدث خطا داخلي");
            }
        }
        public async Task DeleteMessageAsync(RequestRemoveMessageDto request)
        {

            var senderId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(senderId))
            {
                throw new HubException("المستخدم غير مصرح له (Unauthorized) أو التوكن غير صحيح.");
            }
            var complaint = await GetAuthorizedComplaintAsync(senderId, request.ComplaintId);

            var res = await _messageService.RemoveMessageAsync(senderId, request);
            if (!res.IsSuccess)
            {
                throw new HubException(res.Message);
            }
            var ret = res.Value;
            await Clients
      .Group($"complaint-{request.ComplaintId}")
      .SendAsync("MessageDeleted", ret);


        }


        public async Task JoinComplaintGroup(int complaintId)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new HubException("المستخدم غير مصرح له.");
            }

            var complaint = await GetAuthorizedComplaintAsync(userId, complaintId);


            var groupName = $"complaint-{complaintId}";

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);


        }
        private async Task<Complaint> GetAuthorizedComplaintAsync(
    string userId,
    int complaintId)
        {
            var complaint = await _complaintRepository
                .GetByIdAsync(complaintId);

            if (complaint == null)
            {
                throw new HubException("الشكوى غير موجودة.");
            }

            if (complaint.CustomerId != userId &&
                complaint.StoreOwnerId != userId)
            {
                throw new HubException(
                    "ليس لديك صلاحية للدخول إلى هذه المحادثة.");
            }

            return complaint;
        }

    }
}




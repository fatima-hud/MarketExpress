using MarketExpress.Application.Dto.Complaint.Request;
using MarketExpress.Application.Dto.Message.Request;
using MarketExpress.Application.Dto.Message.Response;
using MarketExpress.Application.Services.Complaints;
using MarketExpress.Application.Services.Messages;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Results;
using MarketExpress.Database;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MimeKit;
using System.Security.Claims;

namespace MarketExpress.Hubs
{
    [Authorize]
    public class ChatHub:Hub
    {
        private readonly IMessageService _messageService;
        private readonly IComplaintRepository _complaintRepository;

        public ChatHub(IMessageService messageService, IComplaintRepository complaintRepository)
        {
            _messageService = messageService;
            _complaintRepository = complaintRepository;
        }

        public async Task SendMessageAsync(RequestAddMessageDto request)
        {
            Console.WriteLine("دخل SendMessageAsync");
            var senderId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(senderId))
            {
                throw new HubException("المستخدم غير مصرح له (Unauthorized) أو التوكن غير صحيح.");
            }
            
         
            var res=await _messageService.SendMessageAsync(senderId, request);
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
            Console.WriteLine($"Receiver = {savedMessage.ReceiverId}");
            Console.WriteLine($"Sender = {ret.SenderId}");
            await  Clients.User(savedMessage.ReceiverId).SendAsync("ReceivePrivateMessage", ret);
            await Clients.User(ret.SenderId).SendAsync("ReceivePrivateMessage", ret);
            Console.WriteLine("بعد messageService");
        }
        public async Task EditMessageAsync(RequestUpdateMessageDto request)
        {
            var senderId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(senderId))
            {
                throw new HubException("المستخدم غير مصرح له (Unauthorized) أو التوكن غير صحيح.");
            }
            var res=await _messageService.UpdateMessageAsync(senderId, request);
            if (!res.IsSuccess)
            {
                throw new HubException(res.Message);
            }
            var savedMessage = res.Value;
          
            await Clients.User(senderId).SendAsync("ReceiveEditedMessage", savedMessage);
            await Clients.User(savedMessage.ReceiverId).SendAsync("ReceiveEditedMessage", savedMessage);




        }
        public async Task MarkMessagesAsRead(RequestUpdateReadMessageDto request)
        {
            try
            {
                var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                    throw new HubException("المستخدم غير مصرح له (Unauthorized) أو التوكن غير صحيح.");

                var res = await _messageService.UpdateReadMessageAsync(userId, request);

                if (!res.IsSuccess)
                    throw new HubException(res.Message);
                var complaint = await _complaintRepository.GetByIdAsync(request.ComplaintId);
                if (complaint == null) throw new HubException(res.Message);
                var senderId = complaint.StoreOwnerId == userId ? complaint.CustomerId : complaint.StoreOwnerId;
                await Clients.User(senderId).SendAsync("MessagesRead", new
                {
                    complaintId = request.ComplaintId,
                    readBy = userId
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errors {ex.Message}");
            }
        }
        public async Task DeleteMessageAsync(RequestRemoveMessageDto request)
        {

            var senderId=Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(string.IsNullOrEmpty(senderId))
            {
                throw new HubException("المستخدم غير مصرح له (Unauthorized) أو التوكن غير صحيح.");
            }
            var res= await _messageService.RemoveMessageAsync(senderId, request);
            if (!res.IsSuccess)
            {
                throw new HubException(res.Message);
            }
            var complaint= await _complaintRepository.GetByIdAsync(request.ComplaintId);
            if (complaint == null)
            {
                throw new HubException("الشكوى غير موجودة.");
            }
            var ret = res.Value;
            await Clients.Users(complaint.CustomerId,complaint.StoreOwnerId).SendAsync("ReceiveDeletedMessage", ret);
          

        }
    }
}

    


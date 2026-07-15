using MarketExpress.Application.Dto.Message.Request;
using MarketExpress.Application.Dto.Message.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Identity;

namespace MarketExpress.Application.Services.Messages
{
    public class MessageService : IMessageService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMessageRepository _messageRepository;
        private readonly IComplaintRepository _complaintRepository;

        public MessageService(UserManager<User> userManager, IMessageRepository messageRepository, IComplaintRepository complaintRepository)
        {
            _userManager = userManager;
            _messageRepository = messageRepository;
            _complaintRepository = complaintRepository;
        }

        public async Task<Result<List<ResponseGetMessageDto>>> GetMessagesByComplaintIdAsync(string userId, int complaintId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return Result<List<ResponseGetMessageDto>>.NotFound("مستخدم غير موجود");
            }
            if (user.IsBanned)
            {
                return Result<List<ResponseGetMessageDto>>.BadRequest("مستخدم محظور");
            }
            var complaint = await _complaintRepository.GetByIdAsync(complaintId);
            if (complaint is null)
            {
                return Result<List<ResponseGetMessageDto>>.NotFound("شكوى غير موجودة");
            }
            var isUserInvolved = complaint.CustomerId == userId || complaint.StoreOwnerId == userId;
            if (!isUserInvolved)
            {
                return Result<List<ResponseGetMessageDto>>.BadRequest("انت غير منضم الى هذه المحادثة");
            }
            var messages = await _messageRepository.GetAllMessageByComplaintIdAsync(complaintId);
            if (messages == null || messages.Count == 0)
            {
                return Result<List<ResponseGetMessageDto>>.Success(new List<ResponseGetMessageDto>());
            }
            var response = messages.Select(m => new ResponseGetMessageDto
            {
                Id = m.Id,
                SenderImageUrl = m.Sender.ProfilePhoto,
                SenderName = m.Sender.FullName,
                SenderId = m.SenderId,
                MessageText = m.MessageText,
                SentAt = m.SentAt,
                IsRead=m.IsRead,
               IsEdited= m.IsEdited,
               UpdatedAt=m.UpdatedAt
              

            }).ToList();
            return Result<List<ResponseGetMessageDto>>.Success(response);

        }

        public async Task<Result<ResponseRemoveMessageDto>> RemoveMessageAsync(string SenderId, RequestRemoveMessageDto request)
        {
            var user = await _userManager.FindByIdAsync(SenderId);
            if (user is null)
            {
                return Result<ResponseRemoveMessageDto>.NotFound("مستخدم غير موجود");
            }
            if (user.IsBanned)
            {
                return Result<ResponseRemoveMessageDto>.BadRequest("مستخدم محظور");
            }
            var complaint = await _complaintRepository.GetByIdAsync(request.ComplaintId);
            if (complaint is null)
            {
                return Result<ResponseRemoveMessageDto>.NotFound("شكوى غير موجودة");
            }
            var message = await _messageRepository.GetByIdAsync(request.MessageId);
            if (message is null)
            {
                return Result<ResponseRemoveMessageDto>.NotFound("رسائل غير موجودة");
            }
            var isMessageExistComplaint = await _messageRepository.IsMessageFoundByComplaintAsync(request.ComplaintId, request.MessageId);
            if (!isMessageExistComplaint)
            {
                return Result<ResponseRemoveMessageDto>.BadRequest("هذه رسالة غير تابعة للشكوى");
            }
            var isUserInvolved = complaint.CustomerId == SenderId || complaint.StoreOwnerId == SenderId;
            if (!isUserInvolved)
            {
                return Result<ResponseRemoveMessageDto>.BadRequest("انت غير منضم الى هذه المحادثة");
            }
            var isUserSend = message.SenderId == SenderId;
            if (!isUserSend)
            {
                return Result<ResponseRemoveMessageDto>.BadRequest("الرسالة ليست لك");

            }
            var ret = new ResponseRemoveMessageDto
            {
                ComplaintId = complaint.Id,
                MessageId = message.Id,
                ReceiverId = complaint.StoreOwnerId == SenderId ? complaint.CustomerId : complaint.StoreOwnerId
            };  
            await _messageRepository.DeleteAsync(message);
            return Result<ResponseRemoveMessageDto>.Success(ret);
        }

        public async Task<Result<ResponseMessageDto>> SendMessageAsync(string senderId, RequestAddMessageDto request)
        {
            var sender = await _userManager.FindByIdAsync(senderId);
            if (sender == null)
                return Result<ResponseMessageDto>.NotFound("مستخدم غير موجود");
            if (sender.IsBanned)
            {
                return Result<ResponseMessageDto>.BadRequest("مستخدم محظور");
            }
            var complaint = await _complaintRepository.GetByIdAsync(request.ComplaintId);
            if (complaint == null)
                return Result<ResponseMessageDto>.NotFound("شكوى غير موجودة");
            var isUserInvolved = complaint.CustomerId == senderId || complaint.StoreOwnerId == senderId;
            if (!isUserInvolved)
            {
                return Result<ResponseMessageDto>.BadRequest("انت غير منضم الى هذه المحادثة");
            }

            var message = new Message
            {
                SenderId = senderId,
                ComplaintId = request.ComplaintId,
                MessageText = request.MessageText,
                SentAt = DateTime.UtcNow,
                IsRead = false
                

            };
            await _messageRepository.AddAsync(message);
            var response = new ResponseMessageDto
            {
                Id = message.Id,
                ComplaintId = message.ComplaintId,
                ReceiverId=complaint.StoreOwnerId == senderId ? complaint.CustomerId : complaint.StoreOwnerId,
                SenderId = message.SenderId,
                SenderName = sender.FullName,
                SenderImageUrl = sender.ProfilePhoto,
                MessageText = message.MessageText,
                SentAt = message.SentAt,
                IsRead = message.IsRead
            };
            return Result<ResponseMessageDto>.Success(response);


        }

        public async Task<Result<ResponseUpdateMessageDto>> UpdateMessageAsync(string SenderId, RequestUpdateMessageDto request)
        {
            var user = await _userManager.FindByIdAsync(SenderId);
            if (user is null)
            {
                return Result<ResponseUpdateMessageDto>.NotFound("مستخدم غير موجود");
            }
            if (user.IsBanned)
            {
                return Result<ResponseUpdateMessageDto>.BadRequest("مستخدم محظور");
            }
            var complaint = await _complaintRepository.GetByIdAsync(request.ComplaintId);
            if (complaint is null)
            {
                return Result<ResponseUpdateMessageDto>.NotFound("شكوى غير موجودة");
            }
            var message = await _messageRepository.GetByIdAsync(request.MessageId);
            if (message is null)
            {
                return Result<ResponseUpdateMessageDto>.NotFound("رسالة غير موجودة");
            }
            var isMessageExistComplaint = await _messageRepository.IsMessageFoundByComplaintAsync(request.ComplaintId, request.MessageId);
            if (!isMessageExistComplaint)
            {
                return Result<ResponseUpdateMessageDto>.BadRequest("رسالة غير تابعة للشكوى");
            }
            var isUserInvolved = complaint.CustomerId == SenderId || complaint.StoreOwnerId == SenderId;
            if (!isUserInvolved)
            {
                return Result<ResponseUpdateMessageDto>.BadRequest("انت غير منضم الى هذه المحادثة");
            }
            var isUserSend = message.SenderId == SenderId;
            if (!isUserSend)
            {
                return Result<ResponseUpdateMessageDto>.BadRequest("رسالة ليست لك");

            }
            if (message.MessageText == request.MessageText)
            {
                return Result<ResponseUpdateMessageDto>.BadRequest("لم يتم إجراء أي تعديل على الرسالة");
            }
            message.MessageText = request.MessageText;
            message.IsEdited = true;
            message.UpdatedAt = DateTime.UtcNow;
          
            await _messageRepository.UpdateAsync(message);
            var ret = new ResponseUpdateMessageDto
            {
                ComplaintId = complaint.Id,
                MessageId = message.Id,
                MessageText = message.MessageText,
                UpdatedAt= message.UpdatedAt??DateTime.UtcNow,
                IsEdited=message.IsEdited,
                ReceiverId=complaint.StoreOwnerId==SenderId?complaint.CustomerId:complaint.StoreOwnerId
                
                
            };
            return Result<ResponseUpdateMessageDto>.Success(ret, "تم تعديل الرسالة بنجاح");
        }

        public async Task<Result<int>> UpdateReadMessageAsync(string userId, RequestUpdateReadMessageDto request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return Result<int>.NotFound("مستخدم غير موجود");
            }
            if (user.IsBanned)
            {
                return Result<int>.BadRequest("مستخدم محظور");
            }
            var complaint = await _complaintRepository.GetByIdAsync(request.ComplaintId);
            if (complaint is null)
            {
                return Result<int>.NotFound("شكوى غير موجودة");
            }


            var isUserInvolved = complaint.CustomerId == userId || complaint.StoreOwnerId == userId;
            if (!isUserInvolved)
            {
                return Result<int>.BadRequest("انت غير منضم الى هذه المحادثة");
            }
            var messagesCount = await _messageRepository.MarkAsReadAsync(userId, request.ComplaintId);

       
            return Result<int>.Success(messagesCount, $"تم قراءة {messagesCount} رسالة بنجاح");
         }


    }
}

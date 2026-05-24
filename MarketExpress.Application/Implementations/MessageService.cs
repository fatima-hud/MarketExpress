using MarketExpress.Application.Abstracts;
using MarketExpress.Application.Dto.Message.Request;
using MarketExpress.Application.Dto.Message.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Identity;

namespace MarketExpress.Application.Implementations
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
                return Result<List<ResponseGetMessageDto>>.NotFound("لا يوجد رسائل في هذه الشكوى");
            }
            var response = messages.Select(m => new ResponseGetMessageDto
            {
                Id = m.Id,
                SenderImageUrl = m.Sender.ProfilePhoto,
                SenderName = m.Sender.FullName,
                SenderId = m.SenderId,
                MessageText = m.MessageText,
                SentAt = m.SentAt

            }).ToList();
            return Result<List<ResponseGetMessageDto>>.Success(response);

        }

        public async Task<Result> RemoveMessageAsync(string SenderId, RequestRemoveMessageDto request)
        {
            var user = await _userManager.FindByIdAsync(SenderId);
            if (user is null)
            {
                return Result.NotFound("مستخدم غير موجود");
            }
            if (user.IsBanned)
            {
                return Result.BadRequest("مستخدم محظور");
            }
            var complaint = await _complaintRepository.GetByIdAsync(request.ComplaintId);
            if (complaint is null)
            {
                return Result.NotFound("شكوى غير موجودة");
            }
            var message = await _messageRepository.GetByIdAsync(request.MessageId);
            if (message is null)
            {
                return Result.NotFound("رسائل غير موجودة");
            }
            var isMessageExistComplaint = await _messageRepository.IsMessageFoundByComplaintAsync(request.ComplaintId, request.MessageId);
            if (!isMessageExistComplaint)
            {
                return Result.BadRequest("هذه رسالة غير تابعة للشكوى");
            }
            var isUserInvolved = complaint.CustomerId == SenderId || complaint.StoreOwnerId == SenderId;
            if (!isUserInvolved)
            {
                return Result.BadRequest("انت غير منضم الى هذه المحادثة");
            }
            var isUserSend = message.SenderId == SenderId;
            if (!isUserSend)
            {
                return Result.BadRequest("الرسالة ليست لك");

            }
            await _messageRepository.DeleteAsync(message);
            return Result.Success("تم حذف الرسالة بنجاح");
        }

        public async Task<Result<ResponseMessageDto>> SendMessageAsync(string SenderId, RequestAddMessageDto request)
        {
            var user = await _userManager.FindByIdAsync(SenderId);
            if (user == null)
                return Result<ResponseMessageDto>.NotFound("مستخدم غير موجود");
            if (user.IsBanned)
            {
                return Result<ResponseMessageDto>.BadRequest("مستخدم محظور");
            }
            var complaint = await _complaintRepository.GetByIdAsync(request.ComplaintId);
            if (complaint == null)
                return Result<ResponseMessageDto>.NotFound("شكوى غير موجودة");
            var isUserInvolved = complaint.CustomerId == SenderId || complaint.StoreOwnerId == SenderId;
            if (!isUserInvolved)
            {
                return Result<ResponseMessageDto>.BadRequest("انت غير منضم الى هذه المحادثة");
            }

            var message = new Message
            {
                SenderId = SenderId,
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
                SenderId = message.SenderId,
                SenderName = user.FullName,
                SenderImageUrl = user.ProfilePhoto,
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
            message.MessageText = request.MessageText;
            await _messageRepository.UpdateAsync(message);
            var ret = new ResponseUpdateMessageDto
            {
                ComplaintId = complaint.Id,
                MessageId = message.Id,
                MessageText = message.MessageText
            };
            return Result<ResponseUpdateMessageDto>.Success(ret, "تم تعديل الرسالة بنجاح");
        }

        public async Task<Result<List<ResponseUpdateMessageDto>>> UpdateReadMessageAsync(string userId, RequestUpdateReadMessageDto request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return Result<List<ResponseUpdateMessageDto>>.NotFound("مستخدم غير موجود");
            }
            if (user.IsBanned)
            {
                return Result<List<ResponseUpdateMessageDto>>.BadRequest("مستخدم محظور");
            }
            var complaint = await _complaintRepository.GetByIdAsync(request.ComplaintId);
            if (complaint is null)
            {
                return Result<List<ResponseUpdateMessageDto>>.NotFound("شكوى غير موجودة");
            }


            var isUserInvolved = complaint.CustomerId == userId || complaint.StoreOwnerId == userId;
            if (!isUserInvolved)
            {
                return Result<List<ResponseUpdateMessageDto>>.BadRequest("انت غير منضم الى هذه المحادثة");
            }
            var messages = await _messageRepository.GetAllMessageUnReadAsync(userId, request.ComplaintId);


            foreach (var msg in messages)
            {
                msg.IsRead = true;
            }

            await _messageRepository.UpdateRangeAsync(messages);
            var ret = messages.Select(m => new ResponseUpdateMessageDto
            {
                ComplaintId = complaint.Id,

                MessageId = m.Id,
                MessageText = m.MessageText
            }).ToList();

            return Result<List<ResponseUpdateMessageDto>>.Success(ret, "تم قراءة رسالة بنجاح");
        }


    }
}

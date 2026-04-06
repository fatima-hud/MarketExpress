using MarketExpress.Application.Dto.Message.Request;
using MarketExpress.Application.Dto.Message.Response;
using MarketExpress.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Abstacts
{
   public interface IMessageService
    {
        Task<Result<ResponseMessageDto>> SendMessageAsync(string SenderId,RequestAddMessageDto request);
        Task<Result> RemoveMessageAsync(string SenderId, RequestRemoveMessageDto request);
        Task<Result<ResponseUpdateMessageDto>>UpdateMessageAsync(string SenderId, RequestUpdateMessageDto request);
        Task<Result<List<ResponseUpdateMessageDto>>>UpdateReadMessageAsync(string SenderId, RequestUpdateReadMessageDto request);
        Task<Result<List<ResponseGetMessageDto>>> GetMessagesByComplaintIdAsync(string userId, int complaintId);

    }
}

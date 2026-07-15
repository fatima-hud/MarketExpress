using MarketExpress.Application.Dto.Message.Request;
using MarketExpress.Application.Dto.Message.Response;
using MarketExpress.Core.Results;

namespace MarketExpress.Application.Services.Messages
{
    public interface IMessageService
    {
        Task<Result<ResponseMessageDto>> SendMessageAsync(string SenderId, RequestAddMessageDto request);
        Task<Result<ResponseRemoveMessageDto>> RemoveMessageAsync(string SenderId, RequestRemoveMessageDto request);
        Task<Result<ResponseUpdateMessageDto>> UpdateMessageAsync(string SenderId, RequestUpdateMessageDto request);
        Task<Result<int>> UpdateReadMessageAsync(string SenderId, RequestUpdateReadMessageDto request);
        Task<Result<List<ResponseGetMessageDto>>> GetMessagesByComplaintIdAsync(string userId, int complaintId);

    }
}

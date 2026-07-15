using MarketExpress.Application.Dto.CartItem.Request;
using MarketExpress.Application.Dto.CartItem.Response;
using MarketExpress.Application.Dto.Request;
using MarketExpress.Application.Dto.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Results;

namespace MarketExpress.Application.Services.CartItems
{
    public interface ICartItemService
    {
        Task<Result<ResponseCartItemDto>> AddAsync(string userId, RequestCartItemDto dto);
        Task<Result<ResponseUpdateQuantityItemInCartDto>> UpdateAsync(string userId,int cartItemId, UpdateCartItemDto dto);
        Task<Result> DeleteAsync(string userId, RequestDeleteItemsFromCartDto request);
        Task<Result<ResponseGetInfoCartDto>> GetItemsFromCartIdAsync(string userId);
       


    }
}

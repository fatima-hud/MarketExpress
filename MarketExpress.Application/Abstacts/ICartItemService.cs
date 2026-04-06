using MarketExpress.Application.Dto.Request;
using MarketExpress.Application.Dto.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Abstacts
{
    public interface ICartItemService
    {
        Task<Result<ResponseCartItemDto>> AddAsync(string userId,RequestCartItemDto dto);
        Task<Result<ResponseCartItemDto>> UpdateAsync(int cartId,int productId,UpdateCartItemDto dto);
        Task<Result>DeleteAsync(int CartId,List<int> productId);
        Task<Result<List<CartItem>>> GetItemsFromCartIdAsync(int cartId);


    }
}

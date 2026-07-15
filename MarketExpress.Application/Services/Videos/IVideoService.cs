using MarketExpress.Core.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Services.Videos
{
   public interface IVideoService
    {
        Task<Result<string>> UploadVideoAsync(IFormFile file);
        Task<Result> DeleteVideoAsync(string videoUrl);
    }
}

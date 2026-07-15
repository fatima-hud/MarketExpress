using MarketExpress.Core.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Services.Videos
{
   public class VideoService:IVideoService
    {
        public async Task<Result<string>> UploadVideoAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Result<string>.BadRequest("لم يتم اختيار فيديو");

            var allowedExtensions = new[] { ".mp4", ".mov", ".avi", ".webm", ".mkv" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                return Result<string>.BadRequest("صيغة الفيديو غير مدعومة");

            // حجم أقصى 100MB (يمكنك تغييره)
            const long maxSize = 100 * 1024 * 1024;

            if (file.Length > maxSize)
                return Result<string>.BadRequest("حجم الفيديو أكبر من الحد المسموح");

            var fileName = $"{Guid.NewGuid()}{extension}";

            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            var uploadFolder = Path.Combine(rootPath, "uploads", "videos");

            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            var filePath = Path.Combine(uploadFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var videoUrl = $"/uploads/videos/{fileName}";

            return Result<string>.Success(videoUrl);
        }
        public async Task<Result> DeleteVideoAsync(string videoUrl)
        {
            if (string.IsNullOrWhiteSpace(videoUrl))
                return Result.BadRequest("رابط الفيديو غير صالح");

            var videoPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                videoUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString())
            );

            if (!File.Exists(videoPath))
                return Result.NotFound("الفيديو غير موجود");

            File.Delete(videoPath);

            return Result.Success("تم حذف الفيديو");
        }
    }
}

using MarketExpress.Core.Results;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;

namespace MarketExpress.Application.Services.Images
{
    public class ImageService : IImageService
    {
        private readonly string _wwwroot;

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _wwwroot = webHostEnvironment.WebRootPath
                       ?? Path.Combine(webHostEnvironment.ContentRootPath, "wwwroot");

            if (!Directory.Exists(_wwwroot))
                Directory.CreateDirectory(_wwwroot);
        }
        public async Task<Result<string>> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Result<string>.BadRequest("لم يتم اختيار ملف");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                return Result<string>.BadRequest("صيغة الملف غير مسموحة");

            var fileName = Guid.NewGuid().ToString() + extension;

            // الوصول إلى مجلد wwwroot داخل المشروع
           

            // إنشاء مجلد uploads إذا غير موجود
            var uploadFolder = Path.Combine(_wwwroot, "uploads");
            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            // المسار الكامل لحفظ الصورة
            var filePath = Path.Combine(uploadFolder, fileName);

            // حفظ الملف داخل wwwroot/uploads
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
          


            var imageUrl = $"/uploads/{fileName}";


            return Result<string>.Success(imageUrl);
        }
        public async Task<Result> DeleteImageAsync(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                return Result.BadRequest("رابط الصورة غير صالح");

            var imagePath = Path.Combine(
                _wwwroot,
                imageUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString())
            );

            if (!File.Exists(imagePath))
                return Result.NotFound("الصورة غير موجودة");

            File.Delete(imagePath);

            return Result.Success("تم حذف الصورة");
        }
    }
}

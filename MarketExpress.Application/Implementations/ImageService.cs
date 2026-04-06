using Azure.Core;
using MarketExpress.Application.Abstacts;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Implementations
{
   public class ImageService:IImageService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        

        public ImageService(IHttpContextAccessor httpContextAccessor,UserManager<User>userManager)
        {
            _httpContextAccessor = httpContextAccessor;
          _userManager = userManager;
       
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
            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            // إنشاء مجلد uploads إذا غير موجود
            var uploadFolder = Path.Combine(rootPath, "uploads");
            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            // المسار الكامل لحفظ الصورة
            var filePath = Path.Combine(uploadFolder, fileName);

            // حفظ الملف داخل wwwroot/uploads
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // بناء الرابط
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                // إذا لا يوجد HttpContext → رجّع مسار نسبي فقط
                return Result<string>.Success($"/uploads/{fileName}");
            }

            var request = httpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            var imageUrl = $"{baseUrl}/uploads/{fileName}";
          

            return Result<string>.Success(imageUrl);
        }
    
}
}

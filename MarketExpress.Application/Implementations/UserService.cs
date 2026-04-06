using MarketExpress.Application.Abstacts;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MarketExpress.Application.Implementations
{
    public class UserService : IUserService
    {
        private readonly IImageService _imageService;
        private readonly UserManager<User> _userManager; 

        public UserService(IImageService imageService,UserManager<User>userManager)
        {
            _imageService = imageService;
            _userManager = userManager;
        }

        public async Task<bool> CheckFormPhoneNumberAsync(string phoneNumber)
        {


            string pattern = @"^09[1-9]{1}[0-9]{7}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(phoneNumber);

        }

        public async Task<Result<string>> UpdateProfilePhoto(string userId,IFormFile file)
        {
            var user=await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<string>.NotFound("Not found user");
            }
            if (user.IsBanned)
            {
                return Result<string>.BadRequest("User is banned");
            }
            var image = await _imageService.UploadImageAsync(file);
            if (!image.IsSuccess)
            {
                return image; 
            }
            user.ProfilePhoto = image.Value;
            await _userManager.UpdateAsync(user);
            return Result<string>.Success(image.Value);
          
        }
    }
}

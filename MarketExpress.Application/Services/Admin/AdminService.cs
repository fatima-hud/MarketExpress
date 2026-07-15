using MarketExpress.Application.Dto.Role.Request;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Services.Admin
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<User> _userManager;
        private readonly IRoleRepository _roleRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminService(UserManager<User> userManager, IRoleRepository roleRepository, IStoreRepository storeRepository, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleRepository = roleRepository;
            _storeRepository = storeRepository;
            _roleManager = roleManager;
        }

        public async Task<Result> AddRole(string adminId, RequestAddRoleDto request)
        {
            var admin = await _userManager.FindByIdAsync(adminId);
            if (admin == null)
            {
                return Result.NotFound("الادمن غير موجود");
            }
            var isAdmin = await _userManager.IsInRoleAsync(admin, "Admin");
            if (!isAdmin)
            {
                return Result.BadRequest("ليس لديك صلاحية اضافة دور");
            }

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return Result.NotFound("المستخدم غير موجود");
            }
            var role = await _roleManager.FindByNameAsync(request.RoleName);

            if (role == null)
            {
                return Result.NotFound("الدور غير موجود");
            }
            if (role.Name == "SocialMediaManager")
            {
                var store = await _storeRepository.GetStoreByAdminAsync(adminId);
                if (store == null)
                {
                    return Result.NotFound("المتجر غير موجود");
                }
                if (!string.IsNullOrEmpty(store.SocialMediaManagerUserId))
                {
                    return Result.BadRequest("يرجى حذف مسؤول السوشال ميديا اولا");

                }
                var isUserSocialMediaManager = await _userManager.IsInRoleAsync(user, "SocialMediaManager");
                if (isUserSocialMediaManager)
                {
                    return Result.BadRequest("مستخدم مسؤول السوشال ميديا لمتجر اخر");
                }
                store.SocialMediaManagerUserId = request.UserId;
            }
            await _userManager.AddToRoleAsync(user, request.RoleName);
            await _storeRepository.SaveChangesAsync();
            return Result.Success("تم اضافة دور بنجاح");

        }

        public async Task<Result> RemoveRole(string adminId, RequestAddRoleDto request)
        {
            var admin = await _userManager.FindByIdAsync(adminId);
            if (admin == null)
            {
                return Result.NotFound("الادمن غير موجود");
            }
            var isAdmin = await _userManager.IsInRoleAsync(admin, "Admin");
            if (!isAdmin)
            {
                return Result.BadRequest("ليس لديك صلاحية ازالة دور");
            }

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return Result.NotFound("المستخدم غير موجود");
            }
            var role = await _roleManager.FindByNameAsync(request.RoleName);

            if (role == null)
            {
                return Result.NotFound("الدور غير موجود");
            }
            if (role.Name == "SocialMediaManager")
            {
                var store = await _storeRepository.GetStoreByAdminAsync(adminId);
                if (store == null)
                {
                    return Result.NotFound("المتجر غير موجود");
                }

                store.SocialMediaManagerUserId = null;
                await _storeRepository.SaveChangesAsync();


            }
            await _userManager.RemoveFromRoleAsync(user, role.Name);
            return Result.Success("تم حذف الدور بنجاح");
        }
    }
}
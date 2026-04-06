using MarketExpress.Application.Abstacts;
using MarketExpress.Application.Dto;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using MarketExpress.Infrastructure.Abstracts;
using MarketExpress.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Implementations
{
    public class SAdminService : ISAdminService
    {
        private readonly UserManager<User> _userManager;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly RoleManager<IdentityRole> _roleManager;


        public SAdminService(UserManager<User> userManager,IRefreshTokenRepository refreshTokenRepository, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _refreshTokenRepository = refreshTokenRepository;
            _roleManager = roleManager;
        }

        public async Task<Result<bool>> RevokeTokenAsync(string userId)
        {
           var trans= await _refreshTokenRepository.BeginTransactionAsync();
            try
            {
                
                var user = await _userManager.FindByIdAsync(userId);
                if (user is null)
                {
                    return Result<bool>.NotFound("User not found");
                }
                if (user.IsBanned)
                {
                    return Result<bool>.BadRequest("User is banned");
                }
                 user.IsBanned = true;
                await _refreshTokenRepository.RevokeAllRefreshTokensAsync(userId);


               
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return Result<bool>.Failure("Failed to ban user");
                }
                await trans.CommitAsync();
                return Result<bool>.Success(true, "User banned and all refresh tokens revoked");
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return Result<bool>.Failure("An error occurred while banning the user");

            }
        }
        public async Task<Result> AddRoleAsync(string SadminId, AddRoleDto dto)
        {
            var admin = await _userManager.FindByIdAsync(SadminId);
            if (admin is null || !await _roleManager.RoleExistsAsync("SuperAdmin"))
            {
                return Result.NotFound("not found user or role not superadmin");
            }
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user is null || !await _roleManager.RoleExistsAsync(dto.Role))
            {
                return Result.NotFound("not found user or role");
            }
            if (await _userManager.IsInRoleAsync(user, dto.Role))
            {
                return Result.BadRequest("User Assigned Already");
            }
            var res = await _userManager.AddToRoleAsync(user, dto.Role);
            if (!res.Succeeded)
            {
                return Result.BadRequest("Something was error");
            }
            return Result.Success();


        }


    }
}

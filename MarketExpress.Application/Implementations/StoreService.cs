using MarketExpress.Application.Abstacts;
using MarketExpress.Application.Dto.Request;
using MarketExpress.Application.Dto.RequestStore.Request;
using MarketExpress.Application.Dto.Store.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Implementations
{
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IImageService _imageService;
        private readonly UserManager<User> _userManager;

        private readonly IRoleRepository _roleRepository;
     


        public StoreService(IStoreRepository repository, IImageService imageService, UserManager<User> userManager, IRoleRepository roleRepository)
        {
            _storeRepository = repository;
            _imageService = imageService;
            _userManager = userManager;
            _roleRepository = roleRepository;
       
        }

        
    

        
             

        
           public async Task<Result> SoftDeleteAsync(int storeId)
           {
               var trans = await _storeRepository.BeginTransactionAsync();
               try
               {
                   var store = await _storeRepository.GetByIdAsync(storeId);
                   if (store == null)
                   {
                    await trans.RollbackAsync();
                    return Result.NotFound("Not found store ");
                   }
                if (store.IsDeleted)
                {
                    await trans.RollbackAsync();
                    return Result.BadRequest("Store already deleted");
                }


                var user = await _userManager.FindByIdAsync(store.OwnerId);


                if (user != null && await _userManager.IsInRoleAsync(user, "Admin"))
                {
                   var  roleResult= await _userManager.RemoveFromRoleAsync(user, "Admin");
                    if (!roleResult.Succeeded)
                    {
                        await trans.RollbackAsync();
                        return Result.BadRequest("Failed to remove admin role");
                    }
                }
                store.IsDeleted = true;
                store.DeletedAt = DateTime.UtcNow;
                store.IsActive = false;


                await _storeRepository.UpdateAsync(store);



                await trans.CommitAsync();
                return Result.Success("Store deleted successfully");
            }
               catch (Exception ex)
               {
                   await trans.RollbackAsync();
                return Result.Failure($"Something went wrong: {ex.Message}");

            }

           }

      
           public async Task<Result<ResponseOwnerStoreDto>> GetStoreByAdminAsync(string ownerId)
           {
               var user=await _userManager.FindByIdAsync(ownerId);
               if (user == null)
               {
                   return Result<ResponseOwnerStoreDto>.NotFound("User not found");
               }
            if (!await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return Result<ResponseOwnerStoreDto>.BadRequest("User is not an admin");
            }
           
               var store=await _storeRepository.GetStoreByAdminAsync(ownerId);
            if(store is null)
            {
                return Result<ResponseOwnerStoreDto>.NotFound("Store not found");
            }
            
                
                   var ret = new ResponseOwnerStoreDto
                   {
                       Id = store.Id,
                       StoreName = store.StoreName,
                       StoreEmail = user.Email,
                       StorePhoneNumber = user.PhoneNumber,
                       Address = store.Address,
                       Description = store.Description,
                       IsActive = store.IsActive,
                 
                       Latitude = store.Latitude,
                       Longitude = store.Longitude,
                       Logo = store.Logo,
                       FeaturedImage=store.FeaturedImage,
                       WorkingHoursEnd = store.WorkingHoursEnd,
                       WorkingHoursStart = store.WorkingHoursStart, 
                       StoreStatus = store.StoreStatus,
                       CreatedAt = store.CreatedAt,
                       ApprovedAt = store.ApprovedAt,
                       RejectedAt = store.RejectedAt, 
                       IsDeleted = store.IsDeleted,
                     
                       DeletedAt = store.DeletedAt,
                       
                      
                     
                       Note = store.Note,




                   };



            return Result<ResponseOwnerStoreDto>.Success(ret);

           }

        public async  Task<Result<List<ResponseStoreDto>>> GetAllStoresAsync()
        {
            var stores = await _storeRepository.GetAllActiveStoreAsync();
            if (stores is null || !stores.Any())
            {
                return Result<List<ResponseStoreDto>>.NotFound("Not found any store");
            }
            var ret = stores.Select(e => new ResponseStoreDto
            {
                Id = e.Id,
                StoreName = e.StoreName,
                StorePhoneNumber = e.Owner.PhoneNumber,
                Address = e.Address,
                Description = e.Description,
                StoreEmail = e.Owner.Email,
                Logo = e.Logo,
                FeaturedImage=e.FeaturedImage,
                WorkingHoursStart=e.WorkingHoursStart,
                WorkingHoursEnd=e.WorkingHoursEnd,

             
             
              
                Latitude = e.Latitude,
              
                Longitude = e.Longitude,
                IsActive = e.IsActive
            }).ToList();

            return Result<List<ResponseStoreDto>>.Success(ret); 

        }

     
    }
}

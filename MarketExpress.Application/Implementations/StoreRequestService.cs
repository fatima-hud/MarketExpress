using MarketExpress.Application.Abstacts;
using MarketExpress.Application.Abstracts;
using MarketExpress.Application.Dto.Authontication.Request;
using MarketExpress.Application.Dto.Product.Response;
using MarketExpress.Application.Dto.RequestStore.Request;
using MarketExpress.Application.Dto.RequestStore.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Enums;
using MarketExpress.Core.Helping;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace MarketExpress.Application.Implementations
{
    public class StoreRequestService : IStoreRequestService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IStoreRepository _storeRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IImageService _imageService;

        public StoreRequestService(IUserRepository userRepository, UserManager<User> userManager, IUserService userService, IEmailService emailService, IStoreRepository storeRepository, IImageService imageService,IDocumentRepository documentRepository)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _userService = userService;
            _emailService = emailService;
            _storeRepository = storeRepository;
          
            _imageService = imageService;
            _documentRepository = documentRepository;
        }

        public async Task<Result<ResponseRequestStoreDto>> AddAsync(string userId, RequestStoreDto request)
        {
            using var trans=await _storeRepository.BeginTransactionAsync();
            try
            {
                if (string.IsNullOrWhiteSpace(request.StoreName))
                    return Result<ResponseRequestStoreDto>.BadRequest("Store name is required");

                if (string.IsNullOrWhiteSpace(request.Description))
                    return Result<ResponseRequestStoreDto>.BadRequest("Description is required");

                if (string.IsNullOrWhiteSpace(request.Address))
                    return Result<ResponseRequestStoreDto>.BadRequest("Address is required");

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return Result<ResponseRequestStoreDto>.NotFound("Not found user");
                }
                if (user.IsBanned)
                {
                    return Result<ResponseRequestStoreDto>.BadRequest("This user is banned");

                }

                if (!user.EmailConfirmed)
                {
                    return Result<ResponseRequestStoreDto>.BadRequest("This email is not confirm");
                }

                var isUserHasStore = await _storeRepository.IsUserExistByIdAndStatueApprovedAsync(userId);
                if (isUserHasStore)
                {
                    return Result<ResponseRequestStoreDto>.BadRequest("This user is already has store");
                }
                var isUserHasPending = await _storeRepository.IsUserHasPendingStoreAsync(userId);
                if (isUserHasPending)
                {
                    return Result<ResponseRequestStoreDto>.BadRequest("This user is already has request");
                }



                if (user.BirthDate < BirthDateSettings.minDate || user.BirthDate > BirthDateSettings.maxDate)
                {

                    return Result<ResponseRequestStoreDto>.BadRequest("Birth date is not valid");
                }
                var age = DateTime.Today.Year - user.BirthDate.Year;
                if (user.BirthDate.Date > DateTime.Today.AddYears(-age)) age--;


                if (age < 18)
                {
                    return Result<ResponseRequestStoreDto>.BadRequest("You must be at least 18 years old");
                }
                var StoreName = await _storeRepository.IsNameFoundAsync(request.StoreName);

                if (StoreName)
                {
                    return Result<ResponseRequestStoreDto>.BadRequest("This store name is already exist");
                }
               
                if (request.Latitude < -90 || request.Latitude > 90)
                    return Result<ResponseRequestStoreDto>.BadRequest("Invalid latitude");

                if (request.Longitude < -180 || request.Longitude > 180)
                    return Result<ResponseRequestStoreDto>.BadRequest("Invalid longitude");
                if (request.WorkingHoursEnd <= request.WorkingHoursStart)
                {
                    return Result<ResponseRequestStoreDto>.BadRequest("End time must be greater than start time");
                }

                var duration = request.WorkingHoursEnd - request.WorkingHoursStart;

                if (duration.TotalHours > 24)
                {
                    return Result<ResponseRequestStoreDto>.BadRequest("Invalid working hours range");
                }
                if (request.WorkingHoursStart >= TimeSpan.FromHours(24) || request.WorkingHoursEnd >=TimeSpan.FromHours(24))

                {
                    return Result<ResponseRequestStoreDto>.BadRequest("Invalid working hours");
                }
                if(request.WorkingHoursStart<TimeSpan.Zero || request.WorkingHoursEnd< TimeSpan.Zero)
                {
                    return Result<ResponseRequestStoreDto>.BadRequest("Invalid working hours");
                }
                var FeaturedImage = await _imageService.UploadImageAsync(request.FeaturedImage);
                if (FeaturedImage is null)
                {
                    return Result<ResponseRequestStoreDto>.BadRequest("Failed to upload featured image");
                }
                var Logo = await _imageService.UploadImageAsync(request.Logo);
                if (Logo is null)
                {
                    return Result<ResponseRequestStoreDto>.BadRequest("Failed to upload Logo image");
                }

                var NationalIdFrontImage = await _imageService.UploadImageAsync(request.NationalIdFrontImage);
                if (NationalIdFrontImage is null)
                {
                    return Result<ResponseRequestStoreDto>.BadRequest("Failed to upload National ID front image");
                }
                var NationalIdBackImage = await _imageService.UploadImageAsync(request.NationalIdBackImage);
                if (NationalIdBackImage is null)
                {
                    return Result<ResponseRequestStoreDto>.BadRequest("Failed to upload National ID back image");
                }



                var documentNationalIdFront = new UserDocument
                {
                    UserId = user.Id,
                    FilePath = NationalIdFrontImage.Value,
                    Type = enDocumentType.NationalIdFrontImage
                };
                var documentNationalIdBack = new UserDocument
                {
                    UserId = user.Id,
                    FilePath = NationalIdBackImage.Value,
                    Type = enDocumentType.NationalIdBackImage
                };




                var store = new Store
                {

                  
                 OwnerId=user.Id,

                    StoreName = request.StoreName.Trim(),

                    Description = request.Description.Trim(),

                    Address = request.Address.Trim(),


                    FeaturedImage = FeaturedImage.Value,


                    Logo = Logo.Value,


                    Latitude = request.Latitude,

                    Longitude = request.Longitude,

                    WorkingHoursStart = request.WorkingHoursStart,

                    WorkingHoursEnd = request.WorkingHoursEnd,
                    StoreStatus=enStoreStatus.Pending
                    
                    
                  
                };




               await _documentRepository.AddAsync(documentNationalIdFront);
               await _documentRepository.AddAsync(documentNationalIdBack);

                await _storeRepository.AddAsync(store);
              
                var ret = new ResponseRequestStoreDto
                {
                    StoreName = request.StoreName,
                    status = enStoreStatus.Pending,
                    CreatedAt = store.CreatedAt,
                    RequestId = store.Id
                };
                await trans.CommitAsync();
                return Result<ResponseRequestStoreDto>.Success(ret);
            }
            catch (Exception ex)
            {
               
                
            await trans.RollbackAsync();
                return Result<ResponseRequestStoreDto>.Failure($"Something was wrong {ex.Message}");
                
            }
          

        }

        public async Task<Result<ResponseApprovedStoreRequestDto>> ApprovedAsync(int requestId)
        {
          using  var trans = await _storeRepository.BeginTransactionAsync();
            try
            {
                var request = await _storeRepository.GetByIdAsync(requestId);
                if (request == null)
                {
                    return Result<ResponseApprovedStoreRequestDto>.NotFound("Not found request");
                }
                if (request.StoreStatus == enStoreStatus.Approved)
                {

                    return Result<ResponseApprovedStoreRequestDto>.BadRequest("This store already approved ");
                }
                if (request.StoreStatus == enStoreStatus.Rejected)
                {

                    return Result<ResponseApprovedStoreRequestDto>.BadRequest("This store already rejected ");
                }
                var user = await _userManager.FindByIdAsync(request.OwnerId);
                if (user == null)
                    return Result<ResponseApprovedStoreRequestDto>.NotFound("Owner user not found");
                await _userManager.AddToRoleAsync(user, "Admin");

                var ApprovedTime = DateTime.UtcNow;
                request.ApprovedAt = ApprovedTime;
                request.StoreStatus = enStoreStatus.Approved;
                request.IsActive = true;
                await _storeRepository.UpdateAsync(request);
                var ret = new ResponseApprovedStoreRequestDto
                {
                    StoreName = request.StoreName,
                    ApprovedAt = ApprovedTime,
                    RequestId = requestId,
                    status = enStoreStatus.Approved


                };
                await trans.CommitAsync();
                return Result<ResponseApprovedStoreRequestDto>.Success(ret, " Your request has been approved");
            }
            catch (Exception ex)
            {
               await trans.RollbackAsync();
                return Result<ResponseApprovedStoreRequestDto>.Failure($"Something was wrong {ex.Message}");
            }
        }

     /*   public async Task<Result<List<ResponsePendingStoreDto>> GetAllPendingStore()
        {
            var stores = await _storeRepository.GetAllPendingStoreAsync();
            if(stores is null)
            {
                return Result<List<ResponsePendingStoreDto>>.NotFound("Not found any pending store");
            }
            var ret= new List<ResponsePendingStoreDto>();

            ret = stores.Select(async e => new ResponsePendingStoreDto
            {
                Id = e.Id,
                OwnerId = e.OwnerId,
                StoreName = e.StoreName,
                StoreStatus = e.StoreStatus,
                Address = e.Address,
                WorkingHoursStart = e.WorkingHoursStart,
                WorkingHoursEnd = e.WorkingHoursEnd,
                CreatedAt = e.CreatedAt,

                IsActive = e.IsActive,
                Description = e.Description,
                Latitude = e.Latitude,
                Longitude = e.Longitude,
                Logo = e.Logo,
                FeaturedImage = e.FeaturedImage,
                NationalIdFrontImage = await _userRepository.GetNationalIdFrontImageByUser(OwnerId),
                NationalIdBackImage=await _userRepository.GetNationalIdBackImageByUser(OwnerId)


            }).ToList();
           
        }*/

        public async Task<Result<ResponseRejectedStoreRequestDto>> RejectedAsync(RequestRejectdDto dto)
        {
            var request = await _storeRepository.GetByIdAsync(dto.RequestId);
            if (request == null)
            {
                return Result<ResponseRejectedStoreRequestDto>.NotFound("Not found request");
            }
            if (request.StoreStatus == enStoreStatus.Approved)
            {

                return Result<ResponseRejectedStoreRequestDto>.BadRequest("This store already approved ");
            }
            if (request.StoreStatus == enStoreStatus.Rejected)
            {

                return Result<ResponseRejectedStoreRequestDto>.BadRequest("This store  already rejected ");
            }

            var rejectedTime = DateTime.UtcNow;
            request.RejectedAt = rejectedTime;
            request.StoreStatus = enStoreStatus.Rejected;
            request.Note = dto.RejectionReason;
            await _storeRepository.UpdateAsync(request);
            var ret = new ResponseRejectedStoreRequestDto
            {
                StoreName = request.StoreName,
                RejectedAt =rejectedTime ,
                RequestId = request.Id,
                status = enStoreStatus.Rejected,
                Note=dto.RejectionReason



            };
            return Result<ResponseRejectedStoreRequestDto>.Success(ret, " Your request has been rejected");
        }
    } 

        
    
}

using MarketExpress.Application.Dto.Complaint.Request;
using MarketExpress.Application.Dto.Complaint.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Enums;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Identity;

namespace MarketExpress.Application.Services.Complaints
{
    public class ComplaintService : IComplaintService
    {
        private readonly UserManager<User> _userManager;
        private readonly IStoreRepository _storeRepository;
        private readonly IComplaintRepository _complaintRepository;

        public ComplaintService(UserManager<User> userManager, IStoreRepository storeRepository, IComplaintRepository complaintRepository)
        {
            _userManager = userManager;
            _storeRepository = storeRepository;
            _complaintRepository = complaintRepository;
        }

        public async Task<Result<ResponseAddComplaintDto>> AddAsync(string userId, RequestAddComplaintDto request)
        {
            var customer = await _userManager.FindByIdAsync(userId);
            if (customer == null)
                return Result<ResponseAddComplaintDto>.NotFound("مستخدم غير موجود");
            if (customer.IsBanned)
            {
                return Result<ResponseAddComplaintDto>.BadRequest("مستخدم محظور");
            }
            var store = await _storeRepository.GetByIdAsync(request.StoreId);
            if (store == null)
                return Result<ResponseAddComplaintDto>.NotFound("متجر غير موجود");

            var complaint = await _complaintRepository.GetComplaintAsync(userId, store.OwnerId);
            if (complaint is null)
            {
                complaint = new Complaint
                {
                    CustomerId = userId,
                    StoreOwnerId = store.OwnerId,
                    Description = request.Description,
                    Title = request.Title,
                    CreatedAt = DateTime.UtcNow

                };

                await _complaintRepository.AddAsync(complaint);
            }

            var response = new ResponseAddComplaintDto
            {
                Id = complaint.Id,
                CustomerId = complaint.CustomerId,
                Title = complaint.Title,
                StoreOwnerId = complaint.StoreOwnerId,
                StoreName = store.StoreName,
                StoreLogoUrl = store.Logo,
                CustomerFullName = customer.FullName,
                CustomerImageUrl = customer.ProfilePhoto,
                Description = complaint.Description,
                CreatedAt = complaint.CreatedAt,
               
            };
            return Result<ResponseAddComplaintDto>.Success(response);


        }

        public async Task<Result<List<ResponseGetComplaintDto>>> GetAllAsync(string storeOwnerId)
        {
            var owner = await _userManager.FindByIdAsync(storeOwnerId);
            if (owner == null)
            {
                return Result<List<ResponseGetComplaintDto>>.NotFound("صاحب متجر غير موجود");

            }
            if (owner.IsBanned)
            {
                return Result<List<ResponseGetComplaintDto>>.BadRequest("مستخدم محظور");
            }
            var storeOwner = await _storeRepository.GetStoreByAdminAsync(storeOwnerId);
            if (storeOwner == null)
            {
                return Result<List<ResponseGetComplaintDto>>.NotFound("المستخدم ليس لديه متجر بعد");
            }
            var complaints = await _complaintRepository.GetAllComplaintByUserIdAsync(storeOwnerId);
            var response = complaints.Select(c => new ResponseGetComplaintDto
            {
                ComplaintId = c.Id,
                CustomerId = c.CustomerId,
                Title = c.Title,
                CustomerFullName = owner.FullName,
                CustomerImageUrl = owner.ProfilePhoto,
                StoreName = storeOwner.StoreName,
                StoreLogoUrl = storeOwner.Logo,
                Description = c.Description,
                CreatedAt = c.CreatedAt,
             
              
            }).ToList();
            return Result<List<ResponseGetComplaintDto>>.Success(response);

        }
    }
}

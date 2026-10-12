using MarketExpress.Application.Dto.Complaint.Request;
using MarketExpress.Application.Dto.Complaint.Response;
using MarketExpress.Core.DtoHelping.Complaint;
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
            if (customer == null || customer.IsDeleted)
                return Result<ResponseAddComplaintDto>.NotFound("مستخدم غير موجود");
            if (customer.IsBanned)
            {
                return Result<ResponseAddComplaintDto>.BadRequest("مستخدم محظور");
            }
            if(await _userManager.IsInRoleAsync(customer, "Admin"))
            {
                return Result<ResponseAddComplaintDto>.BadRequest("لا يمكن لصاحب المتجر ارسال شكوى");
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

        public async Task<Result<List<ResponseGetComplaintByStoreDto>>> GetAllByStoreAsync(string storeOwnerId)
        {
            var owner = await _userManager.FindByIdAsync(storeOwnerId);
            if (owner == null || owner.IsDeleted)
            {
                return Result<List<ResponseGetComplaintByStoreDto>>.NotFound("صاحب متجر غير موجود");

          
            }
            if (owner.IsBanned)
            {
                return Result<List<ResponseGetComplaintByStoreDto>>.BadRequest("مستخدم محظور");
            }
            var storeOwner = await _storeRepository.GetStoreByAdminAsync(storeOwnerId);
            if (storeOwner == null)
            {
                return Result<List<ResponseGetComplaintByStoreDto>>.NotFound("المستخدم ليس لديه متجر بعد");
            }
            var complaints = await _complaintRepository.GetAllComplaintByAdminAsync(storeOwnerId);
            var response = complaints.Select(c => new ResponseGetComplaintByStoreDto
            {
                ComplaintId = c.Id,
                CustomerId = c.CustomerId,
                Title = c.Title,
                CustomerFullName = c.Customer.FullName,
                CustomerImageUrl = c.Customer.ProfilePhoto,
              
                Description = c.Description,
                CreatedAt = c.CreatedAt,
                NumberOfUnReadMessage = c.Messages.Count(m => !m.IsRead && m.SenderId == c.CustomerId),


            }).ToList();
            return Result<List<ResponseGetComplaintByStoreDto>>.Success(response);

        }

        public async Task<Result<List<ResponseGetComplaintByUserDto>>> GetAllByUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.IsDeleted)
            {
                return Result<List<ResponseGetComplaintByUserDto>>.NotFound("صاحب متجر غير موجود");


            }
            if (user.IsBanned)
            {
                return Result<List<ResponseGetComplaintByUserDto>>.BadRequest("مستخدم محظور");
            }
       
            var complaints = await _complaintRepository.GetAllComplaintByUserAsync(userId);
           
            return Result<List<ResponseGetComplaintByUserDto>>.Success(complaints);
        }

      
    }
}

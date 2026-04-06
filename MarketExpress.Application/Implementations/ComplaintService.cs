using MarketExpress.Application.Abstacts;
using MarketExpress.Application.Dto.Complaint.Request;
using MarketExpress.Application.Dto.Complaint.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Implementations
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

        public async Task<Result<ResponseAddComplaintDto>> AddAsync(string CustomerId,RequestAddComplaintDto request)
        {
            var user = await _userManager.FindByIdAsync(CustomerId);
            if (user == null)
                return Result<ResponseAddComplaintDto>.NotFound("User not found");
            var store = await _storeRepository.GetByIdAsync(request.StoreId);
            if (store == null)
                return Result<ResponseAddComplaintDto>.NotFound("Store not found");
            var complaint = new Complaint
            {
                CustomerId = CustomerId,
                StoreOwnerId = store.OwnerId,
                Description = request.Description,
                Title = request.Title,
                CreatedAt = DateTime.UtcNow
            };
            await _complaintRepository.AddAsync(complaint);
           
            var response = new ResponseAddComplaintDto
            {
                Id = complaint.Id,
                CustomerId = complaint.CustomerId,
                Title = complaint.Title,
                StoreOwnerId = complaint.StoreOwnerId,
                StoreName = store.StoreName,
                StoreLogoUrl = store.Logo,
                CustomerFullName = user.FullName,
                CustomerImageUrl = user.ProfilePhoto,
                Description = complaint.Description,
                CreatedAt = complaint.CreatedAt
            };
            return Result<ResponseAddComplaintDto>.Success(response);


        }

        public async Task<Result<List<ResponseGetComplaintDto>>> GetAllAsync(string StoreOwnerId)
        {
            var owner = await _userManager.FindByIdAsync(StoreOwnerId);
            if (owner==null)
            {
                return Result<List<ResponseGetComplaintDto>>.NotFound("Store owner not found");

            }
            var storeOwner= await _storeRepository.GetStoreByAdminAsync(StoreOwnerId);
            if (storeOwner== null)
            {
                return Result<List<ResponseGetComplaintDto>>.NotFound("You don't have a store yet");
            }
            var complaints = await _complaintRepository.GetAllComplaintByUserIdAsync(StoreOwnerId);
            var response = complaints.Select(c => new ResponseGetComplaintDto
            {
                Id = c.Id,
                CustomerId = c.CustomerId,
                Title = c.Title,
                CustomerFullName = owner.FullName,
                CustomerImageUrl = owner.ProfilePhoto,
                StoreName = storeOwner.StoreName,
                StoreLogoUrl = storeOwner.Logo,
                Description = c.Description,
                CreatedAt = c.CreatedAt
            }).ToList();
            return Result<List<ResponseGetComplaintDto>>.Success(response);

        }
    }
}

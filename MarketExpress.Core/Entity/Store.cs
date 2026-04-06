using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.Entity
{
   public class Store
    {
        public int Id { get; set; }
       public string OwnerId {  get; set; }
        public string StoreName { get; set; }
        public string Description { get; set; }
      
        public string Address { get; set; }
     
        public string? Logo { get; set; }
        public string? FeaturedImage { get; set; }
      
        public bool IsActive {  get; set; }
        public decimal Latitude {  get; set; }
        public decimal Longitude { get; set; }
        public TimeSpan WorkingHoursStart { get; set; }

        public TimeSpan WorkingHoursEnd { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
       

        public DateTime? RejectedAt { get; set; }
        public string? Note { get; set; }
        public enStoreStatus StoreStatus { get; set; }
        public bool  IsDeleted {  get; set; }
        public DateTime? DeletedAt { get; set; }
        public User Owner{ get;set; }
        public ICollection<User> Employees { get; set; }
   
        public ICollection<Product>Products { get; set; }
        public ICollection<Order> Orders { get; set; }
       
       
     
        




    }
}

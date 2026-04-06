using MarketExpress.Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace MarketExpress.Core.Entity.Identity
{
    public class User:IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string ProfilePhoto { get; set; } = null!;
        public bool IsBanned {  get; set; }
        public DateTime BirthDate { get; set; }
        public enGender Gender { get; set; }
        public ICollection<OtpCode>OtpCodes { get; set; } = new List<OtpCode>();
        public ICollection<Order>?orders { get; set; }
        public List<UserRefreshToken>?RefreshTokens { get; set; }
        public Cart Cart { get;set; }
        public int? StoreId {  get; set; }
        public Store? Store { get; set; }
        public string? ResetPasswordCode { get; set; }
        public DateTime? ResetPasswordCodeExpiry { get; set; }
      
        public string FullName=>FirstName+" "+ LastName;
     
        public ICollection<UserDocument>? Documents { get; set; }
        public ICollection<Complaint>? ComplaintsAsCustomer { get; set; }
        public ICollection<Complaint>? ComplaintsAsStoreOwner { get; set; }
        public ICollection<Message>? Messages { get; set; }
        public ICollection<Notification>? AddNotifications { get; set; }
        public ICollection<UserNotification>? ReceivedNotifications { get; set; }
    }
}

using MarketExpress.Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace MarketExpress.Core.Entity.Identity
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string ProfilePhoto { get; set; } = null!;
        public bool IsBanned { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime BirthDate { get; set; }
        public enGender Gender { get; set; }
        public ICollection<OtpCode> OtpCodes { get; set; } = new List<OtpCode>();
        public ICollection<Order> orders { get; set; } = new List<Order>();
        public List<UserRefreshToken>? RefreshTokens { get; set; }
        public Cart Cart { get; set; }
       
        
        public Wallet Wallet { get; set; }
        public string? ResetPasswordCode { get; set; }
        public DateTime? ResetPasswordCodeExpiry { get; set; }

        public string FullName => FirstName + " " + LastName;

      
        public ICollection<Complaint> ComplaintsAsCustomer { get; set; } = new List<Complaint>();
        public ICollection<Complaint> ComplaintsAsStoreOwner { get; set; } = new List<Complaint>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
     //   public ICollection<Notification> AddNotifications { get; set; } = new List<Notification>();
     //   public ICollection<UserNotification> ReceivedNotifications { get; set; } = new List<UserNotification>();
      
        public ICollection<Store> Stores { get; set; }=new List<Store>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();   
        public ICollection<PostReaction> PostReactions { get; set; } = new List<PostReaction>();
        public ICollection<StoreFollower> StoreFollowers { get; set; }  =new List<StoreFollower>();
        public ICollection<Rating> Ratings {  get; set; }= new List<Rating>();
      
    }
    
}

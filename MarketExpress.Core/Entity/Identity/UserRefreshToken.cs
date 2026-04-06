using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Bcpg;

namespace MarketExpress.Core.Entity.Identity
{
    
    public class UserRefreshToken
    {
      public int Id { get; set; }
        public string Token { get; set; }
        public string RefreshToken {  get; set; }
        public DateTime ExpiresOn { get; set; }
      
      
        public DateTime CreatesOn { get; set; }
        public DateTime? RevokeOn { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresOn;
        public bool IsRevoked { get; set; }

        
       public void Revoke()
        {
            IsRevoked=true;
          

        }
        public bool IsActive()
        {
            return !IsRevoked && !IsExpired;
        }


    }
}

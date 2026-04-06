using MarketExpress.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace MarketExpress.Database
{
    public class User:IdentityUser
    {
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public byte[] ProfilePhoto { get; set; }
        public ICollection<OtpCode>OtpCodes { get; set; } = new List<OtpCode>();
    }
}

using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.User
{
   public class ResponseUserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePhoto {  get; set; }
        public DateTime BirthDate {  get; set; }
        public enGender Gender { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
      
        public string PhoneNumber { get; set; }
    }
}

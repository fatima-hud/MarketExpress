using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Register.Request
{
   public class AddUserRequest
    {
        [Required]
        public string FirstName {  get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string UserName { get; set; }
      
        [Required]
        [EmailAddress]
        public string Email { get; set; }
      
        [Required]
        public string PhoneNumber {  get; set; }

        [Required]
        public enGender Gender { get; set; } 
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public string Password { get; set; }
        

    }
}

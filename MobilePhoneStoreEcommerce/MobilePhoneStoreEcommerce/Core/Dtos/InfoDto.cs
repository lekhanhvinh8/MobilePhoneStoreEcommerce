using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreEcommerce.Core.Dtos
{
    public class InfoDto
    {
        [Required(ErrorMessage = "Please enter your name")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Please enter name with 3 to 50 characters")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Please enter your mail")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Your mail is not valid")]
        [StringLength(50, MinimumLength = 6)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your phone number")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Your phone number should be between 6 and 20 numbers")]
        [RegularExpression(@"[0-9]{6,20}", ErrorMessage = "Your phone number is not valid")]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "Please enter your address")]
        public string Address { get; set; }

        public int CustomerId { get; set; }
    }
}
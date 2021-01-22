using MobilePhoneStoreDBMS.Models.CustomValidations;
using MobilePhoneStoreEcommerce.Persistence.CustomValidations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobilePhoneStoreEcommerce.Core.Dtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Please enter your name")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Please enter name with 3 to 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter your phone number")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Your phone number should be between 6 and 20 numbers")]
        [RegularExpression(@"[0-9]{6,20}", ErrorMessage = "Your phone number is not valid")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter your mail")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Your mail is not valid")]
        [StringLength(50, MinimumLength = 6)]
        [UniqueEmail]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your user name")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Please enter user name with 6 to 20 characters")]
        [ValidUserNameRegister]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please choose your account type")]
        public int AccountType { get; set; }

        [Required(ErrorMessage = "Please enter your address")]
        public string Address { get; set; }
    }
}
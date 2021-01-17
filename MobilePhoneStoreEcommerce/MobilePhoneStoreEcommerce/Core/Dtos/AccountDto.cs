using System.ComponentModel.DataAnnotations;

namespace MobilePhoneStoreEcommerce.Core.Dtos
{
    public class AccountDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
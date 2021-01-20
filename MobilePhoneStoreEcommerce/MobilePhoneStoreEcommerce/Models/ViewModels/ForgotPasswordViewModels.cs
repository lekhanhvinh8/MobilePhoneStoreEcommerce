using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreEcommerce.Models.ViewModels
{
    public class ForgotPasswordViewModels
    {
        [Required(ErrorMessage = "Please choose your account type")]
        public int AccountType { get; set; }

        [Required(ErrorMessage = "Please enter your email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Your email is not valid")]
        [StringLength(50, MinimumLength = 6)]
        public string Email { get; set; }
    }
}
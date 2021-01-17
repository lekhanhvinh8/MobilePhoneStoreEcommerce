using MobilePhoneStoreDBMS.Models.CustomValidations;
using MobilePhoneStoreEcommerce.Core.Dtos;
using MobilePhoneStoreEcommerce.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreEcommerce.Core.ViewModels
{
    public class LoginViewModel
    {
        public AccountDto AccountDto { get; set; }

        [ValidRoleLogin]
        public int RoleID { get; set; }
    }
}
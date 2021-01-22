using MobilePhoneStoreEcommerce.Core;
using MobilePhoneStoreEcommerce.Core.ViewModels;
using MobilePhoneStoreEcommerce.Models.ControllerModels;
using MobilePhoneStoreEcommerce.Persistence;
using MobilePhoneStoreEcommerce.Persistence.Consts;
using System;
using System.ComponentModel.DataAnnotations;

namespace MobilePhoneStoreDBMS.Models.CustomValidations
{
    public class ValidRoleLogin : ValidationAttribute
    {
        private IUnitOfWork _unitOfWork;
        public ValidRoleLogin()
        {
            this._unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var loginViewModel = (LoginViewModel)validationContext.ObjectInstance;

            var accountDto = loginViewModel.AccountDto;

            if (String.IsNullOrEmpty(accountDto.Username) || String.IsNullOrEmpty(accountDto.Password))
                return ValidationResult.Success; // If the user has not typed information yet, pass the logic below

            var result = Login(accountDto.Username, accountDto.Password);

            if (result == 0) // Invalid user name or password
                return new ValidationResult("Your account or your password is incorrect");

            var accountInDb = this._unitOfWork.Accounts.SingleOrDefault(a => a.UserName == accountDto.Username);

            if (accountInDb == null)
                throw new Exception("Not found");

            var roleID = accountInDb.Role.ID;

            if (roleID != loginViewModel.RoleID && loginViewModel.RoleID != RoleIds.Unknown)
                return new ValidationResult("You are not logged in with the correct role");

            return ValidationResult.Success;
        }
        private int Login(string username, string password)
        {
            string pwd = AccountModels.Encrypt(password, true);
            var account = _unitOfWork.Accounts.SingleOrDefault(a => a.UserName == username && a.PasswordHash == pwd);
            var res = 0;
            if (account != null)
            {
                res = account.RoleID;
            }

            return res;
        }
    }
}
using MobilePhoneStoreEcommerce.Core;
using MobilePhoneStoreEcommerce.Core.Dtos;
using MobilePhoneStoreEcommerce.Persistence;
using System;
using System.ComponentModel.DataAnnotations;

namespace MobilePhoneStoreDBMS.Models.CustomValidations
{
    public class ValidUserNameRegister :  ValidationAttribute
    {
        private IUnitOfWork _unitOfWork;
        public ValidUserNameRegister()
        {
            this._unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var registerDto = (RegisterDto) validationContext.ObjectInstance;

            if (String.IsNullOrEmpty(registerDto.Username))
                return ValidationResult.Success; // required username condition is handled by another validation

            var account = this._unitOfWork.Accounts.SingleOrDefault(a => a.UserName == registerDto.Username);

            if (account != null)
                return new ValidationResult("User name is used by another customer. Please try another one");

            return ValidationResult.Success;
        }
    }
}
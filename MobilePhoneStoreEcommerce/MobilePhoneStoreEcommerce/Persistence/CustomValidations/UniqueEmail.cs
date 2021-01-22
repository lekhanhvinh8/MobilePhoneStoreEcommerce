using MobilePhoneStoreEcommerce.Core;
using MobilePhoneStoreEcommerce.Core.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreEcommerce.Persistence.CustomValidations
{
    public class UniqueEmail : ValidationAttribute
    {
        private IUnitOfWork _unitOfWork;
        public UniqueEmail()
        {
            this._unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var regiterDto = (RegisterDto)validationContext.ObjectInstance;

            var sellers = this._unitOfWork.Sellers.Find(s => s.Email == regiterDto.Email);
            var customers = this._unitOfWork.Customers.Find(c => c.Email == regiterDto.Email);

            if (sellers.Count() > 0 || customers.Count() > 0)
                return new ValidationResult("Email is used by another account. Please try another one");

            return ValidationResult.Success;
        }
    }
}
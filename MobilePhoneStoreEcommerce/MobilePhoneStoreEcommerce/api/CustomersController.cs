using MobilePhoneStoreEcommerce.Core;
using MobilePhoneStoreEcommerce.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MobilePhoneStoreEcommerce.api
{
    public class CustomersController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountAuthentication _accountAuthentication;

        public CustomersController(IUnitOfWork unitOfWork, IAccountAuthentication accountAuthentication)
        {
            this._unitOfWork = unitOfWork;
            this._accountAuthentication = accountAuthentication;
        }

       
    }
}

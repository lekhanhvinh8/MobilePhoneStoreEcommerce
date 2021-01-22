using MobilePhoneStoreEcommerce.Core;
using MobilePhoneStoreEcommerce.Core.Domain;
using MobilePhoneStoreEcommerce.Core.Services;
using MobilePhoneStoreEcommerce.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace MobilePhoneStoreEcommerce.Controllers
{
    public class CartController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private IAccountAuthentication _accountAuthentication; 

        public CartController(IUnitOfWork unitOfWork, IAccountAuthentication accountAuthentication)
        {
            this._unitOfWork = unitOfWork;
            this._accountAuthentication = accountAuthentication;
        }
        public ActionResult Index(int customerID)
        {

            Customer customer = this._unitOfWork.Customers.Get(customerID);
            if (customer == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return View(new CartViewModel() { CustomerID = customerID, CustomerAddress = customer.DeliveryAddress});
        }
    }
}
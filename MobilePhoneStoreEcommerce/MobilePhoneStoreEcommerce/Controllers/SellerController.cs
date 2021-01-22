using MobilePhoneStoreEcommerce.Core;
using MobilePhoneStoreEcommerce.Core.Services;
using MobilePhoneStoreEcommerce.Core.ViewModels;
using MobilePhoneStoreEcommerce.Persistence.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace MobilePhoneStoreEcommerce.Controllers
{
    public class SellerController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private IAccountAuthentication _accountAuthentication;
        public SellerController(IUnitOfWork unitOfWork, IAccountAuthentication accountAuthentication)
        {
            this._unitOfWork = unitOfWork;
            this._accountAuthentication = accountAuthentication;
        }
        public ActionResult Index(int sellerID)
        {
            if (!IsAuthorized(sellerID))
                return RedirectToAction("Login", "Account", new { roleID = RoleIds.Seller });

            var productForSellerViewModel = new ProductForSellerViewModel() { SellerID = sellerID };

            return View(productForSellerViewModel);
        }

        public ActionResult AddNewProduct(int sellerID)
        {
            if (!IsAuthorized(sellerID))
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            var productForSellerViewModel = new ProductForSellerViewModel() { SellerID = sellerID };

            return View(productForSellerViewModel);
        }
        public ActionResult UpdateProduct(int productID, int sellerID)
        {
            if (!IsAuthorized(sellerID))
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            var product = this._unitOfWork.Products.Get(productID);

            if (product == null)
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);

            var productForSellerViewModel = new ProductForSellerViewModel();

            productForSellerViewModel.ProductID = product.ID;
            productForSellerViewModel.SellerID = sellerID;

            return View(productForSellerViewModel);
        }

        private bool IsAuthorized(int sellerID)
        {
            var session = Session[SessionNames.SellerID];
            if (!this._accountAuthentication.IsAuthentic(sellerID, session))
                return false;

            return true;
        }
    }
}
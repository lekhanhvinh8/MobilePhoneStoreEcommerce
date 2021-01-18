using MobilePhoneStoreEcommerce.Core;
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
    public class SellerController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public SellerController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public ActionResult Index(int sellerID)
        {
            var productForSellerViewModel = new ProductForSellerViewModel() { SellerID = sellerID };

            return View(productForSellerViewModel);
        }

        public ActionResult AddNewProduct(int sellerID)
        {
            var productForSellerViewModel = new ProductForSellerViewModel() { SellerID = sellerID };

            return View(productForSellerViewModel);
        }
        public ActionResult UpdateProduct(int productID, int sellerID)
        {
            var product = this._unitOfWork.Products.Get(productID);

            if (product == null)
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);

            var productForSellerViewModel = new ProductForSellerViewModel();

            productForSellerViewModel.ProductID = product.ID;
            productForSellerViewModel.SellerID = sellerID;

            return View(productForSellerViewModel);
        }
    }
}
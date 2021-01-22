using MobilePhoneStoreEcommerce.Core.Domain;
using MobilePhoneStoreEcommerce.Core.Dtos;
using MobilePhoneStoreEcommerce.Core.ViewModels;
using MobilePhoneStoreEcommerce.Models.ControllerModels;
using MobilePhoneStoreEcommerce.Persistence.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobilePhoneStoreEcommerce.Controllers
{
    public class CustomerController : Controller
    {
        public ActionResult Index(int customerID)
        {
            var customerViewModel = new CustomerViewModel() { customerID = customerID };
            return View(customerViewModel);
        }
        public ActionResult Info(int customerID)
        {
            ViewBag.Info = "Customer";
            var customer = new InfoModels().getCustomerById(customerID);
            var customerDto = new InfoDto();
            customerDto.CustomerId = customerID;
            customerDto.Name = customer.Name;
            customerDto.Email = customer.Email;
            customerDto.PhoneNumber = customer.PhoneNumber;
            customerDto.Address = customer.DeliveryAddress;
            return View(customerDto);
        }

        [HttpPost]
        public ActionResult Info(InfoDto customerDto)
        {
            if (ModelState.IsValid)
            {
                var customerInDb = new InfoModels().getCustomerById(customerDto.CustomerId);
                customerInDb.Name = customerDto.Name;
                customerInDb.Email = customerDto.Email;
                customerInDb.PhoneNumber = customerDto.PhoneNumber;
                customerInDb.DeliveryAddress = customerDto.Address;
                if (new InfoModels().UpdateCustomer(customerInDb))
                {
                    return View(customerDto);
                }
            }
            return View(customerDto);
        }

        public ActionResult SellerInfo(int sellerID)
        {
            var seller = new InfoModels().getSellerById(sellerID);
            var sellerDto = new InfoDto();
            sellerDto.CustomerId = sellerID;
            sellerDto.Name = seller.Name;
            sellerDto.Email = seller.Email;
            sellerDto.PhoneNumber = seller.PhoneNumber;
            sellerDto.Address = seller.WarehouseAddress;
            return View("Info", sellerDto);
        }

        [HttpPost]
        public ActionResult SellerInfo(InfoDto sellerDto)
        {
            if (ModelState.IsValid)
            {
                var sellerInDb = new InfoModels().getSellerById(sellerDto.CustomerId);
                sellerInDb.Name = sellerDto.Name;
                sellerInDb.Email = sellerDto.Email;
                sellerInDb.PhoneNumber = sellerDto.PhoneNumber;
                sellerInDb.WarehouseAddress = sellerDto.Address;
                if (new InfoModels().UpdateSeller(sellerInDb))
                {
                    return View("Info", sellerDto);
                }
            }
            return View("Info", sellerDto);
        }
    }
}
using MobilePhoneStoreEcommerce.Models.ControllerModels;
using MobilePhoneStoreEcommerce.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobilePhoneStoreEcommerce.Controllers
{
    public class HomeScreenController : Controller
    {
        private ApplicationDbContext _context;
        public HomeScreenController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: HomeScreen
        public ActionResult Index(int page = 1, int pagesize = 10)
        {
            var allProducts = new ProductModel().allAvailableProducts(page, pagesize);
            return View(allProducts);
        }
    }
}
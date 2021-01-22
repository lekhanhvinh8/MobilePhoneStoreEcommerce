using MobilePhoneStoreEcommerce.Models.ControllerModels;
using MobilePhoneStoreEcommerce.Models.ViewModels;
using MobilePhoneStoreEcommerce.Persistence;
using PagedList;
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
        public ActionResult Index(int page = 1, int pagesize = 8)
        {
            var allProducts = new ProductModel().allAvailableProducts(page, pagesize);
            return View(allProducts);
        }

        public ActionResult Details(int id)
        {
            var product = new ProductModel().productDetail(id);
            ViewBag.ProId = id;
            return View(product);
        }
        public ActionResult Store(int page = 1, int pagesize = 8)
        {
            ViewBag.Action = "Store";
            var viewModel = new StoreViewModels()
            {
                allAvailableProducts = new ProductModel().allAvailableProducts(page, pagesize),
                allProducers = new ProductModel().allProducers()
            };
            return View(viewModel);
        }

        public ActionResult Producer(int id, int page = 1, int pagesize = 8)
        {
            //var productOfProducer = new ProductModel().allAvailableProductsOfProducer(id, page, pagesize);
            //return View(productOfProducer);
            ViewBag.Producer = ViewBag.Action = "Producer";
            ViewBag.ProducerName = new ProductModel().getProducerNameById(id);
            var viewModel = new StoreViewModels()
            {
                allAvailableProducts = new ProductModel().allAvailableProductsOfProducer(id, page, pagesize),
                allProducers = new ProductModel().allProducers()
            };
            return View("Store", viewModel);
        }

        public ActionResult ProductsOfSearch(string search = "", int page = 1, int pagesize = 8)
        {
            ViewBag.Action = "Search";
            ViewBag.Search = search;
            var viewModel = new StoreViewModels()
            {
                allAvailableProducts = new ProductModel().allAvailableProductsOfSearch(search, page, pagesize),
                allProducers = new ProductModel().allProducers()
            };
            return View("Store", viewModel);
        }

        public ActionResult Category(int id, int page = 1, int pagesize = 8)
        {
            ViewBag.Action = "Cat";
            ViewBag.CatName = new ProductModel().getCatNameById(id);
            var viewModel = new StoreViewModels()
            {
                allAvailableProducts = new ProductModel().allAvailableProductsOfCategory(id, page, pagesize),
                allProducers = new ProductModel().allProducers()
            };
            return View("Store", viewModel);
        }
    }
}
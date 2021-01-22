using MobilePhoneStoreEcommerce.Core.ViewModels;
using System.Web.Mvc;

namespace MobilePhoneStoreEcommerce.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index(int customerID)
        {
            CustomerViewModel customerViewModel = new CustomerViewModel() { customerID = customerID };
            return View(customerViewModel);
        }
    }
}
using MobilePhoneStoreEcommerce.Core;
using MobilePhoneStoreEcommerce.Core.Domain;
using MobilePhoneStoreEcommerce.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobilePhoneStoreEcommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public ActionResult Index()
        {
            AllRoles allRoles = new AllRoles();
            allRoles.Roles = (List<Role>)this._unitOfWork.Roles.GetAll();
            return View(allRoles);
        }
    }
}
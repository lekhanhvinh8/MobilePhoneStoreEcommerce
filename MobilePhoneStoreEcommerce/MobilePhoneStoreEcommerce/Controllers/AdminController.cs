using MobilePhoneStoreEcommerce.Core.Services;
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
    public class AdminController : Controller
    {
        private IAccountAuthentication _accountAuthentication;
        public AdminController(IAccountAuthentication accountAuthentication)
        {
            this._accountAuthentication = accountAuthentication;
        }
        public ActionResult Index()
        {
            if (!IsAuthorized())
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            return View();
        }

        private bool IsAuthorized()
        {
            if (!this._accountAuthentication.IsAuthentic(Session[SessionNames.AdminID]))
                return false;

            return true;
        }
    }
}
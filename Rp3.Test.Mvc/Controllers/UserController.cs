using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rp3.Test.Proxies;
using Rp3.Test.Mvc.Models;
using Rp3.Test.Common.Models;

namespace Rp3.Test.Mvc.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(UserCreateModel createModel)
        {
            Proxy proxy = new Proxy();

            if (createModel.Name == null || createModel.Password == null || createModel.Name == "" || createModel.Password == "")
                return View(createModel);

            object[] x = { createModel.Name, createModel.Password };

            bool respondeOk = proxy.GetByUserPass(x);

            if (respondeOk)
            {
                Session["user"] = createModel.Name;
                Session.Timeout = 10;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(createModel);
            }
        }

        public ActionResult LogOut()
        {
            Session.Remove("user");
            return RedirectToAction("Index", "Home");
        }

    }
}
﻿using System;
using System.Web.Mvc;
using Market.Cqrsnes.Projection;

namespace Market.Cqrsnes.WebUi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISystemContext context;

        public HomeController(ISystemContext context)
        {
            this.context = context;
        }

        public ActionResult Index()
        {
            if (context.Principal.Identity.IsAuthenticated)
            {
                return RedirectToAction("List", "User");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult Log()
        {
            try
            {
                var path = Server.MapPath("~/market.log");
                return View(System.IO.File.ReadAllText(path) as object);
            }
            catch (Exception)
            {
                return View();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineSuperMartket.Models;
namespace OnlineSuperMartket.Controllers
{
    public class servicesController : Controller
    {
        online_superMarket_systemEntities db = new online_superMarket_systemEntities();
        // GET: services
        public ActionResult services()
        {
            ViewBag.pageName = "Service";
            ViewBag.link_ = "/services/services";

            ViewBag.products = db.Products.ToList();
            ViewBag.brands = db.Brands.ToList();
            ViewBag.category = db.Categories.ToList();
            ViewBag.Vendor = db.users.Where(x => x.role_ID == 1).ToList();
            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineSuperMartket.Models;
namespace OnlineSuperMartket.Controllers
{
    public class aboutUSController : Controller
    {
        online_superMarket_systemEntities db = new online_superMarket_systemEntities();
        
        // GET: aboutUS
        public ActionResult about()
        {
            ViewBag.pageName ="About US" ;
            ViewBag.link_ = "/aboutUS/about";

            ViewBag.products = db.Products.ToList();
            ViewBag.brands = db.Brands.ToList();
            ViewBag.category = db.Categories.ToList();
            ViewBag.Vendor = db.users.Where(x => x.role_ID == 1).ToList();
            return View();
        }
    }
}
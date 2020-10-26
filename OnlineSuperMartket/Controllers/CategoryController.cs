using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineSuperMartket.Models;
namespace OnlineSuperMartket.Controllers
{
    public class CategoryController : Controller
    {
        online_superMarket_systemEntities db = new online_superMarket_systemEntities();
        // GET: Category
        public ActionResult Index(int id)
        {
            ViewBag.products = db.Products.ToList();
            ViewBag.brands = db.Brands.ToList();
            ViewBag.category = db.Categories.ToList();
            ViewBag.Vendor = db.users.Where(x => x.role_ID == 1).ToList();
            ViewBag.page_Category="";
            
            var a =db.Categories.Find(id);
           
            var p = db.Products.Where(x => x.category_ID == id && x.is_active == true).ToList();
            ViewBag.prodts = p;
            ViewBag.pageName = a.category_name;
            //ViewBag.link_ = "/services/services";

            return View();
        }


        public ActionResult customerAfterPurchase() {

            ViewBag.products = db.Products.ToList();
            ViewBag.brands = db.Brands.ToList();
            ViewBag.category = db.Categories.ToList();
            ViewBag.Vendor = db.users.Where(x => x.role_ID == 1).ToList();
            ViewBag.page_Category = "";

            //var a = db.Categories.Find(id);

            //var p = db.Products.Where(x => x.category_ID == id).ToList();
            var p = db.Products.Where(x=>x.is_active == true).ToList();
            ViewBag.prodts = p;
            //ViewBag.pageName = a.category_name;
            ViewBag.pageName = "Shop Now!";
            //ViewBag.link_ = "/services/services";

            return View();

        }
    }
}
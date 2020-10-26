using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineSuperMartket.Models;
namespace OnlineSuperMartket.Controllers
{
    public class singlePrductController : Controller
    {
        online_superMarket_systemEntities db = new online_superMarket_systemEntities();
        
        public ActionResult Index()
        {
            ViewBag.products = db.Products.ToList();
            ViewBag.brands = db.Brands.ToList();
            ViewBag.category = db.Categories.ToList();

            //var a = db.Products.Find(form_data.Product_ID);
            //ViewBag.img_data = a;

            return View();
        }

        public JsonResult return_img(Product form_data) {

            var a =db.Products.Find(form_data.Product_ID);



            return Json( JsonRequestBehavior.AllowGet);
        }


        

    }
}
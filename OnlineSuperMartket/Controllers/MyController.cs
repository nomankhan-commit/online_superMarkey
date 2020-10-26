using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Sql;
using System.Data.SqlTypes;
using OnlineSuperMartket.Models;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SQLHelper;
using System.Data.Entity;
using System.IO;

namespace OnlineSuperMartket.Controllers
{
    public class MyController : Controller
    {
        online_superMarket_systemEntities db = new online_superMarket_systemEntities();
        string cs = ConfigurationManager.ConnectionStrings["MyDefaultConnection"].ConnectionString;
        // GET: My
        public ActionResult home()
        {
            if (Session["UserID"] == null)
            {
                return Redirect("~/Accounts/login_signup");
            }
            else
            {
                ViewBag.brands = db.Brands.Where(x => x.is_active == true).ToList();
                ViewBag.category = db.Categories.Where(x => x.is_active == true).ToList();
                int userID = Convert.ToInt32(Session["UserID"]);
                ViewBag.products = db.Products.Where(x => x.is_active == true && x.sellorID == userID ).ToList();
                ViewBag.Vendor = db.users.Where(x => x.role_ID == 1).ToList();
                return View("home");
            }
          
        }


        public ActionResult home_()
        {
           
            ViewBag.brands = db.Brands.Where(x => x.is_active == true).ToList();
            ViewBag.category = db.Categories.Where(x => x.is_active == true).ToList();
            ViewBag.products = db.Products.Where(x => x.is_active == true).ToList();
            ViewBag.Vendor = db.users.Where(x => x.role_ID == 1).ToList();
            return View("home_");
        }


        public ActionResult All_Data()       
            {

            if (Session["UserID"] == null)
            {
                return Redirect("~/Accounts/login_signup");
            }
            else
            {
                int userID = Convert.ToInt32(Session["UserID"]);
                ViewBag.brands = db.Brands.Where(x => x.is_active == true).ToList();
                ViewBag.category = db.Categories.Where(x => x.is_active == true).ToList();
                ViewBag.products = db.Products.Where(x => x.is_active == true).ToList();
                ViewBag.sp_products = db.sp_Products(userID).ToList();
                ViewBag.Vendor = db.users.Where(x => x.role_ID == 1).ToList();

                return View("All_Data");
            }

               


        }

        public ActionResult ActiveProducts() {
            if (Session["UserID"] == null)
            {
                return Redirect("~/Accounts/login_signup");
            }
            else
            {
                int userID = Convert.ToInt32(Session["UserID"]);
                ViewBag.brands = db.Brands.Where(x => x.is_active == true).ToList();
                ViewBag.category = db.Categories.Where(x => x.is_active == true).ToList();
                ViewBag.products = db.Products.Where(x => x.is_active == true).ToList();
                ViewBag.sp_products = db.sp_ActiveProductsAgainstVendorID(true, userID).ToList();
                ViewBag.Vendor = db.users.Where(x => x.role_ID == 1).ToList();

                return View();
            }

        }
        public ActionResult InActiveProducts() {
            if (Session["UserID"] == null)
            {
                return Redirect("~/Accounts/login_signup");
            }
            else
            {
                int userID = Convert.ToInt32(Session["UserID"]);
                ViewBag.brands = db.Brands.Where(x => x.is_active == true).ToList();
                ViewBag.category = db.Categories.Where(x => x.is_active == true).ToList();
                ViewBag.products = db.Products.Where(x => x.is_active == true).ToList();
                ViewBag.sp_products = db.sp_ActiveProductsAgainstVendorID(false, userID).ToList();
                ViewBag.Vendor = db.users.Where(x => x.role_ID == 1).ToList();

                return View();
            }
        }


        public ActionResult CategoryVisProductsVendor(int category_ID) {

            if (Session["UserID"] == null)
            {
                return Redirect("~/Accounts/login_signup");
            }
            else
            {
                int userID = Convert.ToInt32(Session["UserID"]);
                int userRole = Convert.ToInt32(Session["UserID"]);
                ViewBag.brands = db.Brands.Where(x => x.is_active == true).ToList();
                ViewBag.category = db.Categories.Where(x => x.is_active == true).ToList();
                ViewBag.products = db.Products.Where(x => x.is_active == true).ToList();
                ViewBag.sp_products = db.sp_CategryVisProductsForVendor(userID, category_ID).ToList();
                var caegroname = db.Categories.Where(x => x.category_ID == category_ID).Select(x => x.category_name).ToList();
                ViewBag.CategoryName = caegroname[0].ToString();
                ViewBag.Vendor = db.users.Where(x => x.role_ID == 1).ToList();
                return View();
            }
        }

        public ActionResult contactUs()
        {
            ViewBag.brands = db.Brands.Where(x => x.is_active == true).ToList();
            ViewBag.category = db.Categories.Where(x => x.is_active == true).ToList();
            ViewBag.products = db.Products.Where(x => x.is_active == true).ToList();
            //ViewBag.sp_products = db.sp_Products().ToList();
            ViewBag.Vendor = db.users.Where(x => x.role_ID == 1).ToList();

            return View("contactUs");


        }



        public ActionResult updateProduct(Product form_data) {
        
            string computer_name = Path.GetFileNameWithoutExtension(form_data.ImageFile.FileName);
            string extension = Path.GetExtension(form_data.ImageFile.FileName);
            var name1 = DateTime.Now.ToString("yymmssfff") + extension;
           
            var name2 = name1; //DateTime.Now.ToString("yymmssfff") + extension;
            var fileSize = form_data.ImageFile.ContentLength;      //file size in kb.
            string fileType = form_data.ImageFile.ContentType;

            Stream a = form_data.ImageFile.InputStream;
            System.Drawing.Image image = System.Drawing.Image.FromStream(a);
            int height = image.Height;
            int width = image.Width;

            string folder = "Image";

            string fileName = Path.Combine(Server.MapPath(folder), name1);
            //var name3 = Path.Combine(Server.MapPath(folder), name2);
            string isFolder_exist = Path.Combine(Server.MapPath(folder)); // fileName1 contain only folder name 
                                                                          // ~/ uploadImage / Image / 191848515.jpg
            bool isexist = Directory.Exists(isFolder_exist);
            if (!isexist)
            {
                Directory.CreateDirectory(isFolder_exist);
            }

            form_data.is_active = true;
            form_data.create_at = DateTime.Now;
            form_data.update_at = DateTime.Now;
            form_data.imgPath = name2;
            

           
            var aa =db.Products.Find(form_data.Product_ID);

            db.sp_updateProduct(form_data.Product_ID,
                form_data.category_ID,
                form_data.brand_ID, 
                form_data.Product_name,
                form_data.Product_disc, 
                form_data.whole_sale_price, 
                form_data.Product_code, 
                form_data.retail_price, 
                form_data.stock,
                form_data.imgPath, 
                form_data.create_at, 
                form_data.update_at,
                form_data.is_active
                );


            string path = "E:\\online_superMarkey\\OnlineSuperMartket\\OnlineSuperMartket\\AddProducts\\Image\\" + name2;
            form_data.ImageFile.SaveAs(path);
            ModelState.Clear();

            return RedirectPermanent("/My/All_Data");

        }

        public ActionResult update_category(Category form_data) {
            var db_result = db.Categories.Where(x => x.category_ID == form_data.category_ID).FirstOrDefault();

            form_data.is_active = true;
            form_data.update_at = DateTime.Now;
            form_data.create_at = db_result.create_at;

            form_data.category_disc = "null";

            db.update_category(form_data.category_ID,form_data.category_name,form_data.update_at);
            // return RedirectPermanent("/My/All_Data");
            return RedirectPermanent("/controller/brandsCategory");
        }

        public ActionResult update_brand(Brand form_data)
        {

            var db_result = db.Brands.Where(x => x.brand_ID == form_data.brand_ID).FirstOrDefault();

            form_data.is_active = true;
            form_data.update_at = DateTime.Now;
            form_data.create_at = db_result.create_at;

            form_data.brand_code = 000;
            db.update_brand(form_data.brand_ID,form_data.brand_name,form_data.update_at);

            return RedirectPermanent("/controller/brandsCategory");
            //controller/brandsCategory
            //  return RedirectPermanent("/My/All_Data");

        }

        public RedirectResult dlt(Product from_data) {



            return RedirectPermanent("/My/All_Data");
        }


    }
}

//SELECT p.imgPath, p.Product_name, br.brand_name, c.category_name
//FROM Brand br
//INNER JOIN Product p

//on p.brand_ID = br.brand_ID
//INNER JOIN Category c

//on p.category_ID = c.category_ID
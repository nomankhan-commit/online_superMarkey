using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineSuperMartket.Models;
using System.Net.Mail;
using System.Net;
using System.Configuration;

namespace OnlineSuperMartket.Controllers
{
    public class controllerController : Controller
    {
        online_superMarket_systemEntities db = new online_superMarket_systemEntities();
        // GET: controller
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult vendorListActive()
        {
            ViewBag.products = db.Products.ToList();
            ViewBag.brands = db.Brands.ToList();
            ViewBag.category = db.Categories.ToList();

            var db_result = db.users.Where(x => x.role_ID == 1  &&  x.is_active == true).ToList();
            ViewBag.vendorlistActive = db_result;
            ViewBag.Vendor = db.users.Where(x => x.role_ID == 1).ToList();
            return View();
        }

        public ActionResult vendorListInActive()
        {
            ViewBag.products = db.Products.ToList();
            ViewBag.brands = db.Brands.ToList();
            ViewBag.category = db.Categories.ToList();

            var db_result = db.users.Where(x => x.role_ID == 1 && x.is_active == false).ToList();
            ViewBag.vendorlistActive = db_result;
            ViewBag.Vendor = db.users.Where(x => x.role_ID == 1).ToList();
            return View();
        }

        public ActionResult Approve(int ? id) {
            var db_result__ = db.users.Where(x => x.userID == id).ToList();
            db.ActiveVendor(id);
            string body = "your account has approved kindly login and upload products for sale http://localhost:31574/Accounts/login_signup";
            Emai("Your are Approved", db_result__[0].email.ToString(), body);
            return RedirectToAction("vendorListInActive","controller");

        }

        public ActionResult BlockVendor(int ? id)
        {
            var db_result__ = db.users.Where(x => x.userID == id).ToList();

            db.InActiveVendor(id);
            string body = "your account has Blocked kindly Contact with Admin  http://localhost:31574/Accounts/login_signup";
            Emai("Your are Approved", db_result__[0].email.ToString(), body);
            return RedirectToAction("vendorListActive", "controller");
            
        }

        public ActionResult AllProducts()
        {
        //    var db_result__ = db.).ToList();

        //    db.InActiveVendor(id);
        //    string body = "your account has Blocked kindly Contact with Admin  http://localhost:31574/Accounts/login_signup";
        //    Emai("Your are Approved", db_result__[0].email.ToString(), body);
            return RedirectToAction("vendorListActive", "controller");

        }

        public ActionResult AllProducts_()
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
                ViewBag.sp_products = db.sp_ProductsAll_().ToList();
                ViewBag.Vendor = db.users.Where(x=>x.role_ID == 1).ToList();

                return View();
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
                ViewBag.sp_products = db.sp_ProductsAll(true).ToList();
                ViewBag.Vendor = db.users.Where(x => x.role_ID == 1).ToList();

                return View();
            }
        }

        public ActionResult InActiveProducts()
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
                ViewBag.sp_products = db.sp_ProductsAll(false).ToList();
                ViewBag.Vendor = db.users.Where(x => x.role_ID == 1).ToList();

                return View();
            }

        }


        public ActionResult MakeActiveProducts(int ? id)
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
                ViewBag.sp_products = db.sp_ProductsAll(false).ToList();
                db.ActiveProducts(id);
                var ddb = db.Products.Where(x => x.Product_ID == id).ToList();
                var body = "you Product (product id = " + id + ") is Activated.";
                string email = ddb[0].sellorName.ToString();
                Emai("Product InAtive", email, body);
                return Redirect("~/controller/InActiveProducts");
            }

        }

        public ActionResult MakeINActiveProducts(int ? id)
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
                ViewBag.sp_products = db.sp_ProductsAll(false).ToList();
                // Emai(string subject, string reciever, string message)
                db.InActiveProducts(id);
                var ddb = db.Products.Where(x => x.Product_ID ==id).ToList();
                var body = "you Product (product id = "+id+") is inActivated please contact with admin.";
                string email = ddb[0].sellorName.ToString();
                Emai("Product InAtive", email , body);


                return Redirect("~/controller/ActiveProducts");
            }

        }

        public ActionResult categoryVisProducts(int category_ID)
        {


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
                ViewBag.sp_products = db.sp_CategryVisProducts(category_ID).ToList();
                var caegroname = db.Categories.Where(x => x.category_ID == category_ID).Select(x => x.category_name).ToList();
                ViewBag.CategoryName = caegroname[0].ToString();
                ViewBag.Vendor = db.users.Where(x => x.role_ID == 1).ToList();
                return View();
            }

        }

        public ActionResult VendorVisProducts(int vendorID)
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
                ViewBag.sp_products = db.sp_VendorVisProducts(vendorID).ToList();
                var vendorname = db.users.Where(x => x.userID == vendorID).Select(x => x.email).ToList();
                ViewBag.CategoryName = vendorname[0].ToString();
                ViewBag.Vendor = db.users.Where(x => x.role_ID == 1).ToList();
                return View();
            }

        }
        //

        public ActionResult brandsCategory() {
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
                ViewBag.sp_products = db.sp_ProductsAll(true).ToList();
                ViewBag.Vendor = db.users.Where(x => x.role_ID == 1).ToList();

                return View();
            }

        }
        public void Emai(string subject, string reciever, string message)
        {
            string password_ = ConfigurationManager.AppSettings["passwordEmail"];
            string email = ConfigurationManager.AppSettings["Email"];
            try
            {
                if (ModelState.IsValid)
                {
                    var senderEmail = new MailAddress(email);
                    var receiverEmail = new MailAddress(reciever);
                    var password = password_;
                    var sub = subject;
                    var body = message;
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = subject,
                        Body = body
                    })
                    {
                        smtp.Send(mess);
                    }

                }
            }
            catch (Exception)
            {
                
            }
        }
    }
}
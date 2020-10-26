using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineSuperMartket.Models;
using Newtonsoft.Json;
using System.Configuration;
using System.Net.Mail;
using System.Net;

namespace OnlineSuperMartket.Controllers
{
    public class AdmindashboardController : Controller
    {
        online_superMarket_systemEntities db = new online_superMarket_systemEntities();

        // GET: Admindashboard
        public ActionResult Index()
        {
            if (Session["UserID"] == null)
            {
                return Redirect("~/Accounts/login_signup");
            }
            else {
                ViewBag.countCategorygroubBy = JsonConvert.SerializeObject(db.sp_CountofTotalCategory(true,0).ToList());
                ViewBag.countVendorgroubBy = JsonConvert.SerializeObject(db.sp_CountofTotalVendor().ToList());
                ViewBag.countProductsgroubBy = JsonConvert.SerializeObject(db.sp_CountofTotalProducts(true,0).ToList());

                int userID = Convert.ToInt32(Session["UserID"]);
                ViewBag.brands = db.Brands.Where(x => x.is_active == true).ToList();
                ViewBag.category = db.Categories.Where(x => x.is_active == true).ToList();
                ViewBag.products = db.Products.Where(x => x.is_active == true).ToList();
                ViewBag.sp_products = db.sp_Products(userID).ToList();
                ViewBag.Vendor = db.users.Where(x => x.role_ID == 1).ToList();

                ViewBag.sp_sellAcctoCategory = JsonConvert.SerializeObject(db.sp_sellAcctoCategory().ToList());
                ViewBag.sp_sellAcctoVendor = JsonConvert.SerializeObject(db.sp_sellAcctoVendor().ToList());
                ViewBag.sp_sellAcctoproducts = JsonConvert.SerializeObject(db.sp_sellAcctoproducts().ToList());

                ViewBag.totalproducts = db.Products.Count().ToString();
                ViewBag.totalActiveProducts = db.Products.Where(x => x.is_active == true).Count().ToString();
                ViewBag.totalinActiveProducsts = db.Products.Where(x => x.is_active == false).Count().ToString();


                ViewBag.expectedorders = db.orders.Count().ToString();
                ViewBag.expectedSells = db.orders.Sum(x => x.productsAmount).ToString();


                ViewBag.totalCustomer = db.users.Where(x => x.role_ID == 4).Count().ToString();

                ViewBag.totalVendor = db.users.Where(x => x.role_ID == 1).Count().ToString();
                ViewBag.totalActiveVendor = db.users.Where(x => x.role_ID == 1 && x.is_active == true).Count().ToString();
                ViewBag.totalINActiveVendor = db.users.Where(x => x.role_ID == 1 && x.is_active == false).Count().ToString();
                //ViewBag.mostSellCategory 
                //ViewBag.mostSellProduct
                //ViewBag.topVendor

                ViewBag.TotalOrders = db.orders.Count().ToString();
                ViewBag.TotalOrderspending = db.orders.Where(x=>x.isDispatch == false).Count().ToString();
                ViewBag.TotalOrderscompleted = db.orders.Where(x => x.isDispatch == true).Count().ToString();


                ViewBag.overAllSell = db.orders.Sum(x => x.productsAmount).ToString();
                return View();
            }


        }



        public ActionResult VendorDshboard()
        {
            
            if (Session["UserID"] == null)
            {
                return Redirect("~/Accounts/login_signup");
            }
            else {
                int id = int.Parse(Session["UserID"].ToString());
                ViewBag.countCategorygroubBy = JsonConvert.SerializeObject(db.sp_CountofTotalCategory(false,id).ToList());
                ViewBag.countVendorgroubBy = JsonConvert.SerializeObject(db.sp_CountofTotalVendor().ToList());
                ViewBag.countProductsgroubBy = JsonConvert.SerializeObject(db.sp_CountofTotalProducts(false, id).ToList());

                int userID = Convert.ToInt32(Session["UserID"]);
                ViewBag.brands = db.Brands.Where(x => x.is_active == true).ToList();
                ViewBag.category = db.Categories.Where(x => x.is_active == true).ToList();
                ViewBag.products = db.Products.Where(x => x.is_active == true).ToList();
                ViewBag.sp_products = db.sp_Products(userID).ToList();
                ViewBag.Vendor = db.users.Where(x => x.role_ID == 1).ToList();

                ViewBag.sp_sellAcctoproducts = JsonConvert.SerializeObject(db.sp_sellAcctoproductsforVender(id).ToList());
                ViewBag.sp_sellAcctoVendor = JsonConvert.SerializeObject(db.sp_sellAcctoVendorforVender(id).ToList());
                ViewBag.sp_sellAcctoCategory = JsonConvert.SerializeObject(db.sp_sellAcctocategoryforVender(id).ToList());

                ViewBag.totalproducts = db.Products.Where(x => x.sellorID == id).Count().ToString();
                ViewBag.totalActiveProducts = db.Products.Where(x => x.is_active == true && x.sellorID == id).Count().ToString();
                ViewBag.totalinActiveProducsts = db.Products.Where(x => x.is_active == false && x.sellorID == id).Count().ToString();

                ViewBag.expectedorders = db.orders.Where(x=>x.venderid == id ).Count().ToString();
                ViewBag.expectedSells = db.orders.Where(x => x.venderid == id).Sum(x=>x.productsAmount).ToString();

                ViewBag.totalCustomer = db.users.Where(x => x.role_ID == 4).Count().ToString();

                ViewBag.totalVendor = db.users.Where(x => x.role_ID == 1).Count().ToString();
                ViewBag.totalActiveVendor = db.users.Where(x => x.role_ID == 1 && x.is_active == true).Count().ToString();
                ViewBag.totalINActiveVendor = db.users.Where(x => x.role_ID == 1 && x.is_active == false).Count().ToString();


                ViewBag.TotalOrders = db.orders.Count(x => x.venderid == id).ToString();
                ViewBag.TotalOrderspending = db.orders.Where(x => x.venderid == id && x.isDispatch == false).Count().ToString();
                ViewBag.TotalOrderscompleted = db.orders.Where(x => x.venderid == id && x.isDispatch == true).Count().ToString();


                //ViewBag.mostSellCategory 
                //ViewBag.mostSellProduct
                //ViewBag.topVendor
                ViewBag.overAllSell = db.orders.Where(x => x.venderid == id).Sum(x => x.productsAmount).ToString();
                return View();
            }
           
        }


        public ActionResult ContactusEmail(string subject, string name , string phone , string email, string  body) {
            try
            {
                string email_ = ConfigurationManager.AppSettings["Email"];
                string message = "SUBJECT :" + subject + "\n " + "NAME : " + name + "\n" + "PHONE : " + phone + "\n" + "Email : " + email + "\n" + "MESSAGE : " + body;

                Email(subject, email_, message);

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(false, JsonRequestBehavior.AllowGet);
            }

          
        }
        public void Email(string subject, string reciever, string message)
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
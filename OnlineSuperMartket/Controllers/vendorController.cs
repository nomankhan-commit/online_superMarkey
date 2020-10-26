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
    public class vendorController : Controller
    {
        bool status = false;
        online_superMarket_systemEntities db = new online_superMarket_systemEntities();
        // GET: vendor
        public ActionResult signup()
        {
            ViewBag.products = db.Products.ToList();
            ViewBag.brands = db.Brands.ToList();
            ViewBag.category = db.Categories.ToList();
            ViewBag.Vendor = db.users.Where(x => x.role_ID == 1).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult getRegister(user formData) {


            status = false;
         

            var db_result = db.users.Where(x => x.email == formData.email).FirstOrDefault();

            if (db_result == null)
            {

                formData.role_ID = 1;
                formData.is_active = false;
                formData.create_at = DateTime.Now;

                db.users.Add(formData);
                db.SaveChanges();

                var db_result_ = db.users.Where(x => x.email == formData.email).FirstOrDefault();
                Session["UserID"] = db_result_.userID.ToString();
                Session["FullName"] = db_result_.first_name.ToString() + " " + db_result_.last_name.ToString();
                Session["last_name"] = db_result_.last_name.ToString();
                Session["Role_ID"] = db_result_.role_ID.ToString();
                Session["userDetails"] = Convert.ToString(db_result_);
                status = true;
                //Emai(string subject, string reciever, string message);
                //string emailBody = "<h2>Sellor Registeration Alert</h2></br><h5>Id</h5>"+ Session["UserID"].ToString() + "</br><h5>Email</h5>"+ db_result_ .email.ToString()+ ""; 
                string emailBody = "new "+db_result_.email.ToString()+ " vendor has registered. please approve/reject to enable his access level in the application. http://localhost:31574/Accounts/login_signup '";

            
                Emai("testing","nhr146@gmail.com",emailBody);

            }

            if (db_result != null)
            {

                status = false;

            }


            ///---------------

            // return Json(new { status, isInternalUser}, JsonRequestBehavior.AllowGet);
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        public ActionResult firstScreen()
        {

            return View();
        }


        public ActionResult orderslist() {

            int userID = Convert.ToInt32(Session["UserID"]);
            ViewBag.brands = db.Brands.Where(x => x.is_active == true).ToList();
            ViewBag.category = db.Categories.Where(x => x.is_active == true).ToList();
            ViewBag.products = db.Products.Where(x => x.is_active == true).ToList();
            ViewBag.sp_products = db.sp_Products(userID).ToList();
            ViewBag.Vendor = db.users.Where(x => x.role_ID == 1).ToList();

            ViewBag.orders =  db.orders.Where(x => x.venderid == userID).ToList();
            return View();

        }
        public ActionResult makeDispatch(int ? id) {

            if (Session["UserID"] == null)
            {
                return Redirect("~/Accounts/login_signup");
            }
            else
            {
                int userID = Convert.ToInt32(Session["UserID"]);
                db.completeOrder(userID,id);
                return Redirect("~/vendor/orderslist");
            }
        }

        public void Emai(string subject, string reciever,string message) {
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
                ViewBag.Error = "Some Error";
            }
        } 


    }
}
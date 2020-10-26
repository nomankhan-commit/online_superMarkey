using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineSuperMartket.Models;
using System.Threading;
using System.Net.Http;
using System.Configuration;
using System.Net.Mail;
using System.Net;

namespace OnlineSuperMartket.Controllers
{
    public class AccountsController : Controller
    {
        public static string category { get; set; }
        bool status = false;
        string button;
        online_superMarket_systemEntities db = new online_superMarket_systemEntities();
        // GET: Accounts
        //public ActionResult home()
        //{
        //    return View();
        //}
        public ActionResult login_signup() {
            ViewBag.products = db.Products.ToList();
            ViewBag.brands = db.Brands.ToList();
            ViewBag.category = db.Categories.ToList();
            ViewBag.Vendor = db.users.Where(x => x.role_ID == 1).ToList();
            // if (Session["UserID"] != null)
            //{
            //  return RedirectPermanent("/My/home");
            //}
            //else { 

            return View("login_signup");
          //  }
        }


        public ActionResult create_User(user form_data)
        {
            
            status = false;
            HttpResponseMessage response = new HttpResponseMessage();

            var db_result = db.users.Where(x => x.email == form_data.email).FirstOrDefault();
                          
            if (db_result == null)
            {
               
                    form_data.role_ID = 4;
                    form_data.is_active = true;
                    form_data.create_at = DateTime.Now;

                    db.users.Add(form_data);
                    db.SaveChanges();

                    var db_result_ = db.users.Where(x => x.email == form_data.email).FirstOrDefault();
                    Session["UserID"] = db_result_.userID.ToString();
                    Session["FullName"] = db_result_.first_name.ToString() + " " + db_result_.last_name.ToString();
                    Session["last_name"] = db_result_.last_name.ToString();
                    Session["Role_ID"] = db_result_.role_ID.ToString();
                    Session["userDetails"] = Convert.ToString(db_result_);
                    status = true;
                

             
            }

            if (db_result != null)
            {

                status = false;

            }
           

                ///---------------

                // return Json(new { status, isInternalUser}, JsonRequestBehavior.AllowGet);
                return Json(status, JsonRequestBehavior.AllowGet);
            
        }//----//
      
      
      
        public JsonResult Login_form(user form_data)
        {
            var Login_checker = "Fail";
            var rorid = "";
            var db_result = db.users.Where(x => x.email == form_data.email && x.password == form_data.password && x.is_active == true).FirstOrDefault();

            if (db_result != null)
            {
                Session["UserID"] = db_result.userID.ToString();
                Session["Email"] = db_result.email.ToString() ;
                Session["FullName"] = db_result.first_name.ToString() + " " + db_result.last_name.ToString();
                Session["last_name"] = db_result.last_name.ToString();
                Session["Role_ID"] = db_result.role_ID.ToString();
                //Session["userDetails"] = Convert.ToString(db_result);
                //if (db_result.role_ID.ToString()=="1")
                //{
                //    button = "<a href='@Url.Action('CreateUsers', 'Accounts')' target='_blank'>Create Users</a>";
                //}

                //if (db_result.role_ID.ToString() == "2")
                //{
                //    //
                //    button = "<a href='@Url.Action('CreateUsers', 'Accounts')' target='_blank' style='display: none'>Create Users</a>";
                //}

                //if (db_result.role_ID.ToString() == "3")
                //{
                //    button = "<a href='@Url.Action('CreateUsers', 'Accounts')' target='_blank' style='display: none'>Create Users</a>";
                //}

                //if (db_result.role_ID.ToString() == "4")
                //{
                //    button = "<a href='@Url.Action('CreateUsers', 'Accounts')' target='_blank' style='display: none'>Create Users</a>";
                //}

                //if (db_result.role_ID.ToString() == "5")
                //{
                //    button = "<a href='@Url.Action('CreateUsers', 'Accounts')' target='_blank' style='display: none'>Create Users</a>";
                //}


                //  return Json(new { success = false, responseText = "Your Password or email is incorrect." }, JsonRequestBehavior.AllowGet);
                Login_checker = "Success";
                rorid = Session["Role_ID"].ToString();
            }
            
           
            

                object [] dataaaa = { Login_checker, rorid };

                return Json(dataaaa, JsonRequestBehavior.AllowGet);
           
           
        }


        //public RedirectResult logOut()
        //{
        //    Session["UserID"] = null;
        //   // Session.Clear();
        //     Session.Abandon();
        //    return RedirectPermanent("/Accounts/login_signup");
        //}

        public ActionResult logOut()
        {
            Session["UserID"] = null;
            Session["FullName"] = null;
            Session["last_name"] = null;
            Session["Role_ID"] = null;
            Session["userDetails"] = null;

            Session.Clear();
            Session.Abandon();
            // return RedirectPermanent("/Accounts/login_signup");
            //  return View();
            return Redirect("~/My/home_");
        }




        public JsonResult addCategory(Category form_data) {

            var Category_name = form_data.category_name;
            //string fruit = "Apple,Banana,Orange,Strawberry";
            string[] split = Category_name.Split(',');

            for (int i = 0; i < split.Length; i++)
            {

                form_data.create_at = DateTime.Now;
                form_data.category_disc = "null";
                form_data.is_active = true;
                form_data.category_name = split[i];
                db.Categories.Add(form_data);
                db.SaveChanges();
                status = true;
                Thread.Sleep(250);
            }

          
            
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult addBrand(Brand form_data)
        {

            var Category_name = form_data.brand_name;
            //string fruit = "Apple,Banana,Orange,Strawberry";
            string[] split = Category_name.Split(',');

            for (int i = 0; i < split.Length; i++)
            {

                form_data.create_at = DateTime.Now;
                form_data.brand_code = 00;
                form_data.is_active = true;
                form_data.brand_name = split[i];
                db.Brands.Add(form_data);
                db.SaveChanges();
                status = true;
                Thread.Sleep(250);
            }



            return Json(status, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult discountManager(Discount form_data) {

            if (form_data.Get_One_By_One == null)
            {
                form_data.Get_One_By_One = false;
            }
            form_data.is_active = true;
            form_data.create_at=DateTime.Now;
            db.Discounts.Add(form_data);
           
            db.SaveChanges();


            status = true;
            return Json(status, JsonRequestBehavior.AllowGet);

        }


        public ActionResult CreateUsers(user form_data) {

            if (form_data.email!=null)
            {
                form_data.Mobile = "null";
                form_data.Credit_card_number = 00;
                form_data.Credit_card_expiry = 00;
                form_data.create_at = DateTime.Now;
                form_data.is_active = true;
                   
                db.users.Add(form_data);
                db.SaveChanges();

            }

            ViewBag.products = db.Products.ToList();
            ViewBag.brands = db.Brands.ToList();
            ViewBag.category = db.Categories.ToList();
            ViewBag.role = db.userRoles.ToList();
            ViewBag.pageName ="Create Users";
            return View("createUsers");
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
                ViewBag.Error = "Some Error";
            }
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineSuperMartket.Models;
using System.IO;
using System.Drawing.Imaging;
using System.Configuration;
using System.Net.Mail;
using System.Net;

namespace OnlineSuperMartket.Controllers
{
    public class AddProductsController : Controller
    {
        online_superMarket_systemEntities db = new online_superMarket_systemEntities();
        // GET: AddProducts
        public ActionResult Index()
        {
            return View();
        }

       [HttpPost]
        public RedirectResult addProducts(Product form_data) {
            if (Session["UserID"] == null)
            {
                return Redirect("~/Accounts/login_signup");
            }
            else
            {
                //string computer_name = Path.GetFileNameWithoutExtension(form_data.ImageFile.FileName);// file name before save. 

                string computer_name = Path.GetFileNameWithoutExtension(form_data.ImageFile.FileName);
                string extension = Path.GetExtension(form_data.ImageFile.FileName);
                var name1 = DateTime.Now.ToString("yymmssfff") + extension;
                //fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
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
                form_data.imgPath = name2;
                form_data.sellorID =  Convert.ToInt16(Session["UserID"]);
                form_data.sellorName = Session["Email"].ToString();
                db.Products.Add(form_data);
                db.SaveChanges();
                string path = "E:\\online_superMarkey\\OnlineSuperMartket\\OnlineSuperMartket\\AddProducts\\Image\\" + name2;
                form_data.ImageFile.SaveAs(path);
                ModelState.Clear();
                var productID = db.Products.ToList().Last() ;

                string emailBody = "new Product Uploaded (products id is "+productID.Product_ID.ToString()+") by vendor ( "+ Session["Email"].ToString()+" ). please review  it. http://localhost:31574/Accounts/login_signup '";

                Emai("testing", "nhr146@gmail.com", emailBody);
                // add // E:\online_superMarkey\OnlineSuperMartket\OnlineSuperMartket\AddProducts\Image\190149747.jpg
                //        E:\online_superMarkey\OnlineSuperMartket\OnlineSuperMartket\My\Image\190249693.png

                //return Json( JsonRequestBehavior.AllowGet);
                return RedirectPermanent("/My/home");
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
                ViewBag.Error = "Some Error";
            }
        }


    }
}


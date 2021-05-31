using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Vetro.Models;
using System.Net;
using System.Net.Mail;

namespace Vetro.Controllers
{
    public class ContactUsController : Controller
    {
        // GET: ContactUs
        string smtp_username = System.Configuration.ConfigurationManager.AppSettings["SMTPUsername"];
        string smtp_password = System.Configuration.ConfigurationManager.AppSettings["SMTPPassword"];
        string smtp_port = System.Configuration.ConfigurationManager.AppSettings["SMTPPort"];
        string smtp_Host = System.Configuration.ConfigurationManager.AppSettings["SMTPHost"];

        public ActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ContactUs(SendContactEmail model)
        {
            // To do list for Contact Us page email send 

            // Change email and port address 
            if (ModelState.IsValid)
            {
                var body = "<p>Email <br/> From: {0} ({1}) Email Address:({2})</p> <p>Message:</p> {3} ";
                var message = new MailMessage();
                //message.To.Add(new MailAddress(smtp_username));  // replace with valid value 
                message.To.Add(new MailAddress("chumanimqulo@gmail.com"));
                message.From = new MailAddress(model.Email);  // replace with valid value
                message.Subject = "PPS Foundation - Contact Us Information";
                //message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
                message.Body = string.Format(body,model.Fullname, model.Cellnumber, model.Email, model.Message);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        //Host 
                        //Port
                        //UserName = smtp_username,  // replace with valid value
                        //Password = smtp_password // replace with valid value
                        UserName = "chumanimqulo@gmail.com",
                        Password = "cat25vuli"
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    //smtp.Send(message);
                    return RedirectToAction("Sent");
                }
            }
            return View(model);
            //return View();
        }
        public ActionResult Sent()
        {
            return View();
        }
    }
}
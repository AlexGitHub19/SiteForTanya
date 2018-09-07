using SiteForTanya.WEB.Models;
using System;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace SiteForTanya.WEB.Controllers
{
    public class ContactsController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Contacts";
            ViewBag.Controller = "Contacts";
            return View();
        }

        [HttpPost]
        public ActionResult SendEmail(string name, string email, string text)
        {
            string result = "True";
            try
            {
                using (MailMessage message = new MailMessage("tetianapavliuchenkosite@gmail.com", "tetianapavliuchenko@gmail.com"))
                {
                    message.Subject = "Message from site";
                    message.Body = "email: "+ email + "\n text: "+ text;
                    using (SmtpClient client = new SmtpClient
                    {
                        EnableSsl = true,
                        Host = "smtp.gmail.com",
                        Port = 25,
                        Credentials = new NetworkCredential("tetianapavliuchenkosite@gmail.com", "eds6282A")
                    })
                        client.Send(message);
                }
            }
            catch (Exception ex)
            {
                ProcessException(ex);
                result = "False";
            }

            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        private ActionResult ProcessException(Exception exception)
        {
            ExceptionProcessor exceptionProcessor = new ExceptionProcessor();
            exceptionProcessor.process(exception);
            return View("Exception");
        }
    }
}
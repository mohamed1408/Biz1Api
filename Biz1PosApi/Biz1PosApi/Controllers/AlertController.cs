using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Biz1PosApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    public class AlertController : Controller
    {
        private static TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        private POSDbContext db;
        public AlertController(POSDbContext contextOptions)
        {
            db = contextOptions;
        }


        // GET: AlertController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AlertController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AlertController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AlertController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AlertController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AlertController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AlertController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AlertController/Delete/5
        [HttpPost("handleappcrash")]
        public IActionResult UploadFile()
        {
            try
            {
                IFormCollection form = HttpContext.Request.Form;

                string storedetailsjstring = form["storedetails"];
                dynamic storedetails = JsonConvert.DeserializeObject(storedetailsjstring);
                Alert alert = new Alert();
                alert.StoreId = (int)storedetails.storeid;
                alert.CompanyId = (int)storedetails.companyid;
                alert.AlertDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                alert.AlertName = "biz1pos_crash";
                db.Alerts.Add(alert);
                db.SaveChanges();
                send_alert_email(storedetails.storename.ToString(), alert.AlertDateTime);
                var response = new
                {
                    status = 200,
                    message = "Alert Successful"
                };
                return Ok(response);
            }
            catch(Exception e)
            {
                var response = new
                {
                    status = 500,
                    message = "error",
                    error = new Exception(e.Message, e.InnerException)
                };
                return Ok(response);
            }
        }
        public void send_alert_email(string storename, DateTime dateTime)
        {
            string from = "fruitsandbakes@gmail.com"; //From address    
            MailMessage message = new MailMessage(from, "mohamedanastsi@gmail.com");
            message.To.Add("karthick.nath@gmail.com");
            message.To.Add("masterservice2020@gmail.com");
            message.To.Add("mohamedanastsi@gmail.com");
            message.To.Add("sanjai.nath1995@gmail.com");
            string mailbody = storename + " faced an app crash event @" + dateTime.ToString();
            message.Subject = "Welcome to FBcakes";

            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
            client.UseDefaultCredentials = false;
            NetworkCredential basicCredential1 = new
            NetworkCredential("fbcakes.biz1@gmail.com", "PassworD@1");

            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;
            client.Send(message);
        }
    }
}

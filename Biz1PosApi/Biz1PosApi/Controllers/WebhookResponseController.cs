using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Biz1PosApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Biz1PosApi.Controllers
{
    public class WebhookResponseController : Controller
    {
        private POSDbContext db;
        public WebhookResponseController(POSDbContext contextOptions)
        {
            db = contextOptions;
        }
        public ActionResult Index()
        {
            return View();
        }

        // GET: WebhookResponse/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: WebhookResponse/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WebhookResponse/Create
        [HttpGet("check")]
        public ActionResult Create(string refid)
        {
            try
            {
                var response = new
                {
                    status = 0,
                    message = ""
                };
                WebhookResponse webhookResponse = db.WebhookResponses.Where(x => x.RefId == refid).FirstOrDefault();
                if(webhookResponse.StatusCode == 200)
                {
                    response = new
                    {
                        status = 200,
                        message = webhookResponse.message
                    };
                }
                else if(webhookResponse.StatusCode == 500)
                {
                    response = new
                    {
                        status = 500,
                        message = webhookResponse.message
                    };
                }
                else if (webhookResponse.StatusCode == 0)
                {
                    response = new
                    {
                        status = 0,
                        message = "Pending....."
                    };
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 0,
                    msg = "Something went wrong  Contact our service provider"
                };
                return StatusCode(500, error);
            }
        }

        // GET: WebhookResponse/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WebhookResponse/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WebhookResponse/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WebhookResponse/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
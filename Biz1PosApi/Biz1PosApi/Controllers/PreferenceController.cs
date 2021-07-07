using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Biz1PosApi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    public class PreferenceController : Controller
    {
        private POSDbContext db;
        public PreferenceController(POSDbContext contextOptions)
        {
            db = contextOptions;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("Get")]
        [EnableCors("AllowOrigin")]
        public IActionResult Get(int CompanyId)
        {
            try
            {
                var preference = db.Preferences.Where(d => d.CompanyId == CompanyId).ToList();
                return Ok(preference);
            }
            catch (Exception e)
            {
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 0,
                    msg = "Something went wrong  Contact our service provider"
                };
                return Json(error);
            }
        }
        [HttpPost("Savedata")]
        [EnableCors("AllowOrigin")]
        public IActionResult Savedata([FromForm]string data)
        {
            try
            {
                dynamic pre = JsonConvert.DeserializeObject(data);
                Preference preference = pre.ToObject<Preference>();
                db.Entry(preference).State = EntityState.Modified;
                db.SaveChanges();
                var error = new
                {
                    status = "success",
                    data = new
                    {
                        value = 2
                    },
                    msg = "The data Updated successfully"
                };

                return Json(error);
            }
            catch (Exception e)
            {
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 0,
                    msg = "Something went wrong  Contact our service provider"
                };
                return Json(error);
            }
        }
        }
    }
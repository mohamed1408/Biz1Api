using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Biz1PosApi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    public class PrinterController : Controller
    {
        private POSDbContext db;
        public PrinterController(POSDbContext contextOptions)
        {
            db = contextOptions;
        }
        public IActionResult Index()
        {

            return View();
        }
        [HttpGet("Get")]
        public IActionResult Get()
        {
            var printer = db.Printers.ToList();
            return Ok(printer);
        }

        [HttpPost("Create")]
        [EnableCors("AllowOrigin")]
        public IActionResult Create([FromForm]string data)
        {

            dynamic add = JsonConvert.DeserializeObject(data);

            Printer printer = add.ToObject<Printer>();
            db.Printers.Add(printer);
            db.SaveChanges();
            var error = new
            {
                status = "success",
                data = new
                {
                    value = 2
                },
                msg = "The data added successfully"
            };

            return Json(error);
        }
        [HttpPost("getPrinters")]
        [EnableCors("AllowOrigin")]
        public void getPrinters([FromForm]string data)
        {

        }
    }
}
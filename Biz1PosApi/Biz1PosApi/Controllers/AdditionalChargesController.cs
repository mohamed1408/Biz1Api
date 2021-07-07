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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Biz1BookPOS.Controllers
{
    [Route("api/[controller]")]
    public class AdditionalChargesController : Controller
    {
        private POSDbContext db;
        public AdditionalChargesController(POSDbContext contextOptions)
        {
            db = contextOptions;
        }
        [HttpGet("Index")]
        [EnableCors("AllowOrigin")]
        public IActionResult Index(int CompanyId)
        {
            try
            {
                var prod = new
                {
                    taxGroup = db.TaxGroups.Where(o => o.CompanyId == CompanyId).ToList(),
                    addtncharges = db.AdditionalCharges.Where(o => o.CompanyId == CompanyId).ToList(),
                };
                return Json(prod);
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

        [HttpPost("Create")]
        [EnableCors("AllowOrigin")]
        public IActionResult Create([FromForm]string data)
        {
            try
            {
                dynamic add = JsonConvert.DeserializeObject(data);

                AdditionalCharges additionalCharges = add.ToObject<AdditionalCharges>();
                additionalCharges.CreatedDate = DateTime.Now;
                additionalCharges.ModifiedDate = DateTime.Now;
                db.AdditionalCharges.Add(additionalCharges);
                db.SaveChanges();
                var error = new
                {
                    status = 200,
                    msg = "The additionalCharges added successfully"
                };

                return Json(error);
            }
            catch(Exception e)
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


        [HttpGet("Edit")]
        [EnableCors("AllowOrigin")]
        public IActionResult Edit(int Id)
        {
            try
            {
                var additionalCharges = db.AdditionalCharges.Find(Id);
                return Ok(additionalCharges);
            }
            catch(Exception e)
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
        // POST api/<controller>

        // PUT api/<controller>/5
        [HttpPost("Update")]
        public IActionResult Update([FromForm]string data)
        {
            try {
                dynamic Addchrgs = JsonConvert.DeserializeObject(data);
                AdditionalCharges additionalCharges = Addchrgs.ToObject<AdditionalCharges>();
                additionalCharges.CreatedDate = db.AdditionalCharges.Where(x => x.Id == additionalCharges.Id).AsNoTracking().FirstOrDefault().CreatedDate;
                additionalCharges.ModifiedDate = DateTime.Now;
                db.Entry(additionalCharges).State = EntityState.Modified;
                db.SaveChanges();
                var error = new
                {
                    status = "success",
                    data = new
                    {
                        value = 2
                    },
                    msg = "The AdditionalCharges updated successfully"
                };

                return Json(error);           
            }
            catch(Exception e)
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

        [HttpGet("Delete")]
        [EnableCors("AllowOrigin")]
        public IActionResult Delete(int Id)
        {
            try
            {
                var additionalCharges = db.AdditionalCharges.Find(Id);
                db.AdditionalCharges.Remove(additionalCharges);
                db.SaveChanges();
                var error = new
                {
                    status = 200,
                    msg = "The AdditionalCharges deleted successfully"
                };
                return Json(error);
            }
            catch(Exception e)
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

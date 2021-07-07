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
    public class DiscountRuleController : Controller
    {
        private POSDbContext db;
        public DiscountRuleController(POSDbContext contextOptions)
        {
            db = contextOptions;
        }

        // GET: api/<controller>
        [HttpGet("Get")]
        [EnableCors("AllowOrigin")]
        public IActionResult Get(int CompanyId)
        {
            try
            {
                var discountrule = db.DiscountRules.Where(d => d.CompanyId == CompanyId).ToList();
                return Ok(discountrule);
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


        // GET api/<controller>/5
        [HttpGet("{id}")]

        [HttpPost("AddDiscount")]
        [EnableCors("AllowOrigin")]
        public IActionResult AddDiscount([FromForm]string data)
        {
            try
            {
                dynamic disc = JsonConvert.DeserializeObject(data);
                DiscountRule discountRule = disc.ToObject<DiscountRule>();
                discountRule.CreatedDate = DateTime.Now;
                discountRule.ModifiedDate = DateTime.Now;
                db.DiscountRules.Add(discountRule);
                db.SaveChanges();
                var error = new
                {
                    status = 200,
                    msg = "The discountRule added successfully"
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
                var discountRule = db.DiscountRules.Find(Id);
                return Ok(discountRule);
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
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPost("Update")]
        public IActionResult Update([FromForm]string data)
        {
            try
            {
                dynamic disc = JsonConvert.DeserializeObject(data);
                DiscountRule discountRule = disc.ToObject<DiscountRule>();
                discountRule.CreatedDate = db.DiscountRules.Where(x => x.Id == discountRule.Id).AsNoTracking().FirstOrDefault().CreatedDate;
                discountRule.ModifiedDate = DateTime.Now;
                db.Entry(discountRule).State = EntityState.Modified;
                db.SaveChanges();
                var error = new
                {
                    status = 200,
                    msg = "The discountRule updated successfully"
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
                var discountrule = db.DiscountRules.Find(Id);
                db.DiscountRules.Remove(discountrule);
                db.SaveChanges();
                var error = new
                {
                    status = "success",
                    msg = "The DiscountRules deleted successfully"
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

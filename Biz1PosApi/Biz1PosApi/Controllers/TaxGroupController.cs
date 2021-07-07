using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Biz1BookPOS.Controllers
{
    [Route("api/[controller]")]
    public class TaxGroupController : Controller
    {
        private POSDbContext db;
        public TaxGroupController(POSDbContext contextOptions)
        {
            db = contextOptions;
        }

        // GET: api/<controller>
        [HttpGet("Get")]
        public IActionResult Get(int CompanyId)
        {
            try
            {
                var taxgroup = db.TaxGroups.Where(t => t.CompanyId == CompanyId).ToList();
                //var variantGroups = db.Query(table).ToList();
                return Ok(taxgroup);
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
        // [HttpGet("{id}")]

        [HttpPost("AddTax")]
        [EnableCors("AllowOrigin")]
        public IActionResult AddTax([FromForm]string data)
        {
            try
            {
                dynamic tax = JsonConvert.DeserializeObject(data);
                TaxGroup taxGroup = tax.ToObject<TaxGroup>();
                db.TaxGroups.Add(taxGroup);
                db.SaveChanges();
                var error = new
                {
                    status = 200,
                    msg = "The taxGroup added successfully"
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
                var taxgroup = db.TaxGroups.Find(Id);
                return Ok(taxgroup);
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
                dynamic tax = JsonConvert.DeserializeObject(data);
                TaxGroup taxGroup = tax.ToObject<TaxGroup>();
                db.Entry(taxGroup).State = EntityState.Modified;
                db.SaveChanges();
                var error = new
                {
                    status = 200,
                    msg = "The taxGroup updated successfully"
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
                
                var taxgroup = db.TaxGroups.Find(Id);
                db.TaxGroups.Remove(taxgroup);
                db.SaveChanges();
                var error = new
                {
                    status = 200,
                    msg = "TaxGroup deleted successfully"
                };
                return Json(error);
            }
            catch (Exception ex)
            {
                var error = new
                {
                    status = 0,
                    msg = "This tax group can't be deleted as it is combined with product"
                };
                return Json(error);
            }
        }
    }
}

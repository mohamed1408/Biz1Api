using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Biz1BookPOS.Controllers
{
    [Route("api/[controller]")]
    public class AddonGroupController : Controller
    {
        private POSDbContext db;
        public AddonGroupController(POSDbContext contextOptions)
        {
            db = contextOptions;
        }

        // GET: api/<controller>
        [HttpGet("Get")]
        public IActionResult Get()
        {
            var addonGroups = db.AddonGroups.ToList();
            //var variantGroups = db.Query(table).ToList();
            return Ok(addonGroups);
        }
        public IActionResult AddongrpAutocomplete()
        {
            var addonGroups = db.AddonGroups.ToList();
            //var variantGroups = db.Query(table).ToList();
            return Ok(addonGroups);
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        [HttpPost("CreateVariant")]
        public IActionResult CreateVariant([FromBody]AddonGroup addonGroup)
        {
            
            int count = db.AddonGroups.Where(c => c.Description == addonGroup.Description).ToList().Count();

            if (count > 0)
            {
                var error = new
                {
                    status = "error",
                    data = new
                    {
                        value = 1
                    },
                    msg = "The AddonGroup alredy exists"
                };

                return Json(error);
            }
            else
            {
                db.AddonGroups.Add(addonGroup);
                db.SaveChanges();
                var error = new
                {
                    status = "success",
                    data = new
                    {
                        value = 2
                    },
                    msg = "The AddonGroup added successfully"
                };

                return Json(error);
            }
        }

        [HttpGet("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var addongrp = db.AddonGroups.Find(id);
            return Ok(addongrp);
        }
        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPost("Update")]
        public IActionResult Update([FromBody]AddonGroup addongrp)
        {
            db.Entry(addongrp).State = EntityState.Modified;
            db.SaveChanges();
            var error = new
            {
                status = "success",
                data = new
                {
                    value = 2
                },
                msg = "The AddonGroup updated successfully"
            };
            return Json(error);
        }


        [HttpPost("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var addongrp = db.AddonGroups.Find(id);
            db.AddonGroups.Remove(addongrp);
            db.SaveChanges();
            var error = new
            {
                status = "success",
                data = new
                {
                    value = 2
                },
                msg = "The AddonGroup deleted successfully"
            };
            return Json(error);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    public class StatusController : Controller
    {
        private POSDbContext db;
        public StatusController(POSDbContext contextOptions)
        {
            db = contextOptions;
        }
        // GET: api/<controller>
        //[HttpGet("Status")]
        //public IActionResult Status(int id, string name)
        //{
        //    if (name == null)
        //    {
        //        var status = db.DropDowns.Where(p => p.DropDownGroupId == id).ToList();
        //    }
        //    //else
        //    //{
        //    //    id = 
        //    //    var status = db.DropDowns.Where(p => p.DropDownGroupId == id).ToList();
        //    //}
        //    //return Ok(status);
        //}

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Biz1BookPOS.Controllers
{
    [Route("api/[controller]")]
    public class CommonController : Controller
    {
        private POSDbContext db;
        public CommonController(POSDbContext contextOptions)
        {
            db = contextOptions;
        }

        // GET: api/<controller>
        [HttpGet("Get/{table}")]
        public IActionResult Get(string table)
        {
            //var variantGroups = db.VariantGroups.ToList();
            var variantGroups = db.Query(table).ToList();
            return Ok(variantGroups);
        }
        [HttpGet("GetById/{table}/{id}")]
        public IActionResult Edit(int id)
        {
            var variantGroup = db.VariantGroups.Find(id);
            return Ok(variantGroup);
        }
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

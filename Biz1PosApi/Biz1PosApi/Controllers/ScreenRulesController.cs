using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Biz1PosApi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Biz1PosApi.Controllers
{
    public class ScreenRulesController : Controller
    {
        private POSDbContext db;
        public ScreenRulesController(POSDbContext contextOptions)
        {
            db = contextOptions;
        }
        // GET: ScreenRulesController
        [HttpPost("SetRule")]
        [EnableCors("AllowOrigin")]
        public IActionResult SetRule([FromForm]string Rule)
        {
            dynamic rulejson = JsonConvert.DeserializeObject(Rule);
            ScreenRule screenRule = rulejson.ToObject<ScreenRule>();
            db.ScreenRules.Add(screenRule);
            db.SaveChanges();
            return Ok("saved succesfully");
        }
    }
}

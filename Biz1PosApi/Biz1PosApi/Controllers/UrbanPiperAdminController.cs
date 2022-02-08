using Biz1BookPOS.Models;
using Biz1PosApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class UrbanPiperAdminController : Controller
    {
        private static TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        private object table;
        private POSDbContext db;
        public IConfiguration Configuration { get; }
        public UrbanPiperAdminController(POSDbContext contextOptions, IConfiguration configuration)
        {
            db = contextOptions;
            Configuration = configuration;
        }
        [HttpGet("authorizeTest")]
        public ActionResult<IEnumerable<string>> authorizeTest()
        {
            return new string[] { "value1", "value2", "value3", "value4", "value5" };
        }
        [HttpGet("companies")]
        [Authorize]
        public ActionResult companies()
        {
            List<Accounts> companies = db.Accounts.Where(x => x.UPAPIKey.Length > 0 && x.UPUsername.Length > 0).ToList();
            var response = new
            {
                companies,
                status = 200
            };
            return Ok(response);
        }
    }
}

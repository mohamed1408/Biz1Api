using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Biz1PosApi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    public class PaymentTypeController : Controller
    {
        private POSDbContext db;
        private static TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        public IConfiguration Configuration { get; }
        public PaymentTypeController(POSDbContext contextOptions, IConfiguration configuration)
        {
            db = contextOptions;
            Configuration = configuration;
        }
        
        // GET: PaymentTypeController
        [HttpGet("getpaymenttypes")]
        public IActionResult getpaymenttypes(int companyid)
        {
            List<StorePaymentType> storePaymentTypes = db.StorePaymentTypes.Where(x => x.CompanyId == companyid).Include(x => x.Store).ToList();
            return Ok(storePaymentTypes);
        }
    }
}

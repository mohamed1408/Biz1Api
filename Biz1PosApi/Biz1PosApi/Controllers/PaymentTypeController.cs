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
using Newtonsoft.Json.Linq;

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
        public IActionResult getpaymenttypes(int companyid, int storeid)
        {
            List<StorePaymentType> storePaymentTypes = db.StorePaymentTypes.Where(x => (x.CompanyId == companyid || companyid == 0) && (x.StoreId == storeid || storeid == 0 || companyid == 0) && x.IsActive == true && x.Hidden == false).Include(x => x.Store).ToList();
            return Ok(storePaymentTypes);
        }
        [HttpPost("addPaymentType")]
        public IActionResult addPaymentType([FromBody] StorePaymentType paymentType)
        {
            try
            {
                db.StorePaymentTypes.Add(paymentType);
                db.SaveChanges();
                var resp = new
                {
                    status = 200
                };
                return Ok(resp);
            }
            catch (Exception e)
            {
                var resp = new
                {
                    status = 0,
                    msg = new Exception(e.Message, e.InnerException)
                };
                return Ok(resp);
            }
        }
        [HttpPost("addPaymentTypeMultiStores")]
        public IActionResult addPaymentTypeMultiStores([FromBody] dynamic payload)
        {
            try
            {
                int[] stores = payload.stores.ToObject<int[]>();
                int companyid = payload.CompanyId.ToObject<int>();
                if (stores[0] == 0)
                {
                    stores = db.Stores.Where(x => x.CompanyId == companyid).Select(x => x.Id).ToArray();
                }
                foreach (int storeid in stores)
                {
                    StorePaymentType storePaymentType = payload.paymentType.ToObject<StorePaymentType>();
                    storePaymentType.StoreId = storeid;
                    db.StorePaymentTypes.Add(storePaymentType);
                }
                db.SaveChanges();
                var resp = new
                {
                    status = 200
                };
                return Ok(resp);
            }
            catch (Exception e)
            {
                var resp = new
                {
                    status = 0,
                    msg = new Exception(e.Message, e.InnerException)
                };
                return Ok(resp);
            }
        }

        [HttpPost("updatePaymentType")]
        public IActionResult updatePaymentType([FromBody] StorePaymentType paymentType)
        {
            try
            {
                db.Entry(paymentType).State = EntityState.Modified;
                db.SaveChanges();
                var resp = new
                {
                    status = 200
                };
                return Ok(resp);
            }
            catch (Exception e)
            {
                var resp = new
                {
                    status = 0,
                    msg = new Exception(e.Message, e.InnerException)
                };
                return Ok(resp);
            }
        }
        [HttpPost("deletePaymentType")]
        public IActionResult deletePaymentType(int Id)
        {
            try
            {
                db.StorePaymentTypes.Remove(db.StorePaymentTypes.Find(Id));
                db.SaveChanges();
                var resp = new
                {
                    status = 200
                };
                return Ok(resp);
            }
            catch (Exception e)
            {
                var resp = new
                {
                    status = 0,
                    msg = new Exception(e.Message, e.InnerException)
                };
                return Ok(resp);
            }
        }
    }
}

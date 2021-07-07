using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Biz1PosApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    public class ShiftSummaryController : Controller
    {
        private POSDbContext db;
        public IConfiguration Configuration { get; }
        public ShiftSummaryController(POSDbContext contextOptions, IConfiguration configuration)
        {
            db = contextOptions;
            Configuration = configuration;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet("Post")]
        public IActionResult Post(int compId, int storeId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.ShiftSummary", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@compId", compId));
                cmd.Parameters.Add(new SqlParameter("@storeId", storeId));
                cmd.Parameters.Add(new SqlParameter("@fromDate", fromDate));
                cmd.Parameters.Add(new SqlParameter("@toDate", toDate));

                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                DataTable table = ds.Tables[0];
                var shiftData = new
                {
                    cash = ds.Tables[0],
                    totExpense = ds.Tables[1]
                };
                sqlCon.Close();
                return Json(shiftData);
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

        [HttpPost("AddShift")]
        public IActionResult AddShift([FromForm]string data, DateTime OpeningTime)
        {
            try
            {
                dynamic shiftJson = JsonConvert.DeserializeObject(data);
                ShiftSummary shiftSummary = shiftJson.ToObject<ShiftSummary>();
                shiftSummary.ShiftStartTime = OpeningTime;
                db.ShiftSummaries.Add(shiftSummary);
                db.SaveChanges();
                return Json(shiftSummary);
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
        [HttpGet("EditShift")]
        public IActionResult EditShift(int Id,int ClosingBalanace,DateTime ClosingTime)
        {
            var shift = db.ShiftSummaries.Find(Id);
            shift.ClosingBalance = ClosingBalanace;
            shift.ShiftEndTime = ClosingTime;
            shift.ClosingBalance = ClosingBalanace;
            db.Entry(shift).State = EntityState.Modified;
            db.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

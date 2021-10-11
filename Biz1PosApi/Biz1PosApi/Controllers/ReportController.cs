using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Biz1PosApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        private POSDbContext db;
        public IConfiguration Configuration { get; }

        public ReportController(POSDbContext contextOptions, IConfiguration configuration)
        {
            db = contextOptions;
            Configuration = configuration;
        }
        // GET: ReportController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ReportController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ReportController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReportController/Create
        [HttpPost("OptionReport")]
        public IActionResult OptionReport([FromBody] JObject payload)
        {
            try
            {
                dynamic param = payload;
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.OptionReport", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@optionGroupId", param.optionGroupId.ToObject<Int64>()));
                cmd.Parameters.Add(new SqlParameter("@optionId", param.optionId.ToObject<Int64>()));
                cmd.Parameters.Add(new SqlParameter("@productId", param.productId.ToObject<Int64>()));
                cmd.Parameters.Add(new SqlParameter("@storeId", param.storeId.ToObject<Int64>()));
                cmd.Parameters.Add(new SqlParameter("@sourceId", param.sourceId.ToObject<Int64>()));
                cmd.Parameters.Add(new SqlParameter("@fromDate", param.fromDate.ToString()));
                cmd.Parameters.Add(new SqlParameter("@todate", param.todate.ToString()));
                cmd.Parameters.Add(new SqlParameter("@companyId", param.companyId.ToObject<Int64>()));
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                var response = new
                {
                    status = 200,
                    report = ds.Tables[0],
                };
                sqlCon.Close();
                return Ok(response);
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
        [HttpGet("OrderTypeReport")]
        public IActionResult OrderTypeReport(DateTime? fromdate, DateTime? todate, int companyid)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.OrderTypeSalesReport", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@fromdate", fromdate));
                cmd.Parameters.Add(new SqlParameter("@todate", todate));
                cmd.Parameters.Add(new SqlParameter("@companyId", companyid));
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                var response = new
                {
                    status = 200,
                    report = ds.Tables[0],
                };
                sqlCon.Close();
                return Ok(response);
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
        [HttpGet("DeliveryOrderReport")]
        public IActionResult DeliveryOrderReport(int storeid, int companyid, DateTime? fromdate, DateTime? todate)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.DeliveryOrderReport", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@fromDate", fromdate));
                cmd.Parameters.Add(new SqlParameter("@toDate", todate));
                cmd.Parameters.Add(new SqlParameter("@storeId", storeid));
                cmd.Parameters.Add(new SqlParameter("@companyId", companyid));
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                var response = new
                {
                    status = 200,
                    report = ds.Tables[0],
                };
                sqlCon.Close();
                return Ok(response);
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
        [HttpGet("OrderTypeReport")]
        public IActionResult OrderTypeReport(int storeid, int companyid, DateTime? fromdate, DateTime? todate, int ordertypeid)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.OrderTypeReport", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@fromDate", fromdate));
                cmd.Parameters.Add(new SqlParameter("@toDate", todate));
                cmd.Parameters.Add(new SqlParameter("@storeId", storeid));
                cmd.Parameters.Add(new SqlParameter("@companyId", companyid));
                cmd.Parameters.Add(new SqlParameter("@ordertypeid", ordertypeid));
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                var response = new
                {
                    status = 200,
                    report = ds.Tables[0],
                };
                sqlCon.Close();
                return Ok(response);
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

        [HttpPost("add_report_preset")]
        public IActionResult add_report_preset([FromBody]ReportPreset preset)
        {
            try
            {
                db.ReportPresets.Add(preset);
                db.SaveChanges();
                var response = new
                {
                    status = 200,
                    message = "success",
                };
                return Ok(response);
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

        [HttpGet("delete_report_preset")]
        public IActionResult delete_report_preset(int presetid)
        {
            try
            {
                db.ReportPresets.Remove(db.ReportPresets.Find(presetid));
                db.SaveChanges();
                var response = new
                {
                    status = 200,
                    message = "success",
                };
                return Ok(response);
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

        // GET: ReportController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ReportController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ReportController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReportController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

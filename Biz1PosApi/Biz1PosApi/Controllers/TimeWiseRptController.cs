using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    public class TimeWiseRptController : Controller
    {
        private POSDbContext db;
        public IConfiguration Configuration { get; }
        public TimeWiseRptController(POSDbContext contextOptions, IConfiguration configuration)
        {
            db = contextOptions;
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetRpt")]
        public IActionResult GetRpt(DateTime frmdate, DateTime todate, TimeSpan fromTime, TimeSpan toTime, int interval, int storeId, int sourceId, int productId, int categoryId, int saleproductgroupid, int companyid)
        {
            try
            {
                //SqlConnection sqlCon = new SqlConnection("server=(LocalDb)\\MSSQLLocalDB; database=Biz1POS;Trusted_Connection=True;");
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.TimeWise", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@fromdate", frmdate));
                cmd.Parameters.Add(new SqlParameter("@todate", todate));
                cmd.Parameters.Add(new SqlParameter("@fromTime", fromTime));
                cmd.Parameters.Add(new SqlParameter("@toTime", toTime));
                cmd.Parameters.Add(new SqlParameter("@interval", interval));
                cmd.Parameters.Add(new SqlParameter("@storeId", storeId));
                cmd.Parameters.Add(new SqlParameter("@sourceId", sourceId));
                cmd.Parameters.Add(new SqlParameter("@productId", productId));
                cmd.Parameters.Add(new SqlParameter("@categoryId", categoryId));
                cmd.Parameters.Add(new SqlParameter("@saleproductgroupid", saleproductgroupid));
                cmd.Parameters.Add(new SqlParameter("@companyid", companyid));
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);

                DataTable table = ds.Tables[0];
                //string jsonStr = "";
                //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{
                //    jsonStr += ds.Tables[0].Rows[i].ItemArray[0].ToString();
                //}
                //var orderObj = JsonConvert.DeserializeObject(jsonStr);
                var obj = new
                {
                    status = "ok",
                    //data = orderObj,
                    Order = ds.Tables[0],
                    //transactions = ds.Tables[1],
                    //msg = ""
                };
                sqlCon.Close();
                return Ok(obj);

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
        [HttpGet("GetReportProducts")]
        public IActionResult GetReportProducts(DateTime frmdate, DateTime todate, TimeSpan fromTime, TimeSpan toTime, int storeId, int sourceId, int saleproductgroupid, int companyid, int productId)
        {
            try
            {
                //SqlConnection sqlCon = new SqlConnection("server=(LocalDb)\\MSSQLLocalDB; database=Biz1POS;Trusted_Connection=True;");
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.TimeWiseReportProducts", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@fromdate", frmdate));
                cmd.Parameters.Add(new SqlParameter("@todate", todate));
                cmd.Parameters.Add(new SqlParameter("@fromTime", fromTime));
                cmd.Parameters.Add(new SqlParameter("@toTime", toTime));
                cmd.Parameters.Add(new SqlParameter("@storeId", storeId));
                cmd.Parameters.Add(new SqlParameter("@sourceId", sourceId));
                cmd.Parameters.Add(new SqlParameter("@productId", productId));
                cmd.Parameters.Add(new SqlParameter("@saleproductgroupid", saleproductgroupid));
                cmd.Parameters.Add(new SqlParameter("@companyid", companyid));
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);

                DataTable table = ds.Tables[0];
                //string jsonStr = "";
                //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{
                //    jsonStr += ds.Tables[0].Rows[i].ItemArray[0].ToString();
                //}
                //var orderObj = JsonConvert.DeserializeObject(jsonStr);
                var obj = new
                {
                    status = "ok",
                    //data = orderObj,
                    products = ds.Tables[0],
                    //transactions = ds.Tables[1],
                    //msg = ""
                };
                sqlCon.Close();
                return Ok(obj);

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

        [HttpGet("GetPrdRpt")]
        public IActionResult GetPrdRpt(DateTime frmdate, DateTime todate, TimeSpan fromTime, TimeSpan toTime, int storeId, int sourceId, int categoryId)
        {
            try
            {
                //SqlConnection sqlCon = new SqlConnection("server=(LocalDb)\\MSSQLLocalDB; database=Biz1POS;Trusted_Connection=True;");
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.TimeWisePrd", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@fromdate", frmdate));
                cmd.Parameters.Add(new SqlParameter("@todate", todate));
                cmd.Parameters.Add(new SqlParameter("@fromTime", fromTime));
                cmd.Parameters.Add(new SqlParameter("@toTime", toTime));
                cmd.Parameters.Add(new SqlParameter("@storeId", storeId));
                cmd.Parameters.Add(new SqlParameter("@sourceId", sourceId));
                cmd.Parameters.Add(new SqlParameter("@categoryId", categoryId));
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);

                DataTable table = ds.Tables[0];
                string jsonStr = "";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    jsonStr += ds.Tables[0].Rows[i].ItemArray[0].ToString();
                }
                var orderObj = JsonConvert.DeserializeObject(jsonStr);
                var obj = new
                {
                    status = "ok",
                    data = orderObj,
                    Order = ds.Tables[0],
                    //transactions = ds.Tables[1],
                    //msg = ""
                };
                sqlCon.Close();
                return Ok(obj);

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




    }
}
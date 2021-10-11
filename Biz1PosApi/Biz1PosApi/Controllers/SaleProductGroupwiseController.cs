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

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    public class SaleProductGroupwiseController : Controller
    {
        private POSDbContext db;
        public IConfiguration Configuration { get; }
        public SaleProductGroupwiseController(POSDbContext contextOptions, IConfiguration configuration)
        {
            db = contextOptions;
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("GetRpt")]
        public IActionResult GetRpt(DateTime frmdate, DateTime todate, int Id, int compId, int sourceId, int saleProdId)
        {
            try
            {
                //SqlConnection sqlCon = new SqlConnection("server=(LocalDb)\\MSSQLLocalDB; database=Biz1POS;Trusted_Connection=True;");
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.SaleProdGrpWiseRpt", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@fromdate", frmdate));
                cmd.Parameters.Add(new SqlParameter("@todate", todate));
                cmd.Parameters.Add(new SqlParameter("@storeId", Id));
                cmd.Parameters.Add(new SqlParameter("@companyId", compId));
                cmd.Parameters.Add(new SqlParameter("@sourceId", sourceId));
                cmd.Parameters.Add(new SqlParameter("@type", 1));
                cmd.Parameters.Add(new SqlParameter("@saleProductId", saleProdId));

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
        [HttpGet("GetStockPrdwise")]
        public IActionResult GetStockPrdwise(DateTime frmdate, DateTime todate, int saleProdId, int compId, int sourceId, int storeId, int type)
        {
            try
            {
                //SqlConnection sqlCon = new SqlConnection("server=(LocalDb)\\MSSQLLocalDB; database=Biz1POS;Trusted_Connection=True;");
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.SaleProdGrpWiseRpt", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@fromdate", frmdate));
                cmd.Parameters.Add(new SqlParameter("@todate", todate));
                cmd.Parameters.Add(new SqlParameter("@saleProductId", saleProdId));
                cmd.Parameters.Add(new SqlParameter("@companyId", compId));
                cmd.Parameters.Add(new SqlParameter("@sourceId", sourceId));
                cmd.Parameters.Add(new SqlParameter("@storeId", storeId));
                cmd.Parameters.Add(new SqlParameter("@type", type));
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
                var saleProdGrp = db.SaleProductGroups.Where(s => s.SaleProductId == saleProdId && s.CompanyId == compId)
                    .Select(s => new { s.SaleProductId, s.StockProductId, StockProduct = s.StockProduct.Description }).ToList();

                var obj = new
                {
                    status = "ok",
                    data = orderObj,
                    Order = ds.Tables[0],
                    saleProdGrp = saleProdGrp
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
        [HttpGet("GetCategorywise")]
        public IActionResult GetCategorywise(DateTime frmdate, DateTime todate, int saleProdId, int compId, int sourceId, int storeId)
        {
            try
            {
                //SqlConnection sqlCon = new SqlConnection("server=(LocalDb)\\MSSQLLocalDB; database=Biz1POS;Trusted_Connection=True;");
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.SaleProdGrpWiseRpt1", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@fromdate", frmdate));
                cmd.Parameters.Add(new SqlParameter("@todate", todate));
                cmd.Parameters.Add(new SqlParameter("@saleProdId", saleProdId));
                cmd.Parameters.Add(new SqlParameter("@companyId", compId));
                cmd.Parameters.Add(new SqlParameter("@sourceId", sourceId));
                cmd.Parameters.Add(new SqlParameter("@storeId", storeId));
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
                    data = table,
                    // Order = ds.Tables[0],
                    // saleProdGrp = saleProdGrp
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
        //[HttpGet("PrdwiseRpt")]
        //public IActionResult PrdwiseRpt(int Id, DateTime frmdate, DateTime todate, int companyId, int sourceId,int catId)
        //{
        //    try
        //    {
        //        //SqlConnection sqlCon = new SqlConnection("server=(LocalDb)\\MSSQLLocalDB; database=Biz1POS;Trusted_Connection=True;");
        //        SqlConnection sqlCon = new SqlConnection("Data Source=tcp:biz1server.database.windows.net,1433;Initial Catalog=biz1pos;User Id=dbadmin@biz1server;Password=B1zd0m##");
        //        sqlCon.Open();
        //        SqlCommand cmd = new SqlCommand("dbo.SaleProdGrpWiseRpt1", sqlCon);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add(new SqlParameter("@storeId", Id));
        //        cmd.Parameters.Add(new SqlParameter("@fromdate", frmdate));
        //        cmd.Parameters.Add(new SqlParameter("@todate", todate));
        //        cmd.Parameters.Add(new SqlParameter("@companyId", companyId));
        //        cmd.Parameters.Add(new SqlParameter("@sourceId", sourceId));
        //        cmd.Parameters.Add(new SqlParameter("@categoryId", catId));
        //        DataSet ds = new DataSet();
        //        SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
        //        sqlAdp.Fill(ds);

        //        DataTable table = ds.Tables[0];
        //        string jsonStr = "";
        //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //        {
        //            jsonStr += ds.Tables[0].Rows[i].ItemArray[0].ToString();
        //        }
        //        var orderObj = JsonConvert.DeserializeObject(jsonStr);
        //        var obj = new
        //        {
        //            status = "ok",
        //            data = orderObj,
        //            Order = ds.Tables[0],
        //            //transactions = ds.Tables[1],
        //            //msg = ""
        //        };
        //        sqlCon.Close();
        //        return Ok(obj);

        //    }
        //    catch (Exception e)
        //    {
        //        var error = new
        //        {
        //            error = new Exception(e.Message, e.InnerException),
        //            status = 0,
        //            msg = "Something went wrong  Contact our service provider"
        //        };
        //        return Json(error);
        //    }
        //}
    }
}
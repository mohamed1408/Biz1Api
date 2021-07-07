using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;


namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    public class StorewiseRptController : Controller
    {
        private POSDbContext db;
        public IConfiguration Configuration { get; }
        public StorewiseRptController(POSDbContext contextOptions, IConfiguration configuration)
        {
            db = contextOptions;
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetStoreRpt")]
        [EnableCors("AllowOrigin")]
        public IActionResult GetStoreRpt(DateTime frmdate, DateTime todate, int categoryId, int companyId, int sourceId, int productId, int tagId)
        {
            try
            {
                //SqlConnection sqlCon = new SqlConnection("server=(LocalDb)\\MSSQLLocalDB; database=Biz1POS;Trusted_Connection=True;");
                SqlConnection sqlCon = new SqlConnection("Data Source=tcp:biz1server.database.windows.net,1433;Initial Catalog=biz1pos;User Id=dbadmin@biz1server;Password=B1zd0m##");
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.StoreWisePrdRpt", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@fromDate", frmdate));
                cmd.Parameters.Add(new SqlParameter("@toDate", todate));
                cmd.Parameters.Add(new SqlParameter("@categoryId", categoryId));
                cmd.Parameters.Add(new SqlParameter("@companyId", companyId));
                cmd.Parameters.Add(new SqlParameter("@sourceId", sourceId));
                cmd.Parameters.Add(new SqlParameter("@productId", productId));
                cmd.Parameters.Add(new SqlParameter("@tagId", tagId));

                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                DataTable table = ds.Tables[0];

                var data = new
                {
                    Order = ds.Tables[0]
                };
                return Ok(data);
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

        [HttpGet("GetProduct")]
        public IActionResult GetProduct(int CompanyId)
        {
            try
            {
                var product = db.Products.Where(c => c.CompanyId == CompanyId).ToList();
                return Ok(product);
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
        [HttpGet("StorePrd")]
        [EnableCors("AllowOrigin")]
        public IActionResult StorePrd(int Id, DateTime frmdate, DateTime todate, int companyId, int sourceId, int categoryId, int tagId)
        {
            try
            {
                //SqlConnection sqlCon = new SqlConnection("server=(LocalDb)\\MSSQLLocalDB; database=Biz1POS;Trusted_Connection=True;");
                SqlConnection sqlCon = new SqlConnection("Data Source=tcp:biz1server.database.windows.net,1433;Initial Catalog=biz1pos;User Id=dbadmin@biz1server;Password=B1zd0m##");
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.ProductWiseSalesRpt", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@storeId", Id));
                cmd.Parameters.Add(new SqlParameter("@fromdate", frmdate));
                cmd.Parameters.Add(new SqlParameter("@todate", todate));
                cmd.Parameters.Add(new SqlParameter("@companyId", companyId));
                cmd.Parameters.Add(new SqlParameter("@sourceId", sourceId));
                cmd.Parameters.Add(new SqlParameter("@categoryId", categoryId));
                cmd.Parameters.Add(new SqlParameter("@tagId", tagId));

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
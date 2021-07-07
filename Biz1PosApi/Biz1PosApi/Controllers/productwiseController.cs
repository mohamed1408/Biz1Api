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
    public class productwiseController : Controller
    {
        private POSDbContext db;
        public IConfiguration Configuration { get; }
        public productwiseController(POSDbContext contextOptions, IConfiguration configuration)
        {
            db = contextOptions;
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("GetRpt")]
        public IActionResult GetRpt(DateTime frmdate, DateTime todate, int Id, int compId, int categoryId, int sourceId, int tagId, int datatype)
        {
            try
            {
                //SqlConnection sqlCon = new SqlConnection("server=(LocalDb)\\MSSQLLocalDB; database=Biz1POS;Trusted_Connection=True;");
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.ProductWiseSalesRpt", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@fromdate", frmdate));
                cmd.Parameters.Add(new SqlParameter("@todate", todate));
                cmd.Parameters.Add(new SqlParameter("@storeId", Id));
                cmd.Parameters.Add(new SqlParameter("@companyId", compId));
                cmd.Parameters.Add(new SqlParameter("@categoryId", categoryId));
                cmd.Parameters.Add(new SqlParameter("@sourceId", sourceId));
                cmd.Parameters.Add(new SqlParameter("@tagId", tagId));
                cmd.Parameters.Add(new SqlParameter("@datatype", datatype));

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
                    data = orderObj
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
        [HttpGet("GetStore")]
        public IActionResult GetStore()
        {
            try
            {
                var stores = db.Stores.ToList();
                return Ok(stores);
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

        [HttpGet("Prd")]
        public IActionResult Prd()
        {
            try
            {
                var order = db.Orders.ToList();
                for (int i = 0; i < order.Count(); i++)
                {
                    List<OrderItem> orderItems = db.OrderItems.Where(o => o.OrderId == order[i].Id).AsNoTracking().ToList();
                    for (int j = 0; j < orderItems.Count(); j++)
                    {
                        orderItems[j].OrdItemAddons = db.OrdItemAddons.Where(oa => oa.OrderItemId == orderItems[j].Id).ToList();
                        orderItems[j].OrdItemVariants = db.OrdItemVariants.Where(oa => oa.OrderItemId == orderItems[j].Id).ToList();
                    }
                    order[i].OrderItems = orderItems;
                }
                return Ok(order);
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
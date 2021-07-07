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
using Microsoft.AspNetCore.Cors;

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    public class StoreRptController : Controller
    {
        private POSDbContext db;
        public IConfiguration Configuration { get; }
        public StoreRptController(POSDbContext contextOptions, IConfiguration configuration)
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
        public IActionResult GetStoreRpt(DateTime frmdate, DateTime todate, int companyId)
        {
            try
            {
                //SqlConnection sqlCon = new SqlConnection("server=(LocalDb)\\MSSQLLocalDB; database=Biz1POS;Trusted_Connection=True;");
                SqlConnection sqlCon = new SqlConnection("Data Source=tcp:biz1server.database.windows.net,1433;Initial Catalog=biz1pos;User Id=dbadmin@biz1server;Password=B1zd0m##");
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.StoreWiseRpt", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@fromDate", frmdate));
                cmd.Parameters.Add(new SqlParameter("@toDate", todate));
                cmd.Parameters.Add(new SqlParameter("@companyId", companyId));
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


    }
}

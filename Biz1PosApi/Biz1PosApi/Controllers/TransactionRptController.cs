using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    public class TransactionRptController : Controller
    {
        private POSDbContext db;

        public IConfiguration Configuration { get; }
        public TransactionRptController(POSDbContext contextOptions, IConfiguration configuration)
        {
            db = contextOptions;
            Configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("GetRpt")]
        public IActionResult GetRpt(DateTime frmdate, DateTime todate, int Id, int compId, int sourceId, int paymentId)
        {
            try
            {
                //SqlConnection sqlCon = new SqlConnection("server=(LocalDb)\\MSSQLLocalDB; database=Biz1POS;Trusted_Connection=True;");
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.TransactionRpt", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@fromDate", frmdate));
                cmd.Parameters.Add(new SqlParameter("@toDate", todate));
                cmd.Parameters.Add(new SqlParameter("@storeId", Id));
                cmd.Parameters.Add(new SqlParameter("@companyId", compId));
                cmd.Parameters.Add(new SqlParameter("@sourceId", sourceId));
                cmd.Parameters.Add(new SqlParameter("@paymenttype", paymentId));

                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                DataTable table = ds.Tables[0];
                var data = new
                {
                    Order = ds.Tables[0]
                };
                sqlCon.Close();
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

        [HttpGet("GetTransactions")]
        public IActionResult GetTransactions(DateTime frmdate, DateTime todate, int storeId)
        {
            try
            {
                var transactions = db.Transactions.Where(x => x.TransDate >= frmdate && x.TransDate <= todate && x.StoreId == storeId).Include(x => x.PaymentType).ToList();
                return Ok(transactions);
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

        [HttpGet("Payment")]
        [EnableCors("AllowOrigin")]
        public IActionResult Payment()
        {
            try
            {
                var prod = new
                {
                    paymenttype = db.PaymentTypes.ToList(),
                };
                return Json(prod);
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


        [HttpGet("GetRprtt")]
        public IActionResult GetRprtt(DateTime frmdate, DateTime todate, int Id, int compId, int sourceId)
        {
            try
            {
                //SqlConnection sqlCon = new SqlConnection("server=(LocalDb)\\MSSQLLocalDB; database=Biz1POS;Trusted_Connection=True;");
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.OrdWiseSalesRpt", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@fromDate", frmdate));
                cmd.Parameters.Add(new SqlParameter("@toDate", todate));
                cmd.Parameters.Add(new SqlParameter("@storeId", Id));
                cmd.Parameters.Add(new SqlParameter("@companyId", compId));
                cmd.Parameters.Add(new SqlParameter("@sourceId", sourceId));
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                DataTable table = ds.Tables[0];
                DataTable table1 = ds.Tables[1];
                DataTable table2 = ds.Tables[2];
                DataTable table3 = ds.Tables[3];
                var data = new
                {
                    Order = ds.Tables[0],
                    order1 = ds.Tables[1],
                    order2 = ds.Tables[2],
                    order3 = ds.Tables[3]

                };
                sqlCon.Close();
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
        [HttpGet("gettransactionsbyinvoice")]
        public IActionResult gettransactionsbyinvoice(string invoiceno)
        {
            try
            {
                int orderid = 0;
                int status = 0;
                string message = "Invoice not found!";
                List<Transaction> transactions = new List<Transaction>();
                Order order = new Order();
                if(db.Orders.Where(x => x.InvoiceNo == invoiceno).Any())
                {
                    orderid = db.Orders.Where(x => x.InvoiceNo == invoiceno).FirstOrDefault().Id;
                    transactions = db.Transactions.Where(x => x.OrderId == orderid).Include(x => x.User).ToList();
                    order = db.Orders.Where(x => x.InvoiceNo == invoiceno).Include(x => x.Customer).Include(x => x.User).FirstOrDefault();
                    status = 200;
                    message = "Invoice Found!";
                }
                var response = new
                {
                    status = status,
                    message = message,
                    transactions = transactions,
                    order = order
                };
                return Ok(response);
            }
            catch (Exception e)
            {
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 500,
                    message = "Something went wrong  Contact our service provider"
                };
                return Json(error);
            }
        }
    }
}
   


   


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Biz1PosApi.Models;
using Biz1PosApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    public class ReceiptController : Controller
    {
        //private POSDbContext db;
        private TempDbContext db;
        private ConnectionStringService connserve;
        public IConfiguration Configuration { get; }
        public ReceiptController(POSDbContext contextOptions, IConfiguration configuration, ConnectionStringService _connserve)
        {
            db = DbContextFactory.Create("myconn");
            Configuration = configuration;
            connserve = _connserve;
        }

        // GET: api/<controller>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<controller>/5
        [HttpGet("Invoices")]
        public IActionResult Invoices(int companyid, int storeid, DateTime fromdate, DateTime todate, int startid, int endid, string invoice)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();

                SqlCommand cmd = new SqlCommand("dbo.Invoices", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@comapnyid", companyid));
                cmd.Parameters.Add(new SqlParameter("@storeid", storeid));
                cmd.Parameters.Add(new SqlParameter("@fromdate", fromdate));
                cmd.Parameters.Add(new SqlParameter("@todate", todate));
                cmd.Parameters.Add(new SqlParameter("@startid", startid));
                cmd.Parameters.Add(new SqlParameter("@endid", endid));
                cmd.Parameters.Add(new SqlParameter("@invoice", invoice));
                cmd.Parameters.Add(new SqlParameter("@top", 25));

                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);

                DataTable table = ds.Tables[0];
                var data = new
                {
                    invoices = ds.Tables[0],
                    tot_sales = ds.Tables[1],
                    message = "success",
                    status = 200
                };
                sqlCon.Close();
                return Ok(data);
            }
            catch (Exception e)
            {
                var error = new
                {
                    invoices = Array.Empty<JObject>(),
                    error = new Exception(e.Message, e.InnerException),
                    status = 0,
                    msg = "Something went wrong  Contact our service provider"
                };
                return Json(error);
            }
        }
        [HttpGet("Invoices2")]
        public IActionResult Invoices2(int companyid, int storeid, DateTime fromdate, DateTime todate, int startid, int endid, string invoice)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();

                SqlCommand cmd = new SqlCommand("dbo.Invoices2", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@comapnyid", companyid));
                cmd.Parameters.Add(new SqlParameter("@storeid", storeid));
                cmd.Parameters.Add(new SqlParameter("@fromdate", fromdate));
                cmd.Parameters.Add(new SqlParameter("@todate", todate));
                cmd.Parameters.Add(new SqlParameter("@startid", startid));
                cmd.Parameters.Add(new SqlParameter("@endid", endid));
                cmd.Parameters.Add(new SqlParameter("@invoice", invoice));
                cmd.Parameters.Add(new SqlParameter("@top", 25));

                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);

                DataTable table = ds.Tables[0];
                var data = new
                {
                    invoices = ds.Tables[0],
                    tot_sales = ds.Tables[1],
                    message = "success",
                    status = 200
                };
                sqlCon.Close();
                return Ok(data);
            }
            catch (Exception e)
            {
                var error = new
                {
                    invoices = Array.Empty<JObject>(),
                    error = new Exception(e.Message, e.InnerException),
                    status = 0,
                    msg = "Something went wrong  Contact our service provider"
                };
                return Json(error);
            }
        }
        [HttpGet("GetOrderJson")]
        public IActionResult GetOrderJson(int orderid)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();

                SqlCommand cmd = new SqlCommand("dbo.GetOrderJson", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@orderid", orderid));

                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);

                DataTable table = ds.Tables[0];
                var data = new
                {
                    invoices = ds.Tables[0],
                    message = "success",
                    status = 200
                };
                sqlCon.Close();
                return Ok(data);
            }
            catch (Exception e)
            {
                var error = new
                {
                    invoices = Array.Empty<JObject>(),
                    error = new Exception(e.Message, e.InnerException),
                    status = 0,
                    msg = "Something went wrong  Contact our service provider"
                };
                return Json(error);
            }
        }

        [HttpGet("Get")]
        public IActionResult Get(int StoreId, int CompanyId, int StartId, string type, string dataType, DateTime? fromdate, DateTime? todate, string invoice)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();

                SqlCommand cmd = new SqlCommand("dbo.Receipt", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@compId", CompanyId));
                cmd.Parameters.Add(new SqlParameter("@storeId", StoreId));
                cmd.Parameters.Add(new SqlParameter("@startId", StartId));
                cmd.Parameters.Add(new SqlParameter("@type", type));
                cmd.Parameters.Add(new SqlParameter("@data", dataType));
                cmd.Parameters.Add(new SqlParameter("@fromDate", fromdate));
                cmd.Parameters.Add(new SqlParameter("@todate", todate));
                cmd.Parameters.Add(new SqlParameter("@invoice", invoice));

                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);

                DataTable table = ds.Tables[0];
                var data = new
                {
                    receipts = ds.Tables[0],
                    orderItems = ds.Tables[1],
                    // Options = ds.Tables[2],
                    AdditionalCharges = ds.Tables[2],
                    KOTs = ds.Tables[3],
                    Transaction = ds.Tables[4],
                    FirstOrderId = ds.Tables[5],
                    LastOrderId = ds.Tables[6],
                    TotalPayments = ds.Tables[7],
                    PaymentType = db.PaymentTypes.ToList(),
                    Customers = db.Customers.ToList(),
                    TransactionPayments = ds.Tables[8]
                };
                sqlCon.Close();
                return Ok(data);
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
        [HttpGet("spt")]
        public IActionResult spt(int storeid)
        {
            try
            {
                var data = new
                {
                    paymenttypes = db.StorePaymentTypes.Where(x => x.StoreId == storeid && x.IsActive == true).ToList()
                };
                return Ok(data);
            }
            catch (Exception e)
            {
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 0,
                    msg = "Something went wrong  Contact our service provider",
                    paymenttypes = new List<StorePaymentType>()
                };
                return Json(error);
            }
        }
        [HttpGet("last10transactions")]
        public IActionResult storepaymentsbytype(int storeid, int companyid, string invoiceno)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();

                SqlCommand cmd = new SqlCommand("dbo.last10transactions", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@storeid", storeid));
                cmd.Parameters.Add(new SqlParameter("@companyid", companyid));
                cmd.Parameters.Add(new SqlParameter("@invoiceno", invoiceno));

                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);

                DataTable table = ds.Tables[0];
                var data = new
                {
                    transactions = ds.Tables[0],
                    paymenttypes = db.StorePaymentTypes.Where(x => x.StoreId == storeid).ToList()
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
        [HttpPost("saveTransaction")]
        public IActionResult saveTransaction([FromBody]dynamic data)
        {
            try
            {
                int? orderid = 0;
                Transaction oldTransaction = new Transaction();
                double difference = 0;
                data.PaymentType = null;
                Transaction transaction = data.ToObject<Transaction>();
                oldTransaction = db.Transactions.Where(x => x.TransactionId == transaction.TransactionId).AsNoTracking().FirstOrDefault();
                orderid = transaction.OrderId;
                db.Entry(transaction).State = EntityState.Modified;
                difference = oldTransaction.Amount - transaction.Amount;
                if (orderid != null && orderid > 0 && difference != 0)
                {
                    Odrs order = db.Odrs.Find(orderid);
                    order.pa = order.pa - difference;
                    if(order.oj != null)
                    {
                        dynamic payload = JsonConvert.DeserializeObject(order.oj);
                        payload.PaidAmount = order.pa;
                        order.oj = JsonConvert.SerializeObject(payload);
                    }
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                }
                db.SaveChanges();
                var response = new
                {
                    status = 200,
                    message = "Success"
                };
                return Json(response);
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
        [HttpPost("updateTransactions")]
        public IActionResult updateTransactions([FromBody]dynamic data)
        {
            try
            {
                List<Transaction> transactions = data.ToObject<List<Transaction>>();
                foreach(Transaction transaction in transactions)
                {
                    int? orderid = 0;
                    Transaction oldTransaction = new Transaction();
                    double difference = 0;
                    if(transaction.Id > 0 && transaction.Amount > 0)
                    {
                        oldTransaction = db.Transactions.Where(x => x.Id == transaction.Id).AsNoTracking().FirstOrDefault();
                        orderid = transaction.OrderId;
                        db.Entry(transaction).State = EntityState.Modified;
                    }
                    else if(transaction.Id > 0 && transaction.Amount <= 0)
                    {
                        oldTransaction = db.Transactions.Where(x => x.Id == transaction.Id).AsNoTracking().FirstOrDefault();
                        orderid = transaction.OrderId;
                        db.Transactions.Remove(db.Transactions.Find(transaction.Id));
                    }
                    else if(transaction.Id == 0 && transaction.Amount > 0)
                    {
                        db.Transactions.Add(transaction);
                    }
                    db.SaveChanges();
                    difference = oldTransaction.Amount - transaction.Amount;
                    if(orderid != null && orderid > 0 && difference > 0)
                    {
                        Order order = db.Orders.Find(orderid);
                        order.PaidAmount = order.PaidAmount - difference;
                        dynamic payload = JsonConvert.DeserializeObject(order.OrderJson);
                        payload.PaidAmount = order.PaidAmount;
                        order.OrderJson = JsonConvert.SerializeObject(payload);
                        db.Entry(transaction).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                var response = new
                {
                    status = 200,
                    message = "Success"
                };
                return Json(response);
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

        [HttpGet("getStoreCashSales")]
        public IActionResult getStoreCashSales(int storeid, int companyid, DateTime date)
        {
            try
            {
                string conn_name = connserve.getConnString(companyid);
                //if (companyid == 3)
                //{
                //    conn_name = "logout";
                //}
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString(conn_name));
                sqlCon.Open();

                SqlCommand cmd = new SqlCommand("dbo.StoreCashSales", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@storeid", storeid));
                cmd.Parameters.Add(new SqlParameter("@companyid", companyid));
                cmd.Parameters.Add(new SqlParameter("@date", date));

                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);

                DataTable table = ds.Tables[0];
                var data = new
                {
                    transactions = ds.Tables[0],
                    totalsales = ds.Tables[1]
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
        [HttpGet("storepaymentsbytype")]
        public IActionResult storepaymentsbytype(int storeid, int companyid, DateTime? from, DateTime? to)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();

                SqlCommand cmd = new SqlCommand("dbo.StorePaymentsByTypes", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@storeid", storeid));
                cmd.Parameters.Add(new SqlParameter("@companyid", companyid));
                cmd.Parameters.Add(new SqlParameter("@from", from));
                cmd.Parameters.Add(new SqlParameter("@to", to));

                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);

                DataTable table = ds.Tables[0];
                var data = new
                {
                    pos_summary = ds.Tables[0],
                    pos_transactions = ds.Tables[1],
                    sw_zm_summary = ds.Tables[2]
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
        [HttpGet("transactionsbytype")]
        public IActionResult transactionsbytype(int storeid, int companyid, DateTime? from, DateTime? to, int sourceid, int ptypeid)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();

                SqlCommand cmd = new SqlCommand("dbo.TransactionsByType", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@storeid", storeid));
                cmd.Parameters.Add(new SqlParameter("@companyid", companyid));
                cmd.Parameters.Add(new SqlParameter("@from", from));
                cmd.Parameters.Add(new SqlParameter("@to", to));
                cmd.Parameters.Add(new SqlParameter("@sourceid", sourceid));
                cmd.Parameters.Add(new SqlParameter("@ptypeid", ptypeid));

                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);

                DataTable table = ds.Tables[0];
                var data = new
                {
                    transactions = ds.Tables[0],
                    paymenttypes = db.StorePaymentTypes.Where(x => x.StoreId == storeid).ToList()
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
        [HttpGet("getbyinvoice")]
        public IActionResult getbyinvoice(string invoice)
        {
            Order order = new Order();
            order = db.Orders.Where(x => x.InvoiceNo == invoice).FirstOrDefault();
            return Ok(order);
        }
        public class transaction : Transaction
        {
            public string InvoiceNo { get; set; }
        }
        // POST api/<controller>
        [HttpPost("ordertransaction")]
        public IActionResult ordertransaction([FromBody]dynamic transactionlist)
        {
            try
            {
                List<transaction> transactions = transactionlist.ToObject<List<transaction>>();
                string conn_name = connserve.getConnString(transactions[0].CompanyId);
                db = DbContextFactory.Create(conn_name);
                Odrs order = db.Odrs.Where(x => x.Id == transactions[0].OrderId && x.ino == transactions[0].InvoiceNo).FirstOrDefault();
                List<Transaction> oldtransactions = db.Transactions.Where(x => x.OrderId == order.OdrsId).ToList();
                List<Transaction> alltranasctions = new List<Transaction>();
                alltranasctions.AddRange(oldtransactions);
                foreach (Transaction transaction in transactions)
                {
                    if(!alltranasctions.Where(x => x.TransDateTime == transaction.TransDateTime && x.Amount == transaction.Amount && x.StorePaymentTypeId == transaction.StorePaymentTypeId).Any())
                    {
                        transaction.OrderId = order.OdrsId;
                        alltranasctions.Add(transaction);
                        db.Transactions.Add(transaction);
                        db.SaveChanges();
                    }
                }
                double totalpaid = (alltranasctions.Where(x => x.TranstypeId == 1).Select(x => x.Amount).Sum()) - (alltranasctions.Where(x => x.TranstypeId == 2).Select(x => x.Amount).Sum());
                
                if(totalpaid <= order.ba)
                {
                    order.pa = totalpaid;
                    if (order.oj != null)
                    {
                        dynamic json = JsonConvert.DeserializeObject(order.oj);
                        json.alltransactions = JToken.FromObject(alltranasctions.Select(x => new { x.Amount, x.CompanyId, x.CustomerId, x.Id, x.ModifiedDateTime, x.Notes, x.TransDate, x.TransDateTime, x.TranstypeId, x.UserId }));
                        json.PaidAmount = totalpaid;
                        order.oj = JsonConvert.SerializeObject(json);
                    }
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                }

                var error = new
                {
                    status = 200,
                    msg = "Transactions saved"
                };
                return Json(error);
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
        [HttpPost("Pay")]
        public IActionResult Pay([FromForm]string value)
        {
            try
            {
                dynamic orderJson = JsonConvert.DeserializeObject(value);
                Transaction transaction = orderJson.ToObject<Transaction>();
                //transaction.PaymentTypeId = 1;
                transaction.TransDateTime = DateTime.Now;
                transaction.TransDate = DateTime.Now;
                transaction.TranstypeId = 1;
                if (transaction.Amount < 0)
                {
                    transaction.TranstypeId = 2;
                }
                db.Transactions.Add(transaction);
                db.SaveChanges();
                var order = db.Orders.Find(transaction.OrderId);
                if(order.PaidAmount*-1 == transaction.Amount)
                {
                    order.OrderStatusId = -1;
                    order.RefundAmount = transaction.Amount * -1;
                    transaction.Amount = transaction.Amount * -1;
                }
                if (order.OrderStatusId != -1)
                {
                    order.PaidAmount = order.PaidAmount + transaction.Amount;
                }
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                var error = new
                {
                    status = 200,
                    msg = "Payment done"
                };
                return Json(error);

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

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

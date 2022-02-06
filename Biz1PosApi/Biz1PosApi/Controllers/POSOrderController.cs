using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Cors;
using Biz1PosApi.Models;
using Microsoft.Extensions.Configuration;
//using Quobject.SocketIoClientDotNet.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using Biz1PosApi.Hubs;
using System.Net.Mail;
using System.Text;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    public class POSOrderController : Controller
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        // GET: api/<controller>
        private int OrderId;
        private object Order;
        private int CustomerId;
        private int CustomerNo;
        private string CustomerPhone;
        private POSDbContext db;
        private IHubContext<ChatHub> _hubContext;
        private IHubContext<UrbanPiperHub, IHubClient> _uhubContext;
        private static TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        public IConfiguration Configuration { get; }
        public POSOrderController(POSDbContext contextOptions, IConfiguration configuration, IHubContext<ChatHub> hubContext, IHubContext<UrbanPiperHub, IHubClient> uhubContext, IServiceScopeFactory serviceScopeFactory)
        {
            db = contextOptions;
            Configuration = configuration;
            _hubContext = hubContext;
            _uhubContext = uhubContext;
            _serviceScopeFactory = serviceScopeFactory;
        }

        // GET: api/<controller>
        [HttpGet("Get")]
        public IActionResult Get()
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
        [HttpGet("getorderbyinvoiceno")]
        public IActionResult getorderbyinvoiceno(int companyId, string invoiceno)
        {
            try
            {
                List<Order> orders = db.Orders.Where(x => x.InvoiceNo == invoiceno && x.CompanyId == companyId).Include(x => x.Store).ToList();
                var response = new
                {
                    status = 200,
                    orders = orders
                };
                return Json(response);
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
        [HttpPost("cancellorder")]
        public IActionResult cancellorder(int orderid, [FromBody]JObject orderjson)
        {
            try
            {
                dynamic json = orderjson;
                Order order = db.Orders.Find(orderid);
                order.OrderStatusId = -1;
                order.CancelReason = json.CancelReason;
                order.OrderJson = JsonConvert.SerializeObject(orderjson);
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                Transaction transaction = new Transaction();
                transaction.Amount = order.PaidAmount;
                transaction.CompanyId = order.CompanyId;
                transaction.CustomerId = order.CustomerId;
                transaction.ModifiedDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                transaction.OrderId = order.Id;
                transaction.PaymentTypeId = 6;
                transaction.StoreId = order.StoreId;
                transaction.TransDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                transaction.TransDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                transaction.TranstypeId = 2;
                transaction.UserId = order.UserId;
                db.Transactions.Add(transaction);
                db.SaveChanges();
                var response = new
                {
                    status = 200,
                    message = "Order successfully cancelled"
                };
                return Json(response);
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

        // GET api/<controller>/5
        [HttpGet("POSOrderData")]
        public IActionResult POSOrderData(int compId, int storeId, DateTime POSDate)
        {
            try
            {
                //SqlConnection sqlCon = new SqlConnection("server=(LocalDb)\\MSSQLLocalDB; database=Biz1POS;Trusted_Connection=True;");
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();

                SqlCommand cmd = new SqlCommand("dbo.POSOrderData", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@compId", compId));
                cmd.Parameters.Add(new SqlParameter("@storeId", storeId));
                cmd.Parameters.Add(new SqlParameter("@modDate", POSDate));
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
                    status = 200,
                    data = orderObj,
                    transactions = ds.Tables[1],
                    msg = ""
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
        [HttpPost("saveorder")]
        public IActionResult saveorder([FromBody]OrderPayload payload)
        {
            int? storeid = null;
            int? companyid = null;
            try
            {
                int orderid = 0;
                dynamic data = new { };
                string message = "";
                int status = 200;
                dynamic orderjson = JsonConvert.DeserializeObject(payload.OrderJson);
                string invoiceno = orderjson.InvoiceNo.ToString();
                int paymenttypeid = (int)orderjson.PaymentTypeId;
                int? storepaymenttypeid = null;
                storeid = (int)orderjson.StoreId;
                companyid = (int)orderjson.CompanyId;
                if (orderjson.StorePaymentTypeId != null)
                {
                    storepaymenttypeid = (int)orderjson.StorePaymentTypeId;
                }
                string cphone = orderjson.CustomerDetails.PhoneNo.ToString();
                storeid = (int)orderjson.StoreId;
                companyid = (int)orderjson.CompanyId;
                int customerid = -1;
                int last_orderno = 0;
                if (db.Orders.Where(x => x.StoreId == storeid && x.OrderedDate == TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time).Date).Any())
                {
                    last_orderno = db.Orders.Where(x => x.StoreId == storeid && x.OrderedDate == TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time).Date).Max(x => x.OrderNo);
                }
                int current_orderno = (int)orderjson.OrderNo;
                int orderno_diff = current_orderno - last_orderno;
                if (orderno_diff > 1)
                {
                    Store store = db.Stores.Find(storeid);
                    Alert alert = new Alert();
                    alert.AlertDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                    alert.AlertName = "orderno_skip";
                    alert.CompanyId = (int)companyid;
                    alert.StoreId = (int)storeid;
                    string mailbody = store.Name + " has faced an orderno_skip event @" + alert.AlertDateTime + ". Last orderno is " + last_orderno.ToString() + " and Current orderno is " + current_orderno;
                    alert.Note = mailbody;
                    db.Alerts.Add(alert);
                    db.SaveChanges();
                    send_alert_email(mailbody);
                }
                if (db.Orders.Where(x => x.InvoiceNo == invoiceno).Any())
                {
                    message = "It is a duplicate Order!";
                    status = 409;
                    OrderLog orderLog = new OrderLog();
                    orderLog.CompanyId = companyid;
                    orderLog.StoreId = storeid;
                    orderLog.Payload = payload.OrderJson;
                    orderLog.Error = "It is a Duplicate Order";
                    orderLog.LoggedDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                    db.OrderLogs.Add(orderLog);
                    db.SaveChanges();
                }
                else
                {
                    if(cphone != "" && cphone != null)
                    {
                        customerid = db.Customers.Where(x => x.PhoneNo == cphone).Any()? db.Customers.Where(x => x.PhoneNo == cphone).FirstOrDefault().Id : 0;
                    }
                    using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("myconn")))
                    {
                        conn.Open();
                        SqlTransaction tran = conn.BeginTransaction("Transaction1");
                        try
                        {

                            SqlCommand cmd = new SqlCommand("dbo.SaveOrder", conn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Transaction = tran;

                            cmd.Parameters.Add(new SqlParameter("@orderjson", payload.OrderJson));
                            cmd.Parameters.Add(new SqlParameter("@paymenttypeid", paymenttypeid));
                            cmd.Parameters.Add(new SqlParameter("@storepaymenttypeid", storepaymenttypeid));
                            cmd.Parameters.Add(new SqlParameter("@customerid", customerid));
                            DataSet ds = new DataSet();
                            SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                            sqlAdp.Fill(ds);

                            DataTable table = ds.Tables[0];
                            data = table;
                            //Your Code

                            tran.Commit(); //both are successful
                            conn.Close();
                        }
                        catch (Exception e)
                        {
                            //if error occurred, reverse all actions. By this, your data consistent and correct
                            tran.Rollback();
                            conn.Close();
                            throw e;
                        }
                    }
                }
                var response = new
                {
                    data = data,
                    message = message,
                    status = status
                };
                return Ok(response);
            }
            catch (Exception e)
            {
                OrderLog orderLog = new OrderLog();
                orderLog.CompanyId = companyid;
                orderLog.StoreId = storeid;
                orderLog.Payload = payload.OrderJson;
                orderLog.Error = e.ToString();
                orderLog.LoggedDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                db.OrderLogs.Add(orderLog);
                db.SaveChanges();
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 500,
                    msg = "Something went wrong  Contact our service provider"
                };
                return Json(error);
            }
        }
        [HttpPost("saveorder_2")]
        public IActionResult saveorder_2([FromBody]OrderPayload payload)
        {
            int? storeid = null;
            int? companyid = null;
            try
            {
                int orderid = 0;
                dynamic data = new { };
                string message = "";
                int status = 200;
                dynamic orderjson = JsonConvert.DeserializeObject(payload.OrderJson);
                string invoiceno = orderjson.InvoiceNo.ToString();
                int paymenttypeid = (int)orderjson.PaymentTypeId;
                int? storepaymenttypeid = null;
                storeid = (int)orderjson.StoreId;
                companyid = (int)orderjson.CompanyId;
                if (orderjson.StorePaymentTypeId != null)
                {
                    storepaymenttypeid = (int)orderjson.StorePaymentTypeId;
                }
                string cphone = orderjson.CustomerDetails.PhoneNo.ToString();
                storeid = (int)orderjson.StoreId;
                companyid = (int)orderjson.CompanyId;
                int customerid = -1;
                int last_orderno = 0;
                if (db.Orders.Where(x => x.StoreId == storeid && x.OrderedDate == TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time).Date).Any())
                {
                    last_orderno = db.Orders.Where(x => x.StoreId == storeid && x.OrderedDate == TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time).Date).Max(x => x.OrderNo);
                }
                int current_orderno = (int)orderjson.OrderNo;
                int orderno_diff = current_orderno - last_orderno;
                if (orderno_diff > 1)
                {
                    Store store = db.Stores.Find(storeid);
                    Alert alert = new Alert();
                    alert.AlertDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                    alert.AlertName = "orderno_skip";
                    alert.CompanyId = (int)companyid;
                    alert.StoreId = (int)storeid;
                    string mailbody = store.Name + " has faced an orderno_skip event @" + alert.AlertDateTime + ". Last orderno is " + last_orderno.ToString() + " and Current orderno is " + current_orderno;
                    alert.Note = mailbody;
                    db.Alerts.Add(alert);
                    db.SaveChanges();
                    send_alert_email(mailbody);
                }
                if (db.Orders.Where(x => x.InvoiceNo == invoiceno).Any())
                {
                    message = "It is a duplicate Order!";
                    status = 409;
                    OrderLog orderLog = new OrderLog();
                    orderLog.CompanyId = companyid;
                    orderLog.StoreId = storeid;
                    orderLog.Payload = payload.OrderJson;
                    orderLog.Error = "It is a Duplicate Order";
                    orderLog.LoggedDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                    db.OrderLogs.Add(orderLog);
                    db.SaveChanges();
                }
                else
                {
                    if(cphone != "" && cphone != null)
                    {
                        customerid = db.Customers.Where(x => x.PhoneNo == cphone).Any()? db.Customers.Where(x => x.PhoneNo == cphone).FirstOrDefault().Id : 0;
                    }
                    using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("myconn")))
                    {
                        conn.Open();
                        SqlTransaction tran = conn.BeginTransaction("Transaction1");
                        try
                        {
                            SqlCommand cmd = new SqlCommand("dbo.saveorder2", conn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Transaction = tran;

                            cmd.Parameters.Add(new SqlParameter("@orderjson", payload.OrderJson));
                            cmd.Parameters.Add(new SqlParameter("@paymenttypeid", paymenttypeid));
                            cmd.Parameters.Add(new SqlParameter("@storepaymenttypeid", storepaymenttypeid));
                            cmd.Parameters.Add(new SqlParameter("@customerid", customerid));
                            DataSet ds = new DataSet();
                            SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                            sqlAdp.Fill(ds);

                            DataTable table = ds.Tables[0];
                            data = table;
                            //Your Code

                            tran.Commit(); //both are successful
                            conn.Close();
                        }
                        catch (Exception e)
                        {
                            //if error occurred, reverse all actions. By this, your data consistent and correct
                            tran.Rollback();
                            conn.Close();
                            throw e;
                        }
                    }
                }
                var response = new
                {
                    data = data,
                    message = message,
                    status = status
                };
                return Ok(response);
            }
            catch (Exception e)
            {
                OrderLog orderLog = new OrderLog();
                orderLog.CompanyId = companyid;
                orderLog.StoreId = storeid;
                orderLog.Payload = payload.OrderJson;
                orderLog.Error = e.ToString();
                orderLog.LoggedDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                db.OrderLogs.Add(orderLog);
                db.SaveChanges();
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 500,
                    msg = "Something went wrong  Contact our service provider"
                };
                return Json(error);
            }
        }
        [HttpPost("saveorder_3")]
        public IActionResult saveorder_3([FromBody]OrderPayload payload)
        {
            int? storeid = null;
            int? companyid = null;
            try
            {
                int orderid = 0;
                dynamic data = new { };
                string message = "";
                int status = 200;
                dynamic orderjson = JsonConvert.DeserializeObject(payload.OrderJson);
                string invoiceno = orderjson.InvoiceNo.ToString();
                long createdtimestamp = 0;
                if(orderjson.createdtimestamp != null)
                {
                    createdtimestamp = (long)orderjson.createdtimestamp;
                }
                int paymenttypeid = (int)orderjson.PaymentTypeId;
                int? storepaymenttypeid = null;
                storeid = (int)orderjson.StoreId;
                companyid = (int)orderjson.CompanyId;
                if (orderjson.StorePaymentTypeId != null)
                {
                    storepaymenttypeid = (int)orderjson.StorePaymentTypeId;
                }
                string cphone = orderjson.CustomerDetails.PhoneNo.ToString();
                storeid = (int)orderjson.StoreId;
                companyid = (int)orderjson.CompanyId;
                int customerid = -1;
                int last_orderno = 0;
                if (db.Orders.Where(x => x.StoreId == storeid && x.OrderedDate == TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time).Date).Any())
                {
                    last_orderno = db.Orders.Where(x => x.StoreId == storeid && x.OrderedDate == TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time).Date).Max(x => x.OrderNo);
                }
                int current_orderno = (int)orderjson.OrderNo;
                int orderno_diff = current_orderno - last_orderno;
                if (orderno_diff > 1)
                {
                    Store store = db.Stores.Find(storeid);
                    Alert alert = new Alert();
                    alert.AlertDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                    alert.AlertName = "orderno_skip";
                    alert.CompanyId = (int)companyid;
                    alert.StoreId = (int)storeid;
                    string mailbody = store.Name + " has faced an orderno_skip event @" + alert.AlertDateTime + ". Last orderno is " + last_orderno.ToString() + " and Current orderno is " + current_orderno;
                    alert.Note = mailbody;
                    db.Alerts.Add(alert);
                    db.SaveChanges();
                    send_alert_email(mailbody);
                }
                if (db.Orders.Where(x => x.InvoiceNo == invoiceno && x.CreatedTimeStamp == createdtimestamp).Any())
                {
                    message = "It is a duplicate Order!";
                    status = 409;
                    OrderLog orderLog = new OrderLog();
                    orderLog.CompanyId = companyid;
                    orderLog.StoreId = storeid;
                    orderLog.Payload = payload.OrderJson;
                    orderLog.Error = "It is a Duplicate Order";
                    orderLog.LoggedDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                    db.OrderLogs.Add(orderLog);
                    db.SaveChanges();
                }
                else
                {
                    if(cphone != "" && cphone != null)
                    {
                        customerid = db.Customers.Where(x => x.PhoneNo == cphone).Any()? db.Customers.Where(x => x.PhoneNo == cphone).FirstOrDefault().Id : 0;
                    }
                    using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("myconn")))
                    {
                        conn.Open();
                        SqlTransaction tran = conn.BeginTransaction("Transaction1");
                        try
                        {
                            SqlCommand cmd = new SqlCommand("dbo.saveorder3", conn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Transaction = tran;

                            cmd.Parameters.Add(new SqlParameter("@orderjson", payload.OrderJson));
                            cmd.Parameters.Add(new SqlParameter("@paymenttypeid", paymenttypeid));
                            //cmd.Parameters.Add(new SqlParameter("@storepaymenttypeid", storepaymenttypeid));
                            //cmd.Parameters.Add(new SqlParameter("@customerid", customerid));
                            DataSet ds = new DataSet();
                            SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                            sqlAdp.Fill(ds);

                            DataTable table = ds.Tables[0];
                            data = table;
                            //Your Code

                            tran.Commit(); //both are successful
                            conn.Close();
                            if(orderjson.DeliveryStoreId != null)
                            {
                                _uhubContext.Clients.All.DeliveryOrderUpdate((int)orderjson.StoreId, (int)orderjson.DeliveryStoreId, (string)orderjson.InvoiceNo);
                            }
                        }
                        catch (Exception e)
                        {
                            //if error occurred, reverse all actions. By this, your data consistent and correct
                            tran.Rollback();
                            conn.Close();
                            throw e;
                        }
                    }
                }
                var response = new
                {
                    data = data,
                    message = message,
                    status = status
                };
                return Ok(response);
            }
            catch (Exception e)
            {
                OrderLog orderLog = new OrderLog();
                orderLog.CompanyId = companyid;
                orderLog.StoreId = storeid;
                orderLog.Payload = payload.OrderJson;
                orderLog.Error = e.ToString();
                orderLog.LoggedDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                db.OrderLogs.Add(orderLog);
                db.SaveChanges();
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 500,
                    msg = "Something went wrong  Contact our service provider"
                };
                return Json(error);
            }
        }
        [HttpPost("updateorder_2")]
        public IActionResult updateorder_2([FromBody]OrderPayload payload)
        {
            using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("myconn")))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction("Transaction1");
                try
                {
                    dynamic raworder = JsonConvert.DeserializeObject(payload.OrderJson);
                    if(raworder.DiscAmount == null)
                    {
                        raworder.DiscAmount = 0;
                    }
                    string orderjson = payload.OrderJson;
                    raworder.Id = raworder.OrderId;
                    Order order = raworder.ToObject<Order>();
                    order.OrderStatusId = raworder.OrderStatusId;
                    foreach (string citem in raworder.changeditems)
                    {
                        if (citem == "transaction")
                        {
                            orderjson = ordertransaction(payload).OrderJson;
                        }
                        else if (citem == "kot")
                        {
                            orderjson = orderkot(payload).OrderJson;
                        }
                    }
                    order.OrderJson = orderjson;
                    if(order.OrderStatusId == 5)
                    {
                        order.DeliveredDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                        order.DeliveredDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                    }
                    order.DeliveryDate = order.DeliveryDateTime == null ? order.OrderedDate : order.DeliveryDateTime;
                    order.DeliveryStoreId = order.DeliveryStoreId == null ? order.StoreId : order.DeliveryStoreId;
                    order.OrderedTime = db.Orders.Where(x => x.Id == order.Id).AsNoTracking().FirstOrDefault().OrderedTime;
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                    if(order.DeliveryStoreId != null)
                    {
                        _uhubContext.Clients.All.DeliveryOrderUpdate((int)order.StoreId, (int)order.DeliveryStoreId, order.InvoiceNo);
                    }
                    var response = new
                    {
                        status = 200,
                        msg = "status change success"
                    };
                    return Json(response);
                }
                catch (Exception e)
                {
                    //if error occurred, reverse all actions. By this, your data consistent and correct
                    tran.Rollback();
                    conn.Close();
                    var error = new
                    {
                        error = new Exception(e.Message, e.InnerException),
                        status = 500,
                        msg = "Something went wrong  Contact our service provider"
                    };
                    return Json(error);
                }
            }
        }
        [HttpGet("getorderbyinvoice")]
        public IActionResult getorderbyinvoice(string invoiceno)
        {
            try
            {
                string orderPayload = db.Orders.Where(x => x.InvoiceNo == invoiceno).FirstOrDefault().OrderJson;
                var response = new
                {
                    status = 200,
                    OrderJson = orderPayload
                };
                return Json(response);
            }
            catch (Exception e)
            {
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 500,
                    msg = "Something went wrong  Contact our service provider"
                };
                return Json(error);
            }
        }
        [HttpPost("orderstatuschange")]
        public IActionResult orderstatuschange([FromBody]OrderPayload payload, int orderid, int statusid)
        {
            try
            {
                Order order = db.Orders.Find(orderid);
                order.OrderStatusId = statusid;
                order.OrderJson = payload.OrderJson;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                var response = new
                {
                    status = 200,
                    msg = "status change success"
                };
                return Json(response);
            }
            catch (Exception e)
            {
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 500,
                    msg = "Something went wrong  Contact our service provider"
                };
                return Json(error);
            }
        }
        public OrderPayload ordertransaction([FromBody] OrderPayload orderPayload)
        {
            dynamic raworder = JsonConvert.DeserializeObject(orderPayload.OrderJson);
            orderPayload.Transactions = raworder.Transactions.ToObject<List<Transaction>>();
            int orderid = (int)orderPayload.Transactions.FirstOrDefault().OrderId;
            Order order = db.Orders.AsNoTracking().Where(x => x.Id == orderid).FirstOrDefault();
            List<Transaction> oldTransactions = db.Transactions.Where(x => x.OrderId == orderid).ToList();
            double totaltransactionamnt = 0;
            foreach(Transaction otrnsction in oldTransactions)
            {
                totaltransactionamnt += otrnsction.Amount;
            }
            if(order.PaidAmount > totaltransactionamnt)
            {
                foreach (Transaction transaction in orderPayload.Transactions)
                {
                    transaction.ModifiedDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                    db.Transactions.Add(transaction);
                    db.SaveChanges();
                }
            }
            //var list = raworder.changeditems.ToArray<string>();
            //list.Remove("transaction");
            raworder.closedtransactions = new JArray();
            for(int i = 0; i < raworder.Transactions.Count; i++)
            {
                raworder.closedtransactions.Add(raworder.Transactions[i]);
            }
            raworder.changeditems = new JArray();
            raworder.Transactions = new JArray();
            orderPayload.OrderJson = JsonConvert.SerializeObject(raworder);
            return orderPayload;
        }
        public OrderPayload orderkot([FromBody] OrderPayload orderPayload)
        {
            dynamic raworder = JsonConvert.DeserializeObject(orderPayload.OrderJson);
            int orderid = (int)raworder.OrderId;
            //orderPayload.Transactions = raworder.Transactions.ToObject<List<Transaction>>();
            foreach (var kotobj in raworder.KOTS)
            {
                kotobj.OrderId = orderid;
                KOT kOT = kotobj.ToObject<KOT>();
                if (!db.KOTs.Where(x => x.refid == kOT.refid && x.OrderId == orderid).Any())
                {
                    db.KOTs.Add(kOT);
                    db.SaveChanges();
                    foreach(var item in kotobj.Items)
                    {
                        item.Product = null;
                        item.OrderId = orderid;
                        OrderItem orderItem = item.ToObject<OrderItem>();
                        orderItem.KOTId = kOT.Id;
                        db.OrderItems.Add(orderItem);
                        db.SaveChanges();
                        foreach(var optionGroup in item.OptionGroup)
                        {
                            if(optionGroup.selected == true)
                            {
                                foreach(var option in optionGroup.Option)
                                {
                                    if(option.selected == true)
                                    {
                                        OrdItemOptions itemoption = new OrdItemOptions();
                                        itemoption.OptionId = (int)option.Id;
                                        itemoption.OrderItemId = orderItem.Id;
                                        itemoption.orderitemrefid = option.orderitemrefid;
                                        itemoption.Price = option.Price;
                                        db.OrdItemOptions.Add(itemoption);
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //var list = raworder.changeditems.ToArray<string>();
            //list.Remove("transaction");
            raworder.changeditems = new JArray();
            raworder.Transactions = new JArray();
            orderPayload.OrderJson = JsonConvert.SerializeObject(raworder);
            return orderPayload;
        }

        public int save_order(dynamic ordstring)
        {
            int orderid = 0;
            int companyid = 0;
            int storeid = 0;
            string payload = "";
            dynamic ord = JsonConvert.DeserializeObject(ordstring);
            string order_invoice = ord.InvoiceNo;
            companyid = ord.CompanyId;
            storeid = ord.StoreId;
            payload = ordstring;
            ord.CustomerDetails.Id = 0;
            CustomerPhone = ord.CustomerDetails.PhoneNo;
            if (!(CustomerPhone == null || CustomerPhone == ""))
            {
                Customer customer = db.Customers.Where(x => x.PhoneNo == CustomerPhone).FirstOrDefault();
                if (customer == null)
                {
                    Customer newcustomer = new Customer();
                    newcustomer = ord.CustomerDetails.ToObject<Customer>();
                    newcustomer.CreatedDate = DateTime.Now;
                    newcustomer.ModifiedDate = DateTime.Now;
                    db.Customers.Add(newcustomer);
                    db.SaveChanges();
                    ord.CustomerId = newcustomer.Id;
                }
                else
                {
                    Customer oldcustomer = db.Customers.Where(x => x.PhoneNo == CustomerPhone).FirstOrDefault();
                    oldcustomer.Address = ord.CustomerDetails.Address;
                    oldcustomer.City = ord.CustomerDetails.City;
                    oldcustomer.CompanyId = ord.CustomerDetails.CompanyId;
                    oldcustomer.Email = ord.CustomerDetails.Email;
                    oldcustomer.Name = ord.CustomerDetails.Name;
                    oldcustomer.PhoneNo = ord.CustomerDetails.PhoneNo;
                    oldcustomer.PostalCode = ord.CustomerDetails.PostalCode;
                    oldcustomer.StoreId = ord.CustomerDetails.StoreId;
                    oldcustomer.ModifiedDate = DateTime.Now;
                    db.Entry(oldcustomer).State = EntityState.Modified;
                    db.SaveChanges();
                    ord.CustomerId = oldcustomer.Id;
                }
            }
            ord.Id = 0;
            Order order = ord.ToObject<Order>();
            order.OrderJson = JsonConvert.SerializeObject(ord);
            order.CustomerData = JsonConvert.SerializeObject(ord.CustomerDetails);
            order.ItemJson = JsonConvert.SerializeObject(ord.KOT);
            order.ChargeJson = JsonConvert.SerializeObject(ord.AdditionalCharge);
            order.Charges = ord.AddCharge;
            //order.OrderNo = order.Id;
            if (order.DiningTableId == 0)
            {
                order.DiningTableId = null;
            }
            db.Orders.Add(order);
            db.SaveChanges();
            orderid = order.Id;
            ord.OrderId = order.Id;
            order.OrderJson = JsonConvert.SerializeObject(ord);
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            //Order = order;
            if (ord.PaymentTypeId == 5)
            {
                foreach (var trans in ord.splitPayment)
                {
                    Transaction transaction = ord.ToObject<Transaction>();
                    transaction.OrderId = order.Id;
                    transaction.Amount = trans.Amount;
                    transaction.PaymentTypeId = trans.PaymentId;
                    transaction.TranstypeId = 1;
                    transaction.CustomerId = order.CustomerId;
                    transaction.TransDate = order.OrderedDate;
                    transaction.TransDateTime = order.OrderedDateTime;
                    db.Transactions.Add(transaction);
                    db.SaveChanges();
                }
            }
            else if (ord.PaymentTypeId != 0)
            {
                Transaction transaction = ord.ToObject<Transaction>();
                transaction.Id = 0;
                transaction.OrderId = order.Id;
                transaction.Amount = ord.PaidAmount;
                transaction.PaymentTypeId = ord.PaymentTypeId;
                transaction.TranstypeId = 1;
                transaction.CustomerId = order.CustomerId;
                transaction.TransDateTime = order.OrderedDateTime;
                transaction.TransDate = order.OrderedDate;
                db.Transactions.Add(transaction);
                db.SaveChanges();
            }
            OrderId = order.Id;
            JArray AdditionalChrgObj = ord.AdditionalCharge;
            dynamic AdditionalCharges = AdditionalChrgObj.ToList();
            foreach (var additionalChrg in AdditionalCharges)
            {
                OrderCharges orderCharges = new OrderCharges();
                TaxGroup tax = db.TaxGroups.Find(additionalChrg.TaxGroupId.ToObject<int>());
                orderCharges.AdditionalChargeId = additionalChrg.Id;
                orderCharges.ChargeAmount = additionalChrg.ChargeAmount;
                orderCharges.ChargePercentage = additionalChrg.ChargeValue;
                if (tax != null)
                {
                    orderCharges.Tax1 = tax.Tax1;
                    orderCharges.Tax2 = tax.Tax2;
                    orderCharges.Tax3 = tax.Tax3;
                }
                else
                {
                    orderCharges.Tax1 = 0;
                    orderCharges.Tax2 = 0;
                    orderCharges.Tax3 = 0;
                }
                orderCharges.AdditionalChargeId = additionalChrg.Id;
                orderCharges.CompanyId = order.CompanyId;
                orderCharges.OrderId = order.Id;
                orderCharges.StoreId = order.StoreId;
                db.OrderCharges.Add(orderCharges);
                db.SaveChanges();
            }
            JArray kotObj = ord.KOT;
            dynamic kotJson = kotObj.ToList();
            foreach (var kotitm in kotJson)
            {
                KOT kot = kotitm.ToObject<KOT>();
                kot.KOTStatusId = 1;
                kot.OrderId = OrderId;
                kot.KOTNo = kotitm.KOTNo;
                kot.CreatedDate = DateTime.Now;
                kot.ModifiedDate = DateTime.Now;
                kot.CompanyId = order.CompanyId;
                kot.StoreId = order.StoreId;
                if (kot.KOTGroupId == -1)
                {
                    kot.KOTGroupId = null;
                }
                db.KOTs.Add(kot);
                db.SaveChanges();
                JArray itemObj = kotitm.item;
                dynamic itemJson = itemObj.ToList();
                foreach (var item in itemJson)
                {
                    item.Product = null;
                    OrderItem ordItem = item.ToObject<OrderItem>();
                    ordItem.OrderId = order.Id;
                    ordItem.KOTId = kot.Id;
                    ordItem.CategoryId = db.Products.Find(ordItem.ProductId).CategoryId;
                    ordItem.TotalAmount = item.TotalAmount;
                    dynamic optionjson = new JObject();
                    optionjson.quantity = ordItem.Quantity;
                    optionjson.amount = ordItem.TotalAmount;
                    optionjson.key = "";
                    optionjson.options = new JArray();
                    foreach (var optGrp in item.OptionGroup)
                    {
                        if (optGrp.OptionGroupType == 1)
                        {
                            foreach (var opt in optGrp.Option)
                            {
                                dynamic optjson = new JObject();
                                optjson.Id = opt.Id;
                                optionjson.options.Add(optjson);
                                optionjson.key = optionjson.key + opt.Id.ToString() + "-";
                            }
                        }
                    }
                    ordItem.OptionJson = JsonConvert.SerializeObject(optionjson);
                    db.OrderItems.Add(ordItem);
                    db.SaveChanges();
                    foreach (var optGrp in item.OptionGroup)
                    {
                        foreach (var opt in optGrp.Option)
                        {
                            OrdItemOptions ordItemOpt = opt.ToObject<OrdItemOptions>();
                            ordItemOpt.Id = 0;
                            ordItemOpt.OrderItemId = ordItem.Id;
                            ordItemOpt.OptionId = opt.Id;
                            db.OrdItemOptions.Add(ordItemOpt);
                            db.SaveChanges();
                        }
                    }
                }
            }
            return orderid;
        }
        public void send_alert_email(string mailbody)
        {
            string from = "fruitsandbakes@gmail.com"; //From address    
            MailMessage message = new MailMessage(from, "mohamedanastsi@gmail.com");
            message.To.Add("karthick.nath@gmail.com");
            message.To.Add("masterservice2020@gmail.com");
            message.To.Add("mohamedanastsi@gmail.com");
            message.To.Add("sanjai.nath1995@gmail.com");
            message.Subject = "Welcome to FBcakes";

            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
            client.UseDefaultCredentials = false;
            NetworkCredential basicCredential1 = new
            NetworkCredential("fbcakes.biz1@gmail.com", "PassworD@1");

            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;
            client.Send(message);
        }
        [HttpGet("updateorderstatus")]
        public IActionResult updateorderstatus(int orderid, int statusid)
        {
            try
            {
                Order order = db.Orders.Find(orderid);
                order.PreviousStatusId = order.OrderStatusId;
                order.OrderStatusId = statusid;
                order.ModifiedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                if(statusid == -1)
                {
                    Transaction transaction = new Transaction();
                    transaction.Amount = order.PaidAmount;
                    transaction.CompanyId = order.CompanyId;
                    transaction.CustomerId = order.CustomerId;
                    transaction.OrderId = order.Id;
                    transaction.PaymentTypeId = 1;
                    transaction.StoreId = order.StoreId;
                    transaction.TransDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                    transaction.TransDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time); ;
                    transaction.TranstypeId = 2;
                    transaction.UserId = order.UserId;
                    db.Transactions.Add(transaction);
                    db.SaveChanges();
                }
                var response = new
                {
                    status = 200,
                    msg = "Status Changed"
                };
                return Json(response);
            }
            catch (Exception e)
            {
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 500,
                    msg = "Something went wrong  Contact our service provider"
                };
                return Json(error);
            }
        }

        [HttpGet("KOT")]
        public IActionResult KOT(int compId, int storeId)
        {
            try
            {
                //SqlConnection sqlCon = new SqlConnection("server=(LocalDb)\\MSSQLLocalDB; database=Biz1POS;Trusted_Connection=True;");
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                //string jsonOutputParam = "@jsonOutput";
                SqlCommand cmd = new SqlCommand("dbo.KOTData", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@compId", compId));
                cmd.Parameters.Add(new SqlParameter("@storeId", storeId));
                //cmd.Parameters.Add(new SqlParameter("@modDate", null));
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);

                DataTable table = ds.Tables[0];
                var orderObj = JsonConvert.DeserializeObject(ds.Tables[0].Rows[0][0].ToString());
                var obj = new
                {
                    status = 200,
                    data = orderObj,
                    msg = ""
                };
                sqlCon.Close();
                //JObject obj = JObject.Parse(ds.Tables[0].Rows[0][0].ToString());

                //DataTable order = ds.Tables[0];
                //var data = new
                //{
                //    order,
                //    item = ds.Tables[1]
                //};
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

        [HttpGet("KDS")]
        public IActionResult KDS(int compId, int storeId)
        {
            try
            {
                //SqlConnection sqlCon = new SqlConnection("server=(LocalDb)\\MSSQLLocalDB; database=Biz1POS;Trusted_Connection=True;");
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                //string jsonOutputParam = "@jsonOutput";
                SqlCommand cmd = new SqlCommand("dbo.KOTDisplay", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@compId", compId));
                cmd.Parameters.Add(new SqlParameter("@storeId", storeId));
                //cmd.Parameters.Add(new SqlParameter("@modDate", null));
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
                    msg = ""
                };
                sqlCon.Close();
                //JObject obj = JObject.Parse(ds.Tables[0].Rows[0][0].ToString());

                //DataTable order = ds.Tables[0];
                //var data = new
                //{
                //    order,
                //    item = ds.Tables[1]
                //};
                return Ok(orderObj);
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
        // POST api/<controller>
        [HttpPost("CreateOrder")]
        [EnableCors("AllowOrigin")]
        public async Task<IActionResult> CreateOrder([FromForm]string ordData, int compId, int storeId)
        {
            string message = "";
            using (var connection = new SqlConnection(Configuration.GetConnectionString("myconn")))
            //using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                connection.Open();
                var dbContextTransaction = connection.BeginTransaction();
                int? companyid = null;
                int? storeid = null;
                string payload = "";
                int latestorderno = 0;
                try
                {
                    dynamic orderJson = JsonConvert.DeserializeObject(ordData);
                    foreach (var ord in orderJson)
                    {
                        string order_invoice = ord.InvoiceNo;
                        companyid = ord.CompanyId;
                        storeid = ord.StoreId;
                        payload = JsonConvert.SerializeObject(ord);
                        var oldorder = db.Orders.Where(x => x.InvoiceNo == order_invoice).FirstOrDefault();
                        if(oldorder == null)
                        {
                            ord.CustomerDetails.Id = 0;
                            CustomerPhone = ord.CustomerDetails.PhoneNo;
                            if (!(CustomerPhone == null || CustomerPhone == ""))
                            {
                                Customer customer = db.Customers.Where(x => x.PhoneNo == CustomerPhone).FirstOrDefault();
                                if (customer == null)
                                {
                                    Customer newcustomer = new Customer();
                                    newcustomer = ord.CustomerDetails.ToObject<Customer>();
                                    newcustomer.CreatedDate = DateTime.Now;
                                    newcustomer.ModifiedDate = DateTime.Now;
                                    db.Customers.Add(newcustomer);
                                    db.SaveChanges();
                                    ord.CustomerId = newcustomer.Id;
                                }
                                else
                                {
                                    Customer oldcustomer = db.Customers.Where(x => x.PhoneNo == CustomerPhone).FirstOrDefault();
                                    oldcustomer.Address = ord.CustomerDetails.Address;
                                    oldcustomer.City = ord.CustomerDetails.City;
                                    oldcustomer.CompanyId = ord.CustomerDetails.CompanyId;
                                    oldcustomer.Email = ord.CustomerDetails.Email;
                                    oldcustomer.Name = ord.CustomerDetails.Name;
                                    oldcustomer.PhoneNo = ord.CustomerDetails.PhoneNo;
                                    oldcustomer.PostalCode = ord.CustomerDetails.PostalCode;
                                    oldcustomer.StoreId = ord.CustomerDetails.StoreId;
                                    oldcustomer.ModifiedDate = DateTime.Now;
                                    db.Entry(oldcustomer).State = EntityState.Modified;
                                    db.SaveChanges();
                                    ord.CustomerId = oldcustomer.Id;
                                }
                            }
                            ord.Id = 0;
                            Order order = ord.ToObject<Order>();
                            order.OrderJson = JsonConvert.SerializeObject(ord);
                            order.CustomerData = JsonConvert.SerializeObject(ord.CustomerDetails);
                            order.ItemJson = JsonConvert.SerializeObject(ord.KOT);
                            order.ChargeJson = JsonConvert.SerializeObject(ord.AdditionalCharge);
                            order.Charges = ord.AddCharge;
                            //order.OrderNo = order.Id;
                            if (order.DiningTableId == 0)
                            {
                                order.DiningTableId = null;
                            }
                            db.Orders.Add(order);
                            db.SaveChanges();
                            ord.OrderId = order.Id;
                            order.OrderJson = JsonConvert.SerializeObject(ord);
                            db.Entry(order).State = EntityState.Modified;
                            db.SaveChanges();
                            //Order = order;
                            if (ord.PaymentTypeId == 5)
                            {
                                foreach (var trans in ord.splitPayment)
                                {
                                    Transaction transaction = ord.ToObject<Transaction>();
                                    transaction.OrderId = order.Id;
                                    transaction.Amount = trans.Amount;
                                    transaction.PaymentTypeId = trans.PaymentId;
                                    transaction.TranstypeId = 1;
                                    transaction.CustomerId = order.CustomerId;
                                    transaction.TransDate = order.OrderedDate;
                                    transaction.TransDateTime = order.OrderedDateTime;
                                    db.Transactions.Add(transaction);
                                    db.SaveChanges();
                                }
                            }
                            else if (ord.PaymentTypeId != 0 && order.PaidAmount > 0)
                            {
                                Transaction transaction = ord.ToObject<Transaction>();
                                transaction.Id = 0;
                                transaction.OrderId = order.Id;
                                transaction.Amount = ord.PaidAmount;
                                transaction.PaymentTypeId = ord.PaymentTypeId;
                                transaction.TranstypeId = 1;
                                transaction.CustomerId = order.CustomerId;
                                transaction.TransDateTime = order.OrderedDateTime;
                                transaction.TransDate = order.OrderedDate;
                                db.Transactions.Add(transaction);
                                db.SaveChanges();
                            }
                            OrderId = order.Id;
                            JArray AdditionalChrgObj = ord.AdditionalCharge;
                            dynamic AdditionalCharges = AdditionalChrgObj.ToList();
                            foreach (var additionalChrg in AdditionalCharges)
                            {
                                OrderCharges orderCharges = new OrderCharges();
                                TaxGroup tax = db.TaxGroups.Find(additionalChrg.TaxGroupId.ToObject<int>());
                                orderCharges.AdditionalChargeId = additionalChrg.Id;
                                orderCharges.ChargeAmount = additionalChrg.ChargeAmount;
                                orderCharges.ChargePercentage = additionalChrg.ChargeValue;
                                if (tax != null)
                                {
                                    orderCharges.Tax1 = tax.Tax1;
                                    orderCharges.Tax2 = tax.Tax2;
                                    orderCharges.Tax3 = tax.Tax3;
                                }
                                else
                                {
                                    orderCharges.Tax1 = 0;
                                    orderCharges.Tax2 = 0;
                                    orderCharges.Tax3 = 0;
                                }
                                orderCharges.AdditionalChargeId = additionalChrg.Id;
                                orderCharges.CompanyId = order.CompanyId;
                                orderCharges.OrderId = order.Id;
                                orderCharges.StoreId = order.StoreId;
                                db.OrderCharges.Add(orderCharges);
                                db.SaveChanges();
                            }
                            JArray kotObj = ord.KOT;
                            dynamic kotJson = kotObj.ToList();
                            foreach (var kotitm in kotJson)
                            {
                                KOT kot = kotitm.ToObject<KOT>();
                                kot.KOTStatusId = 1;
                                kot.OrderId = OrderId;
                                kot.KOTNo = kotitm.KOTNo;
                                kot.CreatedDate = DateTime.Now;
                                kot.ModifiedDate = DateTime.Now;
                                kot.CompanyId = order.CompanyId;
                                kot.StoreId = order.StoreId;
                                if (kot.KOTGroupId == -1)
                                {
                                    kot.KOTGroupId = null;
                                }
                                db.KOTs.Add(kot);
                                db.SaveChanges();
                                JArray itemObj = kotitm.item;
                                dynamic itemJson = itemObj.ToList();
                                foreach (var item in itemJson)
                                {
                                    item.Product = null;
                                    OrderItem ordItem = item.ToObject<OrderItem>();
                                    ordItem.IsStockUpdate = false;
                                    ordItem.OrderId = order.Id;
                                    ordItem.KOTId = kot.Id;
                                    ordItem.CategoryId = db.Products.Find(ordItem.ProductId).CategoryId;
                                    ordItem.TotalAmount = item.TotalAmount;
                                    dynamic optionjson = new JObject();
                                    optionjson.quantity = ordItem.Quantity;
                                    optionjson.amount = ordItem.TotalAmount;
                                    optionjson.key = "";
                                    optionjson.options = new JArray();
                                    foreach (var optGrp in item.OptionGroup)
                                    {
                                        if (optGrp.OptionGroupType == 1)
                                        {
                                            foreach (var opt in optGrp.Option)
                                            {
                                                dynamic optjson = new JObject();
                                                optjson.Id = opt.Id;
                                                optionjson.options.Add(optjson);
                                                optionjson.key = optionjson.key + opt.Id.ToString() + "-";
                                            }
                                        }
                                    }
                                    ordItem.OptionJson = JsonConvert.SerializeObject(optionjson);
                                    db.OrderItems.Add(ordItem);
                                    db.SaveChanges();
                                    foreach (var optGrp in item.OptionGroup)
                                    {
                                        foreach (var opt in optGrp.Option)
                                        {
                                            OrdItemOptions ordItemOpt = opt.ToObject<OrdItemOptions>();
                                            ordItemOpt.Id = 0;
                                            ordItemOpt.OrderItemId = ordItem.Id;
                                            ordItemOpt.OptionId = opt.Id;
                                            db.OrdItemOptions.Add(ordItemOpt);
                                            db.SaveChanges();
                                        }
                                    }
                                }
                            }
                            var room = order.StoreId + "/" + order.CompanyId;
                            await _hubContext.Clients.Group(room).SendAsync("kot", order.Id);
                            message = "Order Placed Successfully";
                        }
                        else
                        {
                            message = "It is a Duplicate Order";
                            OrderLog orderLog = new OrderLog();
                            orderLog.CompanyId = companyid;
                            orderLog.StoreId = storeid;
                            orderLog.Payload = payload;
                            orderLog.Error = "It is a Duplicate Order";
                            orderLog.LoggedDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                            db.OrderLogs.Add(orderLog);
                            db.SaveChanges();
                        }
                    }
                    dbContextTransaction.Commit();
                    int deviceId = db.Devices.Where(s => s.CompanyId == companyid).Select(s => s.Id).FirstOrDefault();
                    if (deviceId != 0) ValuesController.fcm(db, deviceId);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    OrderLog orderLog = new OrderLog();
                    orderLog.CompanyId = companyid;
                    orderLog.StoreId = storeid;
                    orderLog.Payload = payload;
                    orderLog.Error = JsonConvert.SerializeObject(new Exception(e.Message, e.InnerException));
                    orderLog.LoggedDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                    db.OrderLogs.Add(orderLog);
                    db.SaveChanges();
                    var error = new
                    {
                        error = new Exception(e.Message, e.InnerException),
                        status = 0,
                        msg = "Something went wrong  Contact our service provider"
                    };
                    connection.Close();
                    return Json(error);
                }
                var response = new
                {
                    status = 200,
                    OrderId,
                    msg = message
                };
                connection.Close();
                return Json(response);
            }
        }

        //[HttpGet("saveorder")]
        //[EnableCors("AllowOrigin")]
        //public void saveorder(JObject order)
        //{
        //    try
        //    {
                
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
        [HttpGet("getAdvancedOrders")]
        [EnableCors("AllowOrigin")]
        public IActionResult getAdvancedOrders(int storeid)
        {
            try
            {
                var orders = db.Orders.Where(x => x.StoreId == storeid && !x.Closed).Select(x => x.OrderJson).ToList();
                var response = new
                {
                    status = 200,
                    orders = orders
                };
                return Json(response);
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

        [HttpPost("UpdateKotStatus")]
        [EnableCors("AllowOrigin")]
        public IActionResult UpdateKotStatus([FromBody]JObject objData)
        {
            try
            {
                dynamic jsonObj = objData;
                int status = jsonObj.changeStatus;
                int currentStatus = jsonObj.currentStatus;
                int kotId = jsonObj.kotId;
                var kot = db.KOTs.Find(kotId);
                kot.KOTStatusId = status;
                db.Entry(kot).State = EntityState.Modified;
                var orderItems = db.OrderItems.Where(o => o.KOTId == kotId).ToList();
                foreach (var orderItem in orderItems)
                {
                    if (currentStatus == orderItem.StatusId)
                    {
                        orderItem.StatusId = status;
                        db.Entry(orderItem).State = EntityState.Modified;
                    }
                }
                db.SaveChanges();
                var response = new
                {
                    status = 200,
                    msg = "Status Updated Successfully"
                };
                return Json(response);
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
        [HttpPost("UpdateOrder")]
        [EnableCors("AllowOrigin")]
        public async Task<IActionResult> UpdateOrder([FromForm]string ordData)
        {
            try
            {
                dynamic orderJson = JsonConvert.DeserializeObject(ordData);
                foreach (var ord in orderJson)
                {
                    int OrderId = 0;
                    string invoiceno = ord.InvoiceNo;
                    if (ord.OrderId != null)
                    {
                        OrderId = ord.OrderId;
                    }
                    else
                    {
                        OrderId = db.Orders.Where(x => x.InvoiceNo == invoiceno).AsNoTracking().FirstOrDefault().Id;
                    }
                    double oldpaidamt = db.Orders.Where(x => x.Id == OrderId).AsNoTracking().FirstOrDefault().PaidAmount;
                    if (ord.PaidAmount > oldpaidamt)
                    {
                        if (ord.PaymentTypeId == 5)
                        {
                            foreach (var trans in ord.splitPayment)
                            {
                                Transaction transaction = ord.ToObject<Transaction>();
                                transaction.OrderId = OrderId;
                                transaction.Amount = Math.Abs(oldpaidamt - ord.PaidAmount.ToObject<double>());
                                transaction.PaymentTypeId = trans.PaymentId;
                                transaction.TranstypeId = 1;
                                transaction.CustomerId = ord.CustomerId;
                                transaction.TransDate = ord.OrderedDate;
                                transaction.TransDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                                db.Transactions.Add(transaction);
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            Transaction transaction = ord.ToObject<Transaction>();
                            transaction.Id = 0;
                            transaction.OrderId = OrderId;
                            transaction.Amount = Math.Abs(oldpaidamt - ord.PaidAmount.ToObject<double>());
                            transaction.PaymentTypeId = ord.PaymentTypeId;
                            transaction.TranstypeId = 1;
                            transaction.CustomerId = ord.CustomerId;
                            transaction.TransDateTime = ord.OrderedDateTime;
                            transaction.TransDate = ord.OrderedDate;
                            db.Transactions.Add(transaction);
                            db.SaveChanges();
                        }
                    }
                    if (ord.PaidAmount > ord.BillAmount.ToObject<double>())
                    {
                        var transaction = new Transaction();
                        transaction.Amount = ord.PaidAmount - ord.BillAmount;
                        transaction.CompanyId = ord.CompanyId;
                        transaction.CustomerId = ord.CustomerId;
                        transaction.OrderId = OrderId;
                        transaction.PaymentTypeId = 1;
                        transaction.StoreId = ord.StoreId;
                        transaction.TranstypeId = 2;
                        transaction.TransDateTime = DateTime.Now;
                        transaction.TransDate = DateTime.Now;
                        transaction.UserId = ord.UserId;
                        db.Transactions.Add(transaction);
                        db.SaveChanges();
                        ord.PaidAmount = ord.BillAmount;
                    }
                    if (ord.OrderStatusId == -1)
                    {
                        var transaction = new Transaction();
                        transaction.Amount = ord.PaidAmount;
                        transaction.CompanyId = ord.CompanyId;
                        transaction.CustomerId = ord.CustomerId;
                        transaction.OrderId = OrderId;
                        transaction.PaymentTypeId = 1;
                        transaction.StoreId = ord.StoreId;
                        transaction.TranstypeId = 2;
                        transaction.TransDateTime = DateTime.Now;
                        transaction.TransDate = DateTime.Now;
                        transaction.UserId = ord.UserId;
                        db.Transactions.Add(transaction);
                        db.SaveChanges();
                        ord.RefundAmount = ord.BillAmount;
                    }
                    Order order = ord.ToObject<Order>();
                    order.Id = OrderId;
                    order.OrderJson = JsonConvert.SerializeObject(ord);
                    order.ModifiedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                    if(ord.changelevelid == 2)
                    {
                        var room = order.StoreId + "/" + order.CompanyId;
                        await _hubContext.Clients.Group(room).SendAsync("orderupdated", order.Id);
                    }
                    if (ord.IsAdvanceOrder.ToObject<bool>())
                    {
                        var orderitems = db.OrderItems.Where(x => x.OrderId == OrderId).ToList();
                        foreach (var oi in orderitems)
                        {
                            db.OrdItemOptions.RemoveRange(db.OrdItemOptions.Where(x => x.OrderItemId == oi.Id));
                            db.SaveChanges();
                        }
                        db.OrderItems.RemoveRange(db.OrderItems.Where(x => x.OrderId == OrderId));
                        db.KOTs.RemoveRange(db.KOTs.Where(x => x.OrderId == OrderId));
                        db.SaveChanges();
                    }
                    var kots = db.KOTs.Where(x => x.OrderId == order.Id).ToList();
                    foreach (var nkot in ord.KOT)
                    {
                        var newKot = true;
                        foreach (var lkot in kots)
                        {
                            if (nkot.KOTNo == lkot.KOTNo)
                            {
                                newKot = false;
                                break;
                            }
                        }
                        if (newKot)
                        {
                            KOT kot = nkot.ToObject<KOT>();
                            kot.KOTStatusId = 1;
                            kot.OrderId = order.Id;
                            kot.CreatedDate = DateTime.Now;
                            kot.ModifiedDate = DateTime.Now;
                            kot.CompanyId = order.CompanyId;
                            kot.StoreId = order.StoreId;
                            if (kot.KOTGroupId == -1)
                            {
                                kot.KOTGroupId = null;
                            }
                            db.KOTs.Add(kot);
                            db.SaveChanges();
                            JArray itemObj = nkot.item;
                            dynamic itemJson = itemObj.ToList();
                            foreach (var item in itemJson)
                            {
                                item.Product = null;
                                OrderItem ordItem = item.ToObject<OrderItem>();
                                ordItem.OrderId = order.Id;
                                ordItem.KOTId = kot.Id;
                                ordItem.Price = ordItem.Quantity * ordItem.Price;
                                db.OrderItems.Add(ordItem);
                                db.SaveChanges();
                                foreach (var optGrp in item.OptionGroup)
                                {
                                    foreach (var opt in optGrp.Option)
                                    {
                                        OrdItemOptions ordItemOpt = opt.ToObject<OrdItemOptions>();
                                        ordItemOpt.Id = 0;
                                        ordItemOpt.OrderItemId = ordItem.Id;
                                        ordItemOpt.OptionId = opt.Id;
                                        db.OrdItemOptions.Add(ordItemOpt);
                                        db.SaveChanges();
                                    }
                                }
                            }
                            var room = order.StoreId + "/" + order.CompanyId;
                            await _hubContext.Clients.Group(room).SendAsync("newkot", kot.Id);
                        }
                        else
                        {
                            if(nkot.dirtystatus == 1)
                            {
                                int kotid = kots.Where(x => x.KOTNo == nkot.KOTNo).FirstOrDefault().Id;
                                var room = order.StoreId + "/" + order.CompanyId;
                                await _hubContext.Clients.Group(room).SendAsync("updatedkot", kotid);
                            }
                        }
                    }
                    //if (ord.IsAdvanceOrder)
                    //{
                    //    foreach (var nkot in ord.KOT)
                    //    {
                    //        foreach (var item in nkot.item)
                    //        {
                    //            int productId = item.ProductId;
                    //            OrderItem orderItem = db.OrderItems.Where(x => x.OrderId == OrderId && x.ProductId == productId).FirstOrDefault();
                    //            if (orderItem != null)
                    //            {
                    //                orderItem.Price = item.Price;
                    //                orderItem.Quantity = item.Quantity;
                    //                orderItem.ComplementryQty = item.ComplementryQty;
                    //                orderItem.DiscAmount = item.DiscAmount;
                    //                db.Entry(orderItem).State = EntityState.Modified;
                    //                db.SaveChanges();
                    //            }
                    //            else
                    //            {
                    //                orderItem = item.ToObject<OrderItem>();
                    //                db.OrderItems.Add(orderItem);
                    //                db.SaveChanges();
                    //            }
                    //        }
                    //        var orderitems = db.OrderItems.Where(x => x.OrderId == OrderId).ToList();
                    //        foreach (var orditem in orderitems)
                    //        {
                    //            bool notexist = true;
                    //            foreach (var liveitem in nkot.item)
                    //            {
                    //                if(liveitem.ProductId == orditem.ProductId)
                    //                {
                    //                    notexist = false;
                    //                    break;
                    //                }
                    //            }
                    //            if (notexist)
                    //            {
                    //                db.OrderItems.Remove(orditem);
                    //                db.SaveChanges();
                    //            }
                    //        }
                    //    }
                    //}
                    //int kotsLen1 = db.KOTs.Where(x => x.OrderId == order.Id).Count();
                    //int kotsLen2 = ord.KOT.Count;
                    //int lenDiff = kotsLen2 - kotsLen1;
                    //if (lenDiff > 0)
                    //{
                    //    for (int i = lenDiff; i < ord.KOT.Count; i++)
                    //    {
                    //        KOT kot = ord.KOT[i].ToObject<KOT>();
                    //        kot.KOTStatusId = 1;
                    //        kot.OrderId = order.Id;
                    //        kot.CreatedDate = DateTime.Now;
                    //        kot.ModifiedDate = DateTime.Now;
                    //        kot.CompanyId = order.CompanyId;
                    //        kot.StoreId = order.StoreId;
                    //        if (kot.KOTGroupId == -1)
                    //        {
                    //            kot.KOTGroupId = null;
                    //        }
                    //        db.KOTs.Add(kot);
                    //        db.SaveChanges();
                    //        JArray itemObj = ord.KOT[i].item;
                    //        dynamic itemJson = itemObj.ToList();
                    //        foreach (var item in itemJson)
                    //        {
                    //            item.Product = null;
                    //            OrderItem ordItem = item.ToObject<OrderItem>();
                    //            ordItem.OrderId = order.Id;
                    //            ordItem.KOTId = kot.Id;
                    //            db.OrderItems.Add(ordItem);
                    //            db.SaveChanges();
                    //            foreach (var optGrp in item.OptionGroup)
                    //            {
                    //                foreach (var opt in optGrp.Option)
                    //                {
                    //                    OrdItemOptions ordItemOpt = opt.ToObject<OrdItemOptions>();
                    //                    ordItemOpt.Id = 0;
                    //                    ordItemOpt.OrderItemId = ordItem.Id;
                    //                    ordItemOpt.OptionId = opt.Id;
                    //                    db.OrdItemOptions.Add(ordItemOpt);
                    //                    db.SaveChanges();
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                }
                var response = new
                {
                    status = 200,
                    msg = "Order Updated Successfully"
                };
                return Json(response);
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

        [HttpPost("UpdateItemStatus")]
        [EnableCors("AllowOrigin")]
        public IActionResult UpdateItemStatus([FromBody]JObject objData)
        {
            try
            {
                dynamic jsonObj = objData;

                int status = jsonObj.changeStatus;
                JArray idArr = jsonObj.itemId;

                int[] itemIds = idArr.ToObject<int[]>();
                foreach (int itemId in itemIds)
                {
                    OrderItem ordItem = db.OrderItems.Find(itemId);
                    ordItem.StatusId = status;
                    db.Entry(ordItem).State = EntityState.Modified;
                    db.SaveChanges();
                }
                var response = new
                {
                    status = 200,
                    msg = "Status Updated Successfully"
                };

                return Json(response);
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
        [HttpPost("StatusUpdate")]
        [EnableCors("AllowOrigin")]
        public IActionResult StatusUpdate([FromForm]string data)
        {
            try
            {
                dynamic orderJson = JsonConvert.DeserializeObject(data);
                string fields = "";
                string condtions = "";
                string TableName = orderJson.table;
                JArray status = orderJson.data;
                //foreach (var property in PropertiesOfType<string>(orderJson.data))
                //{
                //    var name = property.Key;
                //    var val = property.Value;
                //    }
                dynamic itemJson = status.ToList();
                for (int i = 0; i < status.ToList().Count(); i++)
                {
                    if (i == status.ToList().Count() - 1)
                    {
                        string key = itemJson[i].Key;
                        fields += itemJson[i].key + "=" + itemJson[i].value;
                    }
                    else
                    {
                        fields += itemJson[i].key + "=" + itemJson[i].value + ",";
                    }
                }
                JArray cond = orderJson.cond;
                dynamic condition = cond.ToList();
                var last = status.Last();
                for (int i = 0; i < cond.ToList().Count(); i++)
                {
                    if (i == cond.ToList().Count() - 1)
                    {
                        condtions += condition[i].key + "=" + condition[i].value;
                    }
                    else
                    {
                        condtions += condition[i].key + "=" + condition[i].value + "AND";
                    }
                }
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                SqlCommand cmd = sqlCon.CreateCommand();
                cmd.CommandText = "UPDATE " + TableName +
                                  " SET " + fields +
                                  " WHERE " + condtions;

                cmd.ExecuteNonQuery();

                sqlCon.Close();
                return Ok();
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
        [HttpPost("aggregatorOrder")]
        [Authorize(Roles = "Administrator")]
        [EnableCors("AllowOrigin")]
        public IActionResult aggregatorOrder([FromBody]JObject data)
        {
            try
            {
                dynamic Json = data;
                int storeId = Json.order.store.merchant_ref_id;
                int companyId = db.Stores.Find(storeId).CompanyId;
                string room = storeId.ToString() + '/' + companyId.ToString();
                string phone = Json.customer.phone;
                string socketdata = JsonConvert.SerializeObject(Json);
                //var option = new IO.Options()
                //{
                //    QueryString = "Type=Desktop",
                //    Timeout = 5000,
                //    ReconnectionDelay = 5000,
                //    Reconnection = false,
                //    //Transports = Quobject.Collections.Immutable.ImmutableList.Create<string>(Quobject.EngineIoClientDotNet.Client.Transports.WebSocket)
                //};
                //var sckt = IO.Socket("https://biz1socket.azurewebsites.net/");
                //var sckt = IO.Socket("http://localhost:3000/");
                //sckt.Emit("join", room);
                //sckt.Emit("order", socketdata);
                //sckt.On("disc", () =>
                //{
                //    sckt.Disconnect();
                //});
                var response = new
                {
                    status = 200,
                    msg = "Order Recieved!"
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
                return StatusCode(500, error);
            }
        }
        [HttpPost("AcceptOnlineOrder")]
        [EnableCors("AllowOrigin")]
        public IActionResult AcceptOnlineOrder([FromForm]string ordData, int compId, int storeId)
        {
            try
            {
                dynamic orderJson = JsonConvert.DeserializeObject(ordData);
                foreach (var ord in orderJson)
                {
                    long uporderid = ord.UPOrderId;
                    Order oldorder = db.Orders.Where(x => x.UPOrderId == uporderid).FirstOrDefault();
                    if(oldorder == null)
                    {
                        if (ord.OrderStatusId == -1)
                        {
                            Order order = ord.ToObject<Order>();
                            var temp_json = new { placed=0};
                            if (order.OrderStatusDetails == null) { order.OrderStatusDetails = JsonConvert.SerializeObject(temp_json); };
                            order.CustomerData = JsonConvert.SerializeObject(ord.CustomerDetails);
                            if (order.Source == "swiggy")
                            {
                                order.SourceId = 2;
                            }
                            else if (order.Source == "zomato")
                            {
                                order.SourceId = 3;
                            }
                            else if (order.Source == "foodpanda")
                            {
                                order.SourceId = 4;
                            }
                            db.Orders.Add(order);
                            db.SaveChanges();
                            OrderId = order.Id;
                        }
                        else
                        {
                            Order order = ord.ToObject<Order>();
                            var temp_json = new { placed = 0 };
                            if (order.OrderStatusDetails == null) { order.OrderStatusDetails = JsonConvert.SerializeObject(temp_json); };
                            order.CustomerData = JsonConvert.SerializeObject(ord.CustomerDetails);
                            if (order.Source == "swiggy")
                            {
                                order.SourceId = 2;
                            }
                            else if (order.Source == "zomato")
                            {
                                order.SourceId = 3;
                            }
                            else if (order.Source == "foodpanda")
                            {
                                order.SourceId = 4;
                            }
                            db.Orders.Add(order);
                            db.SaveChanges();
                            Transaction transaction = new Transaction();
                            transaction.OrderId = order.Id;
                            transaction.Amount = order.PaidAmount;
                            transaction.PaymentTypeId = 4;
                            transaction.TranstypeId = 1;
                            transaction.TransDateTime = order.OrderedDateTime;
                            transaction.TransDate = order.OrderedDate;
                            transaction.UserId = order.UserId;
                            transaction.StoreId = order.StoreId;
                            transaction.CompanyId = order.CompanyId;
                            db.Transactions.Add(transaction);
                            db.SaveChanges();
                            OrderId = order.Id;
                            JArray AdditionalChrgObj = ord.AdditionalCharge;
                            dynamic AdditionalCharges = AdditionalChrgObj.ToList();
                            foreach (var additionalChrg in AdditionalCharges)
                            {
                                string additionalChrgname = additionalChrg.title;
                                AdditionalCharges additionalCharges = db.AdditionalCharges.Where(x => x.Description == additionalChrgname && x.CompanyId == order.CompanyId).FirstOrDefault();
                                OrderCharges orderCharges = new OrderCharges();
                                TaxGroup tax = db.TaxGroups.Find(additionalCharges.TaxGroupId);
                                orderCharges.AdditionalChargeId = additionalCharges.Id;
                                orderCharges.ChargeAmount = additionalChrg.value;
                                orderCharges.ChargePercentage = 0;
                                if (tax != null)
                                {
                                    orderCharges.Tax1 = tax.Tax1;
                                    orderCharges.Tax2 = tax.Tax2;
                                    orderCharges.Tax3 = tax.Tax3;
                                }
                                else
                                {
                                    orderCharges.Tax1 = 0;
                                    orderCharges.Tax2 = 0;
                                    orderCharges.Tax3 = 0;
                                }
                                orderCharges.CompanyId = order.CompanyId;
                                orderCharges.OrderId = order.Id;
                                orderCharges.StoreId = order.StoreId;
                                db.OrderCharges.Add(orderCharges);
                                db.SaveChanges();
                            }
                            JArray kotObj = ord.KOT;
                            dynamic kotJson = kotObj.ToList();
                            foreach (var kotitm in kotJson)
                            {
                                KOT kot = kotitm.ToObject<KOT>();
                                kot.KOTStatusId = 1;
                                kot.OrderId = OrderId;
                                kot.KOTNo = kotitm.KOTNo;
                                kot.CreatedDate = DateTime.Now;
                                kot.ModifiedDate = DateTime.Now;
                                kot.CompanyId = order.CompanyId;
                                kot.StoreId = order.StoreId;
                                if (kot.KOTGroupId == -1)
                                {
                                    kot.KOTGroupId = null;
                                }
                                db.KOTs.Add(kot);
                                db.SaveChanges();
                                JArray itemObj = kotitm.item;
                                dynamic itemJson = itemObj.ToList();
                                foreach (var item in itemJson)
                                {
                                    item.Product = null;
                                    OrderItem ordItem = item.ToObject<OrderItem>();
                                    ordItem.OrderId = order.Id;
                                    ordItem.KOTId = kot.Id;
                                    db.OrderItems.Add(ordItem);
                                    db.SaveChanges();
                                    foreach (var opt in item.Options)
                                    {
                                        OrdItemOptions ordItemOpt = opt.ToObject<OrdItemOptions>();
                                        ordItemOpt.OrderItemId = ordItem.Id;
                                        ordItemOpt.OptionId = opt.OptionId;
                                        db.OrdItemOptions.Add(ordItemOpt);
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        OrderId = 0;
                    }
                }
            }
            catch (Exception e)
            {
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 0,
                    msg = "Something went wrong  Contact our service provider"
                };
                return StatusCode(500, error);
            }
            var response = new
            {
                status = 200,
                OrderId,
                msg = "Order Placed Successfully"
            };

            return Json(response);
        }
        [HttpGet("getorderlogs")]
        [EnableCors("AllowOrigin")]
        public IActionResult getorderlogs(int storeid, int companyid)
        {
            try
            {
                List<OrderLog> orderLogs = new List<OrderLog>();
                orderLogs = db.OrderLogs.Where(x => x.StoreId == storeid && x.CompanyId == companyid).ToList();
                return Json(orderLogs);
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
        [HttpGet("customerdata")]
        [EnableCors("AllowOrigin")]
        public IActionResult customerdata(int storeid, int companyid)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();

                SqlCommand cmd = new SqlCommand("dbo.customerdata", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);

                DataTable table = ds.Tables[0];
                return Json(table);
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
        [HttpGet("getOrderById")]
        [EnableCors("AllowOrigin")]
        public IActionResult getOrderById(int orderid)
        {
            try
            {
                Order order = db.Orders.Find(orderid);
                return Json(order);
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
        [HttpGet("getOtherStoreOrders")]
        [EnableCors("AllowOrigin")]
        public IActionResult getOtherStoreOrders(int storeid)
        {
            try
            {
                int[] pendingStatusIds = { 0, 1, 2, 3, 4 };
                int[] advancedOrderTypeIds = { 2, 3, 4 };
                
                DateTime today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                List<Order> order = db.Orders.Where(o => (o.OrderedDate == today || pendingStatusIds.Contains(o.OrderStatusId) || o.BillAmount != o.PaidAmount) && o.DeliveryStoreId == storeid && advancedOrderTypeIds.Contains(o.OrderTypeId)).ToList();
                return Json(order);
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
        [HttpGet("getUnfinishedOrders")]
        [EnableCors("AllowOrigin")]
        public IActionResult getUnfinishedOrders(int storeid)
        {
            try
            {
                int[] pendingStatusIds = { 0,1,2,3,4 };
                int[] advancedOrderTypeIds = { 2,3,4 };

                DateTime today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                List<Order> order = db.Orders.Where(o => (o.OrderedDate == today || pendingStatusIds.Contains(o.OrderStatusId) || o.BillAmount != o.PaidAmount) && o.StoreId == storeid && advancedOrderTypeIds.Contains(o.OrderTypeId) && o.OrderJson != null).ToList();
                return Json(order);
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
        [HttpGet("onlineorderstatuschange")]
        [EnableCors("AllowOrigin")]
        public IActionResult onlineorderstatuschange(int orderid, int statusid)
        {
            try
            {
                Order order = db.Orders.Find(orderid);
                order.OrderStatusId = statusid;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                var response = new
                {
                    status = 200,
                    OrderId,
                    msg = "Order Placed Successfully"
                };
                return Json(response);
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
        [HttpGet("Getpendingorder")]
        public IActionResult Getpendingorder(int CompanyId, int storeId)
        {
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("dbo.PendingOrder", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@CompanyId", CompanyId));
            cmd.Parameters.Add(new SqlParameter("@storeId ", storeId));

            DataSet ds = new DataSet();
            SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
            sqlAdp.Fill(ds);
            DataTable table = ds.Tables[0];
            var response = new
            {
                Orders = ds.Tables[0]
            };
            return Ok(response);
        }

        [HttpGet("GetOrderId")]
        public IActionResult GetOrderId(int orderid)
        {
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("dbo.OrderItemsbyOrderid", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@orderid", orderid));

            DataSet ds = new DataSet();
            SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
            sqlAdp.Fill(ds);
            DataTable table = ds.Tables[0];
            var response = new
            {
                Orders = ds.Tables[0]
            };
            return Ok(response);
        }

        [HttpGet("GetTransactionId")]
        public IActionResult GetTransactionId(int orderid)
        {
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("dbo.TransactionByOrderId", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@orderid", orderid));

            DataSet ds = new DataSet();
            SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
            sqlAdp.Fill(ds);
            DataTable table = ds.Tables[0];
            var response = new
            {
                Transactions = ds.Tables[0]
            };
            return Ok(response);
        }
        [HttpGet("GetKotInspect")]
        public IActionResult GetKotInspect(int storeId, int CompanyId, int MinQty, int RemainingHrs)
        {
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("dbo.KOTInspect", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@storeId", @storeId));
            cmd.Parameters.Add(new SqlParameter("@CompanyId", @CompanyId));
            cmd.Parameters.Add(new SqlParameter("@MinQty", @MinQty));
            cmd.Parameters.Add(new SqlParameter("@RemainingHrs", @RemainingHrs));

            DataSet ds = new DataSet();
            SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
            sqlAdp.Fill(ds);
            DataTable table = ds.Tables[0];
            var response = new
            {
                Orders = ds.Tables[0]
            };
            return Ok(response);
        }


        //[HttpGet("testTask")]
        //public IActionResult testTask(int orderid)
        //{
        //    _queue.QueueBackgroundWorkItem(async token =>
        //    {
        //        using (var scope = _serviceScopeFactory.CreateScope())
        //        {
        //            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
        //            sqlCon.Open();
        //            SqlCommand cmd = new SqlCommand("dbo.SaveUPOrder_", sqlCon);
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            cmd.Parameters.Add(new SqlParameter("@uporderid", orderid));

        //            DataSet ds = new DataSet();
        //            SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
        //            sqlAdp.Fill(ds);
        //        }
        //    });
        //    return Ok("In progress..");
        //}
        [HttpGet("GetKOTInspectdetail")]
        public IActionResult GetKOTInspectdetail(int orderid)
        {
            //SqlConnection sqlCon = new SqlConnection("server=(LocalDb)\\MSSQLLocalDB; database=Biz1POS;Trusted_Connection=True;");
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("dbo.KOTInspectDetail", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@orderid", orderid));

            DataSet ds = new DataSet();
            SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
            sqlAdp.Fill(ds);

            DataTable table = ds.Tables[0];

            string[] catStr = new String[20];
            for (int k = 0; k < ds.Tables.Count; k++)
            {
                for (int j = 0; j < ds.Tables[k].Rows.Count; j++)
                {
                    catStr[k] += ds.Tables[k].Rows[j].ItemArray[0].ToString();
                }
                if (catStr[k] == null)
                {
                    catStr[k] = "";
                }
            }
            object kOTs = JsonConvert.DeserializeObject(catStr[0]);
            return Ok(kOTs);
        }
    }

    public class OrderPayload
    {
        public string OrderJson { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}

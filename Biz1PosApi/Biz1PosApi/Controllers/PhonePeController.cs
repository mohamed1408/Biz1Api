using Biz1BookPOS.Models;
using Biz1PosApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    public class PhonePeController : Controller
    {
        private POSDbContext db;
        private static readonly HttpClient httpClient = new HttpClient();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        private static TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        public IConfiguration Configuration { get; }

        public PhonePeController(POSDbContext contextOptions, IConfiguration configuration)
        {
            db = contextOptions;
            Configuration = configuration;
        }
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        //MASTER TEST
        [HttpPost("PhonePePay")]
        public IActionResult PhonePePay([FromBody]PayPayload pyload)
        {
            try
            {
                Biz1BookPOS.Models.Transaction transaction = new Biz1BookPOS.Models.Transaction();
                transaction.Amount = pyload.amount;
                transaction.CompanyId = pyload.companyid;
                transaction.CustomerId = pyload.customerid;
                transaction.ModifiedDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                transaction.OrderId = pyload.orderid;
                transaction.PaymentStatusId = 0;
                transaction.StoreId = pyload.storeid;
                transaction.PaymentTypeId = 8;
                transaction.TransDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                transaction.TransDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                transaction.TranstypeId = 1;
                transaction.Notes = "";
                db.Transactions.Add(transaction);
                db.SaveChanges();

                transaction.Notes = "T" + transaction.Id.ToString() + "S" + transaction.StoreId.ToString();
                db.Entry(transaction).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();
                string key = "bff43f70-7033-4740-ba04-53dd4958a6de";
                string salt = "1";

                PhonePePayload payload = new PhonePePayload();
                payload.merchantId = "FBCAKESONLINE";
                payload.merchantTransactionId = transaction.Notes;
                payload.merchantUserId = "MUID2209";
                payload.amount = (int)pyload.amount * 100;
                payload.redirectUrl = pyload.redirecturl;
                payload.redirectMode = "POST";
                payload.callbackUrl = "https://biz1pos.azurewebsites.net/api/PhonePe/PaymentStatusCallback";
                payload.mobileNumber = pyload.mobilenumber;
                payload.paymentInstrument = new PaymentInstrument();
                payload.paymentInstrument.type = "PAY_PAGE";

                string base64Payload = Base64Encode(JsonConvert.SerializeObject(payload, Formatting.Indented));
                string sha256hash = ComputeSha256Hash(base64Payload + "/pg/v1/pay" + key) + "###" + salt;
                dynamic body = new
                {
                    request = base64Payload
                };
                var client = new RestClient("https://api.phonepe.com/apis/hermes/pg/v1/pay");
                var request = new RestRequest(Method.POST);
                request.AddHeader("accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("X-VERIFY", sha256hash);
                request.AddParameter("application/json", JsonConvert.SerializeObject(body), ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return Ok(response.Content);
            }
            catch (Exception e)
            {

                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 500,
                    msg = "Something went wrong  Contact our service provider"
                };

                return StatusCode(500, error);
            }
        }

        [HttpGet("PhonePeDashboard")]
        public IActionResult PhonePeDashboard(int CompanyId, int StoreId)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.PhonePeDashboard", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@companyId", CompanyId));
                cmd.Parameters.Add(new SqlParameter("@storeId", StoreId));

                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                DataTable table = ds.Tables[0];


                var data = new
                {
                    Report = ds.Tables[0],

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
        [HttpPost("PaymentStatusCallback")]
        public IActionResult PaymentStatusCallback([FromBody] CBPayload payload)
        {
            try
            {
                OrderLog log = new OrderLog();
                log.StoreId = 22;
                log.CompanyId = 3;
                log.Error = payload.response;
                log.LoggedDateTime = DateTime.UtcNow;
                log.Payload = Base64Decode(payload.response);
                db.OrderLogs.Add(log);
                db.SaveChanges();

                string payload_str = Base64Decode(payload.response);
                dynamic payload_parse = JsonConvert.DeserializeObject<dynamic>(payload_str);
                string merchantTransactionId = (string)payload_parse.data.merchantTransactionId;
                int storeid = int.Parse(merchantTransactionId.Split('S')[1]);
                int transactionid = int.Parse(merchantTransactionId.Split('S')[0].Split('T')[1]);
                bool success = (bool)payload_parse.success;
                Biz1BookPOS.Models.Transaction transaction = db.Transactions.Where(x => x.StoreId == storeid && x.Id == transactionid).FirstOrDefault();
                transaction.PaymentStatusId = success ? 1 : -1;
                db.Entry(transaction).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();

                return Ok(log);
            }
            catch (Exception e)
            {

                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 500,
                    msg = "Something went wrong  Contact our service provider"
                };

                return StatusCode(500, error);
            }
        }

        [HttpPost("RefundCallback")]
        public IActionResult RefundCallback(int transactionid)
        {
            try
            {
                Biz1BookPOS.Models.Transaction transaction = db.Transactions.Find(transactionid);
                transaction.PaymentStatusId = 2;
                db.Entry(transaction).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();

                return Ok(transaction);
            }
            catch (Exception e)
            {

                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 500,
                    msg = "Something went wrong  Contact our service provider"
                };

                return StatusCode(500, error);
            }
        }
        [HttpPost("TransactionStatusCheck")]
        public IActionResult TransactionStatusCheck(string merchantTransactionId)
        {
            try
            {
                string key = "bff43f70-7033-4740-ba04-53dd4958a6de";
                string salt = "1";
                string merchantId = "FBCAKESONLINE";


                // string base64Payload = Base64Encode(JsonConvert.SerializeObject(payload, Formatting.Indented));
                string sha256hash = ComputeSha256Hash("/pg/v1/status/" + merchantId + "/" + merchantTransactionId + key) + "###" + salt;
                var client = new RestClient("https://api.phonepe.com/apis/hermes/pg/v1/status/" + merchantId + "/" + merchantTransactionId);
                var request = new RestRequest(Method.GET);
                request.AddHeader("accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("X-VERIFY", sha256hash);
                request.AddHeader("X-MERCHANT-ID", merchantId);
                // request.AddParameter("application/json", JsonConvert.SerializeObject(body), ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return Ok(JsonConvert.DeserializeObject(response.Content));
            }
            catch (Exception e)
            {

                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 500,
                    msg = "Something went wrong  Contact our service provider"
                };

                return StatusCode(500, error);
            }
        }

        [HttpPost("PaymentRefund")]
        public IActionResult PaymentRefund(int TransId)
        {
            try
            {
                var trans = db.Transactions.Where(x => x.Id == TransId).FirstOrDefault();

                string key = "bff43f70-7033-4740-ba04-53dd4958a6de";
                string salt = "1";

                Biz1BookPOS.Models.Transaction originalTransaction = db.Transactions.Find(TransId);
                Biz1BookPOS.Models.Transaction transaction = new Biz1BookPOS.Models.Transaction();
                transaction.Amount = originalTransaction.Amount;
                transaction.CompanyId = originalTransaction.CompanyId;
                transaction.CustomerId = originalTransaction.CustomerId;
                transaction.ModifiedDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                transaction.OrderId = originalTransaction.OrderId;
                transaction.PaymentStatusId = 0;
                transaction.StoreId = originalTransaction.StoreId;
                transaction.PaymentTypeId = 8;
                transaction.TransDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                transaction.TransDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                transaction.TranstypeId = 2;
                transaction.Notes = "";
                db.Transactions.Add(transaction);
                db.SaveChanges();

                transaction.Notes = "T" + transaction.Id.ToString() + "S" + transaction.StoreId.ToString();
                db.Entry(transaction).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();

                PhonePeRefund pyload = new PhonePeRefund();
                pyload.merchantId = "FBCAKESONLINE";
                pyload.merchantUserId = "MUID2209";
                pyload.originalTransactionId = trans.Notes;
                pyload.merchantTransactionId = transaction.Notes;
                pyload.amount = (int)trans.Amount * 100;
                pyload.callbackUrl = "https://biz1pos.azurewebsites.net/api/PhonePe/PaymentStatusCallback";

                string base64Payload = Base64Encode(JsonConvert.SerializeObject(pyload, Formatting.Indented));
                string sha256hash = ComputeSha256Hash(base64Payload + "/pg/v1/refund" + key) + "###" + salt;
                dynamic body = new
                {
                    request = base64Payload
                };
                var client = new RestClient("https://api.phonepe.com/apis/hermes/pg/v1/refund");
                var request = new RestRequest(Method.POST);
                request.AddHeader("accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("X-VERIFY", sha256hash);
                request.AddParameter("application/json", JsonConvert.SerializeObject(body), ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                var res = new
                {
                    //trans,
                    //base64Payload,
                    //sha256hash,
                    response = JsonConvert.DeserializeObject(response.Content)
                };
                return Ok(res);

            }
            catch (Exception e)
            {

                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 500,
                    msg = "Something went wrong  Contact our service provider"
                };

                return StatusCode(500, error);
            }
        }
        
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string encodedString)
        {
            byte[] data = Convert.FromBase64String(encodedString);
            return Encoding.UTF8.GetString(data);
        }

        public static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // GET: PhonePeController
        public ActionResult Index()
        {
            return View();
        }

        // GET: PhonePeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PhonePeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PhonePeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: PhonePeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PhonePeController/Edit/5
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

        // GET: PhonePeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PhonePeController/Delete/5
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
    public class CBPayload
    {
        public string response { get; set; }
    }
    public class PayPayload
    {
        public double amount { get; set; }
        public int orderid { get; set; }
        public int storeid { get; set; }
        public int companyid { get; set; }
        public int customerid { get; set; }
        public string mobilenumber { get; set; }
        public string redirecturl { get; set; }
    }
    internal class PaymentInstrument
    {
        public string type { get; set; }
    }
    internal class PhonePePayload
    {
        public string merchantId { get; set; }
        public string merchantTransactionId { get; set; }
        public string merchantUserId { get; set; }
        public int amount { get; set; }
        public string redirectUrl { get; set; }
        public string redirectMode { get; set; }
        public string callbackUrl { get; set; }
        public string mobileNumber { get; set; }
        public PaymentInstrument paymentInstrument { get; set; }
    }
    internal class PhonePeRefund
    {
        public string merchantId { get; set; }
        public string merchantUserId { get; set; }
        public string originalTransactionId { get; set; }
        public string merchantTransactionId { get; set; }
        public int amount { get; set; }
        public string callbackUrl { get; set; }
    }
}

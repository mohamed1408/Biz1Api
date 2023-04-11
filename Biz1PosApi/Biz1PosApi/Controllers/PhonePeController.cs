using Biz1BookPOS.Models;
using Biz1PosApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
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

        public PhonePeController(POSDbContext contextOptions)
        {
            db = contextOptions;
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
                string key = "03ea1f3e-1fcf-498f-a0f8-655aa69ff7e7";
                string salt = "1";

                PhonePePayload payload = new PhonePePayload();
                payload.merchantId = "FBCAKESUAT";
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
                var client = new RestClient("https://api-preprod.phonepe.com/apis/merchant-simulator/pg/v1/pay");
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
        [HttpPost("PaymentStatusCallback")]
        public IActionResult PaymentStatusCallback([FromBody] CBPayload payload)
        {
            try
            {
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

                OrderLog log = new OrderLog();
                log.StoreId = 22;
                log.CompanyId = 3;
                log.Error = payload.response;
                log.LoggedDateTime = DateTime.UtcNow;
                log.Payload = Base64Decode(payload.response);
                db.OrderLogs.Add(log);
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
        [HttpPost("TransactionStatusCheck")]
        public IActionResult TransactionStatusCheck(string merchantId, string merchantTransactionId)
        {
            try
            {
                string key = "03ea1f3e-1fcf-498f-a0f8-655aa69ff7e7";
                string salt = "1";


                // string base64Payload = Base64Encode(JsonConvert.SerializeObject(payload, Formatting.Indented));
                string sha256hash = ComputeSha256Hash("/pg/v1/status/" + merchantId + "/" + merchantTransactionId + key) + "###" + salt;
                var client = new RestClient("https://api-preprod.phonepe.com/apis/merchant-simulator/pg/v1/status/" + merchantId + "/" + merchantTransactionId);
                var request = new RestRequest(Method.GET);
                request.AddHeader("accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("X-VERIFY", sha256hash);
                request.AddHeader("X-MERCHANT-ID", merchantId);
                // request.AddParameter("application/json", JsonConvert.SerializeObject(body), ParameterType.RequestBody);
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
}

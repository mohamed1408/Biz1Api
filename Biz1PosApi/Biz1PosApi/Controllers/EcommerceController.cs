using Biz1BookPOS.Models;
using Biz1PosApi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;


namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowOrigin")]
    public class EcommerceController : Controller
    {
        private POSDbContext db;
        private static TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        public static IHostingEnvironment _environment;
        private object fileUploader;

        public IConfiguration Configuration { get; }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public EcommerceController(POSDbContext contextOptions, IConfiguration configuration, IHostingEnvironment environment)
        {
            db = contextOptions;
            Configuration = configuration;
            _environment = environment;
        }

        // GET: EcommerceController
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet("viewuserdata")]
        public IActionResult Indexdataid(int id)
        {
            return Json(db.Customers.Where(x => x.Id == id).ToList());
        }

        // GET: EcommerceController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpGet("viewstoredata")]
        public IActionResult Indexstoredata(int id)
        {
            return Json(db.Stores.Where(x => x.Id == id).ToList());
        }

        // GET: EcommerceController/Details/5
        public ActionResult Detail(int id)
        {
            return View();
        }

        // GET: EcommerceController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EcommerceController/Create
        [HttpPost]
        [EnableCors("AllowOrigin")]
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

        // GET: EcommerceController/Edit/5
        [HttpGet("getproducts")]
        [EnableCors("AllowOrigin")]
        public IActionResult getproducts(int companyId)
        {
            var products = db.Products.Where(x => x.CompanyId == companyId).ToList();
            List<Option> options = new List<Option>();
            foreach (var option in db.Options.Where(x => x.CompanyId == 3))
            {
                if (db.ProductOptionGroups.Where(x => x.OptionGroupId == option.OptionGroupId).Any() && db.OptionGroups.Find(option.OptionGroupId).OptionGroupType == 1)
                {
                    int productid = db.ProductOptionGroups.Where(x => x.OptionGroupId == option.OptionGroupId).FirstOrDefault().ProductId;
                    if (products.Where(x => x.Id == productid).Count() > 0)
                    {
                        option.ProductId = productid;
                        options.Add(option);
                    }
                }
            }
            var data = new
            {
                categories = db.Categories.Where(x => x.CompanyId == companyId).ToList(),
                products = db.Products.Where(x => x.CompanyId == companyId).ToList(),
                options = options
            };
            return Ok(data);
        }

        [HttpGet("customer_login")]
        [EnableCors("AllowOrigin")]
        public IActionResult customer_login(string email, int companyId, string name,
            string password, string address, string phoneno, int storeid, string city)
        {
            Customer customer = new Customer();
            if (db.Customers.Where(x => x.Email == email).Any())
            {
                customer = db.Customers.Where(x => x.Email == email).FirstOrDefault();
            }

            else
            {
                customer.CompanyId = companyId;
                customer.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                customer.LastRedeemDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                customer.ModifiedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                customer.Email = email;
                customer.Name = name;
                customer.Password = password;
                customer.Address = address;
                customer.PhoneNo = phoneno;
                customer.StoreId = storeid;
                customer.City = city;
                db.Customers.Add(customer);
                db.SaveChanges();
            }

            string otp = send_otp_emailandphone(email, phoneno);

            customer.OTP = otp;
            db.Entry(customer).State = EntityState.Modified;
            db.SaveChanges();
            var response = new
            {
                status = 200,
                //customerid = customer.Id,
                //name = customer.Name,
                //storeid = customer.StoreId,
                //Phoneno= customer.PhoneNo

            };
            return Ok(response);
        }


        [HttpGet("verify_otp")]
        [EnableCors("AllowOrigin")]
        public IActionResult verify_otp(string email, string otp)
        {
            Customer customer = new Customer();
            customer = db.Customers.Where(x => x.Email == email).FirstOrDefault();
            if (otp == customer.OTP)
            {
                var response = new
                {
                    customer.Id,
                    status = 200,
                    msg = "OTP Verification Successfull.",
                    customerid = customer.Id,
                    name = customer.Name,
                    storeid = customer.StoreId,
                };
                return Ok(response);
            }
            else
            {
                var response = new
                {
                    status = 0,
                    msg = "OTP Verification Failed."
                };
                return Ok(response);
            }
        }

        [HttpPost("saveaddress")]
        [EnableCors("AllowOrigin")]
        public IActionResult saveaddress([FromForm] string Details)
        {
            dynamic address = JsonConvert.DeserializeObject(Details);
            CustomerAddress customerAddress = address.ToObject<CustomerAddress>();
            db.CustomerAddresses.Add(customerAddress);
            db.SaveChanges();
            var customeraddresses = db.CustomerAddresses.Where(x => x.CustomerId == customerAddress.CustomerId).ToList();
            foreach (var caddress in customeraddresses)
            {
                if (caddress.Id == customerAddress.Id)
                {
                    caddress.iscurrentaddress = true;
                }
                else
                {
                    caddress.iscurrentaddress = false;
                }
                db.Entry(caddress).State = EntityState.Modified;
                db.SaveChanges();
            }
            //var response = new
            //{
            //    addresses = db.CustomerAddresses.Where(x => x.CustomerId == customerAddress.CustomerId).ToList()
            //};
            return Ok(db.CustomerAddresses.Where(x => x.CustomerId == customerAddress.CustomerId).ToList());
        }

        [HttpPut("editaddress")]
        [EnableCors("AllowOrigin")]
        public string IndexUpdate([FromBody] CustomerAddress Details)
        {
            try
            {
                db.Entry(Details).State = EntityState.Modified;
                db.SaveChanges();
                return "Value Updated Successfull";
            }
            catch (Exception)
            {
                throw;
            }

        }

        // Delete: Delete Data
        [HttpDelete("deleteaddress")]
        public string IndexDelete(int id)
        {
            db.CustomerAddresses.Remove(db.CustomerAddresses.Find(id));
            db.SaveChanges();
            return "Value Deleted Successfull";
        }

        [HttpGet("setcurrentaddress")]
        [EnableCors("AllowOrigin")]
        public IActionResult setcurrentaddress(int customeraddressid, int customerid)
        {
            var customeraddresses = db.CustomerAddresses.Where(x => x.CustomerId == customerid).ToList();
            foreach (var caddress in customeraddresses)
            {
                if (caddress.Id == customeraddressid)
                {
                    caddress.iscurrentaddress = true;
                }
                else
                {
                    caddress.iscurrentaddress = false;
                }
                db.Entry(caddress).State = EntityState.Modified;
                db.SaveChanges();
            }
            return Ok(db.CustomerAddresses.Where(x => x.CustomerId == customerid).ToList());
        }

        [HttpGet("getcustomerdetails")]
        [EnableCors("AllowOrigin")]
        public IActionResult getcustomerdetails(int customerId)
        {
            var orders = db.Orders.Where(x => x.CustomerId == customerId && x.OrderTypeId == 7).ToList();
            var orderitems = new List<OrderItem>();
            foreach (var order in orders)
            {
                var items = db.OrderItems.Where(x => x.OrderId == order.Id).Include(x => x.Product).ToList();
                foreach (var item in items)
                {
                    orderitems.Add(item);
                }
            }
            var details = new
            {
                addresses = db.CustomerAddresses.Where(x => x.CustomerId == customerId).ToList(),
                orders = orders,
                orderitems
            };
            return Ok(details);
        }

        [HttpGet("getoptions")]
        [EnableCors("AllowOrigin")]
        public IActionResult getoptions(int productId)
        {
            var optiongroups = db.ProductOptionGroups.Include(x => x.OptionGroup).Where(x => x.ProductId == productId).ToList();
            var options = new List<Option>();
            foreach (var group in optiongroups)
            {
                var temp_options = db.Options.Where(x => x.OptionGroupId == group.OptionGroupId).ToList();
                foreach (var option in temp_options)
                {
                    options.Add(option);
                }
            }

            var data = new
            {
                optiongroups = optiongroups,
                options = options
            };
            return Ok(data);
        }

        [HttpGet("gettaxes")]
        [EnableCors("AllowOrigin")]
        public IActionResult gettaxes(int companyId)
        {
            var taxGroups = db.TaxGroups.Where(x => x.CompanyId == companyId).ToList();
            return Ok(taxGroups);
        }

        [HttpPost("placeorder")]
        [EnableCors("AllowOrigin")]
        public IActionResult placeorder([FromForm] string payload)
        {
            int orderid = 0;
            IRestResponse paymentresponse;
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    dynamic pay_load = JsonConvert.DeserializeObject(payload);
                    dynamic items = pay_load.items;
                    Order order = new Order();
                    order.AllItemDisc = 0;
                    order.AllItemTaxDisc = 0;
                    order.AllItemTotalDisc = 0;
                    order.BillAmount = (double)pay_load.total;
                    order.BillDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                    order.BillDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                    order.BillStatusId = 1;
                    order.CompanyId = pay_load.companyid;
                    order.DiscAmount = 0;
                    order.DiscPercent = 0;
                    order.IsAdvanceOrder = true;
                    order.ModifiedDate = order.BillDate;
                    order.Note = "";
                    order.OrderDiscount = 0;
                    order.OrderedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                    order.OrderedDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                    order.OrderNo = db.Orders.Where(x => x.OrderTypeId == 7 && x.CompanyId == 3).Any() ? db.Orders.Where(x => x.OrderTypeId == 7 && x.CompanyId == 3).Max(x => x.OrderNo) : 1;
                    order.InvoiceNo = "w" + order.OrderNo;
                    order.OrderStatusId = 1;
                    order.OrderTaxDisc = 0;
                    order.OrderTotDisc = 0;
                    order.OrderTypeId = 7;
                    order.PaidAmount = order.BillAmount;
                    order.CustomerId = pay_load.customerid;
                    order.CustomerAddressId = pay_load.customeraddressid;
                    order.PreviousStatusId = 0;
                    order.RefundAmount = 0;
                    order.StoreId = 22;
                    order.Tax1 = (double)pay_load.tax1;
                    order.Tax2 = (double)pay_load.tax2;
                    order.Tax3 = (double)pay_load.tax3;
                    db.Orders.Add(order);
                    db.SaveChanges();

                    orderid = order.Id;

                    Transaction transaction = new Transaction();
                    transaction.Amount = order.BillAmount;
                    transaction.CompanyId = order.CompanyId;
                    transaction.CustomerId = order.CustomerId;
                    transaction.OrderId = order.Id;
                    transaction.PaymentTypeId = pay_load.PaymentTypeId;
                    transaction.StoreId = order.StoreId;
                    transaction.TransDate = order.BillDate;
                    transaction.TransDateTime = order.BillDateTime;
                    transaction.TranstypeId = 1;
                    transaction.UserId = null;

                    KOT kot = new KOT();
                    kot.CompanyId = order.CompanyId;
                    kot.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                    kot.Instruction = "";
                    kot.KOTNo = "0";
                    kot.KOTStatusId = 0;
                    kot.ModifiedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                    kot.OrderId = order.Id;
                    kot.StoreId = order.StoreId;
                    db.KOTs.Add(kot);
                    db.SaveChanges();

                    foreach (var item in items)
                    {
                        TaxGroup taxGroup = db.TaxGroups.Find((int)item.TaxGroupId);
                        OrderItem orderItem = new OrderItem();
                        orderItem.CategoryId = item.CategoryId;
                        orderItem.ComplementryQty = 0;
                        orderItem.DiscAmount = 0;
                        orderItem.DiscPercent = 0;
                        orderItem.Extra = 0;
                        orderItem.ItemDiscount = 0;
                        orderItem.KitchenUserId = null;
                        orderItem.KOTId = 0;
                        orderItem.Message = "";
                        orderItem.Note = "";
                        orderItem.OrderDiscount = 0;
                        orderItem.Price = item.TotalPrice;
                        orderItem.ProductId = item.Id;
                        orderItem.Quantity = 1;
                        orderItem.StatusId = 1;
                        orderItem.Tax1 = taxGroup.Tax1;
                        orderItem.Tax2 = taxGroup.Tax2;
                        orderItem.Tax3 = taxGroup.Tax3;
                        orderItem.TaxItemDiscount = 0;
                        orderItem.TaxOrderDiscount = 0;
                        orderItem.TotalAmount = item.TotalPrice * (1 + (taxGroup.Tax1 + taxGroup.Tax2 + taxGroup.Tax3) / 100);
                        orderItem.OrderId = order.Id;
                        orderItem.KOTId = kot.Id;
                        db.OrderItems.Add(orderItem);
                        db.SaveChanges();
                        foreach (var variant in item.variants)
                        {
                            OrdItemOptions itemOptions = new OrdItemOptions();
                            itemOptions.OptionId = variant.Id;
                            itemOptions.OrderItemId = orderItem.Id;
                            itemOptions.Price = variant.Price;
                            db.OrdItemOptions.Add(itemOptions);
                            db.SaveChanges();
                        }
                        foreach (var addon in item.addons)
                        {
                            OrdItemOptions itemOptions = new OrdItemOptions();
                            itemOptions.OptionId = addon.Id;
                            itemOptions.OrderItemId = orderItem.Id;
                            itemOptions.Price = addon.Price;
                            db.OrdItemOptions.Add(itemOptions);
                            db.SaveChanges();
                        }
                    }
                    dbContextTransaction.Commit();
                    var paymentpayload = new JObject(
                        new JProperty("amount", order.BillAmount),
                        new JProperty("purpose", "FBCakes Purchase")
                        );
                    var client = new RestClient();
                    var url = Configuration["Payment:URL"] + "payment-requests/";
                    client = new RestClient(url); //https://www.instamojo.com/api/1.1/payment-requests/

                    var request = new RestRequest(Method.POST);
                    request.AddHeader("content-type", "application/json");
                    request.AddHeader("X-Api-Key", "bd33677f17c780c3d441cc5b19aff719");
                    request.AddHeader("X-Auth-Token", "01973ddb1402c6e354402323c7b182dd");
                    request.AddParameter("application/json", paymentpayload, ParameterType.RequestBody);

                    paymentresponse = client.Execute(request);
                    //return Ok(response);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    var error = new
                    {
                        error = new Exception(e.Message, e.InnerException),
                        status = 0,
                        msg = "Something went wrong  Contact our service provider"
                    };
                    return Json(error);
                }
                var response = new
                {
                    status = 200,
                    orderid,
                    msg = "saved!",
                    paymentresponse = paymentresponse.Content
                };

                return Json(response);
            }
        }



        // POST: EcommerceController/Edit/5
        [HttpPost]
        [EnableCors("AllowOrigin")]
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

        // GET: EcommerceController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EcommerceController/Delete/5
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

        public static DateTime UnixTimeStampToDateTime(Int64 unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp / 1000);
            var istdate = TimeZoneInfo.ConvertTimeFromUtc(dtDateTime, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
            return istdate;
        }
        public static DateTime utc2local(DateTime utcTime)
        {
            //Unix timestamp is seconds past epoch
            //DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            //dtDateTime = dtDateTime.AddSeconds(unixTimeStamp / 1000);
            var istdate = TimeZoneInfo.ConvertTimeFromUtc(utcTime, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
            return istdate;
        }

        //ecom
        public string send_otp_email(string toemail)
        {
            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            string sRandomOTP = GenerateRandomOTP(4, saAllowedCharacters);
            string to = "prince.gopi67@gmail.com"; //To address    
            string from = "fbcakes.biz1@gmail.com"; //From address    
            MailMessage message = new MailMessage(from, toemail);
            string mailbody = "Use this (OTP) One Time Password to validate your Login: G-" + sRandomOTP;
            message.Subject = "Welcome to FBcakes";
            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
            NetworkCredential basicCredential1 = new
            NetworkCredential("fbcakes.biz1@gmail.com", "PassworD@1");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;
            client.Send(message);

            return sRandomOTP;
        }


        public string send_otp_emailandphone(string toemail, string tophone)
        {
            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            string sRandomOTP = GenerateRandomOTP(4, saAllowedCharacters);
            string to = "prince.gopi67@gmail.com"; //To address    
            string from = "fbcakes.biz1@gmail.com"; //From address    
            MailMessage message = new MailMessage(from, toemail);
            string mailbody = "Use this (OTP) One Time Password to validate your Login: G-" + sRandomOTP;
            message.Subject = "Welcome to FBcakes";
            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
            NetworkCredential basicCredential1 = new
            NetworkCredential("fbcakes.biz1@gmail.com", "PassworD@1");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;
            client.Send(message);

            var myclient = new RestClient("https://api.msg91.com/api/v5/flow/");
            var request = new RestRequest(Method.POST);

            request.AddHeader("authkey", "346987Aj0taFCM5fad0e44P1");
            request.AddHeader("content-type", "application/JSON");

            JObject payload = new JObject(
              new JProperty("flow_id", "60f67abdd043a85bca31c381"),
              new JProperty("var", sRandomOTP),
              new JProperty("sender", "FBCAKE"),
              new JProperty("mobiles", tophone)
              );
            request.AddParameter("application/JSON", JsonConvert.SerializeObject(payload), ParameterType.RequestBody);
            IRestResponse response = myclient.Execute(request);

            return sRandomOTP;
        }


        private string GenerateRandomOTP(int iOTPLength, string[] saAllowedCharacters)
        {
            string sOTP = String.Empty;
            string sTempChars = String.Empty;
            Random rand = new Random();
            for (int i = 0; i < iOTPLength; i++)
            {
                int p = rand.Next(0, saAllowedCharacters.Length);
                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];
                sOTP += sTempChars;
            }
            return sOTP;
        }

        [HttpPost("ecommercepaymentgateway")]
        [EnableCors("AllowOrigin")]
        public IActionResult ecommercepaymentgateway([FromBody] JObject payload)
        {
            var client = new RestClient();
            client = new RestClient(Configuration["Payment:URL"]);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("X-Api-Key", "bd33677f17c780c3d441cc5b19aff719");
            request.AddHeader("X-Auth-Token", "01973ddb1402c6e354402323c7b182dd");
            request.AddParameter("application/json", payload, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return Ok(response);
        }


        [HttpPost("register")]
        [EnableCors("AllowOrigin")]
        public IActionResult customer_login2([FromBody] dynamic data, string email, int companyId, string password, string name)
        {
            Customer customer = new Customer();
            string eml = (string)data.Email;
            if (db.Customers.Where(x => x.Email == eml).Any())
            {
                customer = db.Customers.Where(x => x.Email == email).FirstOrDefault();
            }
            else
            {
                customer.CompanyId = 3;
                customer.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                customer.LastRedeemDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                customer.ModifiedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                customer.Email = email;
                customer.Name = name;
                customer.Password = password;
                db.Customers.Add(customer);
                db.SaveChanges();
            }



            // string otp = send_myotp_email((string)data.Email);
            // string otp = send_otp_email2((string)data.Email,"918838300292");
            // customer.OTP = otp;
            db.Entry(customer).State = EntityState.Modified;
            db.SaveChanges();
            //MailMessage mail = new MailMessage();
            //SmtpClient SmtpServer = new SmtpClient("smtp.zoho.com");

            //mail.From = new MailAddress("admin@biz1book.com");
            //mail.To.Add(email);
            //mail.Subject = "Test Mail";
            //mail.Body = "https://localhost:44383/api/Login/password_reset?jwt=" + GenerateExpirableToken(email);

            //SmtpServer.Port = 587;
            //SmtpServer.Credentials = new System.Net.NetworkCredential("admin@biz1book.com", "Sairam@11");
            //SmtpServer.EnableSsl = true;

            //SmtpServer.Send(mail);
            //return Ok("Email Sent!");

            var response = new
            {
                status = 200
            };
            return Ok(response);
        }


        [HttpGet("myLogin")]
        [EnableCors("AllowOrigin")]
        public IActionResult myLogin(string email, string pwd)
        {
            Customer customer = new Customer();
            customer = db.Customers.Where(x => x.Email == email && x.Password == pwd).FirstOrDefault();

            if (customer == null)
            {
                var response = new
                {
                    status = 500,
                    message = "Invalid User"
                };
                return Ok(response);
            }

            else
            {
                var responce = new
                {
                    status = 200,
                    message = "Login Success"
                };
                return Ok(responce);
            }
        }


        [Route("Logincheck")]
        [HttpPost]
        public IActionResult IndexLogin([FromBody] Customer data)
        {
            Customer customer = db.Customers.Where(x => x.Email.Equals(data.Email) && x.Password.Equals(data.Password)).FirstOrDefault();

            if (customer == null)
            {
                var responce = new
                {
                    status = 500,
                    message = "Invalid User",
                };
                return Ok(responce);
            }
            else
            {
                var responce = new
                {
                    status = 250,
                    message = "Login Success",
                    customerid = customer.Id,
                    name = customer.Name,
                    storeid = customer.StoreId,

                };
                return Ok(responce);
            }
        }

        // put: Update Data
        //[HttpPut("update")]
        //public string IndexUpdate([FromBody] Customer data)
        //{
        //    db.Entry(data).State = EntityState.Modified;
        //    db.SaveChanges();
        //    return "Value Updated Successfull";
        //}


        [HttpGet("ph_edit")]
        [EnableCors("AllowOrigin")]
        public IActionResult phonenoedit(int Id, string phoneno)
        {
            Customer customer = new Customer();
            customer = db.Customers.Where(x => x.Id == Id).FirstOrDefault();
            customer.PhoneNo = phoneno;
            db.Entry(customer).State = EntityState.Modified;
            db.SaveChanges();
            var response = new
            {
                status = 200,
            };
            return Ok(response);
        }

        [HttpGet("emailid_check")]
        [EnableCors("AllowOrigin")]
        public IActionResult emailCheck(string email)
        {
            Customer customer = new Customer();
            if (db.Customers.Where(x => x.Email == email).Any())
            {
                customer = db.Customers.Where(x => x.Email == email).FirstOrDefault();
                var response = new
                {
                    status = "500",
                    msg = "Email Already Registered"
                };
                return Ok(response);
            }
            else
            {
                var response = new
                {
                    status = "200",
                    msg = "New Email"
                };
                return Ok(response);
            }
        }


        [HttpGet("customer_fpassword")]
        [EnableCors("AllowOrigin")]
        public IActionResult customer_fpassword(string email)
        {
            Customer customer = new Customer();
            if (db.Customers.Where(x => x.Email == email).Any())
            {
                customer = db.Customers.Where(x => x.Email == email).FirstOrDefault();

                string otp = send_otp_email(email);
                customer.OTP = otp;
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();

                var response = new
                {
                    status = 200,
                    msg = "Successful"
                };
                return Ok(response);
            }

            else
            {
                var response = new
                {
                    status = 0,
                    msg = "No Registred EmailID"

                };
                return Ok(response);
            }
        }

        [HttpGet("pwd_edit")]
        [EnableCors("AllowOrigin")]
        public IActionResult pwdedit(string email, string password)
        {
            Customer customer = new Customer();
            customer = db.Customers.Where(x => x.Email == email).FirstOrDefault();
            customer.Password = password;
            db.Entry(customer).State = EntityState.Modified;
            db.SaveChanges();
            var response = new
            {
                status = 200,
                msg = "Password Changed Succesfull"
            };
            return Ok(response);
        }

        [HttpGet("address_edit")]
        [EnableCors("AllowOrigin")]
        public IActionResult address_edit(int Id, string address, string city, int storeid)
        {
            try
            {
                Customer customer = new Customer();
                customer = db.Customers.Where(x => x.Id == Id).FirstOrDefault();
                customer.Address = address;
                customer.City = city;
                customer.StoreId = storeid;
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                var response = new
                {
                    status = 200,
                    msg = "Address Changed Succesfull"
                };
                return Ok(response);
            }

            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet("getOrderlistdetails")]
        public IActionResult getOrderdetails(int CustomerId)
        {
            // return Json(db.Orders.Where(x => x.CustomerId == CustomerId).ToList());

            var check = db.Orders.Where(x => x.CustomerId == CustomerId).FirstOrDefault();
            if (check == null)
            {
                var responce = new
                {
                    status = 500,
                    message = "Currently No Orders!"
                };
                return Ok(responce);
            }
            else
            {

                return Json(db.Orders.Where(x => x.CustomerId == CustomerId).ToList());

            }

        }

        [HttpGet("getOrderitemdetails")]
        public IActionResult getOrderitemdetails(int OrderId)
        {
            var check = db.OrderItems.Where(x => x.OrderId == OrderId).FirstOrDefault();

            if (check == null)
            {
                var responce = new
                {
                    status = 500,
                    message = "Currently No OrdersItems!"
                };
                return Ok(responce);
            }
            else
            {
                return Json(db.OrderItems.Where(x => x.OrderId == OrderId).ToList());

            }
        }



        [Route("InsertOrderdata")]
        [HttpPost]
        public IActionResult InsertOrderdata([FromBody] JObject data)
        {
            try
            {
                dynamic jsonObj = data;
                JArray items = jsonObj.Orderitems;
                dynamic itemArray = items.ToList();
                Order od = jsonObj.order.ToObject<Order>();
                //Order od = new Order();
                od.OrderNo = 0;
                //od.StoreId = 22;
                //od.CompanyId = 3;
                //od.CustomerId = data.CustomerId;
                //od.BillAmount = data.BillAmount;
                //od.PaidAmount = data.BillAmount;
                od.BillStatusId = 20;
                od.DiscPercent = 0;
                od.DiscAmount = 0;
                od.OrderedDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                od.OrderedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                od.BillDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                od.BillDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                od.ModifiedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                od.Tax1 = 0;
                od.Tax2 = 0;
                od.Tax3 = 0;
                od.OrderTypeId = 3;
                od.OrderStatusId = 5;
                od.RefundAmount = 0;
                od.IsAdvanceOrder = false;
                od.FoodReady = false;
                od.Closed = false;
                //od.Source = "no source";
                db.Orders.Add(od);
                db.SaveChanges();

                //orderitem
                foreach (var item in itemArray)
                {
                    OrderItem orderItem = new OrderItem();

                    orderItem.Quantity = item.Quantity;
                    orderItem.Price = item.Price;
                    orderItem.OrderId = od.Id;
                    orderItem.ProductId = item.ProductId;
                    orderItem.Tax1 = item.Tax1;
                    orderItem.Tax2 = item.Tax2;
                    orderItem.Tax3 = 0;
                    orderItem.DiscPercent = 0;
                    orderItem.DiscAmount = 0;
                    orderItem.StatusId = 5;
                    //orderItem.Product = item.Product;
                    orderItem.Note = item.Note;
                    orderItem.TotalAmount = item.TotalAmount;
                    db.OrderItems.Add(orderItem);
                    db.SaveChanges();
                }

                var responce = new
                {
                    status = 200,
                    message = " Order Saved Success"
                };
                return Ok(responce);

            }
            catch (Exception e)
            {
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 500,
                    msg = "Error Data"
                };
                return Json(error);

                throw;
            }
        }

        [HttpGet("getproducts2")]
        [EnableCors("AllowOrigin")]
        public IActionResult getproducts2(int companyId)
        {
            // var mystore = db.Stores.Where(x => x.CompanyId == companyId);
            var products = db.Products.Where(x => x.CompanyId == companyId).ToList();
            List<Option> options = new List<Option>();
            foreach (var option in db.Options.Where(x => x.CompanyId == 3))
            {
                if (db.ProductOptionGroups.Where(x => x.OptionGroupId == option.OptionGroupId).Any() && db.OptionGroups.Find(option.OptionGroupId).OptionGroupType == 1)
                {
                    int productid = db.ProductOptionGroups.Where(x => x.OptionGroupId == option.OptionGroupId).FirstOrDefault().ProductId;
                    if (products.Where(x => x.Id == productid).Count() > 0)
                    {
                        option.ProductId = productid;
                        options.Add(option);
                    }
                }
            }
            var data = new
            {
                categories = db.Categories.Where(x => x.CompanyId == companyId).ToList(),
                products = db.Products.Where(x => x.CompanyId == companyId).ToList(),
                options = options,
                // store=mystore
            };
            return Ok(data);
        }


        //public string send_otp_phone(string tophone)

        [HttpPost("send_otp_phone")]
        [EnableCors("AllowOrigin")]
        public IActionResult send_otp_phone(string tophone)
        {
            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            string sRandomOTP = GenerateRandomOTP(4, saAllowedCharacters);

            var client = new RestClient("https://api.msg91.com/api/v5/flow/");
            var request = new RestRequest(Method.POST);

            request.AddHeader("authkey", "346987Aj0taFCM5fad0e44P1");
            request.AddHeader("content-type", "application/JSON");

            //request.AddParameter("flow_id", "60f67abdd043a85bca31c381", ParameterType.GetOrPost);
            //request.AddParameter("var", "5454", ParameterType.GetOrPost);
            //request.AddParameter("sender", "FBCAKE", ParameterType.GetOrPost);
            //request.AddParameter("mobiles", tophone, ParameterType.GetOrPost);
            //request.AddParameter("application/JSON", ParameterType.GetOrPost);
            //client.Get(request);

            JObject payload = new JObject(
                new JProperty("flow_id", "60f67abdd043a85bca31c381"),
                new JProperty("var", "5454"),
                new JProperty("sender", "FBCAKE"),
                new JProperty("mobiles", tophone)
                );
            request.AddParameter("application/JSON", JsonConvert.SerializeObject(payload), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            var responce = new
            {
                status = 200,
                message = " OTP SEND"
            };
            return Ok(responce);

        }


        // Ecom
        public string send_otp_email2(string phoneno)
        {
            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            string sRandomOTP = GenerateRandomOTP(4, saAllowedCharacters);
            var myclient = new RestClient("https://api.msg91.com/api/v5/flow/");
            var request = new RestRequest(Method.POST);

            request.AddHeader("authkey", "346987Aj0taFCM5fad0e44P1");
            request.AddHeader("content-type", "application/JSON");

            JObject payload = new JObject(
              new JProperty("flow_id", "60f67abdd043a85bca31c381"),
              new JProperty("var", sRandomOTP),
              new JProperty("sender", "FBCAKE"),
              new JProperty("mobiles", phoneno)
              );
            request.AddParameter("application/JSON", JsonConvert.SerializeObject(payload), ParameterType.RequestBody);
            IRestResponse response = myclient.Execute(request);
            return sRandomOTP;
        }


        [HttpGet("customer_login2")]
        [EnableCors("AllowOrigin")]
        public IActionResult customer_login2(string email, string PhoneNo, int companyId)
        {
            Customer customer = new Customer();
            if (db.Customers.Where(x => x.PhoneNo == PhoneNo).Any())
            {
                customer = db.Customers.Where(x => x.PhoneNo == PhoneNo).FirstOrDefault();
            }

            else
            {
                customer.CompanyId = companyId;
                customer.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                customer.LastRedeemDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                customer.ModifiedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                customer.Email = email;
                customer.PhoneNo = PhoneNo;
                db.Customers.Add(customer);
                db.SaveChanges();
            }

            //string otp = send_otp_email(email);
            string otp = send_otp_email2(PhoneNo);
            customer.OTP = otp;
            db.Entry(customer).State = EntityState.Modified;
            db.SaveChanges();

            var response = new
            {
                status = 200,
                customerid = customer.Id,
                name = customer.Name,
                storeid = customer.StoreId,

            };
            return Ok(response);
        }

        [HttpGet("verify_otp2")]
        [EnableCors("AllowOrigin")]
        public IActionResult verify_otp2(string PhoneNo, string otp)
        {
            Customer customer = new Customer();
            customer = db.Customers.Where(x => x.PhoneNo == PhoneNo).FirstOrDefault();
            if (otp == customer.OTP)
            {
                var response = new
                {
                    customer.Id,
                    status = 200,
                    msg = "OTP Verification Successfull.",
                    customerid = customer.Id,
                    name = customer.Name,
                    storeid = customer.StoreId,
                };
                return Ok(response);
            }
            else
            {
                var response = new
                {
                    status = 0,
                    msg = "OTP Verification Failed."
                };
                return Ok(response);
            }
        }


        //automation react native email
        [HttpPost("sendemail")]
        [EnableCors("AllowOrigin")]
        public IActionResult sendemail(string subject, string body, [FromForm] IFormFile[] files, [FromBody] string[] toaddresses)
        {
            //string to = "prince.gopi67@gmail.com"; //To address    

            string from = "gopi.biz1@gmail.com"; //From address   

            MailMessage message = new MailMessage(from, toaddresses[0]);
            foreach (var m in toaddresses)
            {
                message.To.Add(m);
            }

            message.From = new MailAddress("fbcakes.biz1@gmail.com", "I AM GOPI");
            string mailbody = body;
            message.Subject = subject;

            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            foreach (IFormFile file in files)
            {
                message.Attachments.Add(new Attachment(ImageUpload(file)));
            }

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
            NetworkCredential basicCredential1 = new
            NetworkCredential("fbcakes.biz1@gmail.com", "PassworD@1");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;

            try
            {
                client.Send(message);
                var response = new
                {
                    status = 200,
                    msg = "send Success",
                };
                return Ok(response);
            }

            catch (Exception ex)
            {
                throw ex;
                var response = new
                {
                    status = 500,
                    msg = "Error",
                    err = ex,
                };
                return Ok(response);
            }
        }


        public string ImageUpload(IFormFile file)
        {
            try
            {
                string baseUrl = _environment.WebRootPath;
                string subdir = "\\images\\";
                if (!Directory.Exists(_environment.WebRootPath + subdir))
                {
                    Directory.CreateDirectory(_environment.WebRootPath + subdir);
                }
                using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + subdir + file.FileName))
                {
                    file.CopyTo(filestream);
                    filestream.Flush();
                    var response = new
                    {
                        // url = "https://biz1pos.azurewebsites.net/images/"+file.FileName
                        url = _environment.WebRootPath + subdir + file.FileName
                    };
                    return response.url;

                }
            }
            catch (Exception ex)
            {
                var error = new
                {
                    status = 0,
                    msg = new Exception(ex.Message, ex.InnerException)
                };
                return "Error uploading image";
            }
        }

        [HttpPost("sendemail2")]
        [EnableCors("AllowOrigin")]
        public IActionResult sendemail2(string subject, string body, string[] to)
        {
            MailMessage message = new MailMessage();
            message.To.Add(to[0]);

            foreach (var m in to)
            {
                message.To.Add(m);
            }

            message.From = new MailAddress("myfbcakes.biz1@gmail.com", "I AM GOPI");
            message.Subject = subject;
            message.Body = body;

            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
            NetworkCredential basicCredential1 = new
            NetworkCredential("fbcakes.biz1@gmail.com", "PassworD@1");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;

            try
            {
                client.Send(message);
                var response = new
                {
                    status = 200,
                    msg = "send Success",
                };
                return Ok(response);
            }

            catch (Exception ex)
            {
                throw ex;
                var response = new
                {
                    status = 500,
                    msg = "Error",
                    err = ex,
                };
                return Ok(response);
            }
        }


        [HttpPost("sendemail3")]
        [EnableCors("AllowOrigin")]
        public IActionResult sendemail3(string subject, string body, [FromBody] string[] to)
        {
            MailMessage message = new MailMessage();
            message.To.Add(to[0]);

            foreach (var m in to)
            {
                message.To.Add(m);
            }

            message.From = new MailAddress("myfbcakes.biz1@gmail.com", "I AM GOPI");
            message.Subject = subject;
            message.Body = body;

            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
            NetworkCredential basicCredential1 = new
            NetworkCredential("fbcakes.biz1@gmail.com", "PassworD@1");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;

            try
            {
                client.Send(message);
                var response = new
                {
                    status = 200,
                    msg = "send Success",
                };
                return Ok(response);
            }

            catch (Exception ex)
            {
                throw ex;
                var response = new
                {
                    status = 500,
                    msg = "Error",
                    err = ex,
                };
                return Ok(response);
            }
        }


        [HttpPost("placeorder2")]
        [EnableCors("AllowOrigin")]
        public IActionResult placeorder2([FromForm] string payload)
        {
            int orderid = 0;
            Order order = new Order();
            // IRestResponse paymentresponse;
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    dynamic pay_load = JsonConvert.DeserializeObject(payload);
                    dynamic items = pay_load.items;
                    order.AllItemDisc = 0;
                    order.AllItemTaxDisc = 0;
                    order.AllItemTotalDisc = 0;
                    order.BillAmount = (double)pay_load.total;
                    order.BillDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                    order.BillDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                    order.BillStatusId = 1;
                    order.CompanyId = pay_load.companyid;
                    order.DiscAmount = 0;
                    order.DiscPercent = 0;
                    order.IsAdvanceOrder = true;
                    order.ModifiedDate = order.BillDate;
                    order.Note = "";
                    order.OrderDiscount = 0;
                    order.OrderedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                    order.OrderedDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                    order.OrderNo = db.Orders.Where(x => x.OrderTypeId == 7 && x.CompanyId == 3).Any() ? db.Orders.Where(x => x.OrderTypeId == 7 && x.CompanyId == 3).Max(x => x.OrderNo) : 1;
                    order.InvoiceNo = "w" + order.OrderNo;
                    order.OrderStatusId = 1;
                    order.OrderTaxDisc = 0;
                    order.OrderTotDisc = 0;
                    order.OrderTypeId = 7;
                    order.PaidAmount = order.BillAmount;
                    order.CustomerId = pay_load.customerid;
                    order.CustomerAddressId = pay_load.customeraddressid;
                    order.PreviousStatusId = 0;
                    order.RefundAmount = 0;
                    order.StoreId = 22;
                    order.Tax1 = (double)pay_load.tax1;
                    order.Tax2 = (double)pay_load.tax2;
                    order.Tax3 = (double)pay_load.tax3;
                    db.Orders.Add(order);
                    db.SaveChanges();

                    orderid = order.Id;

                    Transaction transaction = new Transaction();
                    transaction.Amount = order.BillAmount;
                    transaction.CompanyId = order.CompanyId;
                    transaction.CustomerId = order.CustomerId;
                    transaction.OrderId = order.Id;
                    // transaction.PaymentTypeId = pay_load.PaymentTypeId;
                    transaction.PaymentTypeId = 1;
                    transaction.StoreId = order.StoreId;
                    transaction.TransDate = order.BillDate;
                    transaction.TransDateTime = order.BillDateTime;
                    transaction.TranstypeId = 1;
                    transaction.UserId = null;

                    KOT kot = new KOT();
                    kot.CompanyId = order.CompanyId;
                    kot.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                    kot.Instruction = "";
                    kot.KOTNo = "0";
                    kot.KOTStatusId = 0;
                    kot.ModifiedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                    kot.OrderId = order.Id;
                    kot.StoreId = order.StoreId;
                    db.KOTs.Add(kot);
                    db.SaveChanges();

                    foreach (var item in items)
                    {
                        TaxGroup taxGroup = db.TaxGroups.Find((int)item.TaxGroupId);
                        OrderItem orderItem = new OrderItem();
                        orderItem.CategoryId = item.CategoryId;
                        orderItem.ComplementryQty = item.ComplementryQty == null ? 0 : item.ComplementryQty;
                        orderItem.DiscAmount = 0;
                        orderItem.DiscPercent = 0;
                        orderItem.Extra = 0;
                        orderItem.ItemDiscount = 0;
                        orderItem.KitchenUserId = null;
                        orderItem.KOTId = 0;
                        orderItem.Message = item.Message;
                        orderItem.Note = "";
                        orderItem.OrderDiscount = 0;
                        orderItem.Price = item.TotalPrice;
                        orderItem.ProductId = item.Id;
                        //orderItem.Quantity = item.Quantity;
                        orderItem.Quantity = 1;
                        orderItem.StatusId = 1;
                        orderItem.Tax1 = taxGroup.Tax1;
                        orderItem.Tax2 = taxGroup.Tax2;
                        orderItem.Tax3 = taxGroup.Tax3;
                        orderItem.TaxItemDiscount = 0;
                        orderItem.TaxOrderDiscount = 0;
                        orderItem.TotalAmount = item.TotalPrice * (1 + (taxGroup.Tax1 + taxGroup.Tax2 + taxGroup.Tax3) / 100);
                        orderItem.OrderId = order.Id;
                        orderItem.KOTId = kot.Id;
                        db.OrderItems.Add(orderItem);
                        db.SaveChanges();
                        foreach (var variant in item.variants)
                        {
                            OrdItemOptions itemOptions = new OrdItemOptions();
                            itemOptions.OptionId = variant.Id;
                            itemOptions.OrderItemId = orderItem.Id;
                            itemOptions.Price = variant.Price;
                            db.OrdItemOptions.Add(itemOptions);
                            db.SaveChanges();
                        }
                        foreach (var addon in item.addons)
                        {
                            OrdItemOptions itemOptions = new OrdItemOptions();
                            itemOptions.OptionId = addon.Id;
                            itemOptions.OrderItemId = orderItem.Id;
                            itemOptions.Price = addon.Price;
                            db.OrdItemOptions.Add(itemOptions);
                            db.SaveChanges();
                        }
                    }
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    var error = new
                    {
                        error = new Exception(e.Message, e.InnerException),
                        msg = "error occur"
                    };
                    return Json(error);
                }
                var response = new
                {
                    status = 200,
                    orderid,
                    order,
                    msg = "saved!",
                };

                return Json(response);
            }
        }


        [HttpGet("getOrderlistdetails2")]
        public IActionResult getOrderdetails2(int CustomerId)
        {
            var check = db.Orders.Where(x => x.CustomerId == CustomerId).FirstOrDefault();
            if (check == null)
            {
                var responce = new
                {
                    status = 500,
                    message = "Currently No Orders!"
                };
                return Ok(responce);
            }
            else
            {
                var orderlist = db.Orders.Where(x => x.CustomerId == CustomerId).ToList();
                var oId = orderlist.Select(x => new { x.Id }).ToList();
                var count = orderlist.Count;

                foreach (var item in oId)
                {
                    var orderitems = db.OrderItems.Where(x => x.OrderId == item.Id).ToList();
                    var responce2 = new
                    {
                        orderitem = orderitems,
                    };
                    return Ok(responce2);
                }

                var responce = new
                {

                };
                return Ok(responce);
            }

        }



        [HttpGet("getmsgOrderitem")]
        public IActionResult getmsgOrderitem(int OrderId)
        {
            var check = db.OrderItems.Where(x => x.OrderId == OrderId).FirstOrDefault();

            if (check == null)
            {
                var responce = new
                {
                    status = 500,
                    message = "Currently No OrdersItems!"
                };
                return Ok(responce);

            }
            else
            {
                var orderitems = db.OrderItems.Where(x => x.OrderId == OrderId).ToList();
                var responce = new
                {
                    orderitems = orderitems,
                };
                return Ok(responce);
            }
        }

        [HttpGet("getOrderitemdetails2")]
        public IActionResult getOrderitemdetails2(int customerid)
        {
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("dbo.SPGBtest", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@customerid", customerid));
            DataSet ds = new DataSet();
            SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
            sqlAdp.Fill(ds);

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
            var response = new
            {
                orders = JsonConvert.DeserializeObject(catStr[0])
            };
            return Json(response);
        }


    }

}
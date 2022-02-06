using System.Collections.Generic;
//using System.Net.WebSockets;
using Biz1BookPOS.Models;
using Microsoft.AspNetCore.Mvc;
//using System.Net.Sockets;
using System;
//using System;
//using Quobject.SocketIoClientDotNet.Client;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using RestSharp;
using Newtonsoft.Json;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using Biz1PosApi.Hubs;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Biz1PosApi.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Data;
//using Microsoft.AspNetCore.SignalR.Client;

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private IConfiguration _config;
        private IHubContext<ChatHub> _hubContext;
        private IHubContext<UrbanPiperHub, IHubClient> _uhubContext;
        //private ChatHub _chatHub;
        public IConfiguration Configuration { get; }
        public static IHostingEnvironment _environment;
        private POSDbContext db;
        private static TraceSource ts = new TraceSource("TraceTest");
        readonly ILogger<ValuesController> _log;
        public ValuesController(IConfiguration config, POSDbContext contextOptions, IHubContext<ChatHub> hubContext, IHostingEnvironment environment, ILogger<ValuesController> log, IHubContext<UrbanPiperHub, IHubClient> uhubContext)
        {
            _config = config;
            //Configuration = configuration;
            _hubContext = hubContext;
            _uhubContext = uhubContext;
            //_chatHub = chatHub;
            db = contextOptions;
            _environment = environment;
            _log = log;
            Configuration = config;
        }


        // GET api/values
        [HttpGet]
        //[Authorize(Roles = "Administrator")]
        [EnableCors("AllowOrigin")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPost("token")]
        public ActionResult GetToken()
        {
            //security key
            string securityKey = _config["Jwt:Key"];
            //symmetric security key
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

            //signing credentials
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            //add claims
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Role, "Administrator"));
            claims.Add(new Claim("Id", "110"));
            //claims.Add(new Claim(ClaimTypes.Email, "muttapaya@gmail.com"));

            //create token
            var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: "readers",
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: signingCredentials,
                    claims: claims
                );

            //return token
            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
        [HttpGet("requestapi")]
        public IActionResult requestapi()
        {
            int retrycount = 0;
            retry:
            retrycount++;
            var client = new RestClient("https://localhost:44383/api/values/responseapi");
            var request = new RestRequest(Method.GET);
            request.AddHeader("content-type", "application/json");
            IRestResponse response = client.Execute(request);
            var responseobject = new
            {
                status = 0,
                message = ""
            };
            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                responseobject = new
                {
                    status = 429,
                    message = response.StatusDescription + ". Retry after 1 minute."
                };
            }
            if (response.StatusCode == HttpStatusCode.InternalServerError || response.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                if (retrycount < 3)
                {
                    goto retry;
                }
                responseobject = new
                {
                    status = (response.StatusCode == HttpStatusCode.InternalServerError) ? 500 : 503,
                    message = response.StatusDescription + ". Retry after 1 minute."
                };
            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                responseobject = new
                {
                    status = 200,
                    message = "Success"
                };
            }
            return Json(responseobject);
        }
        [HttpGet("responseapi")]
        public IActionResult responseapi()
        {
            string name = "moh";
            return StatusCode(200, name);
        }
        [HttpGet("jstring")]
        public IActionResult jstring()
        {
            Order order = db.Orders.Find(123);
            dynamic json = JsonConvert.DeserializeObject(order.OrderStatusDetails);
            json.accepted = DateTime.Now;
            json.foodready = DateTime.Now;
            order.OrderStatusDetails = JsonConvert.SerializeObject(json);
            return StatusCode(200, order);
        }
        [HttpPost("message")]
        public async Task<IActionResult> message([FromBody] JObject data)
        {
            try
            {
                dynamic Json = data;
                int storeId = Json.order.store.merchant_ref_id;
                int companyId = db.Stores.Find(storeId).CompanyId;
                string room = storeId.ToString() + '/' + companyId.ToString();
                long UPOrderId = Json.order.details.id;
                Order oldorder = db.Orders.Where(x => x.UPOrderId == UPOrderId).FirstOrDefault();
                if (oldorder == null)
                {
                    UPOrder order = new UPOrder();
                    order.StoreId = storeId;
                    order.Json = JsonConvert.SerializeObject(Json);
                    order.UPOrderId = Json.order.details.id;
                    db.UPOrders.Add(order);
                    db.SaveChanges();
                    SaveOnlineOrder(data);
                }
                var orders = db.UPOrders.Where(x => x.StoreId == storeId).ToList();
                //await _hubContext.Clients.All.SendAsync( "RecieveMessage",vs);
                //await _hubContext.Clients.Group(room).SendAsync("order", orders);
                //await _chatHub.Order(room,vs);
                //var hashes = _hubContext.Clients.All;
                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }

        }
        //[HttpGet("bulkorder")]
        //public void bulkorder()
        //{
        //    var orderlogs = db.OrderLogs.Where(x => x.StoreId == 29 && x.Error == ")
        //}

        [HttpGet("fcm_messaging")]
        public void fcm_messaging(string deviceid)
        {
            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "post";
            //serverKey - Key from Firebase cloud messaging server  
            tRequest.Headers.Add(string.Format("Authorization: key={0}", "AAAAjZRFJ5g:APA91bGV7qQTtfuSZhrQ5XGYQee6RhpHwb7ZDf8LJDHOVRk7U91oeQxRu_5e2JtthZCfYjMfB9XadyxVAKoejr4-iGDuF8D7VVHFycIATOUVQR4fv0nCpFKic6PTGPXjsQJUQyVJIVoN"));
            //Sender Id - From firebase project setting  
            tRequest.Headers.Add(string.Format("Sender: id={0}", "608077948824"));
            tRequest.ContentType = "application/json";
            var fcm_token = db.Accounts.Find(1).FCM_Token;
            var accounts = db.Accounts.Where(x => x.FCM_Token != null && x.FCM_Token != "").ToList();
            string[] fcm_tokens = new String[accounts.Count()];
            for(int i = 0; i < 1; i++)
            {
                fcm_tokens[i] = accounts[i].FCM_Token;
            }
            //fcm_tokens[0] = "c2UnviNNTRCNtdRp13J-iu:APA91bFHuq6cvsHWmS6vQdFiR0iClqM_z7bkOOUQTo_daTuOvvRW9gjUEP6Ymx8zpJuD37P-4Oqkvm8p-76OEHUR0408ONieD4RzLU21pK8aNKoGroAKopgLMk7akQzm2_dzreDGVjnU";
            //fcm_tokens[1] = "cDC_xUtORxWkmr9rVZoSSD:APA91bF7AcgiUos-4BANkxdaORIQDn1izqmHbZQbcwVO0fei5IhFC53yoQklcJWbJV1gZVY549uFc01ON_W-Os2YzjXieOeqgXpI1ia3Xcl1mZq9feGxTLulzokfxM921gJ0dcy191aY";
            var payload = new
            {
                //to = "cDC_xUtORxWkmr9rVZoSSD:APA91bF7AcgiUos-4BANkxdaORIQDn1izqmHbZQbcwVO0fei5IhFC53yoQklcJWbJV1gZVY549uFc01ON_W-Os2YzjXieOeqgXpI1ia3Xcl1mZq9feGxTLulzokfxM921gJ0dcy191aY",
                //to = fcm_token,
                registration_ids = fcm_tokens,
                priority = "high",
                content_available = true,
                notification = new
                {
                    body = "Test",
                    title = "Test",
                    badge = 1
                },
                data = new
                {
                    key1 = "value1",
                    key2 = "value2"
                }
            };
            string postbody = JsonConvert.SerializeObject(payload).ToString();
            Byte[] byteArray = Encoding.UTF8.GetBytes(postbody);
            tRequest.ContentLength = byteArray.Length;
            using (Stream dataStream = tRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
                using (WebResponse tResponse = tRequest.GetResponse())
                {
                    using (Stream dataStreamResponse = tResponse.GetResponseStream())
                    {
                        if (dataStreamResponse != null) using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                //result.Response = sResponseFromServer;
                            }
                    }
                }
            }
        }

        [HttpGet("fcm")]
        //public async Task<string> fcm()
        public static string fcm(POSDbContext db, int deviceId)
        {
            try
            {
                var applicationID = "AAAApYHmXyY:APA91bF_KA58EDJ1sMjb00MU7kujYH9OBejAu7bNRTlGg1JDe4akoW5r_YpbZEsqwFDqIPozsnw4YlEY1kbyl0CbpwehJnzlB4rwvosBGfBP3T5TBLpfaIYnIvsvZzd267yzI3Y1eOsJ";
                var senderId = "710848962342";
                //string deviceId = "eG7KeSEHQACd9eOTctUPfS:APA91bEtZI13Ko8K3bcMzIdm5cIBuZMHnhb_K43fAUuS_cb_iztK7-CRr75KedsdiBd2FZPnkLiRqlcVVr2LVMvzXvL39esenZJ1vMHL3Z8BKlsb56sn1Xb9ly9hOamLOo8EDzeS8dBV";
                string token = db.Devices.Find(deviceId).Token;
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    to = token,
                    notification = new
                    {
                        body = "hello",
                        title = "New KOT",
                        icon = "myicon"
                    }
                };

                //var serializer = new JavaScriptSerializer();

                //var json = serializer.Serialize(data);
                var json = JsonConvert.SerializeObject(data);

                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.ContentLength = byteArray.Length;

                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                string str = sResponseFromServer;
                            }
                        }
                    }
                }
                //var app = FirebaseApp.Create(new AppOptions() { Credential = GoogleCredential.FromFile("C:/Users/welcome/SRKDS/android/app/google-services.json").CreateScoped("https://www.googleapis.com/auth/firebase.messaging") });
                //FirebaseMessaging messaging = FirebaseMessaging.GetMessaging(app);
                // Send a message to the devices subscribed to the provided topic.
                //string response = await messaging.SendAsync(message);
                // Response is a message ID string.
                //Console.WriteLine("Successfully sent message: " + response);
                return "hh";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        [HttpPost("emergency")]
        public string emergency()
        {
            var products = db.Products.Where(x => x.CompanyId == 24).ToList();
            foreach (var product in products)
            {
                product.Name = product.Name.Replace("-Old", "");
                if (product.Id <= 7550)
                {
                    product.isactive = true;
                }
                else
                {
                    product.isactive = false;
                }
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
            }
            //var categoris = db.Categories.Where(x => x.CompanyId == 24).ToList();
            //foreach (var product in categoris)
            //{
            //    product.Description = product.Description.Replace("-Old", "");
            //    if (product.Id <= 333)
            //    {
            //        product.isactive = true;
            //    }
            //    else
            //    {
            //        product.isactive = false;
            //    }
            //    db.Entry(product).State = EntityState.Modified;
            //    db.SaveChanges();
            //}
            return "Error logged";
        }
        public void SaveOnlineOrder(JObject data)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    dynamic order = data;
                    long UPOrderId = order.order.details.id;
                    Order oldorder = db.Orders.Where(x => x.UPOrderId == UPOrderId).FirstOrDefault();
                    if (oldorder == null)
                    {
                        Order neworder = new Order();
                        neworder.OrderTypeId = 4;
                        neworder.OrderedDate = UnixTimeStampToDateTime(order.order.details.created.ToObject<Int64>());
                        neworder.OrderedDateTime = UnixTimeStampToDateTime(order.order.details.created.ToObject<Int64>());
                        neworder.DeliveryDateTime = UnixTimeStampToDateTime(order.order.details.delivery_datetime.ToObject<Int64>());
                        neworder.OrderStatusId = 0;
                        neworder.DiscPercent = 0;
                        neworder.DiscAmount = order.order.details.discount - order.order.details.total_external_discount;
                        neworder.Tax1 = order.order.details.total_taxes / 2;
                        neworder.Tax2 = order.order.details.total_taxes / 2;
                        neworder.Tax3 = 0;
                        neworder.StoreId = order.order.store.merchant_ref_id.ToObject<int>();
                        neworder.CompanyId = db.Stores.Find(neworder.StoreId).CompanyId;
                        neworder.BillAmount = order.order.details.order_total + order.order.details.total_external_discount;
                        neworder.PaidAmount = order.order.details.order_total + order.order.details.total_external_discount;
                        neworder.BillDateTime = UnixTimeStampToDateTime(order.order.details.created.ToObject<Int64>());
                        neworder.BillDate = UnixTimeStampToDateTime(order.order.details.created.ToObject<Int64>());
                        neworder.BillStatusId = 5;
                        neworder.DiningTableId = null;
                        neworder.SplitTableId = null;
                        neworder.WaiterId = null;
                        neworder.Note = order.order.details.instructions;
                        neworder.InvoiceNo = "";
                        neworder.OrderNo = 0;
                        neworder.UserId = null;
                        neworder.IsAdvanceOrder = true;
                        neworder.Source = order.order.details.channel;
                        if (neworder.Source == "swiggy")
                        {
                            neworder.SourceId = 2;
                        }
                        else if (neworder.Source == "zomato")
                        {
                            neworder.SourceId = 3;
                        }
                        else if (neworder.Source == "foodpanda")
                        {
                            neworder.SourceId = 4;
                        }
                        neworder.AggregatorOrderId = order.order.details.ext_platforms[0].id;
                        neworder.UPOrderId = order.order.details.id;
                        neworder.CustomerData = JsonConvert.SerializeObject(order.customer);
                        neworder.FoodReady = false;
                        //neworder.UserId = 
                        db.Orders.Add(neworder);
                        db.SaveChanges();
                        Transaction transaction = new Transaction();
                        transaction.OrderId = neworder.Id;
                        transaction.Amount = neworder.PaidAmount;
                        transaction.PaymentTypeId = 4;
                        transaction.TranstypeId = 1;
                        transaction.TransDateTime = neworder.OrderedDateTime;
                        transaction.TransDate = neworder.OrderedDate;
                        transaction.UserId = neworder.UserId;
                        transaction.StoreId = neworder.StoreId;
                        transaction.CompanyId = neworder.CompanyId;
                        db.Transactions.Add(transaction);
                        db.SaveChanges();

                        KOT kot = new KOT();
                        kot.KOTStatusId = 5;
                        kot.Instruction = "";
                        kot.KOTNo = "";
                        kot.OrderId = neworder.Id;
                        kot.CreatedDate = neworder.OrderedDate;
                        kot.ModifiedDate = neworder.OrderedDate;
                        kot.CompanyId = neworder.CompanyId;
                        kot.StoreId = neworder.StoreId;
                        db.KOTs.Add(kot);
                        db.SaveChanges();

                        foreach (var item in order.order.items)
                        {
                            OrderItem orderItem = new OrderItem();
                            orderItem.OrderId = neworder.Id;
                            orderItem.ProductId = Int32.Parse(item.merchant_id.ToString());
                            orderItem.Quantity = Int32.Parse(item.quantity.ToString());
                            orderItem.Price = Int32.Parse(item.price.ToString());
                            orderItem.StatusId = 5;
                            orderItem.KOTId = kot.Id;
                            orderItem.DiscPercent = 0;
                            orderItem.DiscAmount = (item.total / order.order.details.order_subtotal) * neworder.DiscAmount;
                            orderItem.ComplementryQty = 0;
                            orderItem.Tax1 = 0;
                            orderItem.Tax2 = 0;
                            orderItem.Tax3 = 0;
                            foreach (var tax in item.taxes)
                            {
                                if (tax.title.ToString().Contains("CGST"))
                                {
                                    orderItem.Tax1 = tax.rate;
                                }
                                else if (tax.title.ToString().Contains("SGST"))
                                {
                                    orderItem.Tax2 = tax.rate;
                                }
                            }
                            orderItem.Note = "";
                            db.OrderItems.Add(orderItem);
                            db.SaveChanges();

                            foreach (var option in item.options_to_add)
                            {
                                OrdItemOptions ordItemOptions = new OrdItemOptions();
                                ordItemOptions.OrderItemId = orderItem.Id;
                                ordItemOptions.OptionId = option.merchant_id;
                                ordItemOptions.Price = option.price;
                            }

                            foreach (var charge in item.charges)
                            {
                                var chargename = charge.title.Replace(" ", String.Empty).ToLower();
                                AdditionalCharges additionalCharges = new AdditionalCharges();
                                if (chargename == "packagingcharge")
                                {
                                    additionalCharges = db.AdditionalCharges.Where(x => x.Description == "Packaging Charge").FirstOrDefault();
                                }
                                else if (chargename == "deliverycharge")
                                {
                                    additionalCharges = db.AdditionalCharges.Where(x => x.Description == "Delivery Charge").FirstOrDefault();
                                }
                                TaxGroup taxGroup = db.TaxGroups.Where(x => x.Id == additionalCharges.TaxGroupId).FirstOrDefault();
                                OrderCharges orderCharge = new OrderCharges();
                                orderCharge.OrderId = neworder.Id;
                                orderCharge.AdditionalChargeId = additionalCharges.Id;
                                orderCharge.ChargePercentage = 0;
                                orderCharge.ChargeAmount = charge.value.ToObject<int>();
                                orderCharge.Tax1 = taxGroup.Tax1;
                                orderCharge.Tax2 = taxGroup.Tax2;
                                orderCharge.Tax3 = taxGroup.Tax3;
                                orderCharge.CreatedDate = neworder.OrderedDateTime;
                                orderCharge.ModifiedDate = neworder.OrderedDateTime;
                                orderCharge.CompanyId = neworder.CompanyId;
                                orderCharge.StoreId = neworder.StoreId;
                                db.OrderCharges.Add(orderCharge);
                                db.SaveChanges();
                            }

                        }
                        dbContextTransaction.Commit();
                    }
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                }
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

        [HttpPost("ImageUpload")] 
        public string ImageUpload(IFormFile file)
        {
            try
            {
                //long size = file.Sum(f => f.Length);

                // full path to file in temp location
                // var filePath = "https://biz1app.azurewebsites.net/Images/3";
                string subdir = "\\audio\\";
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
                        url = "https://biz1pos.azurewebsites.net/audio/" + file.FileName
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
                return "";
            }
        }

        [HttpPost("SaveDevice")]
        [EnableCors("AllowOrigin")]
        public IActionResult SaveDevice([FromForm] string data)
        {
            try
            {
                dynamic outlet = JsonConvert.DeserializeObject(data);
                Device device = outlet.ToObject<Device>();
                db.Devices.Add(device);
                db.SaveChanges();
                var error = new
                {
                    status = 200,
                    msg = "The data added successfully"
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
        [HttpPost("test")]
        [EnableCors("AllowOrigin")]
        public void test(string message)
        {
            //using (var hubConnection = new HubConnection("https://biz1pos.azurewebsites.net/uphub/"))
            //{
            //    IHubProxy stockTickerHubProxy = hubConnection.CreateHubProxy("StockTickerHub");
            //    //stockTickerHubProxy.On<Stock>("UpdateStockPrice", stock => Console.WriteLine("Stock update for {0} new price {1}", stock.Symbol, stock.Price));
            //    hubConnection.Start().Wait();
            //    stockTickerHubProxy.Invoke("joinroom", "22/3");
            //}
            _uhubContext.Clients.All.Announce(message);
            _uhubContext.Clients.All.NewOrder("swiggy", 4444, 44);
        }

        [HttpPost("customerdata")]
        [EnableCors("AllowOrigin")]
        public IActionResult customerdata()
        {
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();

            SqlCommand cmd = new SqlCommand("dbo.POSOrderData", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
            sqlAdp.Fill(ds);

            DataTable table = ds.Tables[0];
            return Json(ds.Tables[0]);
        }
        [HttpPost("instamojotest")]
        [EnableCors("AllowOrigin")]
        public IActionResult instamojotest([FromForm]double amount)
        {
            try
            {
                var response = new
                {
                    message = "Data recieved successfully!",
                    status = 200
                };
                return Json(response);
            }
            catch(Exception e)
            {
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 200
                };
                return Json(error);
            }
        }
        [HttpGet("bulkcustomerdata")]
        [EnableCors("AllowOrigin")]
        public IActionResult bulkcustomerdata(int companyid, DateTime from)
        {
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();

            SqlCommand cmd = new SqlCommand("dbo.bulkcustomerdata", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@companyid", companyid));
            cmd.Parameters.Add(new SqlParameter("@from", from));
            DataSet ds = new DataSet();
            SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
            sqlAdp.Fill(ds);

            DataTable table = ds.Tables[0];
            return Json(ds.Tables[0]);
        }

        [HttpGet("emailTemplate")]
        [EnableCors("AllowOrigin")]
        public IActionResult emailTemplate()
        {
            string htmlString = System.IO.File.ReadAllText(_environment.WebRootPath + "\\email_template.html");
            htmlString = htmlString.Replace("CUSTOMER_NAME", "Nisha Agarwal");
            htmlString = htmlString.Replace("PHONE_NO", "1234567890");
            htmlString = htmlString.Replace("CUSTOMER_ADDRESS", "Karnataka");
            htmlString = htmlString.Replace("ERROR_STRING", "index cannot be less than zero.");
            var response = new
            {
                htmlString
            };
            return Json(response);
        }
    }
}
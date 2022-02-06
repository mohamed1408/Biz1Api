using Biz1BookPOS.Models;
using Biz1PosApi.Hubs;
using Biz1PosApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
//using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//using Quobject.SocketIoClientDotNet.Client;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(Roles = "Administrator")]
    public class UrbanPiperController : Controller
    {
        //private readonly IServiceScopeFactory _serviceScopeFactory;
        //private readonly ILoggerService loggerService;

        private POSDbContext db;
        private IHubContext<ChatHub> _hubContext;
        private IHubContext<UrbanPiperHub, IHubClient> _uhubContext;
        private static readonly HttpClient httpClient = new HttpClient();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        readonly ILogger<UrbanPiperController> _log;
        private static TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        public IConfiguration Configuration { get; }
        public UrbanPiperController(POSDbContext contextOptions, IConfiguration configuration, IHubContext<ChatHub> hubContext, ILogger<UrbanPiperController> log, IHubContext<UrbanPiperHub, IHubClient> uhubContext)
        {
            db = contextOptions;
            Configuration = configuration;
            _hubContext = hubContext;
            _uhubContext = uhubContext;
            _log = log;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("GetStoreById")]
        [EnableCors("AllowOrigin")]
        public IActionResult GetStoreById(int storeId, int companyId)
        {
            var urbanPiperStores = db.UrbanPiperStores.Where(x => x.StoreId == storeId && x.CompanyId == companyId).Include(x => x.Brand).Include(x => x.Store).ToList();
            return Ok(urbanPiperStores);
        }
        [HttpGet("GetIST")]
        [EnableCors("AllowOrigin")]
        public IActionResult GetIST()
        {
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            return Ok(indianTime);
        }
        [HttpGet("GetPrd")]
        [EnableCors("AllowOrigin")]
        public IActionResult GetPrd(int Id, int compId, int? brandId)
        {
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();

            SqlCommand cmd = new SqlCommand("dbo.UrbanPiperProducts", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@storeId", Id));
            cmd.Parameters.Add(new SqlParameter("@companyId", compId));
            cmd.Parameters.Add(new SqlParameter("@brandId", brandId));
            DataSet ds = new DataSet();
            SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
            sqlAdp.Fill(ds);


            var prod = new
            {
                Products = ds.Tables[0],
                OptionGroups = ds.Tables[1],
                Options = ds.Tables[2],
                TaxGroups = db.TaxGroups.Where(x => x.CompanyId == compId).ToList(),
                Charges = db.AdditionalCharges.Where(x => x.CompanyId == compId).ToList(),
                Categories = db.Categories.Where(x => x.CompanyId == compId).ToList(),
                Tags = db.UPTags.Where(x => x.CompanyId == compId).ToList()
            };
            sqlCon.Close();
            return Json(prod);
        }

        [HttpGet("GetExeUPProducts")]
        [EnableCors("AllowOrigin")]
        public IActionResult GetExeUPProducts(int Id, int compId)
        {
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();

            SqlCommand cmd = new SqlCommand("dbo.ExeUPProducts", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@storeId", Id));
            cmd.Parameters.Add(new SqlParameter("@companyId", compId));
            DataSet ds = new DataSet();
            SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
            sqlAdp.Fill(ds);


            var prod = new
            {
                Products = ds.Tables[0],
                OptionGroups = ds.Tables[1],
                Options = ds.Tables[2],
                TaxGroups = db.TaxGroups.Where(x => x.CompanyId == compId).ToList(),
                Charges = db.AdditionalCharges.Where(x => x.CompanyId == compId).ToList(),
                Categories = db.Categories.Where(x => x.CompanyId == compId).ToList()
            };
            sqlCon.Close();
            return Json(prod);
        }

        [HttpGet("getstoreuporders")]
        [EnableCors("AllowOrigin")]
        public IActionResult getstoreuporders(int storeid)
        {
            try
            {
                List<UPOrder> orders = db.UPOrders.Where(x => x.StoreId == storeid).ToList();
                var response = new
                {
                    status = 200,
                    orders = orders
                };
                return Json(response);
            }
            catch(Exception e)
            {
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = "error",
                    msg = "Something went wrong  Contact our service provider",
                    orders = new JArray()
                };
                return Json(error);
            }
        }

        ////ADDUPDATESTORE
        [HttpGet("AddStore")]
        public IActionResult AddStore(string value, int companyId)
        {
            try
            {
                string username = db.Accounts.Where(x => x.CompanyId == companyId).FirstOrDefault().UPUsername;
                string apikey = db.Accounts.Where(x => x.CompanyId == companyId).FirstOrDefault().UPAPIKey;
                int retrycount = 0;
            retry:
                retrycount++;
                var client = new RestClient(Configuration["UrbanPiper:URL"] + "external/api/v1/stores/");
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", "apikey " + username + ":" + apikey);
                request.AddParameter("application/json", value, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                var responseobject = new
                {
                    status = 0,
                    msg = ""
                };
                dynamic valuejson = JsonConvert.DeserializeObject(value);
                dynamic responsejson = JsonConvert.DeserializeObject(response.Content);
                UPLog uPLog = new UPLog();
                uPLog.Action = "add_store";
                uPLog.CompanyId = companyId;
                uPLog.Json = response.Content;
                uPLog.ReferenceId = responsejson.reference;
                uPLog.LogDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                if (valuejson.stores[0].ref_id.ToString().Contains("-"))
                {
                    String[] strlist = valuejson.stores[0].ref_id.ToString().Split("-");
                    uPLog.BrandId = Int16.Parse(strlist[0]);
                    uPLog.StoreId = Int16.Parse(strlist[1]);
                }
                else
                {
                    uPLog.StoreId = (int)valuejson.stores[0].ref_id;
                }
                db.UPLogs.Add(uPLog);
                db.SaveChanges();
                ////429
                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    responseobject = new
                    {
                        status = 429,
                        msg = response.StatusDescription + ". Retry after 1 minute."
                    };
                    return Json(responseobject);
                }
                ////500 || 503
                else if (response.StatusCode == HttpStatusCode.InternalServerError || response.StatusCode == HttpStatusCode.ServiceUnavailable)
                {
                    if (retrycount < 3)
                    {
                        goto retry;
                    }
                    responseobject = new
                    {
                        status = (response.StatusCode == HttpStatusCode.InternalServerError) ? 500 : 503,
                        msg = response.StatusDescription + ". Retry after 1 minute."
                    };
                    return Json(responseobject);
                }
                ////200
                else if (response.StatusCode == HttpStatusCode.OK)
                {
                    if (response.Content.Contains("error"))
                    {
                        return Json(response.Content);
                    }
                    dynamic json = JsonConvert.DeserializeObject(response.Content);
                    WebhookResponse webhookResponse = new WebhookResponse();
                    webhookResponse.RefId = json.reference;
                    webhookResponse.StatusCode = 0;
                    db.WebhookResponses.Add(webhookResponse);
                    db.SaveChanges();
                    WebhookResponse webhookResponse1 = new WebhookResponse();
                    Task taskA = Task.Run(() =>
                    {
                        for (; ; )
                        {
                            webhookResponse1 = db.WebhookResponses.Where(x => x.RefId == webhookResponse.RefId).AsNoTracking().FirstOrDefault();
                            if (webhookResponse1.StatusCode != 0)
                            {
                                break;
                            }
                            Thread.Sleep(2000);
                        }
                    });
                    taskA.Wait(60000);       // Wait for 1 minute.
                    bool completed = taskA.IsCompleted;
                    if (completed)
                    {
                        return Ok(webhookResponse1);
                    }
                    else
                    {
                        var error = new
                        {
                            status = "error",
                            msg = "Server Timed Out!"
                        };
                        return StatusCode(408, error);
                    }
                }
                else
                {
                    responseobject = new
                    {
                        status = (int)response.StatusCode,
                        msg = response.StatusDescription
                    };
                    return Json(responseobject);
                }
            }
            catch (Exception e)
            {
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = "error",
                    msg = "Something went wrong  Contact our service provider"
                };
                return StatusCode(500, error);
            }
        }

        [HttpPost("AddStoreCallback")]
        public IActionResult AddStoreCallback([FromBody] JObject value)
        {
            try
            {
                dynamic json = value;
                string referenceid = json.reference;
                UPLog actionuplog = db.UPLogs.Where(x => x.ReferenceId == referenceid).FirstOrDefault();
                UPLog uPLog = new UPLog();
                uPLog.Action = "add_store_callback";
                uPLog.ReferenceId = json.reference;
                uPLog.StoreId = actionuplog.StoreId;
                uPLog.BrandId = actionuplog.BrandId;
                uPLog.CompanyId = actionuplog.CompanyId;
                uPLog.LogDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                uPLog.Json = JsonConvert.SerializeObject(value);
                db.UPLogs.Add(uPLog);
                db.SaveChanges();
                if(referenceid != null)
                {
                    foreach (var store in json.stores)
                    {
                        if (store.upipr_status.error == false && store.upipr_status.action == "A")
                        {
                            UrbanPiperStore upstore = new UrbanPiperStore();
                            upstore.LocationName = store.name;
                            if (store.ref_id.ToString().Contains("-"))
                            {
                                String[] strlist = store.ref_id.ToString().Split("-");
                                upstore.BrandId = Int16.Parse(strlist[0]);
                                upstore.StoreId = Int16.Parse(strlist[1]);
                            }
                            else
                            {
                                upstore.StoreId = store.ref_id.ToObject<int>();
                            }
                            //upstore.StoreId = store.ref_id.ToObject<int>();
                            upstore.UPId = store.upipr_status.id;
                            upstore.CompanyId = db.Stores.Find(upstore.StoreId).CompanyId;
                            if (store.included_platforms != null)
                            {
                                upstore.IsZomato = false;
                                upstore.Zomato = false;
                                upstore.IsSwiggy = false;
                                upstore.Swiggy = false;
                                upstore.IsDunzo = false;
                                upstore.Dunzo = false;
                                //upstore.IsUrbanPiper = false;
                                //upstore.UberEats = false;
                                //upstore.FoodPanda = false;
                                //upstore.UrbanPiper = false;
                                foreach (var platform in store.included_platforms)
                                {
                                    if (platform == "zomato")
                                    {
                                        upstore.IsZomato = true;
                                        upstore.Zomato = true;
                                    }
                                    if (platform == "swiggy")
                                    {
                                        upstore.IsSwiggy = true;
                                        upstore.Swiggy = true;
                                    }
                                    //if (platform == "urbanpiper")
                                    //{
                                    //    upstore.IsUrbanPiper = true;
                                    //    upstore.UrbanPiper = true;
                                    //}
                                    if (platform == "dunzo")
                                    {
                                        upstore.IsDunzo = true;
                                        upstore.Dunzo = true;
                                    }
                                    //if (platform == "ubereats")
                                    //{
                                    //    upstore.UberEats = true;
                                    //}
                                    //if (platform == "foodpanda")
                                    //{
                                    //    upstore.FoodPanda = true;
                                    //}
                                }
                            }
                            if (store.excluded_platforms != null)
                            {
                                upstore.IsZomato = true;
                                upstore.Zomato = true;
                                upstore.IsSwiggy = true;
                                upstore.Swiggy = true;
                                upstore.IsDunzo = true;
                                upstore.Dunzo = true;
                                //upstore.IsUrbanPiper = true;
                                //upstore.UberEats = true;
                                //upstore.FoodPanda = true;
                                //upstore.UrbanPiper = true;
                                foreach (var platform in store.excluded_platforms)
                                {
                                    if (platform == "zomato")
                                    {
                                        upstore.IsZomato = false;
                                        upstore.Zomato = false;
                                    }
                                    if (platform == "swiggy")
                                    {
                                        upstore.IsSwiggy = false;
                                        upstore.Swiggy = false;
                                    }
                                    if (platform == "dunzo")
                                    {
                                        upstore.IsDunzo = false;
                                        upstore.Dunzo = false;
                                    }
                                    //if (platform == "ubereats")
                                    //{
                                    //    upstore.IsUrbanPiper = false;
                                    //    upstore.UberEats = false;
                                    //}
                                    //if (platform == "foodpanda")
                                    //{
                                    //    upstore.FoodPanda = false;
                                    //}
                                    //if (platform == "urbanpiper")
                                    //{
                                    //    upstore.UrbanPiper = false;
                                    //}
                                }
                            }
                            db.UrbanPiperStores.Add(upstore);
                            db.SaveChanges();
                        }
                        if (store.upipr_status.error == false && store.upipr_status.action == "U")
                        {
                            //int storeId = 0;
                            //int? brandId = null;
                            //if (store.ref_id.ToString().Contains("-"))
                            //{
                            //    String[] strlist = store.ref_id.ToString().Split("-");
                            //    brandId = Int16.Parse(strlist[0]);
                            //    storeId = Int16.Parse(strlist[1]);
                            //}
                            //else
                            //{
                            //    storeId = store.ref_id.ToObject<int>();
                            //}
                            string upiperid = store.upipr_status.id.ToString();
                            UrbanPiperStore upstore = db.UrbanPiperStores.Where(x => x.UPId == upiperid).FirstOrDefault();
                            if (store.name != null)
                            {
                                upstore.LocationName = store.name;
                            }
                            if (store.included_platforms != null)
                            {
                                upstore.IsZomato = false;
                                upstore.Zomato = false;
                                upstore.IsSwiggy = false;
                                upstore.Swiggy = false;
                                upstore.IsDunzo = false;
                                upstore.Dunzo = false;
                                //upstore.IsUrbanPiper = false;
                                //upstore.UberEats = false;
                                //upstore.FoodPanda = false;
                                //upstore.UrbanPiper = false;
                                foreach (var platform in store.included_platforms)
                                {
                                    if (platform == "zomato")
                                    {
                                        upstore.IsZomato = true;
                                        upstore.Zomato = true;
                                    }
                                    if (platform == "swiggy")
                                    {
                                        upstore.IsSwiggy = true;
                                        upstore.Swiggy = true;
                                    }
                                    if (platform == "dunzo")
                                    {
                                        upstore.IsDunzo = true;
                                        upstore.Dunzo = true;
                                    }
                                    //if (platform == "ubereats")
                                    //{
                                    //    upstore.UberEats = true;
                                    //}
                                    //if (platform == "foodpanda")
                                    //{
                                    //    upstore.FoodPanda = true;
                                    //}
                                    //if (platform == "urbanpiper")
                                    //{
                                    //    upstore.IsUrbanPiper = true;
                                    //    upstore.UrbanPiper = true;
                                    //}
                                }
                            }
                            if (store.excluded_platforms != null)
                            {
                                upstore.IsZomato = true;
                                upstore.Zomato = true;
                                upstore.IsSwiggy = true;
                                upstore.Swiggy = true;
                                upstore.IsDunzo = true;
                                upstore.Dunzo = true;
                                //upstore.IsUrbanPiper = true;
                                //upstore.UberEats = true;
                                //upstore.FoodPanda = true;
                                //upstore.UrbanPiper = true;
                                foreach (var platform in store.excluded_platforms)
                                {
                                    if (platform == "zomato")
                                    {
                                        upstore.IsZomato = false;
                                        upstore.Zomato = false;
                                    }
                                    if (platform == "swiggy")
                                    {
                                        upstore.IsSwiggy = false;
                                        upstore.Swiggy = false;
                                    }
                                    if (platform == "dunzo")
                                    {
                                        upstore.IsDunzo = false;
                                        upstore.Dunzo = false;
                                    }
                                    //if (platform == "ubereats")
                                    //{
                                    //    upstore.UberEats = false;
                                    //}
                                    //if (platform == "foodpanda")
                                    //{
                                    //    upstore.FoodPanda = false;
                                    //}
                                    //if (platform == "urbanpiper")
                                    //{
                                    //    upstore.IsUrbanPiper = false;
                                    //    upstore.UrbanPiper = false;
                                    //}
                                }
                            }
                            db.Entry(upstore).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }
                var error = new
                {
                    status = 200,
                    msg = "Store Added/Updated successfully"
                };
                string response = json.reference;
                if(response != null)
                {
                    WebhookResponse WebhookResponse = db.WebhookResponses.Where(x => x.RefId == response).FirstOrDefault();
                    WebhookResponse.StatusCode = 200;
                    db.Entry(WebhookResponse).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return Json(error);
            }
            catch (Exception e)
            {
                // Get stack trace for the exception with source file information
                var st = new StackTrace(e, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();

                dynamic json = value;
                string response = json.reference;
                if(response != null)
                {
                    WebhookResponse WebhookResponse = db.WebhookResponses.Where(x => x.RefId == response).FirstOrDefault(); ;
                    WebhookResponse.StatusCode = 500;
                    db.Entry(WebhookResponse).State = EntityState.Modified;
                    db.SaveChanges();
                }
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 0,
                    msg = "Something went wrong  Contact our service provider",
                    errorLine = line
                };
                return StatusCode(500, error);
            }
        }

        ////STOREACTION
        [HttpPost("StoreAction")]
        public IActionResult StoreAction([FromForm] string value, int companyId)
        {
            dynamic json = JsonConvert.DeserializeObject(value);
            object[] responses = new object[json.Count];
            int i = 0;
            foreach (var req in json)
            {
                string username = db.Accounts.Where(x => x.CompanyId == companyId).FirstOrDefault().UPUsername;
                string apikey = db.Accounts.Where(x => x.CompanyId == companyId).FirstOrDefault().UPAPIKey;
                int retrycount = 0;
            retry:
                retrycount++;
                var client = new RestClient(Configuration["UrbanPiper:URL"] + "hub/api/v1/location/");
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", "apikey " + username + ":" + apikey);
                request.AddParameter("application/json", JsonConvert.SerializeObject(req), ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                dynamic responsejson = JsonConvert.DeserializeObject(response.Content);
                UPLog uPLog = new UPLog();
                uPLog.Action = "store_action";
                uPLog.CompanyId = companyId;
                uPLog.Json = response.Content;
                uPLog.ReferenceId = responsejson.reference_id;
                uPLog.LogDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                if (req.location_ref_id.ToString().Contains("-"))
                {
                    String[] strlist = req.location_ref_id.Split("-");
                    uPLog.BrandId = Int16.Parse(strlist[0]);
                    uPLog.StoreId = Int16.Parse(strlist[1]);
                }
                else
                {
                    uPLog.StoreId = (int)req.location_ref_id;
                }
                db.UPLogs.Add(uPLog);
                db.SaveChanges();

                var responseobject = new
                {
                    status = 0,
                    message = ""
                };
                ////429
                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    responseobject = new
                    {
                        status = 429,
                        message = response.StatusDescription + ". Retry after 1 minute."
                    };
                    return Json(responseobject);
                }
                ////500 || 503
                else if (response.StatusCode == HttpStatusCode.InternalServerError || response.StatusCode == HttpStatusCode.ServiceUnavailable)
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
                    return Json(responseobject);
                }
                ////200,400,....
                else
                {
                    dynamic res = JsonConvert.DeserializeObject(response.Content);

                    responses[i] = new
                    {
                        status = res.status,
                        message = res.message,
                        storeid = req.location_ref_id
                    };
                    i++;
                    if (response.Content.Contains("success"))
                    {
                        int storeid = 0;
                        int? brandId = null;
                        if (req.location_ref_id.ToString().Contains("-"))
                        {
                            String[] strlist = req.location_ref_id.ToString().Split("-");
                            brandId = Int16.Parse(strlist[0]);
                            storeid = Int16.Parse(strlist[1]);
                        }
                        else
                        {
                            storeid = req.location_ref_id;
                        }
                        UrbanPiperStore urbanPiperStore = db.UrbanPiperStores.Where(x => x.StoreId == storeid && x.BrandId == brandId).FirstOrDefault();
                        foreach (var platform in req.platforms)
                        {
                            if (platform == "swiggy")
                            {
                                urbanPiperStore.Swiggy = (req.action == "enable") ? true : false;
                            }
                            if (platform == "zomato")
                            {
                                urbanPiperStore.Zomato = (req.action == "enable") ? true : false;
                            }
                            if (platform == "dunzo")
                            {
                                urbanPiperStore.Dunzo = (req.action == "enable") ? true : false;
                            }
                            //if (platform == "urbanpiper")
                            //{
                            //    urbanPiperStore.UrbanPiper = (req.action == "enable") ? true : false;
                            //}
                        }
                        db.Entry(urbanPiperStore).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                }
            }
            return Ok(responses);
        }

        [HttpPost("StoreActionCallback")]
        public IActionResult StoreActionCallback([FromBody] JObject value)
        {
            try
            {
                dynamic json = value;
                int storeId = json.location_ref_id.ToObject<int>();
                if (json.location_ref_id.ToString().Contains("-"))
                {
                    String[] strlist = json.location_ref_id.ToString().Split("-");
                    //upstore.BrandId = Int16.Parse(strlist[0]);
                    storeId = Int16.Parse(strlist[1]);
                }
                else
                {
                    storeId = json.location_ref_id;
                }
                UrbanPiperStore upstore = db.UrbanPiperStores.Where(x => x.StoreId == storeId).FirstOrDefault();
                if (json.status == true)
                {
                    if (json.action == "enable")
                    {
                        if (json.platform == "zomato")
                        {
                            upstore.Zomato = true;
                        }
                        if (json.platform == "swiggy")
                        {
                            upstore.Swiggy = true;
                        }
                        //if (json.platform == "ubereats")
                        //{
                        //    upstore.UberEats = true;
                        //}
                        //if (json.platform == "foodpanda")
                        //{
                        //    upstore.FoodPanda = true;
                        //}
                        //if (json.platform == "urbanpiper")
                        //{
                        //    upstore.UrbanPiper = true;
                        //}
                    }
                    else
                    {
                        if (json.platform == "zomato")
                        {
                            upstore.Zomato = false;
                        }
                        if (json.platform == "swiggy")
                        {
                            upstore.Swiggy = false;
                        }
                        //if (json.platform == "ubereats")
                        //{
                        //    upstore.UberEats = false;
                        //}
                        //if (json.platform == "foodpanda")
                        //{
                        //    upstore.FoodPanda = false;
                        //}
                        //if (json.platform == "urbanpiper")
                        //{
                        //    upstore.UrbanPiper = false;
                        //}
                    }
                    db.Entry(upstore).State = EntityState.Modified;
                    db.SaveChanges();
                    var error = new
                    {
                        status = 200,
                        msg = "StoreAction updated succesfully"
                    };
                    return Json(error);
                }
                else
                {
                    var error = new
                    {
                        status = 0,
                        msg = "StoreAction didn't updated please try again later"
                    };
                    return Json(error);
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
                return Json(error);
            }
        }
        [HttpGet("MasterCatalouge")]
        public IActionResult MasterCatalouge(int companyId)
        {
            try
            {
                string username = db.Accounts.Where(x => x.CompanyId == companyId).FirstOrDefault().UPUsername;
                string apikey = db.Accounts.Where(x => x.CompanyId == companyId).FirstOrDefault().UPAPIKey;
                int retrycount = 0;
                List<Product> products = db.Products.Where(x => x.CompanyId == companyId).ToList();
                List<Category> categories = db.Categories.Where(x => x.IsUPCategory == true && x.CompanyId == companyId).ToList();
                List<OptionGroup> optionGroups = db.OptionGroups.Where(x => x.CompanyId == companyId).ToList();
                List<Option> options = db.Options.Where(x => x.CompanyId == companyId).ToList();
                List<TaxGroup> taxGroups = db.TaxGroups.Where(x => x.CompanyId == companyId).ToList();
                MasterCatalogue masterCatalogue = new MasterCatalogue();
                masterCatalogue.categories = new List<UPCategory>();
                masterCatalogue.flush_items = true;
                masterCatalogue.flush_options = true;
                masterCatalogue.flush_option_groups = true;
                masterCatalogue.items = new List<UPItem>();
                masterCatalogue.options = new List<UPOption>();
                masterCatalogue.option_groups = new List<UPOptionGroup>();
                masterCatalogue.taxes = new List<UPTax>();
                foreach (Category category in categories)
                {
                    UPCategory uPCategory = new UPCategory();
                    uPCategory.active = true;
                    uPCategory.description = category.Description;
                    uPCategory.name = category.Description;
                    uPCategory.ref_id = category.Id.ToString();
                    uPCategory.sort_order = category.SortOrder;
                    if(category.ParentCategoryId != null)
                    {
                        uPCategory.parent_ref_id = new List<string>();
                        uPCategory.parent_ref_id.Add(category.ParentCategoryId.ToString());
                    }
                    uPCategory.translations = new List<JObject>();
                    masterCatalogue.categories.Add(uPCategory);
                }
                foreach (Product product in products)
                {
                    if(db.Categories.Find(product.CategoryId).IsUPCategory == true)
                    {
                        UPItem uPItem = new UPItem();
                        uPItem.available = true;
                        uPItem.category_ref_ids = new List<string>();
                        uPItem.category_ref_ids.Add(product.CategoryId.ToString());
                        uPItem.current_stock = -1;
                        uPItem.description = product.Description;
                        uPItem.food_type = product.ProductTypeId;
                        //uPItem.included_platforms = new List<string>();
                        //uPItem.included_platforms.Add("swiggy");
                        //uPItem.included_platforms.Add("zomato");
                        uPItem.tags = new JObject();
                        uPItem.price = product.UPPrice;
                        uPItem.ref_id = product.Id;
                        uPItem.sort_order = (product.SortOrder!=null)?product.SortOrder:0;
                        uPItem.sold_at_store = true;
                        uPItem.title = product.Name;
                        uPItem.recommended = product.Recomended;
                        if(product.ImgUrl != null)
                        {
                            uPItem.img_url = product.ImgUrl;
                        }
                        masterCatalogue.items.Add(uPItem);
                    }
                }
                foreach (OptionGroup optionGroup in optionGroups)
                {
                    List<ProductOptionGroup> productOptionGroups = db.ProductOptionGroups.Where(x => x.OptionGroupId == optionGroup.Id).ToList();
                    foreach (ProductOptionGroup productOptionGroup in productOptionGroups)
                    {
                        int categoryid = db.Products.Find(productOptionGroup.ProductId).CategoryId;
                        if(db.Categories.Find(categoryid).IsUPCategory == true)
                        {
                            if(masterCatalogue.option_groups.Where(x => x.ref_id == optionGroup.Id.ToString()).FirstOrDefault() != null)
                            {
                                masterCatalogue.option_groups.Where(x => x.ref_id == optionGroup.Id.ToString()).FirstOrDefault().item_ref_ids.Add(productOptionGroup.ProductId.ToString());
                            }
                            else
                            {
                                UPOptionGroup uPOptionGroup = new UPOptionGroup();
                                uPOptionGroup.active = true;
                                uPOptionGroup.item_ref_ids = new List<string>();
                                uPOptionGroup.item_ref_ids.Add(productOptionGroup.ProductId.ToString());
                                uPOptionGroup.max_selectable = optionGroup.MaximumSelectable;
                                uPOptionGroup.min_selectable = optionGroup.MinimumSelectable;
                                uPOptionGroup.ref_id = optionGroup.Id.ToString();
                                uPOptionGroup.title = optionGroup.Name;
                                masterCatalogue.option_groups.Add(uPOptionGroup);
                            }
                        }
                    }
                }
                foreach (Option option in options)
                {
                    if(masterCatalogue.option_groups.Where(x => x.ref_id == option.OptionGroupId.ToString()).FirstOrDefault() != null)
                    {
                        UPOption uPOption = new UPOption();
                        uPOption.available = true;
                        uPOption.description = option.Description;
                        uPOption.food_type = db.ProductOptionGroups.Where(x => x.OptionGroupId == option.OptionGroupId).FirstOrDefault().Product.ProductTypeId;
                        uPOption.opt_grp_ref_ids = new List<string>();
                        uPOption.opt_grp_ref_ids.Add(option.OptionGroupId.ToString());
                        uPOption.price = option.UPPrice;
                        uPOption.ref_id = option.Id.ToString();
                        uPOption.sold_at_store = true;
                        uPOption.title = option.Name;
                        masterCatalogue.options.Add(uPOption);
                    }
                }
                foreach (TaxGroup taxGroup in taxGroups)
                {
                    if(products.Where(x => x.TaxGroupId == taxGroup.Id).Count() > 0)
                    {
                        UPTax cgst_tax = new UPTax();
                        cgst_tax.active = true;
                        cgst_tax.code = "SGST_P";
                        cgst_tax.title = "SGST";
                        cgst_tax.description = taxGroup.Tax1.ToString() + "% CGST on all items";
                        cgst_tax.structure = new JObject(
                            new JProperty("value",taxGroup.Tax1)
                            );
                        UPTax sgst_tax = new UPTax();
                        sgst_tax.active = true;
                        sgst_tax.code = "SGST_P";
                        sgst_tax.title = "SGST";
                        sgst_tax.description = taxGroup.Tax1.ToString() + "% CGST on all items";
                        sgst_tax.structure = new JObject(
                            new JProperty("value", taxGroup.Tax1)
                            );
                        cgst_tax.item_ref_ids = new List<string>();
                        sgst_tax.item_ref_ids = new List<string>();
                        foreach (Product product in products.Where(x => x.TaxGroupId == taxGroup.Id).ToList())
                        {
                            cgst_tax.item_ref_ids.Add(product.Id.ToString());
                            sgst_tax.item_ref_ids.Add(product.Id.ToString());
                        }
                        masterCatalogue.taxes.Add(cgst_tax);
                        masterCatalogue.taxes.Add(sgst_tax);
                    }
                }
                //var client = new RestClient();
                //client = new RestClient(Configuration["UrbanPiper:URL"] + "external/api/v1/inventory/locations/-1/");
                //var request = new RestRequest(Method.POST);
                //request.AddHeader("content-type", "application/json");
                //request.AddHeader("Authorization", "apikey " + username + ":" + apikey);
                //request.AddParameter("application/json", JsonConvert.SerializeObject(masterCatalogue), ParameterType.RequestBody);
                //IRestResponse response = client.Execute(request);
                return StatusCode(200, masterCatalogue);
            }
            catch(Exception ex)
            {
                var error = new
                {
                    error = new Exception(ex.Message, ex.InnerException),
                    status = 0,
                    message = "Something went wrong. Check data after sometime"
                };
                return StatusCode(500, error);
            }
        }
        ////CATALOUGE
        [HttpPost("Catalouge")]
        public IActionResult Catalouge([FromForm] string catalogue, int storeId, int? BrandId, int companyId)
        {
            try
            {
                string username = db.Accounts.Where(x => x.CompanyId == companyId).FirstOrDefault().UPUsername;
                string apikey = db.Accounts.Where(x => x.CompanyId == companyId).FirstOrDefault().UPAPIKey;
                int retrycount = 0;
            retry:
                Thread.Sleep(10000);
                retrycount++;
                var client = new RestClient();
                if (BrandId != null)
                {
                    client = new RestClient(Configuration["UrbanPiper:URL"] + "external/api/v1/inventory/locations/" + BrandId + "-" + storeId + "/");
                }
                else
                {
                    client = new RestClient(Configuration["UrbanPiper:URL"] + "external/api/v1/inventory/locations/" + storeId + "/");
                }
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", "apikey " + username + ":" + apikey);
                request.AddParameter("application/json", catalogue, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                dynamic responsejson = JsonConvert.DeserializeObject(response.Content);
                UPLog uPLog = new UPLog();
                uPLog.Action = "catalogue";
                uPLog.CompanyId = companyId;
                uPLog.Json = response.Content;
                uPLog.ReferenceId = responsejson.reference;
                uPLog.LogDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                uPLog.StoreId = storeId;
                uPLog.BrandId = BrandId;
                db.UPLogs.Add(uPLog);
                db.SaveChanges();
                var responseobject = new
                {
                    status = 0,
                    message = "",
                    content = ""
                };
                ////429
                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    responseobject = new
                    {
                        status = 429,
                        message = response.StatusDescription + ". Retry after 1 minute.",
                        content = response.Content
                    };
                    return Json(responseobject);
                }
                ////500 || 503
                else if (response.StatusCode == HttpStatusCode.InternalServerError || response.StatusCode == HttpStatusCode.ServiceUnavailable)
                {
                    if (retrycount < 3)
                    {
                        goto retry;
                    }
                    responseobject = new
                    {
                        status = (response.StatusCode == HttpStatusCode.InternalServerError) ? 500 : 503,
                        message = response.StatusDescription + ". Retry after 1 minute.",
                        content = response.Content
                    };
                    return Json(responseobject);
                }
                ////200
                else if (response.StatusCode == HttpStatusCode.OK)
                {
                    if (response.Content.Contains("error"))
                    {
                        return Json(response.Content);
                    }
                    dynamic json = JsonConvert.DeserializeObject(response.Content);
                    string webhookRefId = json.reference;
                    //WebhookResponse webhookResponse = new WebhookResponse();
                    //webhookResponse.RefId = json.reference;
                    //webhookResponse.StatusCode = 0;
                    //db.WebhookResponses.Add(webhookResponse);
                    //db.SaveChanges();
                    WebhookResponse webhookResponse = new WebhookResponse();
                    Task taskA = Task.Run(() =>
                    {
                        for (; ; )
                        {
                            webhookResponse = db.WebhookResponses.Where(x => x.RefId == webhookRefId).AsNoTracking().FirstOrDefault();
                            if (webhookResponse != null)
                            {
                                break;
                            }
                            Thread.Sleep(2000);
                        }
                    });
                    taskA.Wait(600000);       // Wait for 1 minute.
                    bool completed = taskA.IsCompleted;
                    if (completed)
                    {
                        return Ok(webhookResponse);
                    }
                    else
                    {
                        var error = new
                        {
                            status = "error",
                            message = "Server Timed Out!"
                        };
                        return StatusCode(408, error);
                    }
                }
                else
                {
                    responseobject = new
                    {
                        status = (int)response.StatusCode,
                        message = response.StatusDescription,
                        content = response.Content.ToString()
                    };
                    return Json(responseobject);
                }
            }
            //            {
            //                "status": "success",
            //    "message": "Your request has been queued. Once processed, a callback will be issued to the configured webhook(s) or the URL passed in",
            //    "reference": "6ceae1e171bb44fc983690d4727b69b1"
            //}


            catch (Exception ex)
            {
                var error = new
                {
                    error = new Exception(ex.Message, ex.InnerException),
                    status = 0,
                    message = "Something went wrong. Check data after sometime"
                };
                return StatusCode(500, error);
            }
        }
        [HttpPost("CatalogueCallback")]
        public IActionResult CatalogueCallback([FromBody] JObject value)
        {
            dynamic payload = value;
            string referenceid = payload.reference;
            UPLog actionuplog = db.UPLogs.Where(x => x.ReferenceId == referenceid).FirstOrDefault();
            UPLog uPLog = new UPLog();
            uPLog.Action = "catalogue_callback";
            uPLog.ReferenceId = payload.reference;
            uPLog.StoreId = actionuplog.StoreId;
            uPLog.BrandId = actionuplog.BrandId;
            uPLog.CompanyId = actionuplog.CompanyId;
            uPLog.LogDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            uPLog.Json = JsonConvert.SerializeObject(value);
            db.UPLogs.Add(uPLog);
            db.SaveChanges();
            try
            {
                int i = 0;
                string errorreport = "Following Items are not updated";

                //CATEGORY
                foreach (var categ in payload.categories)
                {
                    int id = categ.ref_id;
                    if (categ.upipr_status.error.ToObject<bool>())
                    {
                        errorreport = errorreport + "/n " + categ.name + " [Category - " + id + "]";
                        i++;
                    }
                }
                //ITEM
                foreach (var item in payload.items)
                {
                    int id = item.ref_id;
                    if (item.upipr_status.error.ToObject<bool>())
                    {
                        errorreport = errorreport + "/n " + item.title + " [Product - " + id + "]";
                        i++;
                    }
                }
                //OPTION_GROUP
                foreach (var item in payload.option_groups)
                {
                    int id = item.ref_id;
                    if (item.upipr_status.error.ToObject<bool>())
                    {
                        errorreport = errorreport + "/n " + item.title + " [OptionGroup - " + id + "]";
                        i++;
                    }
                }
                //OPTION
                foreach (var item in payload.options)
                {
                    int id = item.ref_id;
                    if (item.upipr_status.error.ToObject<bool>())
                    {
                        errorreport = errorreport + "/n " + item.title + " [Option - " + id + "]";
                        i++;
                    }
                }
                string response = payload.reference;
                WebhookResponse WebhookResponse = new WebhookResponse();
                WebhookResponse.RefId = response;
                WebhookResponse.StatusCode = 200;
                WebhookResponse.message = (i > 0) ? errorreport : "Successfully Synced!";
                db.WebhookResponses.Add(WebhookResponse);
                db.SaveChanges();
                var error = new
                {
                    status = 0,
                    msg = "Catalouge Synced Successfully!"
                };
                return StatusCode(200, error);
            }
            catch (Exception ex)
            {
                var error = new
                {
                    error = new Exception(ex.Message, ex.InnerException),
                    status = 0,
                    msg = "Something went wrong. Check data after sometime"
                };
                return StatusCode(500, error);
            }
        }

        [HttpPost("BulkCatalogue")]
        public IActionResult BulkCatalogue([FromForm]string catalogue, int companyId)
        {
            try
            {
                string username = db.Accounts.Where(x => x.CompanyId == companyId).FirstOrDefault().UPUsername;
                string apikey = db.Accounts.Where(x => x.CompanyId == companyId).FirstOrDefault().UPAPIKey;
                dynamic bulkpayload = JsonConvert.DeserializeObject(catalogue);
                var responselist = new List<object>();
                foreach (var payload in bulkpayload)
                {
                    Thread.Sleep(10000);
                    var client = new RestClient();
                    if (payload.brandid != null)
                    {
                        client = new RestClient(Configuration["UrbanPiper:URL"] + "external/api/v1/inventory/locations/" + payload.brandid + "-" + payload.storeid + "/");
                    }
                    else
                    {
                        client = new RestClient(Configuration["UrbanPiper:URL"] + "external/api/v1/inventory/locations/" + payload.storeid + "/");
                    }
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("content-type", "application/json");
                    request.AddHeader("Authorization", "apikey " + username + ":" + apikey);
                    request.AddParameter("application/json", payload.payload, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    dynamic responsejson = JsonConvert.DeserializeObject(response.Content);
                    UPLog uPLog = new UPLog();
                    uPLog.Action = "catalogue";
                    uPLog.CompanyId = companyId;
                    uPLog.Json = response.Content;
                    uPLog.ReferenceId = responsejson.reference;
                    uPLog.LogDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                    uPLog.StoreId = payload.storeid;
                    uPLog.BrandId = payload.brandid;
                    db.UPLogs.Add(uPLog);
                    db.SaveChanges();

                    var responseobject = new
                    {
                        status = 0,
                        message = "",
                        content = "",
                        storeid = 0,
                        brandid = 0
                    };
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        if (response.Content.Contains("error"))
                        {
                            responseobject = new
                            {
                                status = (int)response.StatusCode,
                                message = response.StatusDescription,
                                content = response.Content.ToString(),
                                storeid = (int)payload.storeid,
                                brandid = payload.brandid ? (int)payload.brandid : 0
                            };
                        }
                        else
                        {
                            responseobject = new
                            {
                                status = (int)response.StatusCode,
                                message = response.StatusDescription,
                                content = response.Content.ToString(),
                                storeid = (int)payload.storeid,
                                brandid = payload.brandid != null ? (int)payload.brandid : 0
                            };
                        }
                    }
                    else
                    {
                        responseobject = new
                        {
                            status = (int)response.StatusCode,
                            message = response.StatusDescription,
                            content = response.Content.ToString(),
                            storeid = (int)payload.storeid,
                            brandid = payload.brandid != null ? (int)payload.brandid : 0
                        };
                    }
                    responselist.Add(responseobject);
                }
                return Ok(responselist);
            }
            //            {
            //                "status": "success",
            //    "message": "Your request has been queued. Once processed, a callback will be issued to the configured webhook(s) or the URL passed in",
            //    "reference": "6ceae1e171bb44fc983690d4727b69b1"
            //}


            catch (Exception ex)
            {
                // Get stack trace for the exception with source file information
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                var error = new
                {
                    error = new Exception(ex.Message, ex.InnerException),
                    status = 0,
                    message = "Something went wrong. Check data after sometime",
                    line = line
                };
                return StatusCode(500, error);
            }
        }

        [HttpPost("ItemAction")]
        public IActionResult ItemAction([FromForm] string stock, int companyId)
        {
            try
            {
                dynamic stock_data = JsonConvert.DeserializeObject(stock);
                List<Object> errors = new List<Object>();
                int storeid = 0;
                if (stock_data.location_ref_id.ToString().Contains("-"))
                {
                    String[] strlist = stock_data.location_ref_id.ToString().Split("-");
                    //upstore.BrandId = Int16.Parse(strlist[0]);
                    storeid = Int16.Parse(strlist[1]);
                }
                else
                {
                    storeid = stock_data.location_ref_id;
                }
                var products = db.StoreProducts.Where(x => x.StoreId == storeid).ToList();
                products.ForEach(p => p.UPAction = 0);
                db.SaveChanges();
                string username = db.Accounts.Where(x => x.CompanyId == companyId).FirstOrDefault().UPUsername;
                string apikey = db.Accounts.Where(x => x.CompanyId == companyId).FirstOrDefault().UPAPIKey;
                int retrycount = 0;
            retry:
                retrycount++;
                var client = new RestClient(Configuration["UrbanPiper:URL"] + "hub/api/v1/items/");
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application / json");
                request.AddHeader("Authorization", "apikey " + username + ":" + apikey);
                request.AddParameter("application / json", stock, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                dynamic responsejson = JsonConvert.DeserializeObject(response.Content);
                UPLog uPLog = new UPLog();
                uPLog.Action = "item_action";
                uPLog.CompanyId = companyId;
                uPLog.Json = response.Content;
                uPLog.ReferenceId = responsejson.reference_id;
                uPLog.LogDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                if (stock_data.location_ref_id.ToString().Contains("-"))
                {
                    String[] strlist = stock_data.location_ref_id.ToString().Split("-");
                    uPLog.BrandId = Int16.Parse(strlist[0]);
                    uPLog.StoreId = Int16.Parse(strlist[1]);
                }
                else
                {
                    uPLog.StoreId = (int)stock_data.location_ref_id;
                }
                db.UPLogs.Add(uPLog);
                db.SaveChanges();

                var responseobject = new
                {
                    status = 0,
                    message = ""
                };
                ////429
                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    responseobject = new
                    {
                        status = 429,
                        message = response.StatusDescription + ". Retry after 1 minute."
                    };
                    return Json(responseobject);
                }
                ////500 || 503
                else if (response.StatusCode == HttpStatusCode.InternalServerError || response.StatusCode == HttpStatusCode.ServiceUnavailable)
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
                    return Json(responseobject);
                }
                ////200
                else if (response.StatusCode == HttpStatusCode.OK)
                {
                        var error = new
                        {
                            status = 200,
                            message = response.Content
                        };
                        return StatusCode(200, error);
                }
                else
                {
                    responseobject = new
                    {
                        status = (int)response.StatusCode,
                        message = response.StatusDescription
                    };
                    return Json(responseobject);
                }
            }
            catch (Exception ex)
            {
                var error = new
                {
                    error = new Exception(ex.Message, ex.InnerException),
                    status = 0,
                    msg = "Something went wrong  Contact our service provider"
                };
                return Json(error);
            }
        }
        [HttpPost("ItemActionCallback")]
        public IActionResult ItemActionCallback([FromBody] JObject value)
        {
            try
            {
                dynamic json = value;
                string referenceid = json.reference_id;
                UPLog actionuplog = db.UPLogs.Where(x => x.ReferenceId == referenceid).FirstOrDefault();
                UPLog uPLog = new UPLog();
                uPLog.Action = "item_action_callback";
                uPLog.ReferenceId = referenceid;
                uPLog.LogDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                uPLog.Json = JsonConvert.SerializeObject(value);

                foreach (var status in json.status)
                {
                    int storeid = 0;
                    int? brandid = null;
                    if (status.location.ref_id.ToString().Contains("-"))
                    {
                        String[] strlist = status.location.ref_id.ToString().Split("-");
                        //upstore.BrandId = Int16.Parse(strlist[0]);
                        storeid = Int16.Parse(strlist[1]);
                        brandid = Int16.Parse(strlist[0]);
                        uPLog.StoreId = storeid;
                        uPLog.BrandId = brandid;
                        uPLog.CompanyId = db.Stores.Find(storeid).CompanyId;
                    }
                    else
                    {
                        storeid = status.location.ref_id;
                        uPLog.StoreId = storeid;
                        uPLog.CompanyId = db.Stores.Find(storeid).CompanyId;
                    }
                    foreach (var item in status.items)
                    {
                        int productid = item.ref_id;
                        StoreProduct storeProduct = db.StoreProducts.Where(x => x.ProductId == productid && x.StoreId == storeid).FirstOrDefault();
                        UPProduct upproduct = db.UPProducts.Where(x => x.ProductId == productid && x.StoreId == storeid && x.BrandId == brandid).FirstOrDefault();
                        if (json.action == "stock-in")
                        {
                            if (item.status == "success")
                            {
                                upproduct.Available = true;
                                storeProduct.Available = true;
                                storeProduct.UPAction = 1;
                            }
                            else
                            {
                                storeProduct.UPAction = -1;
                            }
                        }
                        if (json.action == "stock-out")
                        {
                            if (item.status == "success")
                            {
                                upproduct.Available = false;
                                storeProduct.Available = false;
                                storeProduct.UPAction = 1;
                            }
                            else
                            {
                                storeProduct.UPAction = -1;
                            }
                        }
                        db.Entry(upproduct).State = EntityState.Modified;
                        db.Entry(storeProduct).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                db.UPLogs.Add(uPLog);
                db.SaveChanges();
                var response = new
                {
                    status = 200,
                    msg = "Item status synced!"
                };
                return Json(response);
            }
            catch (Exception ex)
            {
                var error = new
                {
                    error = new Exception(ex.Message, ex.InnerException),
                    status = 0,
                    msg = "Something went wrong  Contact our service provider"
                };
                return StatusCode(500, error);
            }
        }
        [HttpPost("updateuporder")]
        public IActionResult updateuporder(int orderid, [FromBody] JObject value)
        {
            try
            {
                dynamic payload = value;
                Order order = db.Orders.Find(orderid);
                order.CancelReason = payload.CancelReason;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                var response = new
                {
                    status = 200,
                    msg = "saved successfully"
                };
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                var error = new
                {
                    error = new Exception(ex.Message, ex.InnerException),
                    status = 0,
                    msg = "Something went wrong  Contact our service provider"
                };
                return StatusCode(500, error);
            }
        }

        ////ORDER
        [HttpPost("Order1")]
        [EnableCors("AllowOrigin")]
        public void Order1([FromBody] JObject data)
        {
            SaveOnlineOrder(data,0);
        }
        //[HttpPost("SaveOrder")]
        //public IActionResult SaveOrder([FromBody] JObject data)
        //{
        //    Task.Run(() => SaveOrderAsync(data));
        //    var response = new
        //    {
        //        status = 200,
        //        message = "Task running..."
        //    };
        //    return Json(response);
        //}
        [HttpGet("sptest")]
        public IActionResult sptest(int uporderid)
        {
            try
            {
                db.Database.ExecuteSqlCommand($"EXECUTE SaveUPOrder_ {uporderid}");
                var response = new
                {
                    status = 200,
                    msg = "Task Running..."
                };
                return Ok(response);
            }
            catch(Exception e)
            {
                var response = new
                {
                    status = 500,
                    error = new Exception(e.Message, e.InnerException)
                };
                return Ok(response);
            }
        }
        [HttpGet("ChannelTest")]
        public async Task<IActionResult> ChannelTest([FromServices]Channel<UPOrderPayload> channel, int uporderid)
        {
            try
            {
                UPOrderPayload payload = new UPOrderPayload()
                {
                    UPOrderId = uporderid,
                    StoreId = 22,
                    Platform = "Zomato"
                };
                await channel.Writer.WriteAsync(payload);
                var response = new
                {
                    status = 200,
                    msg = "Task Running..."
                };
                return Ok(response);
            }
            catch(Exception e)
            {
                var response = new
                {
                    status = 500,
                    error = new Exception(e.Message, e.InnerException)
                };
                return Ok(response);
            }
        }

        [HttpPost("SaveOrderAsyncTest")]
        public async Task<IActionResult> SaveOrderAsyncTest([FromBody]JObject data)
        {
            try
            {
                dynamic Json = data;
                int storeId = 0;
                if (Json.order.store.merchant_ref_id.ToString().Contains("-"))
                {
                    String[] strlist = Json.order.store.merchant_ref_id.ToString().Split("-");
                    //upstore.BrandId = Int16.Parse(strlist[0]);
                    storeId = Int16.Parse(strlist[1]);
                }
                else
                {
                    storeId = Json.order.store.merchant_ref_id;
                }
                Store store = await db.Stores.FindAsync(storeId);
                int companyId = store.CompanyId;
                string UPOrderId = Json.order.details.id.ToString();
                string platform = Json.order.details.channel.ToString();
                string storename = store.Name;
                Json.order.details.platformorderid = Json.order.details.ext_platforms[0].id;
                int i = 0;
                foreach (var orderitem in Json.order.items)
                {
                    i++;
                    orderitem.refid = orderitem.merchant_id;
                    foreach (var option in orderitem.options_to_add)
                    {
                        orderitem.refid += i.ToString() + "_" + option.merchant_id;
                    }
                    foreach (var option in orderitem.options_to_add)
                    {
                        option.itemrefid = orderitem.refid;
                    }
                }
                var statusDetail = new
                {
                    accepted = 0,
                    foodready = 0,
                    dispatched = 0,
                    delivered = 0
                };
                _log.LogInformation(storeId.GetType().ToString());
                UPOrder uPOrder = new UPOrder();
                uPOrder.StoreId = 22;
                uPOrder.Json = "";
                uPOrder.UPOrderId = 1234;
                uPOrder.OrderStatusId = 0;
                uPOrder.OrderedDateTime = DateTime.Now;
                uPOrder.AcceptedTimeStamp = "";
                db.UPOrders.Add(uPOrder);
                db.SaveChanges();
                return Json(uPOrder);
            }
            catch(Exception e)
            {
                var error = new
                {
                    status = 500,
                    error = new Exception(e.Message, e.InnerException)
                };
                return Ok(error);
            }
        }
        [HttpPost("SaveOrderAsync")]
        public async Task<IActionResult> SaveOrderAsync([FromBody]JObject data, [FromServices]Channel<UPRawPayload> channel)
        {
            try
            {
                dynamic Json = data;
                string JsonString = JsonConvert.SerializeObject(Json);
                string UPOrderId = Json.order.details.id.ToString();
                if(!await db.Orders.Where(x => x.UPOrderId == Int32.Parse(UPOrderId)).AnyAsync())
                {
                    UPRawPayload rawPayload = new UPRawPayload()
                    {
                        Payload = JsonString
                    };
                    await channel.Writer.WriteAsync(rawPayload);
                }
                //int storeId = 0;
                //if (Json.order.store.merchant_ref_id.ToString().Contains("-"))
                //{
                //    String[] strlist = Json.order.store.merchant_ref_id.ToString().Split("-");
                //    //upstore.BrandId = Int16.Parse(strlist[0]);
                //    storeId = Int16.Parse(strlist[1]);
                //}
                //else
                //{
                //    storeId = Json.order.store.merchant_ref_id;
                //}

                //int companyId = db.Stores.Find(storeId).CompanyId;
                //string UPOrderId = Json.order.details.id.ToString();
                //string platform = Json.order.details.channel.ToString();
                //string storename = db.Stores.Find(storeId).Name;
                //Json.order.details.platformorderid = Json.order.details.ext_platforms[0].id;
                //int i = 0;
                //foreach (var orderitem in Json.order.items)
                //{
                //    i++;
                //    orderitem.refid = orderitem.merchant_id;
                //    foreach (var option in orderitem.options_to_add)
                //    {
                //        orderitem.refid += i.ToString() + "_" + option.merchant_id;
                //    }
                //    foreach (var option in orderitem.options_to_add)
                //    {
                //        option.itemrefid = orderitem.refid;
                //    }
                //}
                //if (!db.UrbanPiperOrders.Where(x => x.UPOrderId == Int32.Parse(UPOrderId)).Any())
                //{

                //    UrbanPiperOrder order = new UrbanPiperOrder();
                //    order.StoreId = storeId;
                //    order.Json = JsonConvert.SerializeObject(Json);
                //    order.UPOrderId = Json.order.details.id;
                //    order.OrderStatusId = 0;
                //    order.OrderedDateTime = UnixTimeStampToDateTime(Json.order.details.created.ToObject<Int64>());
                //    var j_string = new
                //    {
                //        accepted = 0,
                //        foodready = 0,
                //        dispatched = 0,
                //        delivered = 0
                //    };
                //    order.AcceptedTimeStamp = JsonConvert.SerializeObject(j_string);
                //    db.UrbanPiperOrders.Add(order);
                //    db.SaveChanges();
                //    _log.LogInformation("Order Payload --storeid - " + storeId + " --orderid - " + order.UPOrderId);
                //    UPOrderPayload payload = new UPOrderPayload()
                //    {
                //        UPOrderId = order.UPOrderId,
                //        StoreId = order.StoreId,
                //        Platform = platform
                //    };
                //    await channel.Writer.WriteAsync(payload);

                //    Console.WriteLine($"SignalR Event: NewOrder @ {TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE)} for store {storename}");
                //}
                var response = new
                {
                    status = 200,
                    msg = "Task Running..."
                };
                return Ok(response);
            }
            catch(Exception e)
            {
                var error = new
                {
                    status = 500,
                    error = new Exception(e.Message, e.InnerException)
                };
                return Ok(error);
            }
        }

        public static void SaveOrder(int UPOrderId, string connectionString)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(connectionString);
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.SaveUPOrder_", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@uporderid", UPOrderId));

                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                Console.WriteLine("Success Saved Order To DB");
            }
            catch(Exception e)
            {
                Console.WriteLine($"Message: {e.Message} InnerException: {e.InnerException.Message}");
            }
        }

        [HttpPost("Order")]
        [EnableCors("AllowOrigin")]
        public IActionResult Order([FromBody]JObject data)
        {
            try
            {
                dynamic Json = data;
                int storeId = 0;
                if (Json.order.store.merchant_ref_id.ToString().Contains("-"))
                {
                    String[] strlist = Json.order.store.merchant_ref_id.ToString().Split("-");
                    //upstore.BrandId = Int16.Parse(strlist[0]);
                    storeId = Int16.Parse(strlist[1]);
                }
                else
                {
                    storeId = Json.order.store.merchant_ref_id;
                }

                int companyId = db.Stores.Find(storeId).CompanyId;
                string UPOrderId = Json.order.details.id.ToString();
                string platform = Json.order.details.channel.ToString();
                string storename = db.Stores.Find(storeId).Name;
                Json.order.details.platformorderid = Json.order.details.ext_platforms[0].id;
                int i = 0;
                foreach (var orderitem in Json.order.items)
                {
                    i++;
                    orderitem.refid = orderitem.merchant_id;
                    foreach (var option in orderitem.options_to_add)
                    {
                        orderitem.refid += i.ToString() + "_" + option.merchant_id;
                    }
                    foreach (var option in orderitem.options_to_add)
                    {
                        option.itemrefid = orderitem.refid;
                    }
                }
                if (!db.UPOrders.Where(x => x.UPOrderId == Int32.Parse(UPOrderId)).Any())
                {

                    UPOrder order = new UPOrder();
                    order.StoreId = storeId;
                    order.Json = JsonConvert.SerializeObject(Json);
                    order.UPOrderId = Json.order.details.id;
                    order.OrderStatusId = 0;
                    order.OrderedDateTime = UnixTimeStampToDateTime(Json.order.details.created.ToObject<Int64>());
                    var j_string = new
                    {
                        accepted = 0,
                        foodready = 0,
                        dispatched = 0,
                        delivered = 0
                    };
                    order.AcceptedTimeStamp = JsonConvert.SerializeObject(j_string);
                    db.UPOrders.Add(order);
                    db.SaveChanges();
                    _log.LogInformation("Order Payload --storeid - " + storeId + " --orderid - " + order.UPOrderId);
                    NotifyNewOrder(storeId, storename, "", platform, Int32.Parse(UPOrderId));
                }
                var response = new
                {
                    status = 200,
                    message = "Order Placed",
                    payload = Json
                };
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                var error = new
                {
                    error = new Exception(ex.Message, ex.InnerException),
                    status = 0,
                    msg = "Something went wrong  Contact our service provider"
                };
                return StatusCode(500, error);
            }
        }
        [HttpPost("oldOrder")]
        [EnableCors("AllowOrigin")]
        public async Task<IActionResult> oldOrder([FromBody] JObject data)
        {
            using (var connection = new SqlConnection(Configuration.GetConnectionString("myconn")))
            //using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                connection.Open();
                var dbContextTransaction = connection.BeginTransaction();
                try
                {
                    dynamic Json = data;
                    int storeId = 0;
                    if (Json.order.store.merchant_ref_id.ToString().Contains("-"))
                    {
                        String[] strlist = Json.order.store.merchant_ref_id.ToString().Split("-");
                        //upstore.BrandId = Int16.Parse(strlist[0]);
                        storeId = Int16.Parse(strlist[1]);
                    }
                    else
                    {
                        storeId = Json.order.store.merchant_ref_id;
                    }
                    string storename = db.Stores.Find(storeId).Name;
                    //int storeId = Json.order.store.merchant_ref_id;
                    int companyId = db.Stores.Find(storeId).CompanyId;
                    string room = storeId.ToString() + '/' + companyId.ToString();
                    string phone = Json.customer.phone;
                    string socketdata = JsonConvert.SerializeObject(Json);
                    string UPOrderId = Json.order.details.id.ToString();
                    string platform = Json.order.details.channel.ToString();
                    //Order oldorder = db.Orders.Where(x => x.UPOrderId == UPOrderId).FirstOrDefault();
                    UPOrder olduporder = db.UPOrders.Where(x => x.UPOrderId == Int32.Parse(UPOrderId)).FirstOrDefault();
                    if (olduporder == null)
                    {
                        UPOrder order = new UPOrder();
                        order.StoreId = storeId;
                        order.Json = JsonConvert.SerializeObject(Json);
                        order.UPOrderId = Json.order.details.id;
                        order.OrderStatusId = 0;
                        order.OrderedDateTime = UnixTimeStampToDateTime(Json.order.details.created.ToObject<Int64>());
                        var j_string = new
                        {
                            accepted = 0,
                            foodready = 0,
                            dispatched = 0,
                            delivered = 0
                        };
                        order.AcceptedTimeStamp = JsonConvert.SerializeObject(j_string);
                        db.UPOrders.Add(order);
                        db.SaveChanges();
                        _log.LogInformation("Order Payload -- storeid - " + storeId + " --orderid - " + order.UPOrderId);
                        SaveOnlineOrder(data,0);
                    }
                    dbContextTransaction.Commit();
                    //Program.emit_order(socketdata, room);
                    NotifyNewOrder(storeId, storename, room, platform, Int32.Parse(UPOrderId));
                    var response = new
                    {
                        status = 200,
                        msg = "Order Recieved!"
                    };
                    connection.Close();
                    return Ok(response);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    var error = new
                    {
                        error = new Exception(e.Message, e.InnerException),
                        status = 500,
                        msg = "Something went wrong  Contact our service provider"
                    };
                    connection.Close();
                    return StatusCode(500, error);
                }
            }
        }
        [HttpGet("TestSignalR")]
        public IActionResult TestSignalR(int uporderid)
        {
            NotifyNewOrder(22, "Test", "", "zomato", uporderid);
            return Json(new { status = 200 });
        }
        public async void NotifyNewOrder(int storeid, string storename, string room, string platform, int UPOrderId)
        {
            Console.WriteLine($"Order Saved: NewOrder @ {TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE)} for store {storename} in room {room}");
            //var orders = db.UPOrders.Where(x => x.StoreId == storeid).ToList();
            //await _hubContext.Clients.Group(room).SendAsync("order", orders);
            //await _hubContext.Clients.Group(room).SendAsync("NewOrder", "New Order!!");
            //await _uhubContext.Clients.Group(room).NewOrder(platform, UPOrderId, storeid);
            await _uhubContext.Clients.All.NewOrder(platform, UPOrderId, storeid);
            Console.WriteLine($"SignalR Event: NewOrder @ {TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE)} for store {storename} in room {room}");
        }

        [HttpGet("GetUPOrder")]
        public IActionResult GetUPOrder(int uporderid)
        {
            try
            {
                Order order = db.Orders.Where(x => x.UPOrderId == uporderid).FirstOrDefault();
                var response = new
                {
                    status = 200,
                    order
                };
                return Json(response);
            }
            catch(Exception e)
            {
                var error = new
                {
                    status = 500,
                    error = new Exception(e.Message, e.InnerException)
                };
                return Json(error);
            }
        }
        [HttpGet("OrderStatus")]
        public IActionResult OrderStatus(int orderId, string statusdata, int companyId, int storeid, int orderstatusid)
        {

            string username = db.Accounts.Where(x => x.CompanyId == companyId).FirstOrDefault().UPUsername;
            string apikey = db.Accounts.Where(x => x.CompanyId == companyId).FirstOrDefault().UPAPIKey;

            //string orderHubroom = storeid + "/" + companyId;
            //UPOrder upOrder = db.UPOrders.Where(x => x.UPOrderId == orderId).FirstOrDefault();
            //upOrder.OrderStatusId = orderstatusid;
            //db.Entry(upOrder).State = EntityState.Modified;
            //db.SaveChanges();
            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //httpClient.DefaultRequestHeaders.Add("Authorization", "apikey " + username + ":" + apikey);
            //var resp = httpClient.PutAsync(Configuration["UrbanPiper:URL"] + "external/api/v1/orders/" + orderId + "/status/", new StringContent(statusdata));


            var client = new RestClient(Configuration["UrbanPiper:URL"] + "external/api/v1/orders/" + orderId + "/status/");
            var request = new RestRequest(Method.PUT);
            request.AddHeader("content-type", "application / json");
            request.AddHeader("Authorization", "apikey " + username + ":" + apikey);
            request.AddParameter("application / json", JsonConvert.DeserializeObject(statusdata), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            _log.LogInformation("Order Status -- storeid - " + storeid + " --orderid - " + orderId +" --statusid - "+ orderstatusid);

            var resp = new
            {
                status = response.StatusCode,
                msg = response.Content
            };
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return Ok(resp);
            }
            string orderHubroom = storeid + "/" + companyId;
            UPOrder upOrder = db.UPOrders.Where(x => x.UPOrderId == orderId).FirstOrDefault();
            upOrder.OrderStatusId = orderstatusid;
            db.Entry(upOrder).State = EntityState.Modified;
            db.SaveChanges();

            return Ok(resp);
        }
        [HttpGet("test_signalR")]
        public IActionResult test_signalR(string room, string event_n, string data)
        {
            _hubContext.Clients.Group(room).SendAsync(event_n, data);
            _hubContext.Clients.All.SendAsync("welcome", "Welcome!");
            var response = new
            {
                room = room
            };
            return Json(response);
        }
        [HttpPost("OrderStatusCallBack")]
        public IActionResult OrderStatusCallBack([FromBody] JObject value)
        {
            try
            {
                dynamic json = value;
                long uporderid = json.order_id;
                int orderstatusid = 0;
                Order order = db.Orders.Where(x => x.UPOrderId == uporderid).FirstOrDefault();
                UPOrder upOrder = db.UPOrders.Where(x => x.UPOrderId == uporderid).FirstOrDefault();
                if (json.new_state == "Acknowledged")
                {
                    orderstatusid = 1;
                    if (order != null && 1 >= order.OrderStatusId)
                    {
                        order.PreviousStatusId = order.OrderStatusId;
                        order.OrderStatusId = 1;
                        //dynamic os_json = JsonConvert.DeserializeObject(order.OrderStatusDetails);
                        //json.acknowledged = json.timestamp_unix;
                        //order.OrderStatusDetails = JsonConvert.SerializeObject(os_json);
                    }
                    if(upOrder.AcceptedTimeStamp == null)
                    {
                        dynamic statusdetails = new object();
                        statusdetails.accepted = json.timestamp_unix;
                        upOrder.AcceptedTimeStamp = JsonConvert.SerializeObject(statusdetails);
                    }
                    else
                    {
                        dynamic statusdetails = JsonConvert.DeserializeObject(upOrder.AcceptedTimeStamp);
                        statusdetails.accepted = json.timestamp_unix;
                        upOrder.AcceptedTimeStamp = JsonConvert.SerializeObject(statusdetails);
                    }
                }
                else if (json.new_state == "Food Ready")
                {
                    orderstatusid = 3;
                    if (order != null && 3 >= order.OrderStatusId)
                    {
                        order.PreviousStatusId = order.OrderStatusId;
                        order.OrderStatusId = 3;
                        //dynamic os_json = JsonConvert.DeserializeObject(order.OrderStatusDetails);
                        //json.foodready = json.timestamp_unix;
                        order.FoodReady = true;
                        //order.OrderStatusDetails = JsonConvert.SerializeObject(os_json);
                    }
                    if (upOrder.AcceptedTimeStamp == null)
                    {
                        dynamic statusdetails = new object();
                        statusdetails.foodready = json.timestamp_unix;
                        upOrder.AcceptedTimeStamp = JsonConvert.SerializeObject(statusdetails);
                    }
                    else
                    {
                        dynamic statusdetails = JsonConvert.DeserializeObject(upOrder.AcceptedTimeStamp);
                        statusdetails.foodready = json.timestamp_unix;
                        upOrder.AcceptedTimeStamp = JsonConvert.SerializeObject(statusdetails);
                    }
                }
                else if (json.new_state == "Dispatched")
                {
                    orderstatusid = 4;
                    if (order != null && 4 >= order.OrderStatusId)
                    {
                        order.PreviousStatusId = order.OrderStatusId;
                        order.OrderStatusId = 4;
                        //dynamic os_json = JsonConvert.DeserializeObject(order.OrderStatusDetails);
                        //json.dispatched = json.timestamp_unix;
                        //order.OrderStatusDetails = JsonConvert.SerializeObject(os_json);
                    }
                    if (upOrder.AcceptedTimeStamp == null)
                    {
                        dynamic statusdetails = new object();
                        statusdetails.dispatched = json.timestamp_unix;
                        upOrder.AcceptedTimeStamp = JsonConvert.SerializeObject(statusdetails);
                    }
                    else
                    {
                        dynamic statusdetails = JsonConvert.DeserializeObject(upOrder.AcceptedTimeStamp);
                        statusdetails.dispatched = json.timestamp_unix;
                        upOrder.AcceptedTimeStamp = JsonConvert.SerializeObject(statusdetails);
                    }
                }
                else if (json.new_state == "Completed")
                {
                    orderstatusid = 5;
                    if (order != null)
                    {
                        order.PreviousStatusId = order.OrderStatusId;
                        order.OrderStatusId = 5;
                        order.DeliveredDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                        order.DeliveredDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                        //dynamic os_json = JsonConvert.DeserializeObject(order.OrderStatusDetails);
                        //json.completed = json.timestamp_unix;
                        //order.OrderStatusDetails = JsonConvert.SerializeObject(os_json);
                    }
                    if (upOrder.AcceptedTimeStamp == null)
                    {
                        dynamic statusdetails = new object();
                        statusdetails.delivered = json.timestamp_unix;
                        upOrder.AcceptedTimeStamp = JsonConvert.SerializeObject(statusdetails);
                    }
                    else
                    {
                        dynamic statusdetails = JsonConvert.DeserializeObject(upOrder.AcceptedTimeStamp);
                        statusdetails.delivered = json.timestamp_unix;
                        upOrder.AcceptedTimeStamp = JsonConvert.SerializeObject(statusdetails);
                    }
                }
                else if (json.new_state == "Cancelled")
                {
                    orderstatusid = -1;
                    if (order != null)
                    {
                        order.PreviousStatusId = order.OrderStatusId;
                        order.OrderStatusId = -1;
                        //dynamic os_json = JsonConvert.DeserializeObject(order.OrderStatusDetails);
                        //json.completed = json.timestamp_unix;
                        //order.OrderStatusDetails = JsonConvert.SerializeObject(os_json);
                        
                    }
                    if (upOrder.AcceptedTimeStamp == null)
                    {
                        dynamic statusdetails = new object();
                        statusdetails.acceptedtime = json.timestamp_unix;
                        upOrder.AcceptedTimeStamp = JsonConvert.SerializeObject(statusdetails);
                    }
                    else
                    {
                        dynamic statusdetails = JsonConvert.DeserializeObject(upOrder.AcceptedTimeStamp);
                        statusdetails.acceptedtime = json.timestamp_unix;
                        upOrder.AcceptedTimeStamp = JsonConvert.SerializeObject(statusdetails);
                    }
                }
                if (order != null)
                {
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                }
                int storeid = 0;
                if (json.store_id.ToString().Contains("-"))
                {
                    String[] strlist = json.store_id.ToString().Split("-");
                    //upstore.BrandId = Int16.Parse(strlist[0]);
                    storeid = Int16.Parse(strlist[1]);
                }
                else
                {
                    storeid = json.store_id;
                }
                int companyid = db.Stores.Find(storeid).CompanyId;
                string room = storeid.ToString() + '/' + companyid.ToString();
                string orderid = json.store_id + '/' + json.order_id;
                //Program.change_order_status(room, orderid, order.OrderStatusId, json.timestamp_unix.ToString());
                upOrder.OrderStatusId = order.OrderStatusId;
                db.Entry(upOrder).State = EntityState.Modified;
                db.SaveChanges();
                orderupdatenotify(storeid, room, uporderid);
                //var orders = db.UPOrders.Where(x => x.StoreId == storeid).ToList();
                //await _hubContext.Clients.Group(room).SendAsync("order", orders);
                //await _hubContext.Clients.Group(room).SendAsync("orderupdate", orders);
                //await _uhubContext.Clients.Group(room).OrderUpdate(Int32.Parse(uporderid));
                _log.LogInformation("Order Status -- storeid - " + order.StoreId + " --orderid - " + uporderid + " --statusid - " + orderstatusid);
                var response = new
                {
                    status = 200,
                    msg = "Order Status " + json.new_state + " updated!"
                };
                return Json(response);
            }
            catch (Exception ex)
            {
                var error = new
                {
                    error = new Exception(ex.Message, ex.InnerException),
                    status = 500,
                    msg = "Something went wrong Contact our service provider"
                };
                return StatusCode(500, error);
            }
        }
        public async void orderupdatenotify(int storeid, string room, long uporderid)
        {
            var orders = db.UPOrders.Where(x => x.StoreId == storeid).ToList();
            await _hubContext.Clients.Group(room).SendAsync("order", orders);
            await _hubContext.Clients.Group(room).SendAsync("orderupdate", orders);
            await _uhubContext.Clients.All.OrderUpdate(uporderid, storeid);
        }
        [HttpPost("RiderStatusCallback")]
        public async Task<IActionResult> RiderStatusCallback([FromBody] JObject value)
        {
            try
            {
                dynamic json = value;
                //var sckt = IO.Socket("https://biz1socket.azurewebsites.net/");
                //var sckt = IO.Socket("http://localhost:3000/");
                int storeid = 0;
                if (json.store.ref_id.ToString().Contains("-"))
                {
                    String[] strlist = json.store.ref_id.ToString().Split("-");
                    //upstore.BrandId = Int16.Parse(strlist[0]);
                    storeid = Int16.Parse(strlist[1]);
                }
                else
                {
                    storeid = json.store.ref_id;
                }
                int companyid = db.Stores.Find(storeid).CompanyId;
                string room = storeid.ToString() + '/' + companyid.ToString();
                string orderid = json.store.ref_id + '/' + json.order_id;
                int uporderid = (int)json.order_id;
                UPOrder upOrder = db.UPOrders.Where(x => x.UPOrderId == uporderid).FirstOrDefault();
                upOrder.RiderDetails = JsonConvert.SerializeObject(value);
                db.Entry(upOrder).State = EntityState.Modified;
                db.SaveChanges();
                var orders = db.UPOrders.Where(x => x.StoreId == storeid).ToList();
                await _hubContext.Clients.Group(room).SendAsync("order", orders);
                await _hubContext.Clients.Group(room).SendAsync("orderupdate", orders);
                await _uhubContext.Clients.Group(room).OrderUpdate(uporderid, storeid);
                //sckt.Emit("join", room);
                //sckt.Emit("rider_status", orderid, json);
                //Program.update_riderstatus(room, orderid, json);
                var response = new
                {
                    status = 200,
                    msg = "CallBack recieved!"
                };
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                var error = new
                {
                    error = new Exception(ex.Message, ex.InnerException),
                    status = 0,
                    msg = "Something went wrong  Contact our service provider"
                };
                return StatusCode(500, error);
            }
        }
        [HttpGet("DeleteUPOrder")]
        public IActionResult DeleteUPOrder(int uporderid)
        {
            try
            {
                UPOrder uPOrder = db.UPOrders.Where(x => x.UPOrderId == uporderid).FirstOrDefault();
                db.UPOrders.Remove(uPOrder);
                db.SaveChanges();
                var response = new
                {
                    status = 200,
                    msg = "Order Deleted!"
                };
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                var error = new
                {
                    error = new Exception(ex.Message, ex.InnerException),
                    status = 0,
                    msg = "Something went wrong  Contact our service provider"
                };
                return StatusCode(500, error);
            }
        }
        public void SaveOnlineOrder(JObject data, int orderstatusid)
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
                neworder.OrderStatusId = orderstatusid;
                neworder.PreviousStatusId = 0;
                neworder.DiscPercent = 0;
                neworder.DiscAmount = order.order.details.discount - order.order.details.total_external_discount;
                neworder.Tax1 = order.order.details.total_taxes / 2;
                neworder.Tax2 = order.order.details.total_taxes / 2;
                neworder.Tax3 = 0;
                if (order.order.store.merchant_ref_id.ToString().Contains("-"))
                {
                    String[] strlist = order.order.store.merchant_ref_id.ToString().Split("-");
                    //upstore.BrandId = Int16.Parse(strlist[0]);
                    neworder.StoreId = Int16.Parse(strlist[1]);
                }
                else
                {
                    neworder.StoreId = order.order.store.merchant_ref_id;
                }
                //neworder.StoreId = order.order.store.merchant_ref_id.ToObject<int>();
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
                neworder.OrderNo = 0;
                neworder.UserId = null;
                neworder.IsAdvanceOrder = true;
                neworder.Source = order.order.details.channel;
                if (neworder.Source == "swiggy")
                {
                    neworder.SourceId = 2;
                    neworder.InvoiceNo = "S" + order.order.details.ext_platforms[0].id;
                }
                else if (neworder.Source == "zomato")
                {
                    neworder.SourceId = 3;
                    neworder.InvoiceNo = "Z" + order.order.details.ext_platforms[0].id;
                }
                else if (neworder.Source == "foodpanda")
                {
                    neworder.SourceId = 4;
                    neworder.InvoiceNo = "F" + order.order.details.ext_platforms[0].id;
                }
                neworder.AggregatorOrderId = order.order.details.ext_platforms[0].id;
                neworder.UPOrderId = order.order.details.id;
                neworder.CustomerData = JsonConvert.SerializeObject(order.customer);
                neworder.FoodReady = false;
                neworder.ChargeJson = ChargeJsonConvert(order.order.details.charges);
                neworder.ItemJson = ItemJsonConvert(order.order.items, order.order.details.order_subtotal, neworder.DiscAmount);
                var chrgs = 0;
                foreach (var charge in order.order.details.charges)
                {
                    chrgs = chrgs + charge.value.ToObject<int>();
                }
                neworder.Charges = chrgs;
                db.Orders.Add(neworder);
                db.SaveChanges();

                foreach (var charge in order.order.details.charges)
                {
                    //var chargename = charge.title.Replace(" ", String.Empty).ToLower();
                    AdditionalCharges additionalCharges = new AdditionalCharges();
                    if (charge.title.ToString().ToLower().Contains("packag"))
                    {
                        additionalCharges = db.AdditionalCharges.Where(x => x.Description == "Packaging Charge").FirstOrDefault();
                    }
                    else if (charge.title.ToString().ToLower().Contains("deliver"))
                    {
                        additionalCharges = db.AdditionalCharges.Where(x => x.Description == "Delivery Charge").FirstOrDefault();
                    }
                    TaxGroup taxGroup = db.TaxGroups.Where(x => x.Id == additionalCharges.TaxGroupId).FirstOrDefault();
                    if(taxGroup != null)
                    {
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
                    else
                    {
                        UPLog uPLog = new UPLog();
                        uPLog.Action = "Place Order";
                        uPLog.CompanyId = neworder.CompanyId;
                        uPLog.Json = JsonConvert.SerializeObject(data);
                        uPLog.LogDateTime = DateTime.Now;
                        uPLog.StoreId = (int)neworder.StoreId;
                        db.UPLogs.Add(uPLog);
                        db.SaveChanges();
                    }
                }

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
                    foreach (var option in item.options_to_add)
                    {
                        orderItem.Price = orderItem.Price + (float)option.price;
                    }
                    orderItem.StatusId = 5;
                    orderItem.KOTId = kot.Id;
                    orderItem.DiscPercent = 0;
                    orderItem.DiscAmount = (item.total / order.order.details.order_subtotal) * neworder.DiscAmount;
                    orderItem.ComplementryQty = 0;
                    orderItem.Tax1 = 0;
                    orderItem.Tax2 = 0;
                    orderItem.Tax3 = 0;
                    dynamic optionjson = new JObject();
                    optionjson.quantity = orderItem.Quantity;
                    optionjson.amount = item.total_with_tax;
                    optionjson.key = "";
                    optionjson.options = new JArray();
                    foreach (var option in item.options_to_add)
                    {
                        int optionid = option.merchant_id;
                        int? optiongrpid = db.Options.Find(optionid).OptionGroupId;
                        if (db.OptionGroups.Find(optiongrpid).OptionGroupType == 1)
                        {
                            dynamic optjson = new JObject();
                            optjson.Id = optionid;
                            optionjson.options.Add(optjson);
                            optionjson.key = optionjson.key + optionid.ToString() + "-";
                        }
                    }
                    orderItem.OptionJson = JsonConvert.SerializeObject(optionjson);
                    orderItem.CategoryId = db.Products.Find(orderItem.ProductId).CategoryId;
                    orderItem.TotalAmount = item.total_with_tax;
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
                        db.ordItemOptions.Add(ordItemOptions);
                        db.SaveChanges();
                    }
                }
            }

        }
        [HttpPost("savefailedorders")]
        public IActionResult savefailedorders([FromBody] JObject data)
        {
            try
            {
                SaveOnlineOrder(data, 5);
                var response = new
                {
                    msg = "success",
                    status = 200
                };
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                var error = new
                {
                    error = new Exception(ex.Message, ex.InnerException),
                    status = 0,
                    msg = "Something went wrong  Contact our service provider"
                };
                return StatusCode(500, error);
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
        public string ItemJsonConvert(JArray data, JValue order_subtotal, double order_discount)
        {
            dynamic dyndata = data;
            dynamic itemjson = new JObject();
            itemjson.KOTNo = "";
            itemjson.KotStatusId = 0;
            itemjson.item = new JArray();
            int i = 0;
            foreach (var item in dyndata)
            {
                int productid = item.merchant_id;
                dynamic product = new JObject();
                product.ComplementryQty = 0;
                product.DiscAmount = (item.total / order_subtotal) * order_discount;
                product.DiscPercent = 0;
                product.DiscType = 1;
                product.Discount = 0;
                product.FreeQtyPercentage = 0;
                product.KOTGroupId = db.Products.Find(productid).KOTGroupId;
                product.KOTId = 0;
                product.KOTNo = 0;
                product.Key = 0;
                product.MinimumQty = 0;
                //// product.OptionGroup = item.options_to_add;
                product.OptionGroup = new JArray();
                foreach (var option in item.options_to_add)
                {
                    int optionid = option.merchant_id;
                    int? optiongrpid = db.Options.Find(optionid).OptionGroupId;
                    OptionGroup optionGroup = db.OptionGroups.Find(optiongrpid);
                    Option option1 = db.Options.Find(optionid);
                    dynamic optiongrpjson = new JObject();
                    optiongrpjson.Id = optionGroup.Id;
                    optiongrpjson.Name = optionGroup.Name;
                    optiongrpjson.Option = new JArray();
                    optiongrpjson.OptionGroupType = optionGroup.OptionGroupType;
                    dynamic optjson = new JObject();
                    optjson.Id = option1.Id;
                    optjson.Price = option.price;
                    optjson.Name = option1.Name;
                    optiongrpjson.Option.Add(optjson);
                    product.OptionGroup.Add(optiongrpjson);
                }
                product.Price = item.price;
                product.Product = db.Products.Find(productid).Description;
                product.ProductId = productid;
                product.Quantity = item.quantity;
                product.StatusId = 1;
                product.Tax1 = 0;
                product.Tax2 = 0;
                product.Tax3 = 0;
                if (item.taxes.Count > 0)
                {
                    product.Tax1 = item.taxes[0].value;
                    product.Tax2 = item.taxes[1].value;
                    product.Tax3 = 0;
                }
                product.TaxGroupId = db.Products.Find(productid).TaxGroupId;
                product.TotalPrice = item.total_with_tax;
                itemjson.item.Add(product);
                i++;
            };
            dynamic array = new JArray(itemjson);
            return JsonConvert.SerializeObject(array);
        }
        public string ChargeJsonConvert(dynamic data)
        {
            dynamic chargearray = new JArray();
            int i = 0;
            foreach (var chrg in data)
            {
                dynamic obj = chrg;
                JObject chargejson = new JObject(
                    new JProperty("Id", obj.title == "Packaging Charge" ? db.AdditionalCharges.Find(2).Id : db.AdditionalCharges.Find(6).Id),
                    new JProperty("Description", obj.title),
                    new JProperty("ChargeType", 1),
                    new JProperty("ChargeValue", obj.value),
                    new JProperty("TaxGroupId", obj.title == "Packaging Charge" ? db.AdditionalCharges.Find(2).TaxGroupId : db.AdditionalCharges.Find(6).TaxGroupId),
                    new JProperty("ChargeAmount", 0)
                    );
                chargearray.Add(chargejson);
                i++;
            }
            return JsonConvert.SerializeObject(chargearray);
        }
        //public string OptionJsonConvert(JObject data)
        //{
        //    return istdate;
        //}
    }
}
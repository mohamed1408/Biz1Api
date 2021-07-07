using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Biz1BookPOS.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Biz1PosApi.Models;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    public class StoreDataController : Controller
    {
        private object table;
        private POSDbContext db;
        public IConfiguration Configuration { get; }
        public StoreDataController(POSDbContext contextOptions, IConfiguration configuration)
        {
            db = contextOptions;
            Configuration = configuration;
        }
        // GET: api/<controller>
        [HttpGet("Get")]
        public IActionResult Get(int compId, int storeId, string tables, string fKey, string fValue, int? kotGroupId = null)
        {
            try
            {
                //SqlConnection sqlCon = new SqlConnection("server=(LocalDb)\\MSSQLLocalDB; database=Biz1POS;Trusted_Connection=True;");
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.StoreData", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@compId", compId))
                    ;
                cmd.Parameters.Add(new SqlParameter("@storeId", storeId));
                cmd.Parameters.Add(new SqlParameter("@modDate", null));
                cmd.Parameters.Add(new SqlParameter("@table", tables));
                cmd.Parameters.Add(new SqlParameter("@fkey", fKey));
                cmd.Parameters.Add(new SqlParameter("@fvalue", fValue));
                cmd.Parameters.Add(new SqlParameter("@kotGroupId", kotGroupId));
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
                if (tables == "all")
                {
                    var data = new
                    {
                        Category = JsonConvert.DeserializeObject(catStr[0]),
                        TaxGroup = JsonConvert.DeserializeObject(catStr[1]),
                        Product = JsonConvert.DeserializeObject(catStr[2]),
                        DiningArea = JsonConvert.DeserializeObject(catStr[3]),
                        DiningTable = JsonConvert.DeserializeObject(catStr[4]),
                        DiscountRule = JsonConvert.DeserializeObject(catStr[5]),
                        AdditionalCharge = JsonConvert.DeserializeObject(catStr[6]),
                        OrderType = JsonConvert.DeserializeObject(catStr[7]),
                        Customers = JsonConvert.DeserializeObject(catStr[8]),
                        PaymentType = JsonConvert.DeserializeObject(catStr[9]),
                        KotGroups = JsonConvert.DeserializeObject(catStr[10]),
                        PendingOrders = JsonConvert.DeserializeObject(catStr[11]),
                        StorePaymentTypes = JsonConvert.DeserializeObject(catStr[13]),
                        OrderStatusButtons = JsonConvert.DeserializeObject(catStr[14]),
                        Stores = db.Stores.Where(x => x.CompanyId == compId).ToList(),
                        OrderStatus = new[]
                                {
                                new { Id = -1,Status = "Canceled" },
                                new { Id = 0, Status = "Pending" },
                                new { Id = 1, Status = "Accepted" },
                                new { Id = 2, Status = "Preparing" },
                                new { Id = 3, Status = "Prepared" },
                                new { Id = 4, Status = "Out For Delivery" },
                                new { Id = 5, Status = "Completed" }
                            },
                        ItemStatus = new[]
                                {
                                new { Id = -1,Status = "Canceled" },
                                new { Id = 1, Status = "Accepted" },
                                new { Id = 2, Status = "Started" },
                                new { Id = 3, Status = "Completed" }
                            },
                        KOTStatus = new[]
                                {
                                new { Id = -1,Status = "Canceled" },
                                new { Id = 1, Status = "Accepted" },
                                new { Id = 2, Status = "Started" },
                                new { Id = 3, Status = "Completed" }
                            },
                        PaymentStatus = new[]
                                {
                                new { Id = -1,Status = "Canceled" },
                                new { Id = 1, Status = "Pending" },
                                new { Id = 2, Status = "Completed" }
                            },
                        TableStatus = new[]
                                {
                                new { Id = 0,Status = "Empty" },
                                new { Id = 1, Status = "Seated" },
                                new { Id = 2, Status = "Ordered" },
                                new { Id = 3, Status = "Served" }
                            },
                        TransType = new[]
                                {
                                new { Id = 1, Status = "Sale" },
                                new { Id = 2, Status = "Sale_Refund" },
                                new { Id = 3, Status = "Expense" }
                            },
                        Aggregators = new[]
                                {
                                new { Id = 0, Status = "All" },
                                new { Id = 1, Status = "POS" },
                                new { Id = 2, Status = "Swiggy" },
                                new { Id = 3, Status = "Zomato" },
                                new { Id = 4, Status = "FoodPanda" }
                            }
                        //}
                        //}
                    };
                    sqlCon.Close();
                    return Ok(data);
                }
                else
                {
                    var data = JsonConvert.DeserializeObject(catStr[0]);
                    return Ok(data);
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

        // GET api/<controller>/5
        [HttpGet("GetPrice")]
        public IActionResult GetPrice(int storeId)
        {
            var prod = new
            {
                streprd =  from sp in db.StoreProducts
                           join p in db.Products on sp.ProductId equals p.Id
                           where sp.StoreId ==storeId
                           select new {p.Description,sp.Price,sp.TakeawayPrice,sp.DeliveryPrice,sp.StoreId,sp.CompanyId,sp.ProductId,sp.Id, sp.SortOrder, sp.Recommended},
                streopt = from os in db.StoreOptions
                          join o in db.Options on os.OptionId equals o.Id
                          where os.StoreId ==storeId
                          select new { o.Name, os.Price, os.TakeawayPrice, os.DeliveryPrice,os.StoreId,os.CompanyId,os.Id,os.OptionId }
            };
            return Json(prod);
        }

        // POST api/<controller>
        [HttpPost("Update")]
        public IActionResult UpdatePrd([FromForm]string data)
        {
            try
            {
                dynamic prds = JsonConvert.DeserializeObject(data);
                foreach (var item in prds)
                {
                    StoreProduct storeProduct = item.ToObject<StoreProduct>();
                    storeProduct.IsDineInService = true;
                    storeProduct.IsDeliveryService = true;
                    storeProduct.IsTakeAwayService = true;
                    storeProduct.UPPrice = storeProduct.Price;
                    storeProduct.CreatedDate = DateTime.Now;
                    storeProduct.ModifiedDate = DateTime.Now;
                    storeProduct.SyncedAt = DateTime.Now;
                    storeProduct.IsActive = true;
                    db.Entry(storeProduct).State = EntityState.Modified;
                    db.SaveChanges();
                }
                var error = new
                {
                    status = 200,
                    msg = "Price Book Succefully Updated"
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
        [HttpPost("UpdateOption")]
        public void UpdateOption([FromForm] string data)
        {
            dynamic optn = JsonConvert.DeserializeObject(data);
            foreach (var item in optn)
            {
                StoreOption storeOption = item.ToObject<StoreOption>();
                storeOption.CreatedDate = DateTime.Now;
                storeOption.ModifiedDate = DateTime.Now;
                storeOption.IsActive = true;
                db.Entry(storeOption).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
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
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    public class StoreDataController : Controller
    {
        private static TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
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
                var time = 0;
                var time1 = DateTime.Now;
                //SqlConnection sqlCon = new SqlConnection("server=(LocalDb)\\MSSQLLocalDB; database=Biz1POS;Trusted_Connection=True;");
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.StoreData", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@compId", compId));
                cmd.Parameters.Add(new SqlParameter("@storeId", storeId));
                cmd.Parameters.Add(new SqlParameter("@modDate", null));
                cmd.Parameters.Add(new SqlParameter("@table", tables));
                cmd.Parameters.Add(new SqlParameter("@fkey", fKey));
                cmd.Parameters.Add(new SqlParameter("@fvalue", fValue));
                cmd.Parameters.Add(new SqlParameter("@kotGroupId", kotGroupId));
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                //db.Database.ExecuteSqlCommand(cmd.ToString());
                DataTable table = ds.Tables[0];
                var time2 = DateTime.Now;
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
                var time3 = DateTime.Now;
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
                            },
                            sqltime = time2 - time1,
                            deserializationtime = time3 - time2,
                            
                        //}
                        //}
                    };
                    var time4 = DateTime.Now;
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
        [HttpGet("GetPrice")]
        public IActionResult GetPrice(int storeId)
        {
            var prod = new
            {
                streprd =  from sp in db.StoreProducts
                           join p in db.Products on sp.ProductId equals p.Id
                           where sp.StoreId ==storeId
                           select new {p.Name, p.Description,sp.Price,sp.TakeawayPrice,sp.DeliveryPrice,sp.StoreId,sp.CompanyId,sp.ProductId,sp.Id, sp.SortOrder, sp.Recommended},
                streopt = from os in db.StoreOptions
                          join o in db.Options on os.OptionId equals o.Id
                          where os.StoreId ==storeId
                          select new { o.Name, os.Price, os.TakeawayPrice, os.DeliveryPrice,os.StoreId,os.CompanyId,os.Id,os.OptionId }
            };
            return Json(prod);
        }

        public string OptionProducts(int opgid)
        {
            List<string> prods = db.Products.Where(p => db.ProductOptionGroups.Where(pog => pog.OptionGroupId == opgid && pog.ProductId == p.Id).Any()).Select(s => s.Name).ToList();

            return String.Join(", ", prods);
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

        [HttpGet("getstoredatav2")]
        public async Task<IActionResult> getstoredatav2(int storeid, int companyid, DateTime? lastsynceddatetime, string data = "ALL")
        {
            System.Diagnostics.Debug.WriteLine("STarting at ", DateTime.Now);
            List<Task> tasks = new List<Task>();
            var pendingorderstask = pendingordersTask(storeid, companyid);
            var productstask = productsTask2(storeid, companyid, lastsynceddatetime);
            var categorytask = categoryTask(storeid, companyid);
            var taxgrouptask = taxGroupTask(storeid, companyid);
            var diningareatask = diningAreaTask(storeid, companyid);
            var diningtablestask = diningTablesTask(storeid, companyid);
            var discountrulestask = discountRulesTask(storeid, companyid);
            var additionalchargestask = additionalChargesTask(storeid, companyid);
            var ordertypestask = orderTypesTask(storeid, companyid);
            var customerstask = customersTask(storeid, companyid);
            var paymenttypestask = paymentTypesTask(storeid, companyid);
            var kotgroupprinterstask = KOTGroupPrintersTask(storeid, companyid);
            var storepaymenttypestask = storePaymentTypesTask(storeid, companyid);
            var storestask = db.Stores.Where(x => x.CompanyId == companyid).ToListAsync();
            var ordernoTask = ordernoKotnoTask(storeid, companyid);
            if(data == "ALL")
            {
                tasks = new List<Task>() { productstask, pendingorderstask, categorytask, taxgrouptask, diningareatask, diningtablestask, discountrulestask,
             additionalchargestask, ordertypestask, customerstask, paymenttypestask, kotgroupprinterstask,
             storepaymenttypestask, storestask, ordernoTask };
            }
            else if(data == "MENU")
            {
                tasks = new List<Task>() { productstask, categorytask, taxgrouptask, discountrulestask, additionalchargestask, storepaymenttypestask };
            }
            else if(data == "ORDERS")
            {
                tasks = new List<Task>() { pendingorderstask, ordernoTask };
            }
            await Task.WhenAll(tasks);
            //await Task.WhenAll(productstask, pendingorderstask, categorytask, taxgrouptask, diningareatask, diningtablestask, discountrulestask,
            // additionalchargestask, ordertypestask, customerstask, paymenttypestask, kotgroupprinterstask,
            // storepaymenttypestask, storestask, ordernoTask);

            //int orderno = (int)ordernoTask.Result.Rows[0].ItemArray[0];
            //int kotno = Int32.Parse((string)ordernoTask.Result.Rows[0].ItemArray[1]);

            var response = new
            {
                Category = tasks.Contains(categorytask) ? categorytask.Result : new DataTable(),
                TaxGroup = tasks.Contains(taxgrouptask) ? taxgrouptask.Result : new DataTable(),
                Product = tasks.Contains(productstask) ? productstask.Result ?? new DataTable() : new DataTable(),
                DiningArea = tasks.Contains(diningareatask) ? diningareatask.Result : new DataTable(),
                DiningTable = tasks.Contains(diningtablestask) ? diningtablestask.Result : new DataTable(),
                DiscountRule = tasks.Contains(discountrulestask) ? discountrulestask.Result : new DataTable(),
                AdditionalCharge = tasks.Contains(additionalchargestask) ? additionalchargestask.Result : new DataTable(),
                OrderType = tasks.Contains(ordertypestask) ? ordertypestask.Result : new DataTable(),
                Customers = tasks.Contains(customerstask) ? customerstask.Result : new DataTable(),
                PaymentType = tasks.Contains(paymenttypestask) ? paymenttypestask.Result : new DataTable(),
                KotGroups = tasks.Contains(kotgroupprinterstask) ? kotgroupprinterstask.Result : new DataTable(),
                PendingOrders = tasks.Contains(pendingorderstask) ? pendingorderstask.Result : new DataTable(),
                StorePaymentTypes = tasks.Contains(storepaymenttypestask) ? storepaymenttypestask.Result : new DataTable(),
                Stores = tasks.Contains(storestask) ? storestask.Result : new List<Store>(),
                orderInfo = tasks.Contains(ordernoTask) ? ordernoTask.Result : new DataTable(),
                //Printers = db.Printers.Where(x => x.StoreId == storeid).ToList(),
                //OrderStatus = new[]
                //                {
                //                new { Id = -1,Status = "Canceled" },
                //                new { Id = 0, Status = "Pending" },
                //                new { Id = 1, Status = "Accepted" },
                //                new { Id = 2, Status = "Preparing" },
                //                new { Id = 3, Status = "Prepared" },
                //                new { Id = 4, Status = "Out For Delivery" },
                //                new { Id = 5, Status = "Completed" }
                //            },
                //ItemStatus = new[]
                //                {
                //                new { Id = -1,Status = "Canceled" },
                //                new { Id = 1, Status = "Accepted" },
                //                new { Id = 2, Status = "Started" },
                //                new { Id = 3, Status = "Completed" }
                //            },
                //KOTStatus = new[]
                //                {
                //                new { Id = -1,Status = "Canceled" },
                //                new { Id = 1, Status = "Accepted" },
                //                new { Id = 2, Status = "Started" },
                //                new { Id = 3, Status = "Completed" }
                //            },
                //PaymentStatus = new[]
                //                {
                //                new { Id = -1,Status = "Canceled" },
                //                new { Id = 1, Status = "Pending" },
                //                new { Id = 2, Status = "Completed" }
                //            },
                //TableStatus = new[]
                //                {
                //                new { Id = 0,Status = "Empty" },
                //                new { Id = 1, Status = "Seated" },
                //                new { Id = 2, Status = "Ordered" },
                //                new { Id = 3, Status = "Served" }
                //            },
                //TransType = new[] {
                //                new { Id = 1, Status = "Sale" },
                //                new { Id = 2, Status = "Sale_Refund" },
                //                new { Id = 3, Status = "Expense" }
                //            },
                //Aggregators = new[]
                //                {
                //                new { Id = 0, Status = "All" },
                //                new { Id = 1, Status = "POS" },
                //                new { Id = 2, Status = "Swiggy" },
                //                new { Id = 3, Status = "Zomato" },
                //                new { Id = 4, Status = "FoodPanda" }
                //            },
            };
            return Json(response);
        }

        [HttpGet("getStoredata")]
        public async Task<IActionResult> getStoredata(int storeid, int companyid)
        {
            System.Diagnostics.Debug.WriteLine("STarting at ", DateTime.Now);
            var pendingorderstask = pendingordersTask(storeid, companyid);
            var productstask = newProductsTask(storeid, companyid);
            var categorytask = categoryTask(storeid, companyid);
            var taxgrouptask = taxGroupTask(storeid, companyid);
            var diningareatask = diningAreaTask(storeid, companyid);
            var diningtablestask = diningTablesTask(storeid, companyid);
            var discountrulestask = discountRulesTask(storeid, companyid);
            var additionalchargestask = additionalChargesTask(storeid, companyid);
            var ordertypestask = orderTypesTask(storeid, companyid);
            var customerstask = customersTask(storeid, companyid);
            var paymenttypestask = paymentTypesTask(storeid, companyid);
            var kotgroupprinterstask = KOTGroupPrintersTask(storeid, companyid);
            var storepaymenttypestask = storePaymentTypesTask(storeid, companyid);
            var storestask = db.Stores.Where(x => x.CompanyId == companyid).ToListAsync();
            var ordernoTask = ordernoKotnoTask(storeid, companyid);

            await Task.WhenAll(productstask, pendingorderstask, categorytask, taxgrouptask, diningareatask, diningtablestask, discountrulestask,
             additionalchargestask, ordertypestask, customerstask, paymenttypestask, kotgroupprinterstask,
             storepaymenttypestask, storestask, ordernoTask);

            //DataTable category = new DataTable();
            //DataTable taxGroup = new DataTable();
            //DataTable products = new DataTable();
            //DataTable diningArea = new DataTable();
            //DataTable diningTables = new DataTable();
            //DataTable discountRules = new DataTable();
            //DataTable additionalCharges = new DataTable();
            //DataTable orderTypes = new DataTable();
            //DataTable customers = new DataTable();
            //DataTable paymentTypes = new DataTable();
            //DataTable KOTGroupPrinters = new DataTable();
            //DataTable pendingorders = new DataTable();
            //DataTable storePaymentTypes = new DataTable();

            //Debug.WriteLine("LOAD START: -- " + DateTime.Now.ToString());
            //category.Load(categorytask.Result);
            //taxGroup.Load(taxgrouptask.Result);
            //products.Load(productstask.Result);
            //diningArea.Load(diningareatask.Result);
            //diningTables.Load(diningtablestask.Result);
            //discountRules.Load(discountrulestask.Result);
            //additionalCharges.Load(additionalchargestask.Result);
            //orderTypes.Load(ordertypestask.Result);
            //customers.Load(customerstask.Result);
            //paymentTypes.Load(paymenttypestask.Result);
            //KOTGroupPrinters.Load(kotgroupprinterstask.Result);
            //pendingorders.Load(pendingorderstask.Result);
            //storePaymentTypes.Load(storepaymenttypestask.Result);
            //Debug.WriteLine("LOAD END: -- " + DateTime.Now.ToString());

            //var response = new
            //{
            //    category,
            //    taxGroup,
            //    products,
            //    diningArea,
            //    diningTables,
            //    discountRules,
            //    additionalCharges,
            //    orderTypes,
            //    customers,
            //    paymentTypes,
            //    KOTGroupPrinters,
            //    pendingorders,
            //    storePaymentTypes
            //};
            int orderno = (int)ordernoTask.Result.Rows[0].ItemArray[0];
            int kotno = Int32.Parse((string)ordernoTask.Result.Rows[0].ItemArray[1]);
            //int orderno = db.Orders.Where(x => x.StoreId == storeid && x.OrderTypeId < 6 && x.OrderedDate == TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time).Date).Select(x => x.OrderNo).DefaultIfEmpty(0).Max();
            //int kotno = db.KOTs.Where(x => x.StoreId == storeid && x.Order.OrderTypeId < 6 && x.CreatedDate.Date == TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time).Date).Select(x => int.Parse(x.KOTNo)).DefaultIfEmpty(0).Max();
            var response = new
            {
                Category = categorytask.Result,
                TaxGroup = taxgrouptask.Result,
                Product = productstask.Result,
                DiningArea = diningareatask.Result,
                DiningTable = diningtablestask.Result,
                DiscountRule = discountrulestask.Result,
                AdditionalCharge = additionalchargestask.Result,
                OrderType = ordertypestask.Result,
                Customers = customerstask.Result,
                PaymentType = paymenttypestask.Result,
                KotGroups = kotgroupprinterstask.Result,
                PendingOrders = pendingorderstask.Result,
                StorePaymentTypes = storepaymenttypestask.Result,
                Stores = storestask.Result,
                OrderStatusButtons = Array.Empty<OrderStatusButton>(),
                orderInfo = new {
                    orderno =  orderno+1,
                    kotno = kotno+1,
                },
                Printers = db.Printers.Where(x => x.StoreId == storeid).ToList(),
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
                TransType = new[] {
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
                            },
            };
            return Json(response);
        }
        public async Task<DataTable> ordernoKotnoTask(int storeid, int companyid)
        {
            System.Diagnostics.Debug.WriteLine("categoryTask START: -- " + DateTime.Now.ToString());
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();

            SqlCommand cmd = new SqlCommand(@"SELECT isnull(MAX(o.OrderNo),0) orderno, isnull(MAX(k.KOTNo),0) kotno FROM Orders o
                                            JOIN KOTs k ON k.OrderId = o.Id
                                            WHERE o.OrderTypeId <= 5 AND o.OrderedDate =  CONVERT(VARCHAR(10), getdate(), 111) AND o.StoreId = @storeid
                                            
                                            --SELECT ca.Id,  ca.Description as Category, ISNULL(ca.ParentCategoryId,0) AS ParentId FROM Categories ca
                                            --JOIN Companies co on co.Id = ca.CompanyId
                                            --JOIN Preferences p ON co.Id = p.CompanyId
                                            --WHERE( ca.CompanyId = @companyid) AND
                                            --ca.isactive = 1 AND ((p.ShowUpcategory = 0 AND ca.IsUPCategory = 0) OR (p.ShowUpcategory = 1))", sqlCon);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@storeid", storeid));

            //Task<SqlDataReader> reader = cmd.ExecuteReaderAsync();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable table = new DataTable();
            table.Load(reader);
            System.Diagnostics.Debug.WriteLine("categoryTask END: -- " + DateTime.Now.ToString());
            return table;
        }
        public async Task<DataTable> categoryTask(int storeid, int companyid)
        {
            System.Diagnostics.Debug.WriteLine("categoryTask START: -- " + DateTime.Now.ToString());
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();

            SqlCommand cmd = new SqlCommand(@"SELECT ca.Id,  ca.Description as Category, ISNULL(ca.ParentCategoryId,0) AS ParentId FROM Categories ca
                                            JOIN Companies co on co.Id = ca.CompanyId
                                            JOIN Preferences p ON co.Id = p.CompanyId
                                            WHERE( ca.CompanyId = @companyid)AND
                                            ca.isactive = 1 AND ((p.ShowUpcategory = 0 AND ca.IsUPCategory = 0) OR (p.ShowUpcategory = 1))", sqlCon);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@companyid", companyid));

            //Task<SqlDataReader> reader = cmd.ExecuteReaderAsync();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable table = new DataTable();
            table.Load(reader);
            System.Diagnostics.Debug.WriteLine("categoryTask END: -- " + DateTime.Now.ToString());
            return table;
        }
        public async Task<DataTable> taxGroupTask(int storeid, int companyid)
        {
            System.Diagnostics.Debug.WriteLine("taxGroupTask START: -- " + DateTime.Now.ToString());
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();

            SqlCommand cmd = new SqlCommand(@"SELECT tg.Id, tg.Description as TaxGroup, ISNULL(tg.Tax1,0) as CGST, ISNULL(tg.Tax2,0) as SGST, ISNULL(tg.Tax3,0) as IGST, tg.IsInclusive FROM TaxGroups tg
                                            WHERE (tg.CompanyId = @companyid)", sqlCon);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@companyid", companyid));

            //Task<SqlDataReader> reader = cmd.ExecuteReaderAsync();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable table = new DataTable();
            table.Load(reader);
            System.Diagnostics.Debug.WriteLine("taxGroupTask END: -- " + DateTime.Now.ToString());
            return table;
        }
        public async Task<JArray> productsTask(int storeid, int companyid)
        {
            System.Diagnostics.Debug.WriteLine("productsTask START: -- " + DateTime.Now.ToString());
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();

            SqlCommand cmd = new SqlCommand(@"SELECT sp.ProductId AS Id, p.Name as Product, p.BarCode, p.ProductCode, u.Description as Unit, sp.Price,sp.TakeawayPrice,
                                              sp.DeliveryPrice, sp.IsActive,
                                              sp.IsDineInService, sp.IsDeliveryService, sp.IsTakeAwayService, tg.Id AS TaxGroupId,ca.Id as CategoryId,ca.ParentCategoryId,p.KOTGroupId,
                                            ca.MinimumQty, ca.FreeQtyPercentage, tg.Tax1, tg.Tax2, tg.Tax3,p.ProductTypeId,tg.IsInclusive as IsTaxInclusive, 
                                            CAST(CASE WHEN ca.IsUPCategory = 0 OR sr.ShowUpcategory = ca.IsUPCategory THEN 0 ELSE 1 END AS BIT) ishidden,
                                              (
                                              SELECT pog.OptionGroupId AS Id, og.Name,og.OptionGroupType,
                                              (
                                                SELECT so.OptionId AS Id, so.Price, so.TakeawayPrice, so.DeliveryPrice, o.Name, o.IsSingleQtyOption
                                                FROM dbo.StoreOptions so
                                                JOIN Options o ON so.OptionId = o.Id
                                                WHERE (so.StoreId = @storeid AND o.OptionGroupId = pog.OptionGroupId)
                                                FOR JSON PATH
                                              )AS 'Option'
                                              FROM dbo.ProductOptionGroups pog
                                              JOIN dbo.OptionGroups og ON pog.OptionGroupId = og.Id
                                              WHERE (pog.ProductId = sp.ProductId AND pog.CompanyId = @companyid)
                                              FOR JSON PATH
                                            )AS 'OptionGroup'
                                        FROM StoreProducts sp
                                        JOIN Products p on p.Id = sp.ProductId
                                        JOIN Categories ca on ca.Id = p.CategoryId
                                        JOIN Units u on u.Id = p.UnitId
                                        JOIN TaxGroups tg on tg.Id = p.TaxGroupId
                                        JOIN StorePreferences sr ON sp.StoreId = sr.StoreId
                                        WHERE p.isactive = 1 AND ca.isactive = 1 AND
                                             (sp.StoreId = @storeid OR @storeid = 0) AND (sp.CompanyId = @companyid) --AND ((sr.ShowUpcategory = 0 AND ca.IsUPCategory = 0) OR (sr.ShowUpcategory = 1))
                                        FOR JSON PATH", sqlCon);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@companyid", companyid));
            cmd.Parameters.Add(new SqlParameter("@storeid", storeid));

            //Task<SqlDataReader> reader = cmd.ExecuteReaderAsync();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable table = new DataTable();
            table.Load(reader);
            string catStr = "";
            for (int j = 0; j < table.Rows.Count; j++)
            {
                catStr += table.Rows[j].ItemArray[0].ToString();
            }
            JArray products = (JArray)JsonConvert.DeserializeObject(catStr);
            System.Diagnostics.Debug.WriteLine("productsTask END: -- " + DateTime.Now.ToString());
            return products;
        }
        public async Task<DataTable> diningAreaTask(int storeid, int companyid)
        {
            System.Diagnostics.Debug.WriteLine("diningAreaTask START: -- " + DateTime.Now.ToString());
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();

            SqlCommand cmd = new SqlCommand(@"SELECT da.Id, da.Description as DiningArea FROM DiningAreas da
                                            WHERE (da.StoreId = @storeid) AND (@companyid is NULL OR da.CompanyId = @companyid)", sqlCon);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@companyid", companyid));
            cmd.Parameters.Add(new SqlParameter("@storeid", storeid));

            //Task<SqlDataReader> reader = cmd.ExecuteReaderAsync();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable table = new DataTable();
            table.Load(reader);
            System.Diagnostics.Debug.WriteLine("diningAreaTask END: -- " + DateTime.Now.ToString());
            return table;
        }
        public async Task<DataTable> diningTablesTask(int storeid, int companyid)
        {
            System.Diagnostics.Debug.WriteLine("diningTablesTask START: -- " + DateTime.Now.ToString());
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();

            SqlCommand cmd = new SqlCommand(@"SELECT dt.Id, dt.Description as TableName, dt.DiningAreaId, dt.TableStatusId, CAST(dt.Id as varchar(10)) as TableKey, 0 as LastSplitTableId   FROM DiningTables dt
                                            WHERE (dt.StoreId = @storeid) AND
                                            (@companyid is null OR dt.CompanyId = @companyid)", sqlCon);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@companyid", companyid));
            cmd.Parameters.Add(new SqlParameter("@storeid", storeid));

            //Task<SqlDataReader> reader = cmd.ExecuteReaderAsync();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable table = new DataTable();
            table.Load(reader);
            System.Diagnostics.Debug.WriteLine("diningTablesTask END: -- " + DateTime.Now.ToString());
            return table;
        }
        public async Task<DataTable> discountRulesTask(int storeid, int companyid)
        {
            System.Diagnostics.Debug.WriteLine("discountRulesTask START: -- " + DateTime.Now.ToString());
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();

            SqlCommand cmd = new SqlCommand(@"SELECT dr.Id, dr.CouponCode, dr.DiscountType, dr.DiscountValue, dr.MiniOrderValue, dr.MaxDiscountAmount           
                                            FROM dbo.DiscountRules dr
                                            WHERE (@companyid is null OR dr.CompanyId = @companyid)
                                            UNION
                                            SELECT 0, NULL, 0, 0, 0, 0", sqlCon);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@companyid", companyid));

            //Task<SqlDataReader> reader = cmd.ExecuteReaderAsync();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable table = new DataTable();
            table.Load(reader);
            System.Diagnostics.Debug.WriteLine("discountRulesTask END: -- " + DateTime.Now.ToString());
            return table;
        }
        public async Task<DataTable> additionalChargesTask(int storeid, int companyid)
        {
            System.Diagnostics.Debug.WriteLine("additionalChargesTask START: -- " + DateTime.Now.ToString());
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();

            SqlCommand cmd = new SqlCommand(@"SELECT ac.Id, ac.Description, ac.ChargeType, ac.ChargeValue, ac.TaxGroupId FROM dbo.AdditionalCharges ac
                                            WHERE (@companyid is null OR ac.CompanyId = @companyid)
                                            UNION
                                            SELECT 0, NULL, 0,0,0 as OrderJson", sqlCon);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@companyid", companyid));

            //Task<SqlDataReader> reader = cmd.ExecuteReaderAsync();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable table = new DataTable();
            table.Load(reader);
            System.Diagnostics.Debug.WriteLine("additionalChargesTask END: -- " + DateTime.Now.ToString());
            return table;
        }
        public async Task<DataTable> orderTypesTask(int storeid, int companyid)
        {
            System.Diagnostics.Debug.WriteLine("orderTypesTask START: -- " + DateTime.Now.ToString());
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();

            SqlCommand cmd = new SqlCommand(@"SELECT ot.Id, ot.Description FROM OrderTypes ot", sqlCon);
            cmd.CommandType = CommandType.Text;

            //Task<SqlDataReader> reader = cmd.ExecuteReaderAsync();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable table = new DataTable();
            table.Load(reader);
            System.Diagnostics.Debug.WriteLine("orderTypesTask END: -- " + DateTime.Now.ToString());
            return table;
        }
        public async Task<DataTable> customersTask(int storeid, int companyid)
        {
            System.Diagnostics.Debug.WriteLine("customersTask START: -- " + DateTime.Now.ToString());
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();

            SqlCommand cmd = new SqlCommand(@"SELECT c.Id, c.Name, c.Email, c.PhoneNo, c.Address, c.City, c.PostalCode, 1 as Sync FROM Customers c
                                            where (c.CompanyId = @companyid AND (c.StoreId = @storeid OR @storeid = 0))", sqlCon);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@companyid", companyid));
            cmd.Parameters.Add(new SqlParameter("@storeid", storeid));

            //Task<SqlDataReader> reader = cmd.ExecuteReaderAsync();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable table = new DataTable();
            table.Load(reader);
            System.Diagnostics.Debug.WriteLine("customersTask END: -- " + DateTime.Now.ToString());
            return table;
        }
        public async Task<DataTable> paymentTypesTask(int storeid, int companyid)
        {
            System.Diagnostics.Debug.WriteLine("paymentTypesTask START: -- " + DateTime.Now.ToString());
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();

            SqlCommand cmd = new SqlCommand(@"SELECT pt.Id, pt.Description FROM PaymentTypes pt WHERE pt.Id != 6", sqlCon);
            cmd.CommandType = CommandType.Text;

            //Task<SqlDataReader> reader = cmd.ExecuteReaderAsync();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable table = new DataTable();
            table.Load(reader);
            System.Diagnostics.Debug.WriteLine("paymentTypesTask END: -- " + DateTime.Now.ToString());
            return table;
        }
        public async Task<DataTable> KOTGroupPrintersTask(int storeid, int companyid)
        {
            System.Diagnostics.Debug.WriteLine("KOTGroupPrintersTask START: -- " + DateTime.Now.ToString());
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();

            SqlCommand cmd = new SqlCommand(@"SELECT kgp.*, kg.Description FROM KOTGroupPrinters kgp
                                            JOIN KOTGroups kg ON kgp.KOTGroupId = kg.Id
                                            where (kgp.CompanyId = @companyid AND kgp.StoreId = @storeid)", sqlCon);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@companyid", companyid));
            cmd.Parameters.Add(new SqlParameter("@storeid", storeid));

            //Task<SqlDataReader> reader = cmd.ExecuteReaderAsync();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable table = new DataTable();
            table.Load(reader);
            System.Diagnostics.Debug.WriteLine("KOTGroupPrintersTask END: -- " + DateTime.Now.ToString());
            return table;
        }
        public async Task<DataTable> pendingordersTask(int storeid, int companyid)
        {
            System.Diagnostics.Debug.WriteLine("pendingordersTask START: -- " + DateTime.Now.ToString());
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();

            SqlCommand cmd = new SqlCommand(@"SELECT o.Id, o.OrderNo, o.OrderJson FROM Orders o
                                            where o.CompanyId = @companyid 
                                            AND (o.StoreId = @storeid OR o.DeliveryStoreId = @storeid) 
                                            AND (
                                                (o.OrderedDate = CONVERT(VARCHAR(10), getdate(), 111) 
                                                 AND o.OrderTypeId IN (3,4)) OR (o.OrderTypeId IN (2,3,4) AND o.OrderStatusId NOT IN (-1,5)) OR (o.OrderTypeId IN (3,4) AND o.BillAmount != o.PaidAmount)) -- OR (o.OrderTypeId IN (3,4) AND o.BillAmount != o.PaidAmount)
                                            UNION
                                            SELECT 0, 0, NULL as OrderJson", sqlCon);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@companyid", companyid));
            cmd.Parameters.Add(new SqlParameter("@storeid", storeid));

            //Task<SqlDataReader> reader = cmd.ExecuteReaderAsync();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable table = new DataTable();
            table.Load(reader);
            System.Diagnostics.Debug.WriteLine("pendingordersTask END: -- " + DateTime.Now.ToString());
            return table;
        }
        public async Task<DataTable> storePaymentTypesTask(int storeid, int companyid)
        {
            System.Diagnostics.Debug.WriteLine("storePaymentTypesTask START: -- " + DateTime.Now.ToString());
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();

            SqlCommand cmd = new SqlCommand(@"SELECT spt.Id, spt.Name as Description, spt.MachineId FROM StorePaymentTypes spt
                                            where spt.CompanyId = @companyid AND spt.StoreId = @storeid AND spt.IsActive = 1", sqlCon);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@companyid", companyid));
            cmd.Parameters.Add(new SqlParameter("@storeid", storeid));

            //Task<SqlDataReader> reader = cmd.ExecuteReaderAsync();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable table = new DataTable();
            table.Load(reader);
            System.Diagnostics.Debug.WriteLine("storePaymentTypesTask END: -- " + DateTime.Now.ToString());
            return table;
        }
        public async Task<DataTable> storesTask(int storeid, int companyid)
        {
            System.Diagnostics.Debug.WriteLine("storePaymentTypesTask START: -- " + DateTime.Now.ToString());
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();

            SqlCommand cmd = new SqlCommand(@"SELECT * FROM Stores s
                                            where s.CompanyId = @companyid", sqlCon);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@companyid", companyid));
            //cmd.Parameters.Add(new SqlParameter("@storeid", storeid));

            //Task<SqlDataReader> reader = cmd.ExecuteReaderAsync();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable table = new DataTable();
            table.Load(reader);
            System.Diagnostics.Debug.WriteLine("storePaymentTypesTask END: -- " + DateTime.Now.ToString());
            return table;
        }
        public async Task<object> newProductsTask(int storeid, int companyid)
        {
            System.Diagnostics.Debug.WriteLine("productsTask START: -- " + DateTime.Now.ToString());
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();

            SqlCommand cmd = new SqlCommand(@"                                            
                                            SELECT p.Id, spg.SaleProductId INTO #fbonlineprods FROM SaleProductGroups spg 
                                            JOIN Products p ON spg.StockProductId = p.Id
                                            WHERE spg.SaleProductId IN (SELECT p1.Id FROM Products p1 WHERE p1.isonline = 1 AND p1.CompanyId = @companyid) AND p.IsSaleProdGroup != 1

                                            SELECT sp.ProductId AS Id, p.Name as Product, p.BarCode, p.ProductCode, u.Description as Unit, sp.Price,sp.TakeawayPrice,
		                                    sp.DeliveryPrice, sp.IsActive,
		                                    sp.IsDineInService, sp.IsDeliveryService, sp.IsTakeAwayService, tg.Id AS TaxGroupId,ca.Id as CategoryId,ca.ParentCategoryId,p.KOTGroupId,
                                            ca.MinimumQty, ca.FreeQtyPercentage, tg.Tax1, tg.Tax2, tg.Tax3,p.ProductTypeId,tg.IsInclusive as IsTaxInclusive, 
                                            CAST(CASE WHEN ca.IsUPCategory = 0 OR sr.ShowUpcategory = ca.IsUPCategory THEN 0 ELSE 1 END AS BIT) ishidden, p.Recomended,
		                                    (
                                                SELECT pog.OptionGroupId AS Id, og.Name,og.OptionGroupType,
                                                (
                                                    SELECT so.OptionId AS Id, so.Price, so.TakeawayPrice, so.DeliveryPrice, o.Name, o.IsSingleQtyOption
                                                    FROM dbo.StoreOptions so
                                                    JOIN Options o ON so.OptionId = o.Id
                                                    WHERE (so.StoreId = @storeid AND o.OptionGroupId = pog.OptionGroupId)
                                                    FOR JSON PATH
                                                )AS 'Option'
                                                FROM dbo.ProductOptionGroups pog
                                                JOIN dbo.OptionGroups og ON pog.OptionGroupId = og.Id
                                                WHERE (pog.ProductId = sp.ProductId AND pog.CompanyId = @companyid)
                                                FOR JSON PATH
                                            )AS 'OptionGroup'
	                                        FROM StoreProducts sp
	                                        JOIN Products p on p.Id = sp.ProductId
	                                        JOIN Categories ca on ca.Id = p.CategoryId
	                                        JOIN Units u on u.Id = p.UnitId
	                                        JOIN TaxGroups tg on tg.Id = p.TaxGroupId
                                            JOIN StorePreferences sr ON sp.StoreId = sr.StoreId
	                                        WHERE p.isactive = 1 AND ca.isactive = 1 AND (sp.StoreId = @storeid OR @storeid = 0) AND (sp.CompanyId = @companyid)
                                            --AND p.IsSaleProdGroup <> 1 AND p.Id NOT IN (SELECT fb.Id FROM #fbonlineprods fb)
                                            --AND ((sr.ShowUpcategory = 0 AND ca.IsUPCategory = 0) OR (sr.ShowUpcategory = 1))
	                                        FOR JSON PATH", sqlCon);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@companyid", companyid));
            cmd.Parameters.Add(new SqlParameter("@storeid", storeid));

            //Task<SqlDataReader> reader = cmd.ExecuteReaderAsync();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable table = new DataTable();
            table.Load(reader);
            string catStr = "";
            for (int j = 0; j < table.Rows.Count; j++)
            {
                catStr += table.Rows[j].ItemArray[0].ToString();
            }
            dynamic products = JsonConvert.DeserializeObject(catStr);
            System.Diagnostics.Debug.WriteLine("productsTask END: -- " + DateTime.Now.ToString());
            return products;
        }
        public async Task<object> productsTask2(int storeid, int companyid, DateTime? lastSyncedDateTime=null)
        {
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();

            SqlCommand cmd = new SqlCommand(@"                                            
                                            SELECT p.Id, spg.SaleProductId INTO #fbonlineprods FROM SaleProductGroups spg 
                                            JOIN Products p ON spg.StockProductId = p.Id
                                            WHERE spg.SaleProductId IN (SELECT p1.Id FROM Products p1 WHERE p1.isonline = 1 AND p1.CompanyId = @companyid) AND p.IsSaleProdGroup != 1

                                            SELECT sp.ProductId AS Id, p.Name as Product, p.BarCode, p.ProductCode, u.Description as Unit, sp.Price,sp.TakeawayPrice,
		                                    sp.DeliveryPrice, sp.IsActive,
		                                    sp.IsDineInService, sp.IsDeliveryService, sp.IsTakeAwayService, tg.Id AS TaxGroupId,ca.Id as CategoryId,ca.ParentCategoryId,p.KOTGroupId,
                                            ca.MinimumQty, ca.FreeQtyPercentage, tg.Tax1, tg.Tax2, tg.Tax3,p.ProductTypeId,tg.IsInclusive as IsTaxInclusive, 
                                            CAST(CASE WHEN ca.IsUPCategory = 0 OR sr.ShowUpcategory = ca.IsUPCategory THEN 0 ELSE 1 END AS BIT) ishidden, p.Recomended,
		                                    (
                                                 SELECT pog.OptionGroupId AS Id, og.Name,og.OptionGroupType,
                                                 (
                                                    SELECT so.OptionId AS Id, so.Price, so.TakeawayPrice, so.DeliveryPrice, o.Name, o.IsSingleQtyOption
                                                    FROM dbo.StoreOptions so
                                                    JOIN Options o ON so.OptionId = o.Id
                                                    WHERE (so.StoreId = @storeid AND o.OptionGroupId = pog.OptionGroupId)
                                                    FOR JSON PATH
                                                 )AS 'Option'
                                                 FROM dbo.ProductOptionGroups pog
                                                 JOIN dbo.OptionGroups og ON pog.OptionGroupId = og.Id
                                                 WHERE (pog.ProductId = sp.ProductId AND pog.CompanyId = @companyid)
                                                 FOR JSON PATH
                                             )AS 'OptionGroup'
	                                        FROM StoreProducts sp
	                                        JOIN Products p on p.Id = sp.ProductId
	                                        JOIN Categories ca on ca.Id = p.CategoryId
	                                        JOIN Units u on u.Id = p.UnitId
	                                        JOIN TaxGroups tg on tg.Id = p.TaxGroupId
                                            JOIN StorePreferences sr ON sp.StoreId = sr.StoreId
	                                       WHERE p.isactive = 1 AND ca.isactive = 1 AND (sp.StoreId = @storeid OR @storeid = 0) 
                                             AND (sp.CompanyId = @companyid) 
                                             AND (p.ModifiedDate >= @lastsynceddatetime OR @lastsynceddatetime IS NULL)
	                                         FOR JSON PATH", sqlCon);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@companyid", companyid));
            cmd.Parameters.Add(new SqlParameter("@storeid", storeid));
            if(lastSyncedDateTime == null)
            {
                cmd.Parameters.Add(new SqlParameter("@lastsynceddatetime", DBNull.Value));
            }
            else
            {
                cmd.Parameters.Add(new SqlParameter("@lastsynceddatetime", lastSyncedDateTime));
            }

            //Task<SqlDataReader> reader = cmd.ExecuteReaderAsync();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable table = new DataTable();
            table.Load(reader);
            string catStr = "";
            for (int j = 0; j < table.Rows.Count; j++)
            {
                catStr += table.Rows[j].ItemArray[0].ToString();
            }
            dynamic products = JsonConvert.DeserializeObject(catStr);
            return products;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Biz1PosApi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    public class KOTController : Controller
    {
        private int var_status;
        private Array var_value;
        private string var_msg;
        private POSDbContext db;
        public IConfiguration Configuration { get; }
        private static TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        public KOTController(POSDbContext contextOptions, IConfiguration configuration)
        {
            db = contextOptions;
            Configuration = configuration;
        }
        // GET: api/<controller>
        [HttpGet("Get")]
        public IActionResult Get(KOT kot)
        {
            try
            {
                var kots = db.KOTs.ToList();

                for (int i = 0; i < kots.Count(); i++)
                {
                    List<OrderItem> orderItems = db.OrderItems.Where(o => o.KOTId == kots[i].Id).Include(p => p.Product).ToList();

                    for (int j = 0; j < orderItems.Count(); j++)
                    {
                        orderItems[j].OrdItemAddons = db.OrdItemAddons.Where(oa => oa.OrderItemId == orderItems[j].Id).Include(ao => ao.Addon.Product).Include(ao => ao.DropDown).ToList();
                        orderItems[j].OrdItemVariants = db.OrdItemVariants.Where(oa => oa.OrderItemId == orderItems[j].Id).Include(ov => ov.Variant).ToList();
                    }
                    //kots[i].OrderItems = orderItems;
                }
                return Ok(kots);
            }
            catch (Exception ex)
            {
                var error = new
                {
                    status = 0,
                    msg = "There is some problem to get your data"
                };
                return Json(error);
            }
        }
        [HttpGet("GetStoreKots")]
        public IActionResult GetStoreKots(int storeId, int orderid,int kotGroupId)
        {
            //SqlConnection sqlCon = new SqlConnection("server=(LocalDb)\\MSSQLLocalDB; database=Biz1POS;Trusted_Connection=True;");
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("dbo.StoreKots", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@storeid", storeId));
            cmd.Parameters.Add(new SqlParameter("@orderid", orderid));
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
            object kOTs = JsonConvert.DeserializeObject(catStr[0]);
            return Ok(kOTs);
        }
        // GET api/<controller>/5
        [HttpGet("KOT")]
        public IActionResult KOT(int compId, int storeId)
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
            return Ok(obj);
        }

        // POST api/<controller>
        [HttpPost("OrdItemSts")]
        public IActionResult OrdItemSts(int id, int ordItemStsId, int kotStsId)
        {
            try
            {
                OrderItem orderitem = db.OrderItems.Find(id);
                orderitem.StatusId = ordItemStsId;
                db.Entry(orderitem).State = EntityState.Modified;
                KOT kOT = db.KOTs.Find(orderitem.KOTId);
                kOT.KOTStatusId = kotStsId;
                db.Entry(kOT).State = EntityState.Modified;
                db.SaveChanges();
                var error = new
                {
                    status = 200,
                    msg = "Successsfully updated"
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

        [HttpPost("OrdItemAdddonSts")]
        public void OrdItemAdddonSts(int id, int statusId)
        {
            OrdItemAddon orditemadd = db.OrdItemAddons.Find(id);
            orditemadd.StatusId = statusId;
            db.Entry(orditemadd).State = EntityState.Modified;
            db.SaveChanges();
        }

        [HttpPost("KOTSts")]
        public IActionResult KOTSts(int id, int currentStsId, int updStsId)
        {
            try
            {

                KOT kOT = db.KOTs.Find(id);
                kOT.KOTStatusId = updStsId;
                db.Entry(kOT).State = EntityState.Modified;
                // string status = db.DropDowns.Where(dd => dd.Id == statusId).Select(dd => dd.Name).First();
                // int orditsts = db.DropDowns.Where(d => (d.DropDownGroupId == 3) && (d.Name == status)).Select(p => p.Id).First();
                List<OrderItem> orderItem = db.OrderItems.Where(s => s.KOTId == id && s.StatusId == currentStsId).ToList();
                for (int i = 0; i < orderItem.Count(); i++)
                {
                    orderItem[i].StatusId = updStsId;
                    db.Entry(orderItem[i]).State = EntityState.Modified;
                }
                db.SaveChanges();
                var error = new
                {
                    status = 200,
                    msg = "Successsfully updated"
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

        // PUT api/<controller>/5
        [HttpPost("CreateKOT")]
        [EnableCors("AllowOrigin")]
        public IActionResult CreateKOT([FromBody]JObject[] objData, int compId, int storeId)
        {
            dynamic kotJson = objData.ToList();
            try
            {
                foreach (var kots in kotJson)
                {
                    KOT kot = kots.ToObject<KOT>();
                    kot.CompanyId = compId;
                    kot.StoreId = storeId;
                    db.KOTs.Add(kot);
                    db.SaveChanges();

                    JArray itemObj = kots.item;
                    dynamic itemJson = itemObj.ToList();
                    foreach (var item in itemJson)
                    {
                        OrderItem ordItem = item.ToObject<OrderItem>();
                        ordItem.KOTId = kot.Id;
                        ordItem.OrderId = kot.OrderId;
                        db.OrderItems.Add(ordItem);
                        db.SaveChanges();

                        JArray variantObj = item.Variant;
                        if (variantObj != null)
                        {
                            dynamic variantJson = variantObj.ToList();
                            foreach (var variant in variantJson)
                            {
                                OrdItemVariant ordItemVar = variant.ToObject<OrdItemVariant>();
                                ordItemVar.OrderItemId = ordItem.Id;
                                db.OrdItemVariants.Add(ordItemVar);
                                db.SaveChanges();
                            }
                        }

                        JArray addonObj = item.Addon;
                        if (addonObj != null)
                        {
                            dynamic addonJson = addonObj.ToList();
                            foreach (var addon in addonJson)
                            {
                                OrdItemAddon ordItemAddon = addon.ToObject<OrdItemAddon>();
                                ordItemAddon.OrderItemId = ordItem.Id;
                                db.OrdItemAddons.Add(ordItemAddon);
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                var error = new
                {
                    status = 0,
                    msg = ex.InnerException.Message
                };
                return Json(error);
            }
            var response = new
            {
                status = 200,
                data = new
                {

                },
                msg = "KOT created Successfully"
            };

            return Json(response);
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

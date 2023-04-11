using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        public static IHostingEnvironment _environment;

        private static TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        public KOTController(POSDbContext contextOptions, IConfiguration configuration, IHostingEnvironment environment)
        {
            db = contextOptions;
            Configuration = configuration;
            _environment = environment;
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
        public List<KOTInstruction> getInstructions(int orderid)
        {
            return db.KOTInstructions.Where(o => o.OrderId == orderid).ToList();
        }
        [HttpGet("GetStoreKots")]
        public IActionResult GetStoreKots(int storeId, int orderid,int kotGroupId)
        {
            try
            {
                int[] pendingStatusIds = { 0, 1, 2, 3, 4 };
                int[] advancedOrderTypeIds = { 2, 3, 4 };

                List<KOTInstruction> instructions = new List<KOTInstruction>();

                List<int> orderids = db.Orders.Where(x => x.StoreId == storeId && (x.Id == orderid || orderid == 0) && pendingStatusIds.Contains(x.OrderStatusId) && advancedOrderTypeIds.Contains(x.OrderTypeId)).Select(s => s.Id).ToList();
                List<KOTInstruction> ins = db.KOTInstructions.Where(x => orderids.Contains(x.OrderId)).ToList();
                var kots = db.KOTs.Where(x => orderids.Contains(x.OrderId) && x.StoreId == storeId && x.KOTGroupId == kotGroupId).Select(s => new { s.json, s.Order.DeliveryDateTime, instructions = ins.Where(x => x.OrderId == s.OrderId).ToList(), s.Order.Note, s.Id, JsonConvert.DeserializeObject<OrderJson>(s.Order.OrderJson).Message, JsonConvert.DeserializeObject<OrderJson>(s.Order.OrderJson).CustomerDetails }).ToList();
                return Json(kots);
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
        [HttpGet("AudioUpload")]
        public string AudioUpload(int companyid, IFormFile file)
        {
            try
            {
                //long size = file.Sum(f => f.Length);

                // full path to file in temp location
                // var filePath = "https://biz1app.azurewebsites.net/Images/3";
                string subdir = "\\images\\" + companyid + "\\";
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
                        url = "https://biz1pos.azurewebsites.net/images/" + companyid + "/" + file.FileName
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
        [HttpPost("fileUpload")]
        public IActionResult ImageUpload(int companyid, int orderid, IFormFile image = null, IFormFile audio = null)
        {
            try
            {
                string imageurl = "";
                string audiourl = "";

                //long size = file.Sum(f => f.Length);

                // full path to file in temp location
                // var filePath = "https://biz1app.azurewebsites.net/Images/3";
                string subdir = "\\images\\" + companyid + "\\KOT\\";
                if (!Directory.Exists(_environment.WebRootPath + subdir))
                {
                    Directory.CreateDirectory(_environment.WebRootPath + subdir);
                }

                if(image != null)
                {
                    using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + subdir + image.FileName))
                    {
                        image.CopyTo(filestream);
                        filestream.Flush();
                        var response = new
                        {
                            url = "https://biz1pos.azurewebsites.net/images/" + companyid + "/KOT/" + image.FileName
                        };

                        imageurl = response.url;
                        //return response.url;
                    }
                    if(!db.KOTInstructions.Where(x => x.InstructionType == 1 && x.OrderId == orderid).Any())
                    {
                        KOTInstruction instruction = new KOTInstruction();
                        instruction.InstructionDateTime = DateTime.Now;
                        instruction.url = imageurl;
                        instruction.OrderId = orderid;
                        instruction.InstructionType = 1;
                        db.KOTInstructions.Add(instruction);
                        db.SaveChanges();
                    } 
                    else
                    {
                        KOTInstruction instruction = db.KOTInstructions.Where(x => x.InstructionType == 1 && x.OrderId == orderid).FirstOrDefault();
                        instruction.InstructionDateTime = DateTime.Now;
                        instruction.url = imageurl;
                        db.Entry(instruction).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }


                subdir = "\\audios\\" + companyid + "\\KOT\\";
                if (!Directory.Exists(_environment.WebRootPath + subdir))
                {
                    Directory.CreateDirectory(_environment.WebRootPath + subdir);
                }
                if (audio != null)
                {
                    using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + subdir + audio.FileName))
                    {
                        audio.CopyTo(filestream);
                        filestream.Flush();
                        var response = new
                        {
                            url = "https://biz1pos.azurewebsites.net/audios/" + companyid + "/KOT/" + audio.FileName
                        };
                        audiourl = response.url;
                        //return response.url;
                    }
                    if (!db.KOTInstructions.Where(x => x.InstructionType == 2 && x.OrderId == orderid).Any())
                    {
                        KOTInstruction instruction = new KOTInstruction();
                        instruction.InstructionDateTime = DateTime.Now;
                        instruction.url = audiourl;
                        instruction.OrderId = orderid;
                        instruction.InstructionType = 2;
                        db.KOTInstructions.Add(instruction);
                        db.SaveChanges();
                    }
                    else
                    {
                        KOTInstruction instruction = db.KOTInstructions.Where(x => x.InstructionType == 1 && x.OrderId == orderid).FirstOrDefault();
                        instruction.InstructionDateTime = DateTime.Now;
                        instruction.url = audiourl;
                        db.Entry(instruction).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                }
                var resp = new
                {
                    audiourl,
                    imageurl
                };
                return Ok(resp);
            }
            catch (Exception ex)
            {
                var error = new
                {
                    status = 0,
                    msg = new Exception(ex.Message, ex.InnerException)
                };
                return Ok(error);
            }
        }

        [HttpGet("KOTStatusChange")]
        public IActionResult KOTStatusChange(int kotid, int statusid)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                //string jsonOutputParam = "@jsonOutput";
                SqlCommand cmd = new SqlCommand("dbo.KOTStatusChange", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@kotid", kotid));
                cmd.Parameters.Add(new SqlParameter("@statusid", statusid));
                //cmd.Parameters.Add(new SqlParameter("@modDate", null));
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                sqlCon.Close();
                var obj = new
                {
                    status = 200,
                    msg = "Status Changed"
                };
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
    public class KDSKot
    {
        public string json { get; set; }
        public DateTime DeliveryDateTime { get; set; }
    }
    public class JsonCustomer: Customer
    {
        public int? Id { get; set; }
    }
    public class OrderJson
    {
        public JsonCustomer CustomerDetails { get; set; }
        public string Message { get; set; }
    }
}

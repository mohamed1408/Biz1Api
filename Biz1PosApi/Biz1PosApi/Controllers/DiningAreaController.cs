using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    public class DiningAreaController : Controller
    {
        private POSDbContext db;
        public DiningAreaController(POSDbContext contextOptions)
        {
            db = contextOptions;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Get")]
        [EnableCors("AllowOrigin")]
        public IActionResult Get(int CompanyId)
        {
            try
            {

                var diningarea = (from da in db.DiningAreas
                                 join s in db.Stores on da.StoreId equals s.Id
                                 where da.CompanyId == CompanyId
                                 select new { da.Id, da.Description, s.Name,da.StoreId,store =s.Id }).ToList();
                //var diningarea = db.DiningAreas.Where(v => v.CompanyId == CompanyId).ToList();
                System.Collections.Generic.Dictionary<string, object>[] objData = new System.Collections.Generic.Dictionary<string, object>[diningarea.Count()];

                for (int i = 0; i < diningarea.Count(); i++)
                {
                    objData[i] = new Dictionary<string, object>();
                    objData[i].Add("Id", diningarea[i].Id);
                    objData[i].Add("DiningArea", diningarea[i].Description);
                    objData[i].Add("StoreId", diningarea[i].StoreId);
                    objData[i].Add("StoreName", diningarea[i].Name);
                    string str = "";

                    var dining = db.DiningTables.Where(v => v.DiningAreaId == diningarea[i].Id).ToList();
                    int varCount = dining.Count();
                    for (int j = 0; j < varCount; j++)
                    {
                        if (j < varCount - 1)
                        {
                            str += dining[j].Description + ",";
                        }
                        else
                        {
                            str += dining[j].Description;
                        }

                    }
                    objData[i].Add("DiningTable", str);
                }

                return Ok(objData);
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
        [HttpPost("CreateArea")]
        [EnableCors("AllowOrigin")]
        public IActionResult CreateArea([FromForm]string data)
        {
            try
            {
                dynamic orderJson = JsonConvert.DeserializeObject(data);
                {
                    DiningArea diningArea = orderJson.ToObject<DiningArea>();
                    diningArea.ModifiedDate = DateTime.Now;
                    db.DiningAreas.Add(diningArea);
                    db.SaveChanges();
                    JArray itemObj = orderJson.DiningTable;
                    dynamic itemJson = itemObj.ToList();
                    foreach (var item in itemJson)
                    {
                        DiningTable diningTable = item.ToObject<DiningTable>();
                        diningTable.ModifiedDate = DateTime.Now;
                        diningTable.DiningAreaId = diningArea.Id;
                        diningTable.StoreId = diningArea.StoreId;
                        db.DiningTables.Add(diningTable);
                        db.SaveChanges();
                    }
                    var error = new
                    {
                        status = "success",
                        data = new
                        {
                            value = 2
                        },
                        msg = "The DiningArea added successfully"
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

        [HttpGet("GetById")]
        [EnableCors("AllowOrigin")]
        public IActionResult GetById(int Id)
        {
            try
            {
                var diningarea = db.DiningAreas.Find(Id);
                System.Collections.Generic.Dictionary<string, object>[] objData = new System.Collections.Generic.Dictionary<string, object>[1];

                var diningtable = db.DiningTables.Where(v => v.DiningAreaId == Id).ToList();
                objData[0] = new Dictionary<string, object>();
                objData[0].Add("Id", diningarea.Id);
                objData[0].Add("DiningArea", diningarea.Description);
                objData[0].Add("StoreId", diningarea.StoreId);
                //string str = "";
                //for (int j = 0; j < variant.Count(); j++)
                //{
                //    str += variant[j].Description + ",";
                //}
                objData[0].Add("DiningTable", diningtable);


                return Ok(objData);
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

        [HttpPost("Update")]
        [EnableCors("AllowOrigin")]
        public IActionResult Update([FromForm]string Data)
        {
            try
            {
                dynamic orderJson = JsonConvert.DeserializeObject(Data);
                DiningArea diningArea = orderJson.ToObject<DiningArea>();
                diningArea.ModifiedDate = DateTime.Now;
                db.Entry(diningArea).State = EntityState.Modified;
                db.SaveChanges();
                JArray itemObj = orderJson.DiningTable;
                JArray itemDel = orderJson.Del;
                dynamic itemJson = itemObj.ToList();
                dynamic itemJsonDel = itemDel.ToList();
                foreach (var item in itemJson)
                {
                    if (item.Id == 0)
                    {
                        DiningTable diningTable = item.ToObject<DiningTable>();
                        diningTable.ModifiedDate = DateTime.Now;
                        db.DiningTables.Add(diningTable);
                        db.SaveChanges();
                    }
                    else
                    {
                        DiningTable diningTable = item.ToObject<DiningTable>();
                        diningTable.ModifiedDate = DateTime.Now;
                        db.Entry(diningTable).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                }
                foreach (var item in itemJsonDel)
                {
                    int itemId = item.Id.ToObject<int>();
                    DiningTable diningTable = db.DiningTables.Find(itemId);
                    db.DiningTables.RemoveRange(diningTable);
                    db.SaveChanges();
                }
                var error = new
                {
                    status = "success",
                    data = new
                    {
                        value = 2
                    },
                    msg = "The Data Updated successfully"
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

        [HttpGet("Delete")]
        [EnableCors("AllowOrigin")]
        public IActionResult Delete(int Id)
        {
            try
            {
                var dining = db.DiningTables.Where(x => x.DiningAreaId == Id).ToList();
                foreach (var item in dining)
                {
                    var opt = db.DiningTables.Find(item.Id);
                    db.DiningTables.Remove(opt);
                }
                var area = db.DiningAreas.Find(Id);
                db.DiningAreas.Remove(area);
                db.SaveChanges();
                var error = new
                {
                    status = "success",
                    data = new
                    {
                        value = 2
                    },
                    msg = "The Data deleted successfully"
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


    }
}
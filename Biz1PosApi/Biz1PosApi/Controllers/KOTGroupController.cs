using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Biz1PosApi.Models;
using System.Data.Common;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Biz1BookPOS.Controllers
{
    [Route("api/[controller]")]
    public class KOTGroupController : Controller
    {
        private POSDbContext db;
        public KOTGroupController(POSDbContext contextOptions)
        {
            db = contextOptions;
        }

        // GET: api/<controller>
        [HttpGet("GetIndex")]
        public IActionResult GetIndex(int CompanyId)
        {
            try
            {
                var kotgroup = db.KOTGroups.Where(k => k.CompanyId == CompanyId).ToList();
                //var variantGroups = db.Query(table).ToList();
                return Ok(kotgroup);
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

        [HttpPost("UpdateKotPrinters")]
        [EnableCors("AllowOrigin")]
        public IActionResult UpdateKotPrinters([FromForm] string data)
        {
            try
            {
                dynamic kotgrps = JsonConvert.DeserializeObject(data);
                foreach (var kotgrp in kotgrps)
                {
                    KOTGroup kotGroup = db.KOTGroups.Find(kotgrp.Id);
                    kotGroup.Printer = kotgrp.Printer;
                    db.Entry(kotGroup).State = EntityState.Modified;
                    db.SaveChanges();
                }
                var error = new
                {
                    status = 200,
                    msg = "Data updated successfully"
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
        [HttpPost("Create")]
        [EnableCors("AllowOrigin")]
        public IActionResult Create([FromForm]string data)
        {
            try
            {
                dynamic kot = JsonConvert.DeserializeObject(data);
                KOTGroup kotGroup = kot.ToObject<KOTGroup>();
                kotGroup.CreatedDate = DateTime.Now;
                kotGroup.ModifiedDate = DateTime.Now;
                db.KOTGroups.Add(kotGroup);
                db.SaveChanges();
                var stores = db.Stores.Where(x => x.CompanyId == kotGroup.CompanyId).ToList();
                foreach(Store store in stores)
                {
                    KOTGroupPrinter kOTGroupPrinter = new KOTGroupPrinter();
                    kOTGroupPrinter.Printer = "None";
                    kOTGroupPrinter.KOTGroupId = kotGroup.Id;
                    kOTGroupPrinter.StoreId = store.Id;
                    kOTGroupPrinter.CompanyId = kotGroup.CompanyId;
                    db.KOTGroupPrinters.Add(kOTGroupPrinter);
                    db.SaveChanges();
                }
                var error = new
                {
                    status = 200,
                    msg = "The Data added successfully"
                };

                return Json(error);
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
        
        [HttpGet("Edit")]
        [EnableCors("AllowOrigin")]
        public IActionResult Edit(int Id)
        {
            try
            {
                var kotgrp = db.KOTGroups.Find(Id);
                return Ok(kotgrp);
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

        [HttpGet("Delete")]
        [EnableCors("AllowOrigin")]
        public IActionResult Delete(int Id)
        {
            try
            {
                var products = db.Products.Where(x => x.KOTGroupId == Id).ToList();
                if (products.Count == 0)
                {
                    var kotgrp = db.KOTGroups.Find(Id);
                    db.KOTGroups.Remove(kotgrp);
                    db.SaveChanges();
                    var error = new
                    {
                        status = 200,
                        msg = "The Data deleted successfully"
                    };
                    return Json(error);
                }
                else
                {
                    var error = new
                    {
                        status = 0,
                        msg = "Something went wrong  Contact our service provider"
                    };
                    return Json(error);
                }
            }
            catch(Exception ex)
            {
                string err = "";
                if(ex.Message != null)
                {
                    err = ex.Message;
                }
                if(ex.InnerException != null)
                {
                    err = ex.InnerException.ToString();
                }
                var error = new
                {
                    status = 0,
                    msg = "Something went wrong. Contact service provider",
                    error = err
                };
                return Json(error);
            }
        }
        [HttpPost("Update")]
        public IActionResult Update([FromForm]string data)
        {
            try
            {
                dynamic kotgrp = JsonConvert.DeserializeObject(data);
                KOTGroup kotgroup = kotgrp.ToObject<KOTGroup>();
                kotgroup.CreatedDate = db.KOTGroups.Where(x => x.Id == kotgroup.Id).AsNoTracking().FirstOrDefault().CreatedDate;
                kotgroup.ModifiedDate = DateTime.Now;
                db.Entry(kotgroup).State = EntityState.Modified;
                db.SaveChanges();
                var error = new
                {
                    status = "success",
                    data = new
                    {
                        value = 2
                    },
                    msg = "The kotgroup updated successfully"
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
        [HttpGet("GetKOTGroupUser")]
        public IActionResult GetKOTGroupUser(int userId, int compId)
        {
            try
            {
                var kotgroupuser = db.KOTGroupUsers.Where(k => k.UserId == userId && k.CompanyId == compId).ToList();
                return Ok(kotgroupuser);
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
        [HttpPost("CreateKOTGroupUser")]
        public IActionResult CreateKOTGroupUser([FromForm] string data)
        {
            try
            {
                dynamic kotgrp = JsonConvert.DeserializeObject(data);
                KOTGroupUser KOTGroupUser = kotgrp.ToObject<KOTGroupUser>();
                KOTGroupUser.CreatedDate = DateTime.Now;
                KOTGroupUser.ModifiedDate = DateTime.Now;
                db.KOTGroupUsers.Add(KOTGroupUser);
                db.SaveChanges();
                var error = new
                {
                    status = 200,
                    msg = "The Data added successfully"
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




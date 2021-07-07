using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Biz1PosApi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Biz1BookPOS.Controllers
{
    [Route("api/[controller]")]
    public class AddonController : Controller
    {
        private POSDbContext db;
        private object addon;

        public AddonController(POSDbContext contextOptions)
        {
            db = contextOptions;
        }

       
        // GET: api/<controller>
        [HttpGet("GetAddOn")]
        [EnableCors("AllowOrigin")]
        public IActionResult GetAddOn(int CompanyId)
        {
            var addonGroups = db.AddonGroups.Where(a =>a.CompanyId==CompanyId).ToList();
            System.Collections.Generic.Dictionary<string, object>[] objData = new System.Collections.Generic.Dictionary<string, object>[addonGroups.Count()];

            var addon = db.Addons.Include(p => p.Product).ToList();
            for (int i = 0; i < addonGroups.Count(); i++)
            {
                objData[i] = new Dictionary<string, object>();
                objData[i].Add("Id", addonGroups[i].Id);
                objData[i].Add("AddonGroup", addonGroups[i].Description);
                string str = "";
                int count = addon.Where(a => a.AddonGroupId == addonGroups[i].Id).Count();

                var addonProd = db.Addons.Where(a => a.AddonGroupId == addonGroups[i].Id).ToList();
                for (int j = 0; j < count; j++)
                {
                    if(j < count - 1)
                    {
                        str += addonProd[j].Product.Description + ",";
                    }
                    else
                    {
                        str += addonProd[j].Product.Description;
                    }
                    
                }
                
                objData[i].Add("Addons", str);
            }
            return Ok(objData);
        }


        // GET api/<controller>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var prod = new
            {           //var variantGroups = db.Query(table).ToList();
               //addon = from a in db.Addons
               //       join p in db.Products on a.ProductId equals p.Id
               //       select new { a.Id, a.AddonGroupId, p.Description, ProductId = p.Id }

            };
            return Json(prod);
        }

        //public string Get()
        //{
        //    var addon = db.Addons.ToList();

        //    return "value";
        //}
        [HttpPost("Create")]
        [EnableCors("AllowOrigin")]
        public IActionResult Create([FromForm]string data)
        {    
                dynamic orderJson = JsonConvert.DeserializeObject(data);
                AddonGroup addonGroup = orderJson.ToObject<AddonGroup>();
                addonGroup.CompanyId = 1;
                db.AddonGroups.Add(addonGroup);
                db.SaveChanges();
                JArray itemObj = orderJson.arr;
                dynamic itemJson = itemObj.ToList();
                foreach (var item in itemJson)
                {


                    Addon addon = item.ToObject<Addon>();
                    addon.CompanyId = 1;
                    addon.ProductId = item.productId;
                    addon.AddonGroupId = addonGroup.Id;
                    db.Addons.Add(addon);
                    db.SaveChanges();
                }
                var error = new
                {
                    status = "success",
                    data = new
                    {
                        value = 2
                    },
                    msg = "The VariantGroup added successfully"
                };

                return Json(error);
        }
        [HttpGet("GetById")]
        [EnableCors("AllowOrigin")]
        public IActionResult GetById(int Id)
        {
            //var addonGroups = db.AddonGroups.Find(Id);
            //System.Collections.Generic.Dictionary<string, object>[] objData = new System.Collections.Generic.Dictionary<string, object>[1];
            //var addon = db.Addons.Where(a => a.AddonGroupId == Id).ToList();
            //objData[0] = new Dictionary<string, object>();
            //objData[0].Add("Id", addonGroups.Id);
            //objData[0].Add("AddonGroups", addonGroups.Description);
            //objData[0].Add("Addon", addon);
            var data = new
            {
                addonGroups = db.AddonGroups.Find(Id),
                Addon = db.Addons.Where(x => x.AddonGroupId == Id).Include(p => p.Product).ToList(),
                Product = from p in db.Products
                          where p.CompanyId == 1
                          select new {p.Id, p.Description}
            };

            return Ok(data);
        }
        [HttpGet("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var addon = db.Addons.Find(id);
            return Ok(addon);
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT api/<controller>/5
        [HttpPost("Update")]
        [EnableCors("AllowOrigin")]
        public IActionResult Update([FromForm]string Data)
        {
            dynamic orderJson = JsonConvert.DeserializeObject(Data);
            AddonGroup addonGroup = orderJson.ToObject<AddonGroup>();
            addonGroup.CompanyId = 1;
            db.Entry(addonGroup).State = EntityState.Modified;
            db.SaveChanges();
            JArray itemObj = orderJson.arr;
            JArray AddonObj = orderJson.del;
            dynamic itemJson = itemObj.ToList();
            dynamic itemJsondel = AddonObj.ToList();
            foreach (var item in itemJson)
            {
                if (item.id == 0)
                {
                   Addon addon = new Addon();
                    addon.CompanyId = 1;
                    addon.ProductId = item.productId;
                    addon.AddonGroupId = addonGroup.Id;
                    db.Addons.Add(addon);
                    db.SaveChanges();
                }
                else
                {
                    Addon addon = item.ToObject<Addon>();
                    addon.CompanyId = 1;
                    addon.ProductId = item.productId;
                    addon.AddonGroupId = addonGroup.Id;
                    db.Entry(addon).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            foreach (var item in itemJsondel)
            {
                int itemId = item.id.ToObject<int>();
                Addon addon = db.Addons.Find(itemId);
                db.Addons.RemoveRange(addon);
                db.SaveChanges();
            }
            var error = new
            {
                status = "success",
                data = new
                {
                    value = 2
                },
                msg = "The Addon Updated successfully"
            };

            return Json(error);
        }


        // DELETE api/<controller>/5
        [HttpGet("Delete")]
        public IActionResult Delete(int Id)
        {
            var addongroup = db.AddonGroups.Find(Id);
            db.AddonGroups.Remove(addongroup);
            db.SaveChanges();
            var error = new
            {
                status = "success",
                data = new
                {
                    value = 2
                },
                msg = "The AddonGroup deleted successfully"
            };

            return Json(error);
        }

        [HttpGet("GetProductlist/{term}")]
        [EnableCors("AllowOrigin")]
        public IActionResult GetProductlist( string term = "")
        {
             var objProductlist = (db.Products
                                .Where(P => (term == "" || P.Description.ToUpper()
                                .Contains(term.ToUpper())))
                                .Select(P => new { Name = P.Description, Id = P.Id })
                                .Distinct().ToList());
                return Ok(objProductlist);
        }
    }
}

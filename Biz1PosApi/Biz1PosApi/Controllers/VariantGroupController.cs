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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Biz1BookPOS.Controllers
{
    [Route("api/[controller]")]
    public class VariantGroupController : Controller
    {
        private POSDbContext db;
        public VariantGroupController(POSDbContext contextOptions)
        {
            db = contextOptions;
        }

        // GET: api/<controller>
        [HttpGet("Get")]
        [EnableCors("AllowOrigin")]
        public IActionResult Get()
        {
            var variantGroups = db.VariantGroups.ToList();
            System.Collections.Generic.Dictionary<string, object>[] objData = new System.Collections.Generic.Dictionary<string, object>[variantGroups.Count()];

            var variant = db.Variants.ToList();
            for (int i = 0; i < variantGroups.Count(); i++)
            {
                objData[i] = new Dictionary<string, object>();
                objData[i].Add("Id", variantGroups[i].Id);
                objData[i].Add("VariantGroup", variantGroups[i].Description);
                string str = "";
                for (int j = 0; j < variant.Where(v => v.VariantGroupId == variantGroups[i].Id).Count(); j++)
                {
                    str += variant[j].Description + ",";
                }
                objData[i].Add("Variants", str);
            }

            return Ok(objData);
        }

        [HttpGet("GetById")]
        [EnableCors("AllowOrigin")]
        public IActionResult GetById(int Id)
        {
            var variantGroups = db.VariantGroups.Find(Id);
            System.Collections.Generic.Dictionary<string, object>[] objData = new System.Collections.Generic.Dictionary<string, object>[1];

            var variant = db.Variants.Where(v => v.VariantGroupId == Id).ToList();
            objData[0] = new Dictionary<string, object>();
            objData[0].Add("Id", variantGroups.Id);
            objData[0].Add("VariantGroup", variantGroups.Description);
            //string str = "";
            //for (int j = 0; j < variant.Count(); j++)
            //{
            //    str += variant[j].Description + ",";
            //}
            objData[0].Add("Variants", variant);


            return Ok(objData);
        }        // // GET api/<controller>/5
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}


        public DbSet<Object> obj { get; set; }

        // POST api/<controller>
        [HttpPost("CreateVariant")]
        [EnableCors("AllowOrigin")]
        public IActionResult CreateVariant([FromForm]string data)
        {
            //int count = db.VariantGroups.Where(c => c.Description == variantGroup.Description).ToList().Count();
            dynamic orderJson = JsonConvert.DeserializeObject(data);
            if (false)
            {

                //return Ok("Customer already exists");
                var error = new
                {
                    status = "error",
                    data = new
                    {
                        value = 1
                    },
                    msg = "The VariantGroup alredy exists"
                };

                return Json(error);
            }
            else
            {
                VariantGroup variantGroup = orderJson.ToObject<VariantGroup>();
                variantGroup.CompanyId = 1;
                db.VariantGroups.Add(variantGroup);
                db.SaveChanges();
                JArray itemObj = orderJson.Variants;
                dynamic itemJson = itemObj.ToList();
                foreach (var item in itemJson)
                {                    
                        Variant variant = item.ToObject<Variant>();
                        variant.CompanyId = 1;
                        variant.VariantGroupId = variantGroup.Id;
                        db.Variants.Add(variant);
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
        }
        [HttpGet("Edit")]
        [EnableCors("AllowOrigin")]
        public IActionResult Edit(int id)
        {      
            var variantGroup = db.VariantGroups.Find(id);
            return Ok(variantGroup);
        }
        // PUT api/<controller>/5
        [HttpPost("Update")]
        [EnableCors("AllowOrigin")]
        public IActionResult Update([FromForm]string Data)
        {
            dynamic orderJson = JsonConvert.DeserializeObject(Data);
            VariantGroup variantGroup = orderJson.ToObject<VariantGroup>();
            variantGroup.CompanyId = 1;
            db.Entry(variantGroup).State = EntityState.Modified;
            db.SaveChanges();
            JArray itemObj = orderJson.Variants;
            dynamic itemJson = itemObj.ToList();
            foreach (var item in itemJson)
            {
                if (item.id == 0)
                {
                    Variant variant = item.ToObject<Variant>();
                    variant.CompanyId = 1;
                    db.Variants.Add(variant);
                    db.SaveChanges();
                }
                else
                {
                    Variant variant = item.ToObject<Variant>();
                    variant.CompanyId = 1;
                    db.Entry(variant).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
                var error = new
            {
                status = "success",
                data = new
                {
                    value = 2
                },
                msg = "The VariantGroup edited successfully"
            };

            return Json(error);
        }

        // DELETE api/<controller>/5
        [HttpGet("Delete")]
        [EnableCors("AllowOrigin")]
        public IActionResult Delete(int Id)
        {
            var variantGroup = db.VariantGroups.Find(Id);
            db.VariantGroups.Remove(variantGroup);
            db.SaveChanges();
            var error = new
            {
                status = "success",
                data = new
                {
                    value = 2
                },
                msg = "The VariantGroup deleted successfully"
            };

            return Json(error);
        }
    }
}


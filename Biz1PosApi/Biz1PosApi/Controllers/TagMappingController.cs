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
    public class TagMappingController : Controller
    {
        private int var_status;
        private Array var_value;
        private string var_msg;
        private POSDbContext db;
        public IConfiguration Configuration { get; }
        public TagMappingController(POSDbContext contextOptions, IConfiguration configuration)
        {
            db = contextOptions;
            Configuration = configuration;
        }
        // GET: api/<controller>
        [HttpGet("GetTag")]
        public IActionResult GetTag(int compId)
        {
            try
            {
                var tags = db.Tags.Where(s => s.CompanyId == compId).ToList();
                return Ok(tags);
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
        [HttpPost("SaveTagMapping")]
        [EnableCors("AllowOrigin")]
        public IActionResult SaveTagMapping([FromForm] string objData)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    dynamic jsonObj = JsonConvert.DeserializeObject(objData);

                    //dynamic jsonObj = objData;
                    int tagId = jsonObj.tagId;
                    int companyId = jsonObj.companyId;
                    JArray itemObj = jsonObj.item;
                    dynamic itemJson = itemObj.ToList();

                    foreach (var prod in itemJson)
                    {
                        int prodId = prod.Id;
                        var existing = db.TagMappings.Where(s => s.ProductId == prodId && s.CompanyId == companyId).ToList();
                        if (existing.Count() == 0)
                        {
                            TagMapping tagMapping = new TagMapping();
                            tagMapping.ProductId = prod.Id;
                            tagMapping.CompanyId = companyId;
                            tagMapping.TagId = tagId;
                            db.TagMappings.Add(tagMapping);
                        }
                    }
                    db.SaveChanges();
                    dbContextTransaction.Commit();
                    var response = new
                    {
                        status = 200,
                        msg = "Status Updated Successfully"
                    };
                    return Json(response);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
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
}

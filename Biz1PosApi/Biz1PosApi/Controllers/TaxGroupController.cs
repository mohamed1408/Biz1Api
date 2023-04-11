using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Biz1BookPOS.Controllers
{
    [Route("api/[controller]")]
    public class TaxGroupController : Controller
    {
        private POSDbContext db;
        public IConfiguration Configuration { get; }

        public TaxGroupController(POSDbContext contextOptions, IConfiguration configuration)
        {
            db = contextOptions;
            Configuration = configuration;
        }

        // GET: api/<controller>
        [HttpGet("Get")]
        public IActionResult Get(int CompanyId)
        {
            try
            {
                var taxgroup = db.TaxGroups.Where(t => t.CompanyId == CompanyId).ToList();
                //var variantGroups = db.Query(table).ToList();
                return Ok(taxgroup);
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


        // GET api/<controller>/5
        // [HttpGet("{id}")]

        [HttpPost("AddTax")]
        [EnableCors("AllowOrigin")]
        public IActionResult AddTax([FromForm]string data)
        {
            try
            {
                dynamic tax = JsonConvert.DeserializeObject(data);
                TaxGroup taxGroup = tax.ToObject<TaxGroup>();
                db.TaxGroups.Add(taxGroup);
                db.SaveChanges();
                var error = new
                {
                    status = 200,
                    msg = "The taxGroup added successfully"
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
                var taxgroup = db.TaxGroups.Find(Id);
                return Ok(taxgroup);
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
        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPost("Update")]
        public IActionResult Update([FromForm]string data)
        {
            try
            {
                dynamic tax = JsonConvert.DeserializeObject(data);
                TaxGroup taxGroup = tax.ToObject<TaxGroup>();
                db.Entry(taxGroup).State = EntityState.Modified;
                db.SaveChanges();
                var error = new
                {
                    status = 200,
                    msg = "The taxGroup updated successfully"
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

        [HttpGet("Delete")]
        [EnableCors("AllowOrigin")]
        public IActionResult Delete(int Id)
        {
            try
            {
                
                var taxgroup = db.TaxGroups.Find(Id);
                db.TaxGroups.Remove(taxgroup);
                db.SaveChanges();
                var error = new
                {
                    status = 200,
                    msg = "TaxGroup deleted successfully"
                };
                return Json(error);
            }
            catch (Exception ex)
            {
                var error = new
                {
                    status = 0,
                    msg = "This tax group can't be deleted as it is combined with product"
                };
                return Json(error);
            }
        }
        [HttpGet("getMapedProducts")]
        public IActionResult getMapedProducts(int CompanyId, int TaxGroupId)
        {
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("dbo.Product_TaxGroups", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@CompanyId", CompanyId));
            cmd.Parameters.Add(new SqlParameter("@taxgroupid", TaxGroupId));

            DataSet ds = new DataSet();
            SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
            sqlAdp.Fill(ds);
            DataTable table = ds.Tables[0];
            var response = new
            {
                Products = ds.Tables[0]
            };
            return Ok(response);
        }
        [HttpPost("BulkSave")]
        public IActionResult BulkSave(int TaxGroupId, int CompanyId, [FromBody] int[] ProductIds)
        {
            try
            {
                List<Product> products = db.Products.Where(x => ProductIds.Contains(x.Id) && x.TaxGroupId != TaxGroupId).ToList();
                foreach (Product product in products)
                {
                    product.TaxGroupId = TaxGroupId;
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                }
                var response = new
                {
                    message = "success",
                    status = 200
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
                return Json(error);
            }
        }
    }
}

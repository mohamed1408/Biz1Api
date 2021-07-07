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

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    public class UrbanPiperStoresController : Controller
    {
        private POSDbContext db;
        public UrbanPiperStoresController(POSDbContext contextOptions)
        {
            db = contextOptions;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetIndex")]
        [EnableCors("AllowOrigin")]
        public IActionResult GetIndex(int companyId)
        {
            var urbanpiper = db.UrbanPiperStores.Where(x => x.CompanyId == companyId).Include(x => x.Brand).Include(x => x.Store).ToList();
            return Ok(urbanpiper);
        }

        [HttpGet("GetById")]
        [EnableCors("AllowOrigin")]
        public IActionResult GetById(int Id)
        {
            var urbanpiper = db.UrbanPiperStores.Where(x => x.Id == Id).FirstOrDefault();
            return Ok(urbanpiper);
        }

        [HttpGet("GetPrd")]
        [EnableCors("AllowOrigin")]
        public IActionResult GetPrd(int Id)
        {
            var prod = new
            {
                urbanPiper = from sp in db.StoreProducts
                             join p in db.Products on sp.ProductId equals p.Id
                             join pt in db.ProductTypes on p.ProductTypeId equals pt.Id
                             join c in db.Categories on p.CategoryId equals c.Id
                             where sp.StoreId == Id
                             select new { sp.Id, p.Description, sp.UPPrice, p.ProductTypeId, Category = c.Description, p.CategoryId, ProductId = p.Id, sp.Available },
            };
            return Json(prod);
        }

        [HttpPost("StoreProduct")]
        [EnableCors("AllowOrigin")]
        public IActionResult StoreProduct([FromForm]string ProductData)
        {
            try
            {
                dynamic Products = JsonConvert.DeserializeObject(ProductData);
                foreach (var Product in Products)
                {
                    int Id = Product.Id;
                    var storeProduct = db.StoreProducts.SingleOrDefault(x => x.Id == Id);
                    storeProduct.UPPrice = Product.Price;
                    db.Entry(storeProduct).State = EntityState.Modified;
                    db.SaveChanges();
                    UPProduct uPProduct = db.UPProducts.Where(x => x.ProductId == storeProduct.ProductId && x.StoreId == storeProduct.StoreId).FirstOrDefault();
                    uPProduct.Price = Product.Price;
                    db.Entry(uPProduct).State = EntityState.Modified;
                    db.SaveChanges();
                }
                var error = new
                {
                    status = 200,
                    msg = "UPPrice is updated"
                };
                return Ok(error);
            }
            catch (Exception e)
            {
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 0,
                    msg = "Something went wrong. Contact your service provider"
                };
                return Ok(error);
            }
        }

        [HttpPost("StoreOption")]
        [EnableCors("AllowOrigin")]
        public IActionResult StoreOption([FromForm]string OptionData)
        {
            try
            {
                dynamic Options = JsonConvert.DeserializeObject(OptionData);
                foreach (var option in Options)
                {
                    int Id = option.Id;
                    var storeOption = db.StoreOptions.SingleOrDefault(x => x.Id == Id);
                    storeOption.UPPrice = option.Price;
                    db.Entry(storeOption).State = EntityState.Modified;
                    db.SaveChanges();
                }
                var response = new
                {
                    status = 200,
                    msg = "UPPrice is updated"
                };
                return Ok(response);
            }
            catch(Exception e)
            {
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 0,
                    msg = "Something went wrong. Contact your service provider"
                };
                return Ok(error);
            }
        }

    }
}
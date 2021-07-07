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
    public class SaleProductGroupController : Controller
    {
        private int var_status;
        private Array var_value;
        private string var_msg;
        private POSDbContext db;
        public IConfiguration Configuration { get; }
        public SaleProductGroupController(POSDbContext contextOptions, IConfiguration configuration)
        {
            db = contextOptions;
            Configuration = configuration;
        }
        [HttpGet("GetChildProds")]
        public IActionResult GetChildProds(int saleProdId, int compId)
        {
            var prod = new
            {
                products = from spg in db.SaleProductGroups
                           join p in db.Products on spg.StockProductId equals p.Id
                           join op in db.Options on spg.OptionId equals op.Id into options
                           from l in options.DefaultIfEmpty()

                           where spg.SaleProductId == saleProdId && p.CompanyId == compId
                           select new
                           {
                               SaleProductId = p.Id,
                               StockProduct = spg.OptionId != null ? p.Name + "-" + l.Name : p.Name
                            ,
                               Option = l.Name,
                               spg.StockProductId
                           }

            };
            return Json(prod);
        }
        [HttpGet("GetSaleProducts")]
        public IActionResult GetSaleProducts(int companyid)
        {
            var prod = new
            {
                products = db.Products.Where(s => s.CompanyId == companyid && s.isactive == true && s.IsSaleProdGroup == true).ToList()
            };
            return Json(prod);
        }
        [HttpGet("GetProductsNoptions")]
        public IActionResult GetProductsNoptions(int companyid)
        {
            var productOption = from pog in db.ProductOptionGroups
                                join p in db.Products on pog.ProductId equals p.Id
                                join opg in db.OptionGroups on pog.OptionGroupId equals opg.Id
                                join op in db.Options on opg.Id equals op.OptionGroupId
                                where pog.CompanyId == companyid && opg.OptionGroupType == 1 &&
                                p.isactive == true
                                select new { p.Id, Name = p.Name + "-" + op.Name, OptionId = op.Id };
            var exceptProds = from pog in db.ProductOptionGroups
                              join p in db.Products on pog.ProductId equals p.Id
                              where pog.CompanyId == companyid && p.IsSaleProdGroup == false
                              select new { p.Id, p.Name };
            var prod = db.Products.Where(s => s.CompanyId == companyid && s.isactive == true && s.IsSaleProdGroup == false)
                .Select(s => new { s.Id, s.Name }).Except(exceptProds).ToList();
            //var prod = db.Products.Where(s => s.CompanyId == companyid && s.isactive == true && s.IsSaleProdGroup != true)
            //    .Select(s => new { s.Id, s.Name }).ToList();
            var data = new { productOption = productOption, prod = prod };
            return Json(data);
        }
        [HttpPost("Save")]
        [EnableCors("AllowOrigin")]
        public IActionResult Save([FromForm] string objData)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    dynamic jsonObj = JsonConvert.DeserializeObject(objData);
                    int saleProductId = jsonObj.saleProductId;

                    int companyId = jsonObj.companyId;
                    JArray itemObj = jsonObj.item;
                    dynamic itemJson = itemObj.ToList();

                    JArray removeItem = jsonObj.removeStockProds;
                    dynamic removeItemJson = removeItem.ToList();

                    foreach (var item in itemJson)
                    {
                        int stockProductId = item.StockProductId;
                        int? optionId = item.OptionId;
                        var existing = db.SaleProductGroups.Where(s => s.SaleProductId == saleProductId
                                   && s.StockProductId == stockProductId && s.CompanyId == companyId).ToList();
                        if (existing.Count() == 0)
                        {
                            SaleProductGroup saleProductGroup = new SaleProductGroup();
                            saleProductGroup.StockProductId = stockProductId;
                            saleProductGroup.CompanyId = companyId;
                            saleProductGroup.SaleProductId = saleProductId;
                            saleProductGroup.OptionId = optionId;
                            saleProductGroup.CreatedDate = DateTime.Now;
                            db.SaleProductGroups.Add(saleProductGroup);
                        }
                    }
                    foreach (var item in removeItemJson)
                    {
                        int stockProductId = item.StockProductId;
                        var saleproduct = db.SaleProductGroups.Where(s => s.StockProductId == stockProductId &&
                          s.SaleProductId == saleProductId && s.CompanyId == companyId).FirstOrDefault();
                        db.SaleProductGroups.Remove(saleproduct);
                    }
                    db.SaveChanges();
                    dbContextTransaction.Commit();
                    var response = new
                    {
                        status = 200,
                        msg = "Data Updated Successfully"
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
        [HttpGet("GetProductsNoptions")]
        public IActionResult GetProductsNoptions(int companyid, int? catId)
        {
            var productOption = from pog in db.ProductOptionGroups
                                join p in db.Products on pog.ProductId equals p.Id
                                join opg in db.OptionGroups on pog.OptionGroupId equals opg.Id
                                join op in db.Options on opg.Id equals op.OptionGroupId
                                //join c in db.Categories on p.CategoryId equals c.Id
                                where pog.CompanyId == companyid && opg.OptionGroupType == 1 &&
                                p.isactive == true && (catId == null || catId == 0 || p.CategoryId == catId)
                                select new { p.Id, Name = p.Name + "-" + op.Name, OptionId = op.Id, p.CategoryId };
            var exceptProds = from pog in db.ProductOptionGroups
                              join p in db.Products on pog.ProductId equals p.Id
                              join opg in db.OptionGroups on pog.OptionGroupId equals opg.Id
                              where pog.CompanyId == companyid && p.IsSaleProdGroup == false &&
                              opg.OptionGroupType == 1
                              select new { p.Id, p.Name };
            var prod = db.Products.Where(s => s.CompanyId == companyid && s.isactive == true
                            && s.IsSaleProdGroup == false && (catId == null || catId == 0 || s.CategoryId == catId))
               .Select(s => new { s.Id, s.Name }).Except(exceptProds).ToList();
            //var prod = db.Products.Where(s => s.CompanyId == companyid && s.isactive == true && s.IsSaleProdGroup != true)
            //    .Select(s => new { s.Id, s.Name }).ToList();
            var data = new { productOption = productOption, prod = prod };
            return Json(data);
        }
    }
}

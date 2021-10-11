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
                           orderby spg.SortOrder.HasValue
                                  ? (spg.SortOrder == 0 ? (int.MaxValue - 1) : spg.SortOrder)
                                  : int.MaxValue
                           select new
                           {
                               SaleProductId = spg.SaleProductId,
                               StockProduct = spg.OptionId != null ? p.Name + "-" + l.Name : p.Name
                            ,
                               Option = l.Name,
                               spg.StockProductId,
                               spg.Id,
                               spg.Factor
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
        public IActionResult GetProductsNoptions(int companyid, int? catId, string desc)
        {
            if (desc == null) desc = "";
            var productOption = from pog in db.ProductOptionGroups
                                join p in db.Products on pog.ProductId equals p.Id
                                join opg in db.OptionGroups on pog.OptionGroupId equals opg.Id
                                join op in db.Options on opg.Id equals op.OptionGroupId
                                join c in db.Categories on p.CategoryId equals c.Id
                                where pog.CompanyId == companyid && opg.OptionGroupType == 1 &&
                                p.isactive == true && (catId == null || catId == 0 || p.CategoryId == catId || c.ParentCategoryId == catId)
                                //&& (desc == "" || p.Name.ToUpper().Contains(desc.ToUpper()))
                                && (desc == "" || (p.Name + "-" + op.Name).ToUpper().Contains(desc.ToUpper()))
                                //op.Name.ToUpper().Contains(desc.ToUpper()))
                                select new { p.Id, Name = p.Name + "-" + op.Name, OptionId = op.Id, p.CategoryId };
            var exceptProds = from pog in db.ProductOptionGroups
                              join p in db.Products on pog.ProductId equals p.Id
                              join opg in db.OptionGroups on pog.OptionGroupId equals opg.Id
                              where pog.CompanyId == companyid && p.IsSaleProdGroup != true &&
                              opg.OptionGroupType == 1
                              select new { p.Id, p.Name };
            var prod = from p in db.Products
                       join c in db.Categories on p.CategoryId equals c.Id
                       where p.CompanyId == companyid && p.isactive == true
                             && (catId == null || catId == 0 ||
                             p.CategoryId == catId || c.ParentCategoryId == catId)
                             && (desc == "" || p.Name.ToUpper().Contains(desc.ToUpper()))
                              && !(from s in exceptProds select s.Id).Contains(p.Id)
                       select new { p.Id, p.Name };

            //var prod1 = db.Products.Where(s => s.CompanyId == companyid && s.isactive == true
            //               && s.IsSaleProdGroup == false && (catId == null || catId == 0 || s.CategoryId == catId))
            //  .Select(s => new { s.Id, s.Name }).Except(exceptProds).ToList();
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
                    JArray updatedItemsRaw = jsonObj.updatedItems;
                    dynamic updatedItems = updatedItemsRaw.ToList();
                    dynamic itemJson = itemObj.ToList();

                    JArray removeItem = jsonObj.removeStockProds;
                    dynamic removeItemJson = removeItem.ToList();
                    foreach (var item in updatedItems)
                    {
                        SaleProductGroup saleProductGroup = db.SaleProductGroups.Find(item.Id.ToObject<int>());
                        saleProductGroup.Factor = item.Factor;
                        db.Entry(saleProductGroup).State = EntityState.Modified; 
                    }
                    foreach (var item in itemJson)
                    {
                        int stockProductId = item.StockProductId;
                        int? optionId = item.OptionId;
                        int? sortOrder = item.SortOrder;
                        double? factor = item.Factor;
                        bool? isOnline = item.IsOnline;
                        var existing = db.SaleProductGroups.Where(s => s.SaleProductId == saleProductId
                                   && s.StockProductId == stockProductId && s.CompanyId == companyId).ToList();
                        if (existing.Count() == 0)
                        {
                            SaleProductGroup saleProductGroup = new SaleProductGroup();
                            saleProductGroup.StockProductId = stockProductId;
                            saleProductGroup.CompanyId = companyId;
                            saleProductGroup.SaleProductId = saleProductId;
                            saleProductGroup.OptionId = optionId;
                            saleProductGroup.Factor = factor;
                            saleProductGroup.IsOnline = isOnline;
                            saleProductGroup.SortOrder = sortOrder;
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
    }
}
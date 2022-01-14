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
                               spg.Factor,
                               p.IsSaleProdGroup
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
        public IActionResult GetProductsNoptions(int companyid, int? categoryid, string desc, int? saleproductid)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.SaleProductsNoptions", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@companyid", companyid));
                cmd.Parameters.Add(new SqlParameter("@categoryid", categoryid));
                cmd.Parameters.Add(new SqlParameter("@searchterm", desc));
                cmd.Parameters.Add(new SqlParameter("@saleproductid", saleproductid));
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);

                DataTable table = ds.Tables[0];
                var data = new { productOption = table };
                return Json(data);
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
        [HttpGet("GetSPGProducts")]
        public IActionResult GetSPGProducts(int companyid, int saleproductid)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.GetSPGProducts", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@companyid", companyid));
                cmd.Parameters.Add(new SqlParameter("@saleproductid", saleproductid));
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);

                DataTable table = ds.Tables[0];
                var data = new { products = table };
                return Json(data);
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
                                   && s.StockProductId == stockProductId && s.CompanyId == companyId && s.OptionId == optionId).ToList();
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
                        int spgId = item.spgId;
                        var saleproduct = db.SaleProductGroups.Find(spgId);
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
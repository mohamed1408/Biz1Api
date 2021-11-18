using System;
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

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    public class OptionGroupController : Controller
    {
        public IConfiguration Configuration { get; }
        private POSDbContext db;
        public OptionGroupController(POSDbContext contextOptions, IConfiguration configuration)
        {
            db = contextOptions;
            Configuration = configuration;
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
                var optionGroups = db.OptionGroups.Where(v => v.CompanyId == CompanyId).ToList();
                System.Collections.Generic.Dictionary<string, object>[] objData = new System.Collections.Generic.Dictionary<string, object>[optionGroups.Count()];

                //var variant = db.Variants.ToList();
                for (int i = 0; i < optionGroups.Count(); i++)
                {
                    objData[i] = new Dictionary<string, object>();
                    objData[i].Add("Id", optionGroups[i].Id);
                    objData[i].Add("OptionGroup", optionGroups[i].Name);
                    objData[i].Add("isactive", optionGroups[i].isactive);
                    string str = "";

                    var options = db.Options.Where(v => v.OptionGroupId == optionGroups[i].Id).ToList();
                    int varCount = options.Count();
                    for (int j = 0; j < varCount; j++)
                    {
                        if (j < varCount - 1)
                        {
                            str += options[j].Name + ",";
                        }
                        else
                        {
                            str += options[j].Name;
                        }

                    }
                    objData[i].Add("Options", str);
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

        [HttpPost("CreateOption")]
        [EnableCors("AllowOrigin")]
        public IActionResult CreateOption([FromForm]string data)
        {
            try
            {
                dynamic orderJson = JsonConvert.DeserializeObject(data);
                {
                    OptionGroup optionGroup = orderJson.ToObject<OptionGroup>();
                    optionGroup.CreatedDate = DateTime.Now;
                    optionGroup.ModifiedDate = DateTime.Now;
                    db.OptionGroups.Add(optionGroup);
                    db.SaveChanges();
                    JArray itemObj = orderJson.Options;
                    dynamic itemJson = itemObj.ToList();
                    foreach (var item in itemJson)
                    {
                        Option option = item.ToObject<Option>();
                        option.OptionGroupId = optionGroup.Id;
                        option.CreatedDate = DateTime.Now;
                        option.ModifiedDate = DateTime.Now;
                        db.Options.Add(option);
                        db.SaveChanges();
                        var stores = db.Stores.Where(x => x.CompanyId == optionGroup.CompanyId).ToList();
                        foreach (var store in stores)
                        {
                            StoreOption storeOption = new StoreOption();
                            storeOption.CompanyId = optionGroup.CompanyId;
                            storeOption.CreatedDate = DateTime.Now;
                            storeOption.ModifiedDate = DateTime.Now;
                            storeOption.StoreId = store.Id;
                            storeOption.OptionId = option.Id;
                            storeOption.IsActive = true;
                            storeOption.Price = option.Price;
                            storeOption.TakeawayPrice = option.Price;
                            storeOption.DeliveryPrice = option.Price;
                            storeOption.UPPrice = option.Price;
                            db.StoreOptions.Add(storeOption);
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
                        msg = "The OptionGroup added successfully"
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
        [HttpGet("GetOptions")]
        public IActionResult GetOptions(int companyid)
        {
            try
            {
                List<Option> options = db.Options.Where(x => x.CompanyId == companyid).ToList();
                return Ok(options);
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
                var optionGroups = db.OptionGroups.Find(Id);
                System.Collections.Generic.Dictionary<string, object>[] objData = new System.Collections.Generic.Dictionary<string, object>[1];

                var option = db.Options.Where(v => v.OptionGroupId == Id).ToList();
                objData[0] = new Dictionary<string, object>();
                objData[0].Add("Id", optionGroups.Id);
                objData[0].Add("OptionGroup", optionGroups.Name);
                objData[0].Add("OptionGroupType", optionGroups.OptionGroupType);
                objData[0].Add("Description", optionGroups.Description);
                objData[0].Add("MinimumSelectable", optionGroups.MinimumSelectable);
                objData[0].Add("MaximumSelectable", optionGroups.MaximumSelectable);
                //string str = "";
                //for (int j = 0; j < variant.Count(); j++)
                //{
                //    str += variant[j].Description + ",";
                //}
                objData[0].Add("Options", option);


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
                OptionGroup optionGroup = orderJson.ToObject<OptionGroup>();
                optionGroup.CreatedDate = db.OptionGroups.Where(x => x.Id == optionGroup.Id).AsNoTracking().FirstOrDefault().CreatedDate;
                optionGroup.ModifiedDate = DateTime.Now;
                db.Entry(optionGroup).State = EntityState.Modified;
                db.SaveChanges();
                JArray itemObj = orderJson.Options;
                JArray itemDel = orderJson.Del;
                dynamic itemJson = itemObj.ToList();
                dynamic itemJsonDel = itemDel.ToList();
                foreach (var item in itemJson)
                {
                    if (item.Id == 0)
                    {
                        Option option = item.ToObject<Option>();
                        option.CreatedDate = DateTime.Now;
                        option.ModifiedDate = DateTime.Now;
                        option.TakeawayPrice = option.Price;
                        option.DeliveryPrice = option.Price;
                        option.UPPrice = option.Price;
                        db.Options.Add(option);
                        db.SaveChanges();

                        var stores = db.Stores.Where(x =>x.CompanyId == option.CompanyId).ToList();
                        foreach (var store in stores)
                        {
                            StoreOption storeOption = new StoreOption();
                            storeOption.Price = option.Price;
                            storeOption.StoreId = store.Id;
                            storeOption.OptionId = option.Id;
                            storeOption.CompanyId = option.CompanyId;
                            storeOption.IsActive = false;
                            storeOption.ModifiedDate = DateTime.Now;
                            storeOption.CreatedDate = DateTime.Now;
                            storeOption.TakeawayPrice = option.Price;
                            storeOption.DeliveryPrice = option.Price;
                            storeOption.UPPrice = option.Price;
                            db.StoreOptions.Add(storeOption);
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        Option option = item.ToObject<Option>();
                        option.CreatedDate = db.Options.Where(x => x.Id == option.Id).AsNoTracking().FirstOrDefault().CreatedDate;
                        option.ModifiedDate = DateTime.Now;
                        option.TakeawayPrice = option.Price;
                        option.DeliveryPrice = option.Price;
                        option.UPPrice = option.Price;
                        db.Entry(option).State = EntityState.Modified;
                        db.SaveChanges();


                        var stores = db.StoreOptions.Where(x => x.OptionId == option.Id && x.CompanyId == option.CompanyId).ToList();
                        foreach (var store in stores)
                        {
                            StoreOption storeOption = store;
                            storeOption.Price = option.Price;
                            storeOption.ModifiedDate = DateTime.Now;
                            storeOption.TakeawayPrice = option.TakeawayPrice;
                            storeOption.DeliveryPrice = option.DeliveryPrice;
                            storeOption.UPPrice = option.UPPrice;
                            db.Entry(storeOption).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                    }

                }
                foreach (var item in itemJsonDel)
                {
                    int itemId = item.Id.ToObject<int>();
                    Option option = db.Options.Find(itemId);
                    var stores = db.StoreOptions.Where(x => x.OptionId == option.Id && x.CompanyId == option.CompanyId).ToList();
                    foreach (var store in stores)
                    {
                        StoreOption storeOption = store;
                        db.StoreOptions.Remove(storeOption);
                        db.SaveChanges();
                    }
                    db.Options.RemoveRange(option);
                    db.SaveChanges();
                    
                }
                var error = new
                {
                    status = "success",
                    data = new
                    {
                        value = 2
                    },
                    msg = "The OptionData Updated successfully"
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
                var productoption = db.ProductOptionGroups.Where(x => x.OptionGroupId == Id).ToList();
                var catoption = db.CategoryOptionGroups.Where(x => x.OptionGroupId == Id).ToList();
                if(productoption.Count == 0 && catoption.Count == 0)
                {
                    var option = db.Options.Where(x => x.OptionGroupId == Id).ToList();
                    foreach (var item in option)
                    {
                        var storeoption = db.StoreOptions.Where(x => x.OptionId == item.Id).ToList();
                        {
                            foreach(var storeoptn in storeoption)
                            {
                                var stroptn = db.StoreOptions.Find(storeoptn.Id);
                                db.StoreOptions.Remove(stroptn);
                                db.SaveChanges();
                            }
                        }
                        var opt = db.Options.Find(item.Id);
                        db.Options.Remove(opt);
                    }
                    var optiongrp = db.OptionGroups.Find(Id);
                    db.OptionGroups.Remove(optiongrp);
                    db.SaveChanges();
                    var error = new
                    {
                        status = 200,
                        msg = "The OptionGroup deleted successfully"
                    };
                    return Json(error);
                }
                else
                {
                    var error = new
                    {
                        status = 0,
                        msg = "The OptionGroup is mapped to Products and Category"
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
        [HttpGet("UpdateAct")]
        [EnableCors("AllowOrigin")]
        public IActionResult UpdateAct(int Id, bool active)
        {
            try
            {
                var options = db.OptionGroups.Find(Id);
                options.isactive = active;
                db.Entry(options).State = EntityState.Modified;
                db.SaveChanges();
                var error = new
                {
                    status = "success",
                    data = new
                    {
                        value = 2
                    },
                    msg = "The data updated successfully"
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
        [HttpGet("getMapedProducts")]
        public IActionResult getMapedProducts(int CompanyId, int OptionGroupId)
        {
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("dbo.Product_OptionGroups", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@CompanyId", CompanyId));
            cmd.Parameters.Add(new SqlParameter("@OptionGroupId", OptionGroupId));

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
        public IActionResult BulkSave(int OptionGroupId, int CompanyId, [FromBody] int[] ProductIds)
        {
            try
            {
                List<ProductOptionGroup> delpogs = db.ProductOptionGroups.Where(x => x.OptionGroupId == OptionGroupId && !ProductIds.Contains(x.ProductId)).ToList();
                foreach (int productId in ProductIds)
                {
                    if (!db.ProductOptionGroups.Where(x => x.OptionGroupId == OptionGroupId && x.ProductId == productId).Any())
                    {
                        ProductOptionGroup pog = new ProductOptionGroup();
                        pog.CompanyId = CompanyId;
                        pog.CreatedDate = DateTime.Now;
                        pog.ModifiedDate = DateTime.Now;
                        pog.OptionGroupId = OptionGroupId;
                        pog.ProductId = productId;
                        db.ProductOptionGroups.Add(pog);
                        db.SaveChanges();
                    }
                }
                db.ProductOptionGroups.RemoveRange(delpogs);
                db.SaveChanges();
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

        //[HttpGet("GetOptions")]
        //[EnableCors("AllowOrigin")]
        //public IActionResult GetOptions(int companyid)
        //{
        //    try
        //    {
        //        List<Option> options = db.Options.Where(x => x.CompanyId == companyid).ToList();
        //        var error = new
        //        {
        //            status = "success",
        //            data = new
        //            {
        //                value = 2
        //            },
        //            msg = "The data updated successfully"
        //        };

        //        return Ok(options);
        //    }
        //    catch (Exception e)
        //    {
        //        var error = new
        //        {
        //            error = new Exception(e.Message, e.InnerException),
        //            status = 0,
        //            msg = "Something went wrong  Contact our service provider"
        //        };
        //        return Json(error);
        //    }

        //}


    }
}
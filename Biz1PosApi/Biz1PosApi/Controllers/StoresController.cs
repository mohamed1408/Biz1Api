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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    public class StoresController : Controller
    {
        private POSDbContext db;
        public IConfiguration Configuration { get; }
        public StoresController(POSDbContext contextOptions, IConfiguration configuration)
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
                var stores = db.Stores.Where(s => s.CompanyId == CompanyId).ToList();
                return Ok(stores);
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

        [HttpGet("GetData")]
        public IActionResult GetData(int prodId, int storeId)
        {
            try
            {
                var prod = new
                {
                    storeproduct = from pa in db.StoreProducts
                                   where pa.StoreId == storeId && pa.ProductId == prodId
                                   select new { pa.Price, pa.TakeawayPrice, pa.DeliveryPrice, pa.Id },
                    storeproductOption = from pv in db.StoreProductOptions
                                         join v in db.Options on pv.OptionId equals v.Id
                                         where pv.StoreId == storeId && pv.ProductId == prodId
                                         select new { pv.Price, pv.TakeawayPrice, pv.DeliveryPrice, v.Name, v.OptionGroupId, pv.Id },
                    OptionGroup = (from spo in db.StoreProductOptions
                                   join o in db.Options on spo.OptionId equals o.Id
                                   join og in db.OptionGroups on o.OptionGroupId equals og.Id
                                   where spo.StoreId == storeId && spo.ProductId == prodId
                                   select new { og.Id, og.Name }).Distinct()


                    //storeproductAddons = from pg in db.StoreProductAddons
                    //                     join ad in db.Addons on pg.AddOnId equals ad.Id
                    //                     join a in db.Products on ad.ProductId equals a.Id
                    //                  where pg.StoreId == storeId && pg.ProductId == prodId
                    //                  select new { pg.Id,pg.Price, pg.TakeawayPrice, pg.DeliveryPrice, a.Description, ad.AddonGroupId},

                    //AddonGroup =      (from pg in db.StoreProductAddons
                    //                   join a in db.Products on pg.ProductId equals a.Id
                    //                   join ad in db.Addons on pg.AddOnId equals ad.Id
                    //                    join ap in db.AddonGroups on ad.AddonGroupId equals ap.Id
                    //                   where pg.StoreId == storeId && pg.ProductId == prodId
                    //            select new { pg.Price, pg.TakeawayPrice, pg.DeliveryPrice, ap.Description, ad.AddonGroupId, ap.Id }).Distinct(),


                };
                return Json(prod);
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



        [HttpPost("AddStore")]
        [EnableCors("AllowOrigin")]
        public IActionResult AddStore([FromForm]string data)
        {
            try
            {
                dynamic outlet = JsonConvert.DeserializeObject(data);

                Store store = outlet.ToObject<Store>();
                db.Stores.Add(store);
                db.SaveChanges();
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.StoreCreation", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@storeid", store.Id));
                cmd.Parameters.Add(new SqlParameter("@companyid", store.CompanyId));
                int success = cmd.ExecuteNonQuery();
                sqlCon.Close();
                var error = new
                {
                    status = 200,
                    msg = "The data added successfully"
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
        [HttpPost("UpdateData")]
        public IActionResult UpdateData([FromForm]string data, [FromForm] string kotprinterdata)
        {
            try
            {
                dynamic outlet = JsonConvert.DeserializeObject(data);
                Store Store = outlet.ToObject<Store>();
                db.Entry(Store).State = EntityState.Modified;
                db.SaveChanges();
                dynamic kotprinters = JsonConvert.DeserializeObject(kotprinterdata);
                foreach(var kotprinter in kotprinters)
                {
                    KOTGroupPrinter kOTGroupPrinter = kotprinter.ToObject<KOTGroupPrinter>();
                    db.Entry(kOTGroupPrinter).State = EntityState.Modified;
                    db.SaveChanges();
                }
                var error = new
                {
                    status = 200,
                    msg = "The Store updated successfully"
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


        [HttpGet("Edit")]
        [EnableCors("AllowOrigin")]
        public IActionResult Edit(int Id)
        {
            try
            {
                var store = db.Stores.Find(Id);
                return Ok(store);


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


        // DELETE api/<controller>/5
        [HttpGet("Delete")]
        public IActionResult Delete(int Id)
        {
            try
            {
                var store = db.Stores.Find(Id);
                db.Stores.Remove(store);
                db.SaveChanges();
                var error = new
                {
                    status = 200,
                    data = new
                    {
                        value = 5
                    },
                    msg = "The Store deleted successfully"
                };
                return Json(error);
            }
            catch (Exception e)
            {
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 0,
                    msg = "The Outlet can't be deleted as it has some products are mapped"
                };
                return Json(error);
            }
        }

        [HttpPost("Update")]
        [EnableCors("AllowOrigin")]
        public IActionResult Update([FromForm]string data)
        {
            try
            {
                dynamic orderJson = JsonConvert.DeserializeObject(data);
                foreach (var strProd in orderJson.storeproduct)
                {
                    StoreProduct storeProduct = strProd.ToObject<StoreProduct>();
                    StoreProduct storeProdFromDb = db.StoreProducts.Find(storeProduct.Id);
                    storeProdFromDb.Price = storeProduct.Price;
                    storeProdFromDb.DeliveryPrice = storeProduct.DeliveryPrice;
                    storeProdFromDb.TakeawayPrice = storeProduct.TakeawayPrice;
                    db.Entry(storeProdFromDb).State = EntityState.Modified;

                }
                foreach (var strprdvrt in orderJson.storeproductOption)
                {
                    StoreProductOption storeProductOption = strprdvrt.ToObject<StoreProductOption>();
                    StoreProductOption storeVarFromDb = db.StoreProductOptions.Find(storeProductOption.Id);
                    storeVarFromDb.Price = storeProductOption.Price;
                    storeVarFromDb.DeliveryPrice = storeProductOption.DeliveryPrice;
                    storeVarFromDb.TakeawayPrice = storeProductOption.TakeawayPrice;
                    db.Entry(storeVarFromDb).State = EntityState.Modified;

                }
                //foreach (var strProdAddon in orderJson.storeproductAddons)
                //{
                //    StoreProductAddon storeProductAddon = strProdAddon.ToObject<StoreProductAddon>();
                //    StoreProductAddon storeAddonFromDb = db.StoreProductAddons.Find(storeProductAddon.Id);
                //    storeAddonFromDb.Price = storeProductAddon.Price;
                //    storeAddonFromDb.DeliveryPrice = storeProductAddon.DeliveryPrice;
                //    storeAddonFromDb.TakeawayPrice = storeProductAddon.TakeawayPrice;

                //    db.Entry(storeAddonFromDb).State = EntityState.Modified;

                //}

                db.SaveChanges();
                var error = new
                {
                    status = 200,
                    msg = "Data saved successfully"
                };

                return Json(error);

            }
            catch (Exception e)
            {
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 0,
                    msg = "The Outlet can't be deleted as it has some products are mapped"

                };
                return Json(error);
            }
        }
        [HttpPost("temp")]
        public IActionResult temp()
        {
            try
            {
                var stores = db.UrbanPiperStores.Where(x => x.CompanyId == 3).ToList();
                foreach (var store in stores)
                {
                    var products = db.Products.Where(x => x.CategoryId == 335 || x.CategoryId == 336).ToList();
                    foreach (var product in products)
                    {
                        UPProduct oldupproduct = db.UPProducts.Where(x => x.ProductId == product.Id && x.StoreId == store.StoreId && x.BrandId == store.BrandId).FirstOrDefault();
                        if (oldupproduct == null)
                        {
                            UPProduct upproduct = new UPProduct();
                            upproduct.Available = true;
                            upproduct.BrandId = store.BrandId;
                            upproduct.CompanyId = product.CompanyId;
                            upproduct.Price = product.Price;
                            upproduct.ProductId = product.Id;
                            upproduct.StoreId = store.StoreId;
                            upproduct.IsEnabled = true;
                            db.UPProducts.Add(upproduct);
                            db.SaveChanges();
                        }
                        else
                        {
                            oldupproduct.IsEnabled = true;
                            db.Entry(oldupproduct).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }
                var response = new
                {
                    status = 200,
                    msg = "Successsfully updated"
                };
                return Json(response);
            }
            catch (Exception ex)
            {
                var error = new
                {
                    status = 0,
                    msg = "Something went wrong",
                    error = new Exception(ex.Message, ex.InnerException)
                };
                return Json(error);
            }
        }

        [HttpPost("itemaction")]
        public IActionResult itemaction([FromForm]string pddata, int? BrandId)
        {
            try
            {
                dynamic data = JsonConvert.DeserializeObject(pddata);
                foreach (var product in data.products)
                {
                    StoreProduct storeProduct = db.StoreProducts.Find(product.ToObject<int>());
                    if (data.action == "enable")
                    {
                        UPProduct oldupproduct = db.UPProducts.Where(x => x.ProductId == storeProduct.ProductId && x.StoreId == storeProduct.StoreId && x.BrandId == BrandId).FirstOrDefault();
                        if(oldupproduct == null)
                        {
                            UPProduct upproduct = new UPProduct();
                            upproduct.Available = true;
                            upproduct.BrandId = BrandId;
                            upproduct.CompanyId = storeProduct.CompanyId;
                            upproduct.Price = storeProduct.Price;
                            upproduct.ProductId = storeProduct.ProductId;
                            upproduct.StoreId = storeProduct.StoreId;
                            upproduct.IsEnabled = true;
                            db.UPProducts.Add(upproduct);
                            db.SaveChanges();
                        }
                        else
                        {
                            oldupproduct.IsEnabled = true;
                            db.Entry(oldupproduct).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    else if (data.action == "disable")
                    {
                        UPProduct upproduct = db.UPProducts.Where(x => x.ProductId == storeProduct.ProductId && x.StoreId == storeProduct.StoreId && x.BrandId == BrandId).FirstOrDefault();
                        if(upproduct != null)
                        {
                            upproduct.IsEnabled = false;
                            db.Entry(upproduct).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }
                var response = new
                {
                    status = 200,
                    msg = "Successsfully updated"
                };
                return Json(response);
            }
            catch (Exception ex)
            {
                var error = new
                {
                    status = 0,
                    msg = "Something went wrong",
                    error = new Exception(ex.Message, ex.InnerException)
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
                var stores = db.Stores.Find(Id);
                stores.isactive = active;
                db.Entry(stores).State = EntityState.Modified;
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

        [HttpPost("UpdatePreferences")]
        [EnableCors("AllowOrigin")]
        public IActionResult UpdatePreferences([FromForm]StorePreference storePreference)
        {
            try
            {
                db.Entry(storePreference).State = EntityState.Modified;
                db.SaveChanges();
                var error = new
                {
                    status = 200,
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
                    msg = "Something went wrong  Contact your service provider"
                };
                return Json(error);
            }

        }
        [HttpGet("GetPreference")]
        [EnableCors("AllowOrigin")]
        public IActionResult GetPreference(int storeid)
        {
            try
            {
                StorePreference storePreference = db.StorePreferences.Where(x => x.StoreId == storeid).FirstOrDefault();
                var error = new
                {
                    status = 200,
                    msg = "The data updated successfully"
                };

                return Ok(storePreference);
            }
            catch (Exception e)
            {
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 0,
                    msg = "Something went wrong  Contact your service provider"
                };
                return Json(error);
            }

        }
    }
}

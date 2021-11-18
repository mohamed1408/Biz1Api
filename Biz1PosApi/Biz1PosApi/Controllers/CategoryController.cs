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
    public class CategoryController : Controller
    {
        private POSDbContext db;
        public IConfiguration Configuration { get; }
        public CategoryController(POSDbContext contextOptions, IConfiguration configuration)
        {
            db = contextOptions;
            Configuration = configuration;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("Index")]
        public IActionResult Index(int CompanyId)
        {
            try
            {
                var category = db.Categories.Where(c => c.CompanyId == CompanyId).ToList();
                return Ok(category);
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
        public IActionResult Edit(int Id, int companyId)
        {
            try
            {
                var data = new
                {
                    category = db.Categories.Find(Id),
                    categoryOptions = db.CategoryOptionGroups.Where(x => x.CategoryId == Id && x.CompanyId == companyId).ToList(),
                    parentCategories = db.Categories.Where(x => x.ParentCategoryId == null && x.CompanyId == companyId).ToList(),
                    //categoryOptionGroup = db.CategoryOptionGroups.Where(c => c.CategoryId == Id && c.CompanyId == companyId).ToList(),
                    categoryOptionGroup = from cog in db.CategoryOptionGroups
                                          join og in db.OptionGroups on cog.OptionGroupId equals og.Id
                                          where (cog.CategoryId == Id && cog.CompanyId == companyId)
                                          select new { cog.OptionGroupId, og.Name, cog.Id }
                };

                return Ok(data);
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
                //var variants = db.CategoryVariants.Where(x => x.CategoryId == Id).ToList();
                // var addons = db.CategoryAddons.Where(x => x.CategoryId == Id).ToList();
                var category = db.Categories.Where(x => x.Id == Id).ToList();
                var options = db.CategoryOptionGroups.Where(x => x.CategoryId == Id).ToList();
                if (options != null)
                {
                    foreach (var item in options)
                    {
                        var opt = db.CategoryOptionGroups.Find(item.Id);
                        db.CategoryOptionGroups.Remove(opt);
                        db.SaveChanges();
                    }
                }
                //if (variants != null)
                //{
                //    foreach (var item in variants)
                //    {
                //        var var = db.CategoryVariants.Find(item.Id);
                //        db.CategoryVariants.Remove(var);
                //        db.SaveChanges();
                //    }
                //}
                //if (addons != null)
                //{
                //    foreach (var item in addons)
                //    {
                //        var add = db.CategoryAddons.Find(item.Id);
                //        db.CategoryAddons.Remove(add);
                //        db.SaveChanges();
                //    }
                //}
                if (category != null)
                {
                    foreach (var item in category)
                    {
                        var cat = db.Categories.Find(item.Id);
                        db.Categories.Remove(cat);
                        db.SaveChanges();
                    }
                }



                var error = new
                {
                    status = 200,
                    data = new
                    {
                        value = 2
                    },
                    msg = "The Product and corresponding variants/addons deleted successfully"
                };

                return Json(error);
            }
            catch (Exception e)
            {
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 0,
                    msg = "We Cannot Delete It Already associate With another Category"
                };
                return Json(error);
            }

        }
        [HttpGet("CatVariant")]
        [EnableCors("AllowOrigin")]
        public IActionResult CatVariant()
        {
            try
            {
                var prod = new
                {
                    catVgp = db.OptionGroups.ToList()
                    //catvar = db.Variants.ToList(),
                    //catAdgp = db.AddonGroups.ToList(),
                    //catAddon = from  a in db.Addons
                    //           join p in db.Products on a.ProductId equals p.Id
                    //           select new { a.Id,p.Description, a.AddonGroupId }
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

        [HttpPost("SaveVariant")]
        [EnableCors("AllowOrigin")]
        public IActionResult SaveVariant([FromForm] string data)
        {
            dynamic cat = JsonConvert.DeserializeObject(data);
            CategoryOptionGroup categoryOptionGroup = cat.ToObject<CategoryOptionGroup>();
            db.CategoryOptionGroups.Add(categoryOptionGroup);
            db.SaveChanges();

            var error = new
            {
                status = 200,
                msg = "The data saved successfully"
            };

            return Json(error);
        }

        [HttpPost("CopyToSaleProductGroup")]
        [EnableCors("AllowOrigin")]
        public IActionResult CopyToSaleProductGroup(int companyId, [FromBody]int[] catIds)
        {
            var list = new[] {new{message = "", success = 0}}.ToList();
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("dbo.CopyCategoryToSaleProductGroup", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@categoryId", 0));
            cmd.Parameters.Add(new SqlParameter("@companyId", companyId));
            list.RemoveAll(x => x.success == 0);
            foreach(int catId in catIds)
            {
                cmd.Parameters[0].Value = catId;
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                DataTable table = ds.Tables[0];
                list.Add(new { message = ds.Tables[0].Rows[0].ItemArray[0].ToString(), success = (int)ds.Tables[0].Rows[0].ItemArray[1] });
            }

            return Json(list);
        }

        [HttpGet("EditVariant")]
        [EnableCors("AllowOrigin")]
        public IActionResult EditVariant(int Id)
        {
            try
            {
                var prod = new
                {

                    category = from c in db.Categories
                               where c.Id == Id
                               select new { c.Description, c.Id, c.ParentCategoryId },

                    categoryVariant = from cv in db.CategoryVariants
                                      join c in db.Categories on cv.CategoryId equals c.Id
                                      join v in db.Variants on cv.VariantId equals v.Id
                                      where c.Id == Id
                                      select new { c.ParentCategoryId, cv.Id, cv.VariantId, v.Description, cv.VariantGroupId },

                    categoryAddon = from ca in db.CategoryAddons
                                    join c in db.Categories on ca.CategoryId equals c.Id
                                    join a in db.Addons on ca.AddonId equals a.Id
                                    join p in db.Products on a.ProductId equals p.Id
                                    where ca.CategoryId == Id
                                    select new { ca.Id, ca.AddonId, p.Description, ca.AddonGroupId },

                    categoryOptionGroup = from cv in db.CategoryOptionGroups
                                          join c in db.Categories on cv.CategoryId equals c.Id
                                          join v in db.OptionGroups on cv.OptionGroupId equals v.Id
                                          where c.Id == Id
                                          select new { c.ParentCategoryId, cv.Id, v.Name, cv.OptionGroupId },

                };
                return Json(prod);
            }
            catch (Exception ex)
            {
                var error = new
                {
                    status = 0,
                    msg = ex.Message
                };
                return Json(error);
            }
        }

        [HttpPost("SaveData")]
        public IActionResult SaveData([FromForm]string objData)
        {
            dynamic catJson = JsonConvert.DeserializeObject(objData);
            try
            {
                Category category = catJson.ToObject<Category>();
                category.CreatedDate = DateTime.Now;
                category.ModifiedDate = DateTime.Now;
                db.Categories.Add(category);
                db.SaveChanges();

                JArray optionObj = catJson.options;
                if (optionObj != null)
                {
                    dynamic OptionJson = optionObj.ToList();
                    foreach (var item in OptionJson)
                    {
                        ////item.OptionGroup = null;
                        CategoryOptionGroup option = new CategoryOptionGroup();
                        option.Id = 0;
                        option.CategoryId = category.Id;
                        option.OptionGroupId = item.Id;
                        option.CompanyId = category.CompanyId;
                        option.CreatedDate = DateTime.Now;
                        option.ModifiedDate = DateTime.Now;
                        db.CategoryOptionGroups.Add(option);
                        db.SaveChanges();
                    }
                }

                //JArray variantObj = catJson.Variants;
                //if (variantObj != null)
                //{
                //    dynamic VariantJson = variantObj.ToList();
                //    foreach (var item in VariantJson)
                //    {
                //            item.VariantGroup = null;
                //            CategoryVariant variant = item.ToObject<CategoryVariant>();
                //            variant.Id = 0;
                //            variant.CategoryId = category.Id;
                //            variant.CompanyId = category.CompanyId;
                //            variant.CompanyId = 1;
                //            variant.VariantId = item.Id;
                //            variant.CreatedDate = DateTime.Now;
                //            variant.ModifiedDate = DateTime.Now;
                //            db.CategoryVariants.Add(variant);
                //            db.SaveChanges();
                //    }
                //}
                //JArray AddonObj = catJson.Addons;
                //if (AddonObj != null)
                //{
                //    dynamic AddonJson = AddonObj.ToList();
                //    foreach (var item in AddonJson)
                //    {
                //        item.AddonGroup = null;
                //            CategoryAddon Addon = item.ToObject<CategoryAddon>();
                //        Addon.Id = 0;
                //            Addon.CategoryId = category.Id;
                //            Addon.CompanyId = category.CompanyId;
                //            Addon.CompanyId = 1;
                //        Addon.AddonId = item.Id;
                //            Addon.CreatedDate = DateTime.Now;
                //            Addon.ModifiedDate = DateTime.Now;
                //            db.CategoryAddons.Add(Addon);
                //            db.SaveChanges();
                //        }
                //    }

                return Json(catJson);
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
        //[HttpPost("Updatedata")]
        //[EnableCors("AllowOrigin")]
        //public IActionResult Updatedata([FromForm]string objData)
        //{
        //    try
        //    {
        //        dynamic catJson = JsonConvert.DeserializeObject(objData);
        // dynamic catJsondel = JsonConvert.DeserializeObject(objData);
        //dynamic catJsondel1 = JsonConvert.DeserializeObject(objData);
        //if (catJson.Delete != null)
        //{
        //foreach (var item in catJson.Delete.Variants)
        //{
        //    if (item > 0)
        //    {
        //        int id = item.ToObject<int>();
        //        var variants = db.CategoryVariants.Find(id);
        //        db.CategoryVariants.Remove(variants);
        //    }
        //}
        //foreach (var item in catJsondel.Delete.Addons)
        //{
        //    if (item > 0)
        //    {
        //        int id = item.ToObject<int>();
        //        var addons = db.CategoryAddons.Find(id);
        //        db.CategoryAddons.Remove(addons);
        //    }
        //}
        //    foreach (var item in catJsondel1.Delete)
        //    {
        //        if (item > 0)
        //        {
        //            int id = item.ToObject<int>();
        //            var options = db.CategoryOptionGroups.Find(id);
        //            db.CategoryOptionGroups.Remove(options);

        //        }
        //    }
        //}
        //Category category = catJson.ToObject<Category>();
        //category.ModifiedDate = DateTime.Now;
        //db.Entry(category).State = EntityState.Modified;
        //db.SaveChanges();

        //JArray itemObj = catJson.Variants;
        //JArray AddonObj = catJson.Addons;
        //dynamic itemJson = itemObj.ToList();
        //dynamic itemJsondel = AddonObj.ToList();
        //JArray optObj = catJson.options;
        //dynamic itemJsondel1 = optObj.ToList();
        //foreach (var item in itemJson)
        //{
        //    if (item.Id == 0)
        //    {
        //        CategoryVariant categoryVariant = new CategoryVariant();
        //        categoryVariant.CompanyId = 1;
        //        categoryVariant.CategoryId = category.Id;
        //        categoryVariant.CreatedDate = DateTime.Now;
        //        categoryVariant.ModifiedDate = DateTime.Now;
        //        categoryVariant.VariantId = item.VariantId;
        //        categoryVariant.VariantGroupId = item.VariantGroup.Id;
        //        db.CategoryVariants.Add(categoryVariant);
        //        db.SaveChanges();
        //    }
        //    else
        //    {
        //        CategoryVariant categoryVariant = item.ToObject<CategoryVariant>();
        //        categoryVariant.CompanyId = 1;
        //        categoryVariant.CreatedDate = DateTime.Now;
        //        categoryVariant.ModifiedDate = DateTime.Now;
        //        categoryVariant.CategoryId = category.Id;
        //        db.Entry(categoryVariant).State = EntityState.Modified;
        //        db.SaveChanges();
        //    }
        //}

        //foreach (var item in itemJsondel)
        //{
        //    if (item.Id == 0)
        //    {
        //        CategoryAddon categoryAddon = new CategoryAddon();
        //        categoryAddon.CompanyId = 1;
        //        categoryAddon.CategoryId = category.Id;
        //        categoryAddon.CreatedDate = DateTime.Now;
        //        categoryAddon.ModifiedDate = DateTime.Now;
        //        categoryAddon.AddonId = item.AddonId;
        //        categoryAddon.AddonGroupId = item.AddonGroupId;
        //        db.CategoryAddons.Add(categoryAddon);
        //        db.SaveChanges();
        //    }
        //    else
        //    {
        //        CategoryAddon categoryAddon = item.ToObject<CategoryAddon>();
        //        categoryAddon.CompanyId = 1;
        //        categoryAddon.CreatedDate = DateTime.Now;
        //        categoryAddon.ModifiedDate = DateTime.Now;
        //        categoryAddon.CategoryId = category.Id;
        //        db.Entry(categoryAddon).State = EntityState.Modified;
        //        db.SaveChanges();
        //    }
        //}
        //        foreach (var item in itemJsondel1)
        //        {
        //            if (item.Id == 0)
        //            {
        //                CategoryOptionGroup categoryOption = new CategoryOptionGroup();
        //                categoryOption.CategoryId = category.Id;
        //                categoryOption.CompanyId = category.CompanyId;
        //                categoryOption.CreatedDate = DateTime.Now;
        //                categoryOption.ModifiedDate = DateTime.Now;
        //                categoryOption.OptionGroupId = item.Id;
        //                db.CategoryOptionGroups.Add(categoryOption);
        //                db.SaveChanges();
        //            }
        //            else
        //            {
        //                CategoryOptionGroup categoryOption = item.ToObject<CategoryOptionGroup>();
        //                categoryOption.CreatedDate = DateTime.Now;
        //                categoryOption.ModifiedDate = DateTime.Now;
        //                categoryOption.CategoryId = category.Id;
        //                categoryOption.CompanyId = category.CompanyId;
        //                categoryOption.OptionGroupId = item.Id;
        //                db.Entry(categoryOption).State = EntityState.Modified;
        //                db.SaveChanges();
        //            }
        //        }
        //        var error = new
        //        {
        //            status = 200,
        //            msg = "The Data Updated successfully"
        //        };

        //        return Json(error);
        //    }
        //    catch(Exception e)
        //    {
        //        var error = new
        //        {
        //            error = new Exception(e.Message, e.InnerException),
        //            ststus = 0,
        //            msg = "Something went wrong  Contact our service provider"
        //        };
        //        return Json(error);
        //    }
        //}

        [HttpPost("Update")]
        [EnableCors("AllowOrigin")]
        public IActionResult Update([FromForm]string objData)
        {
            try
            {
                dynamic catJson = JsonConvert.DeserializeObject(objData);
                Category category = catJson.ToObject<Category>();
                category.CreatedDate = db.Categories.Where(x => x.Id == category.Id).AsNoTracking().FirstOrDefault().CreatedDate;
                category.ModifiedDate = DateTime.Now;
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();

                JArray catoptobj = catJson.options;
                if (catoptobj != null && catoptobj.Count > 0)
                {
                    IEnumerable<dynamic> CatoptJson = catoptobj.ToList();
                    foreach (var item in CatoptJson)
                    {
                        int itemId = item.ToObject<int>();
                        var group = db.CategoryOptionGroups.Where(c => c.CategoryId == category.Id && c.OptionGroupId == itemId).FirstOrDefault();
                        if (group == null)
                        {
                            CategoryOptionGroup categoryOptionGroup = new CategoryOptionGroup();
                            categoryOptionGroup.OptionGroupId = itemId;
                            categoryOptionGroup.CategoryId = category.Id;
                            categoryOptionGroup.CompanyId = catJson.CompanyId;
                            categoryOptionGroup.CreatedDate = DateTime.Now;
                            categoryOptionGroup.ModifiedDate = DateTime.Now;
                            db.CategoryOptionGroups.Add(categoryOptionGroup);
                            db.SaveChanges();

                        }

                        var grp2 = db.CategoryOptionGroups.Where(x => x.CategoryId == category.Id).ToList();
                        foreach (var str in grp2)
                        {
                            var deloptn = CatoptJson.Where(s => s == str.OptionGroupId).FirstOrDefault();
                            if (deloptn == null)
                            {
                                var deloption = db.CategoryOptionGroups.Find(str.Id);
                                db.CategoryOptionGroups.Remove(deloption);
                                db.SaveChanges();
                            }
                        }
                    }
                }
                else
                {
                    var group = db.CategoryOptionGroups.Where(c => c.CategoryId == category.Id).ToList();
                    if (group != null)
                    {
                        foreach (var cog in group)
                        {
                            db.CategoryOptionGroups.Remove(cog);
                            db.SaveChanges();
                        }
                    }
                }
                var mesg = new
                {
                    status = 200,
                    msg = "option Successfully Added or Updated"
                };

                return Ok(mesg);
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

        [HttpGet("CatOption")]
        [EnableCors("AllowOrigin")]
        public IActionResult CatOption(int companyId)
        {
            try
            {
                var catr = new
                {
                    catOptgp = db.OptionGroups.Where(x => x.CompanyId == companyId).ToList()

                };
                return Json(catr);
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
        public IActionResult UpdateAct(int Id, bool active,int CompanyId)
        {
            try
            {
                Category category = db.Categories.Find(Id);
                category.isactive = active;
                category.CompanyId = CompanyId;
                //category.CreatedDate = db.Categories.Where(x => x.Id == Id).AsNoTracking().FirstOrDefault().CreatedDate;
                category.ModifiedDate = DateTime.Now;
                db.Entry(category).State = EntityState.Modified;
                   db.SaveChanges();
                var error = new
                {
                    status = "success",
                    data = new
                    {
                        value = 2
                    },
                    msg = "The category updated successfully"
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


    } }



   


      
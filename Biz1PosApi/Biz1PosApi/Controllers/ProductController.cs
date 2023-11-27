using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Biz1PosApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Biz1BookPOS.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private int var_status;
        private int var_value;
        private string var_msg;

        private POSDbContext db;
        public IConfiguration Configuration { get; }
        public static IHostingEnvironment _environment;
        private static TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        public ProductController(POSDbContext contextOptions, IConfiguration configuration, IHostingEnvironment environment)
        {
            db = contextOptions;
            Configuration = configuration;
            _environment = environment;
        }

        // GET: api/<controller>
        [HttpGet("OptionProducts")]
        public IActionResult OptionProducts(int companyid)
        {
            var prod = new
            {
                productOptionGroups = from pog in db.ProductOptionGroups
                                      join p in db.Products on pog.ProductId equals p.Id
                                      where pog.CompanyId == companyid
                                      select new { p.Id, pog.OptionGroupId, p.Description, p.Name },
            };
            return Json(prod);
        }
        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        [HttpGet("CompanyProducts")]
        public IActionResult CompanyProducts(int companyid, int storeid)
        {
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("dbo.CompanyProducts", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@companyid", companyid));
            cmd.Parameters.Add(new SqlParameter("@storeid", storeid));

            DataSet ds = new DataSet();
            SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
            sqlAdp.Fill(ds);

            string jstring = "";
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                jstring += ds.Tables[0].Rows[j].ItemArray[0].ToString();
            }

            return Json(JsonConvert.DeserializeObject(jstring));
        }
        [HttpGet("ProductOptionGroups")]
        public IActionResult ProductOptionGroups(int companyid)
        {
            var pogs = db.ProductOptionGroups.Where(p => p.CompanyId == companyid).Select(s => new { s.CompanyId, s.Id, s.OptionGroupId, s.OptionGroup.Name, s.ProductId }).ToList();
            return Json(pogs);
        }
        [HttpGet("OGOptions")]
        public IActionResult OGOptions(int ogid)
        {
            var options = db.Options.Where(o => o.OptionGroupId == ogid).Select(s => new { s.CompanyId, s.Id, s.OptionGroupId, s.OptionGroup.Name, s.ProductId }).ToList();
            return Json(options);
        }
        [HttpPost("temp")]
        public void temp([FromForm]string objData)
        {
            var products = db.Products.ToList();
            foreach (var product in products)
            {
                int compId = product.CompanyId;
                int productId = product.Id;
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.StoreProduct", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@compId", compId));
                cmd.Parameters.Add(new SqlParameter("@productId", productId));
                int success = cmd.ExecuteNonQuery();
                sqlCon.Close();
            }
        }
        [HttpPost("CopyProductByPcatId")]
        public IActionResult CopyProductByPcatId(int pcatid, int fromcompanyid, int tocompanyid)
        {
            int _pcatid = 0;
            Category pcategory = db.Categories.AsNoTracking().Where(x => x.Id == pcatid).FirstOrDefault();
            List<Category> childcats = db.Categories.AsNoTracking().Where(x => x.ParentCategoryId == pcatid).ToList();

            pcategory.Id = 0;
            Category _pcategory = ((dynamic)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(pcategory))).ToObject<Category>();
            _pcategory = (Category)pcategory.Clone();
            _pcategory.CompanyId = tocompanyid;
            db.Categories.Add(_pcategory);
            db.SaveChanges();
            _pcatid = _pcategory.Id;
            foreach(Category childcat in childcats)
            {
                List<Product> products = db.Products.AsNoTracking().Where(x => x.CategoryId == childcat.Id).ToList();
                Category _childcat = childcat;
                _childcat.Id = 0;
                _childcat.CompanyId = tocompanyid;
                _childcat.ParentCategoryId = _pcatid;
                db.Categories.Add(_childcat);
                db.SaveChanges();

                foreach(Product product in products)
                {
                    Product _product = product;
                    _product.Id = 0;
                    _product.CategoryId = _childcat.Id;
                    _product.CompanyId = tocompanyid;
                    db.Products.Add(_product);
                    db.SaveChanges();

                    SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                    sqlCon.Open();
                    SqlCommand cmd = new SqlCommand("dbo.StoreProduct", sqlCon);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@compId", _product.CompanyId));
                    cmd.Parameters.Add(new SqlParameter("@productId", _product.Id));
                    int success = cmd.ExecuteNonQuery();
                    sqlCon.Close();
                }
            }
            var response = new
            {
                pcat = _pcatid,
                childcats = db.Categories.Where(x => x.ParentCategoryId == _pcategory.Id).ToList(),
                prods = db.Products.Where(x => x.Category.ParentCategoryId == _pcategory.Id).ToList()
            };
            return Json(response);
        }
        [HttpPost("ImportProduct")]
        public IActionResult ImportProduct(int companyId, [FromForm]string productData)
        {
            string errorpd = "";
            using (var connection = new SqlConnection(Configuration.GetConnectionString("myconn")))
            //using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                connection.Open();
                var dbContextTransaction = connection.BeginTransaction();
                try
                {
                    dynamic productJson = JsonConvert.DeserializeObject(productData);
                    int OptionGroupId = 0;
                    int CategoryId = 0;
                    int? ParentCategoryId = 0;
                    float Price = 0;
                    int ProductId = 0;
                    int OptionId = 0;
                    int TaxGroupId = 0;
                    float Optionprice = 0;
                    List<TaxGroup> taxgroup = db.TaxGroups.Where(x => x.CompanyId == companyId).ToList();
                    foreach (var prod in productJson)
                    {
                        errorpd = JsonConvert.SerializeObject(prod);
                        bool prntAvail = false;
                        bool catAvail = false;
                        bool prdAvail = false;
                        bool vgpAvail = false;
                        bool vatAvail = false;
                        bool strAvail = false;
                        //////////parentcategory
                        var parentCategory = db.Categories.Where(x => x.ParentCategoryId == null && x.CompanyId == companyId).ToList();
                        foreach (var prntCat in parentCategory)
                        {
                            //string pcat1 = prod.ParentCategory.Replace(" ", "");
                            //string pcat2 = prntCat.Description.Replace(" ","");
                            string pcat1 = Regex.Replace(prod.ParentCategory.ToString(), @"\s", "");
                            string pcat2 = Regex.Replace(prntCat.Description.ToString(), @"\s", "");
                            if (string.Equals((string)pcat1, pcat2, StringComparison.OrdinalIgnoreCase))
                            {
                                prntAvail = true;
                                ParentCategoryId = prntCat.Id;
                                break;
                            }
                        }
                        if (!prntAvail)
                        {
                            var parentcat = new Category();
                            parentcat.CompanyId = companyId;
                            parentcat.Description = FirstCharToUpper(prod.ParentCategory.ToString());
                            parentcat.isactive = true;
                            db.Categories.Add(parentcat);
                            //db.SaveChanges();
                            ParentCategoryId = parentcat.Id;
                        }

                        /////////category
                        var categories = db.Categories.Where(x => x.ParentCategoryId != null && x.CompanyId == companyId).ToList();
                        foreach (var categ in categories)
                        {
                            string cat1 = Regex.Replace(prod.Category.ToString(), @"\s", "");
                            string cat2 = Regex.Replace(categ.Description.ToString(), @"\s", "");

                            if (string.Equals((string)cat1, cat2, StringComparison.OrdinalIgnoreCase) && categ.ParentCategoryId == ParentCategoryId)
                            {
                                catAvail = true;
                                CategoryId = categ.Id;
                                break;
                            }
                        }
                        if (!catAvail)
                        {
                            var cat = new Category();
                            cat.CompanyId = companyId;
                            cat.Description = FirstCharToUpper(prod.Category.ToString());
                            cat.ParentCategoryId = ParentCategoryId;
                            cat.FreeQtyPercentage = 0;
                            cat.MinimumQty = 0;
                            cat.CreatedDate = DateTime.Now;
                            cat.ModifiedDate = DateTime.Now;
                            cat.isactive = true;
                            db.Categories.Add(cat);
                            // db.SaveChanges();
                            CategoryId = cat.Id;
                        }

                        //////////product
                        ///
                        var product = db.Products.Where(x => x.CompanyId == companyId).ToList();
                        Product availableProd = null;
                        foreach (var prd in product)
                        {
                            string svar1 = Regex.Replace(prd.Description.ToString(), @"\s", "");
                            string svar2 = Regex.Replace(prod.Product.ToString(), @"\s", "");

                            if (string.Equals((string)svar1, svar2, StringComparison.OrdinalIgnoreCase))
                            {
                                prdAvail = true;
                                ProductId = prd.Id;
                                break;
                            }
                        }
                        if (!prdAvail)
                        {
                            var prodt = new Product();
                            prodt.CompanyId = companyId;
                            prodt.Description = FirstCharToUpper(prod.Product.ToString());
                            prodt.Name = FirstCharToUpper(prod.Product.ToString());
                            prodt.CategoryId = CategoryId;
                            prodt.UnitId = 2;
                            prodt.isactive = true;

                            foreach (var taxgrp in taxgroup)
                            {
                                string staxgrp1 = Regex.Replace(prod.TaxGroup.ToString(), @"\s", "");
                                string staxgrp2 = Regex.Replace(taxgrp.Description.ToString(), @"\s", "");

                                if (string.Equals((string)staxgrp1, staxgrp2, StringComparison.OrdinalIgnoreCase))
                                {
                                    prodt.TaxGroupId = taxgrp.Id;
                                    break;

                                }
                            }
                            prodt.ProductTypeId = prod.Type;
                            prodt.Price = prod.Price;
                            prodt.TakeawayPrice = prod.Price;
                            prodt.DeliveryPrice = prod.Price;
                            prodt.UPPrice = prod.Price;
                            prodt.CreatedDate = DateTime.Now;
                            prodt.ModifiedDate = DateTime.Now;
                            db.Products.Add(prodt);
                            ProductId = prodt.Id;
                            // db.SaveChanges();
                            var stre = db.Stores.Where(s => s.CompanyId == companyId).ToList();
                            foreach (var str in stre)
                            {
                                var strprod = db.StoreProducts.Where(x => x.ProductId == ProductId && x.StoreId == str.Id && x.CompanyId == companyId).FirstOrDefault();
                                if (strprod == null)
                                {
                                    var storeprodt = new StoreProduct();
                                    storeprodt.ProductId = prodt.Id;
                                    storeprodt.CompanyId = companyId;
                                    storeprodt.Price = prod.Price;
                                    storeprodt.TakeawayPrice = prod.Price;
                                    storeprodt.DeliveryPrice = prod.Price;
                                    storeprodt.StoreId = str.Id;
                                    storeprodt.CreatedDate = DateTime.Now;
                                    storeprodt.ModifiedDate = DateTime.Now;
                                    db.StoreProducts.Add(storeprodt);
                                }
                            }
                        }
                        else
                        {
                            Product prodt = db.Products.Find(ProductId);
                            //prodt.CompanyId = companyId;
                            prodt.Description = FirstCharToUpper(prod.Product.ToString());
                            prodt.Name = FirstCharToUpper(prod.Product.ToString());
                            prodt.CategoryId = CategoryId;
                            prodt.UnitId = 2;
                            prodt.isactive = true;

                            foreach (var taxgrp in taxgroup)
                            {
                                string staxgrp1 = Regex.Replace(prod.TaxGroup.ToString(), @"\s", "");
                                string staxgrp2 = Regex.Replace(taxgrp.Description.ToString(), @"\s", "");

                                if (string.Equals((string)staxgrp1, staxgrp2, StringComparison.OrdinalIgnoreCase))
                                {
                                    prodt.TaxGroupId = taxgrp.Id;
                                    break;

                                }
                            }
                            prodt.ProductTypeId = prod.Type;
                            prodt.Price = prod.Price;
                            prodt.TakeawayPrice = prod.Price;
                            prodt.DeliveryPrice = prod.Price;
                            prodt.UPPrice = prod.Price;
                            //prodt.CreatedDate = DateTime.Now;
                            prodt.ModifiedDate = DateTime.Now;
                            db.Entry(prodt).State = EntityState.Modified;
                            List<StoreProduct> storeProducts = db.StoreProducts.Where(x => x.ProductId == ProductId).ToList();
                            foreach(StoreProduct sp in storeProducts)
                            {
                                //sp.ProductId = prodt.Id;
                                //sp.CompanyId = companyId;
                                sp.Price = prod.Price;
                                sp.TakeawayPrice = prod.Price;
                                sp.DeliveryPrice = prod.Price;
                                //sp.StoreId = str.Id;
                                //sp.CreatedDate = DateTime.Now;
                                sp.ModifiedDate = DateTime.Now;
                                db.Entry(prodt).State = EntityState.Modified;
                            }
                        }
                        ///////Optiongroup
                        ///
                        var optiongrp = db.OptionGroups.Where(x => x.CompanyId == companyId).ToList();
                        if (prod.OptionGroup != null)
                        {
                            //foreach (var vrtgp in optiongrp)
                            //{
                            //    string svargp1 = Regex.Replace(prod.OptionGroup.ToString(), @"\s", "");
                            //    string svargp2 = Regex.Replace(vrtgp.Name.ToString(), @"\s", "");

                            //    if (string.Equals((string)svargp1, svargp2, StringComparison.OrdinalIgnoreCase))
                            //    {
                            //        vgpAvail = true;
                            //        OptionGroupId = vrtgp.Id;
                            //        ProductOptionGroup productoptiongroup = new ProductOptionGroup();
                            //        productoptiongroup.OptionGroupId = OptionGroupId;
                            //        productoptiongroup.ProductId = ProductId;
                            //        productoptiongroup.CompanyId = companyId;
                            //        productoptiongroup.CreatedDate = DateTime.Now;
                            //        db.ProductOptionGroups.Add(productoptiongroup);
                            //    }
                            //}


                            ///Tax Group
                            //if (!vgpAvail)
                            //{
                            //    var optionGroup = new OptionGroup();
                            //    optionGroup.CompanyId = companyId;
                            //    optionGroup.Name = FirstCharToUpper(prod.OptionGroup.ToString());
                            //    optionGroup.MaximumSelectable = 1;
                            //    optionGroup.MinimumSelectable = 1;
                            //    optionGroup.CreatedDate = DateTime.Now;
                            //    optionGroup.ModifiedDate = DateTime.Now;
                            //    db.OptionGroups.Add(optionGroup);
                            //    //db.SaveChanges();
                            //    OptionGroupId = optionGroup.Id;

                            //var prdopgrp = db.ProductOptionGroups.Where(x => x.OptionGroupId == OptionGroupId && x.CompanyId == companyId && x.ProductId == ProductId).FirstOrDefault();
                            //if (prdopgrp == null)
                            //{
                            //    var prdoptgp = new ProductOptionGroup();
                            //    prdoptgp.ProductId = ProductId;
                            //    prdoptgp.CompanyId = companyId;
                            //    prdoptgp.OptionGroupId = OptionGroupId;
                            //    prdoptgp.CreatedDate = DateTime.Now;
                            //    prdoptgp.ModifiedDate = DateTime.Now;
                            //    db.ProductOptionGroups.Add(prdoptgp);
                            //}
                            //}
                        }

                        //////////////////Option
                        //var vrnt = db.Options.Where(x => x.CompanyId == companyId).ToList();
                        //foreach (var vrt in vrnt)
                        //{
                        //    string svrt1 = Regex.Replace(prod.Option.ToString(), @"\s", "");
                        //    string svrt2 = Regex.Replace(vrt.Name.ToString(), @"\s", "");

                        //    if (string.Equals((string)svrt1, svrt2, StringComparison.OrdinalIgnoreCase))
                        //    {
                        //        vatAvail = true;
                        //        OptionId = vrt.Id;
                        //        break;
                        //    }
                        //}
                        //if (!vatAvail)
                        //{
                        //    var option = new Option();
                        //    option.CompanyId = companyId;
                        //    option.Name = FirstCharToUpper(prod.Option.ToString());
                        //    option.OptionGroupId = OptionGroupId;
                        //    db.Options.Add(option);
                        //    //db.SaveChanges();
                        //    OptionId = option.Id;

                        //    var prdoptn = db.ProductOptionGroups.Where(x => x.OptionGroupId == OptionGroupId && x.CompanyId == companyId && x.ProductId == ProductId).SingleOrDefault();
                        //    if (prdoptn == null)
                        //    {
                        //        var prdopt = new ProductOptionGroup();
                        //        prdopt.ProductId = ProductId;
                        //        prdopt.CompanyId = companyId;
                        //        prdopt.OptionGroupId = OptionGroupId;
                        //        //prdopt.Price = prod.OptionPrice;
                        //        //prdopt.TakeawayPrice = prod.OptionPrice;
                        //        //prdopt.DeliveryPrice = prod.OptionPrice;
                        //        prdopt.CreatedDate = DateTime.Now;
                        //        prdopt.ModifiedDate = DateTime.Now;
                        //        db.ProductOptionGroups.Add(prdopt);
                        //        }
                        //        //}

                        //        ////productvariant
                        //        //    var prdvrt = db.ProductOptions.Where(pv => pv.ProductId == ProductId && pv.OptionId == OptionId).FirstOrDefault();
                        //        //    if (prdvrt == null)
                        //        //    {
                        //        //        prdvrt = new ProductOption();
                        //        //        prdvrt.ProductId = ProductId;
                        //        //        prdvrt.OptionId = OptionId;
                        //        //        prdvrt.Price = prod.OptionPrice;
                        //        //        prdvrt.CreatedDate = DateTime.Now;
                        //        //        prdvrt.ModifiedDate = DateTime.Now;
                        //        //        prdvrt.CompanyId = 1;
                        //        //        db.ProductOptions.Add(prdvrt);
                        //        //        // db.SaveChanges();
                        //        //        //StoreProductOption

                        //        var stre = db.Stores.Where(s => s.CompanyId == companyId).ToList();
                        //        foreach (var str in stre)
                        //        {
                        //            var strprodvar = db.StoreProductOptions.Where(x => x.ProductId == ProductId && x.StoreId == str.Id && x.OptionId == OptionId).FirstOrDefault();
                        //            if (strprodvar == null)
                        //            {
                        //                var stdvrt = new StoreProductOption();
                        //                stdvrt.ProductId = ProductId;
                        //                stdvrt.OptionId = OptionId;
                        //                stdvrt.Price = prod.OptionPrice;
                        //                stdvrt.DeliveryPrice = prod.OptionPrice;
                        //                stdvrt.TakeawayPrice = prod.OptionPrice;
                        //                stdvrt.CreatedDate = DateTime.Now;
                        //                stdvrt.ModifiedDate = DateTime.Now;
                        //                stdvrt.CompanyId = companyId;
                        //                stdvrt.StoreId = str.Id;
                        //                db.StoreProductOptions.Add(stdvrt);
                        //            }
                        //        }
                        //    }

                        db.SaveChanges();

                    }
                    dbContextTransaction.Commit();
                    var error = new
                    {
                        status = 200,
                        msg = "Product imported successfully."
                    };
                    connection.Close();
                    return Json(error);
                }

                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    var error = new
                    {
                        error = new Exception(e.Message, e.InnerException),
                        status = 0,
                        msg = "Something went wrong  Contact our service provider",
                        errorpd = errorpd
                    };
                    connection.Close();
                    return Json(error);
                }


            }
        }
        public static string FirstCharToUpper(string value)
        {
            char[] array = value.ToCharArray();
            // Handle the first letter in the string.  
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }
        //    var vat = db.Variants.ToList();
        //    foreach (var vatp in productJson)
        //    {
        //        bool vatavailable = false;
        //      string  Description = vatp.Variant;
        //        foreach (var cat in vat)
        //        {
        //            if (string.Equals((string)Description, cat.Description, StringComparison.OrdinalIgnoreCase) && cat.VariantGroupId == VariantGroupId)
        //            {
        //                vatavailable = true;
        //                break;
        //            }
        //        }
        //        if (vatavailable == false)           
        //        {
        //            Variant variant = new Variant();
        //            variant.CompanyId = 1;
        //            variant.Description = Description;
        //            variant.VariantGroupId = VariantGroupId;
        //            db.Variants.Add(variant);
        //            db.SaveChanges();
        //            VariantId = variant.Id;
        //        }
        //    }
        //    var prdvrt = db.ProductVariants.Where(pv => pv.ProductId == ProductId && pv.VariantId == VariantId).FirstOrDefault();
        //    if (prdvrt == null)
        //    {
        //        prdvrt = new ProductVariant();
        //        prdvrt.ProductId = ProductId;
        //        prdvrt.VariantId = VariantId;
        //        prdvrt.Price = Variantprice;
        //        prdvrt.CreatedDate = DateTime.Now;
        //        prdvrt.ModifiedDate = DateTime.Now;
        //        prdvrt.CompanyId = 1;
        //        db.ProductVariants.Add(prdvrt);
        //        db.SaveChanges();
        //    }
        //    return Ok();
        //}
        // POST api/<controller>

        [HttpPost("AddProduct")]
        public IActionResult AddProduct([FromForm]string objData, IFormFile image)
        {
            dynamic orderJson = JsonConvert.DeserializeObject(objData);
            try
            {
                if (orderJson.KOTGroupId == 0)
                {
                    orderJson.KOTGroupId = null;
                }
                Product product = orderJson.ToObject<Product>();
                product.CreatedDate = DateTime.Now;
                product.ModifiedDate = DateTime.Now;
                // product.UPPrice = product.Price;
                product.Name = product.Name;
                product.Description = product.Description;
                if (image != null)
                    product.ImgUrl = ImageUpload(product.CompanyId, image);
                db.Products.Add(product);
                db.SaveChanges();
                int? akountzCompId = db.Companies.Find(product.CompanyId).AkountzCompanyId;
                if (akountzCompId.HasValue) AddProdInAkountz(product.Id, product.CompanyId, akountzCompId);
                JArray OptionGroupJson = orderJson.ProductOptionGroups;
                if (OptionGroupJson != null)
                {
                    dynamic optionGroups = OptionGroupJson.ToList();
                    foreach (var item in optionGroups)
                    {
                        int itemId = item.ToObject<int>();
                        if (item != 0)
                        {
                            ProductOptionGroup productOptionGroup = new ProductOptionGroup();
                            productOptionGroup.ProductId = product.Id;
                            productOptionGroup.OptionGroupId = item;
                            productOptionGroup.CompanyId = product.CompanyId;
                            productOptionGroup.CreatedDate = DateTime.Now;
                            productOptionGroup.ModifiedDate = DateTime.Now;
                            db.ProductOptionGroups.Add(productOptionGroup);
                            db.SaveChanges();
                            if (akountzCompId.HasValue) AddProdInAkountz(product.Id, product.CompanyId, akountzCompId);
                        }
                    }
                }

                //JArray optionJson = orderJson.ProductOptions;
                //if (optionJson != null)
                //{
                //    dynamic Options = optionJson.ToList();
                //    foreach (var item in Options)
                //    {
                //        if (item.Id == 0 && item.del == 0)
                //        {
                //            ProductOption productOption = item.ToObject<ProductOption>();
                //            productOption.ProductId = product.Id;
                //            productOption.CompanyId = product.CompanyId;
                //            productOption.CreatedDate = DateTime.Now;
                //            productOption.ModifiedDate = DateTime.Now;
                //            productOption.MappedProductId = null;
                //            db.ProductOptions.Add(productOption);
                //            db.SaveChanges();
                //        }
                //    }
                //}
                ////JArray AddonGrpObj = orderJson.AddOnGroup;
                //if (AddonGrpObj != null)
                //{
                //    dynamic AddonGrpJson = AddonGrpObj.ToList();
                //    foreach (var item in AddonGrpJson)
                //    {
                //        if (item.Id == 0 && item.del == 0)
                //        {
                //            ProductAddonGroup addonGroup = item.ToObject<ProductAddonGroup>();
                //            addonGroup.ProductId = product.Id;
                //            addonGroup.CompanyId = product.CompanyId;
                //            addonGroup.CreatedDate = DateTime.Now;
                //            addonGroup.ModifiedDate = DateTime.Now;
                //            db.ProductAddonGroups.Add(addonGroup);
                //            db.SaveChanges();
                //        }
                //    }
                //}
                //JArray AddonObj = orderJson.Addon;
                //if (AddonObj != null)
                //{
                //    dynamic AddonJson = AddonObj.ToList();
                //    foreach (var item in AddonJson)
                //    {
                //        if (item.Id == 0 && item.del == 0)
                //        {
                //            ProductAddOn Addon = item.ToObject<ProductAddOn>();
                //            Addon.ProductId = product.Id;
                //            Addon.CompanyId = product.CompanyId;
                //            Addon.CreatedDate = DateTime.Now;
                //            Addon.ModifiedDate = DateTime.Now;
                //            db.ProductAddOns.Add(Addon);
                //            db.SaveChanges();
                //        }
                //    }
                //}
                int compId = product.CompanyId;
                int productId = product.Id;
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.StoreProduct", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@compId", compId));
                cmd.Parameters.Add(new SqlParameter("@productId", productId));
                int success = cmd.ExecuteNonQuery();

                var_status = 200;
                var_value = product.Id;
                var_msg = "Product added Successfully";
                sqlCon.Close();
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
            var response = new
            {
                status = var_status,
                data = new
                {
                    value = var_value
                },
                msg = var_msg
            };
            return Json(response);

        }
        [HttpPost("AddProduct1")]
        public IActionResult AddProduct1()
        {
            try
            {
                var products = db.Products.Where(x => x.CategoryId == 336).ToList();
                foreach(var product in products)
                {
                    int compId = product.CompanyId;
                    int productId = product.Id;
                    SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                    sqlCon.Open();
                    SqlCommand cmd = new SqlCommand("dbo.StoreProduct", sqlCon);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@compId", compId));
                    cmd.Parameters.Add(new SqlParameter("@productId", productId));
                    int success = cmd.ExecuteNonQuery();

                    var_status = 200;
                    var_value = product.Id;
                    var_msg = "Product added Successfully";
                    sqlCon.Close();
                }
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
            var response = new
            {
                status = var_status,
                data = new
                {
                    value = var_value
                },
                msg = var_msg
            };
            return Json(response);

        }

        //[EnableCors("AllowOrigin")]
        //[HttpGet("GetProdById")]
        //public IActionResult GetProdById(int id, int compId)
        //{
        //    Product product = null
        //}
        [EnableCors("AllowOrigin")]
        [HttpGet("GetById")]
        public IActionResult GetById(int id, int compId)
        {
            try
            {
                List<int> groupids = db.MenuMappings.Where(x => x.companyid == compId).Select(s => s.groupid).ToList();
                var prod = new
                {
                    products = db.OldProducts.Where(x => groupids.Contains(x.groupid)).ToList(),
                    product = from p in db.Products
                              join mm in db.MenuMappings on p.groupid equals mm.groupid
                              where p.Id == id && mm.companyid == compId
                              select new { p.Id, Product = p.Name,p.Description, p.Price, p.BarCode, p.TakeawayPrice, p.DeliveryPrice, p.CategoryId, p.TaxGroupId, p.CompanyId, p.UnitId, p.ProductTypeId, p.KOTGroupId, p.ImgUrl, p.ProductCode, p.UPPrice, p.Recomended, p.SortOrder, p.isactive, p.minquantity, p.minblock, p.IsSaleProdGroup, p.MakingCost },
                    productOptionGroups = from pog in db.ProductOptionGroups
                                          join og in db.OptionGroups on pog.OptionGroupId equals og.Id
                                          where pog.ProductId == id && pog.CompanyId == compId
                                          select new { pog.Id, pog.OptionGroupId, pog.ProductId, og.Name },
                    optionGroups = db.OptionGroups.Where(x => x.CompanyId == compId).ToList(),
                    category = db.Categories.Where(o => o.CompanyId == compId).ToList(),
                    categoryOptionGroups = db.CategoryOptionGroups.Where(x => x.CompanyId == compId).ToList(),
                    taxGroup = db.TaxGroups.Where(o => o.CompanyId == compId).ToList(),
                    units = db.Units.ToList(),
                    productType = db.ProductTypes.ToList(),
                    Kot = db.KOTGroups.Where(k => k.CompanyId == compId).ToList(),
                    PredefinedQuantities = db.PredefinedQuantities.Where(x => x.ProductId == id).ToList(),
                    CakeQuantities = db.CakeQuantities.ToList()
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

        [HttpPost("Update")]
        [EnableCors("AllowOrigin")]
        public IActionResult Update([FromForm]string objData, IFormFile image)
        {
            try
            {
                dynamic orderJson = JsonConvert.DeserializeObject(objData);
                if (orderJson.KOTGroupId == 0)
                {
                    orderJson.KOTGroupId = null;
                }
                Product product = orderJson.ToObject<Product>();
                product.CreatedDate = db.Products.Where(x => x.Id == product.Id).AsNoTracking().FirstOrDefault().CreatedDate;
                product.ModifiedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time); ;
                product.Name = product.Name;
                product.Description = product.Description;
                product.IsQtyPredefined = db.PredefinedQuantities.Where(x => x.ProductId == product.Id).Any();
                if(image != null)
                product.ImgUrl = ImageUpload(product.CompanyId, image);
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();

                List<UPProduct> uPProducts = db.UPProducts.Where(x => x.ProductId == product.Id && x.CompanyId == product.CompanyId).ToList();
                foreach(var upproduct in uPProducts)
                {
                    upproduct.Price = product.UPPrice;
                    db.Entry(upproduct).State = EntityState.Modified;
                    db.SaveChanges();
                }

                JArray OptionGroupJson = orderJson.ProductOptionGroups;
                if (OptionGroupJson != null)
                {
                    IEnumerable<dynamic> optionGroups = OptionGroupJson.ToList();

                    foreach (var item in optionGroups)
                    {
                        int itemId = item.ToObject<int>();
                        var prdopgp = db.ProductOptionGroups.Where(x => x.ProductId == product.Id && x.OptionGroupId == itemId).FirstOrDefault();
                        if (prdopgp == null)
                        {
                            ProductOptionGroup productOptionGroup = new ProductOptionGroup();
                            productOptionGroup.ProductId = product.Id;
                            productOptionGroup.OptionGroupId = itemId;
                            productOptionGroup.CompanyId = product.CompanyId;
                            productOptionGroup.CreatedDate = DateTime.Now;
                            productOptionGroup.ModifiedDate = DateTime.Now;
                            db.ProductOptionGroups.Add(productOptionGroup);
                            db.SaveChanges();
                        }
                    }
                    var prdopgp1 = db.ProductOptionGroups.Where(x => x.ProductId == product.Id).ToList();
                    foreach (var opgp in prdopgp1)
                    {
                        var delopgp = optionGroups.Where(x => x == opgp.OptionGroupId).FirstOrDefault();
                        if (delopgp == null)
                        {
                            var delopgp1 = db.ProductOptionGroups.Find(opgp.Id);
                            db.ProductOptionGroups.Remove(delopgp1);
                            db.SaveChanges();
                        }
                    }

                }



                int compId = product.CompanyId;
                int productId = product.Id;
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.UpdateStoreProduct", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@compId", compId));
                cmd.Parameters.Add(new SqlParameter("@productId", productId));
                int success = cmd.ExecuteNonQuery();
                sqlCon.Close();
                var error = new
                {
                    status = 200,
                    msg = "Product updated successfully"
                };
                sqlCon.Close();
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
        public IActionResult Delete(int Id, int companyId)
        {
            try
            {
                //var variantGroups = db.ProductVariantGroups.Where(x => x.ProductId == Id).ToList();
                //var variants = db.ProductVariants.Where(x => x.ProductId == Id).ToList();
                //var addonGroups = db.ProductAddonGroups.Where(x => x.ProductId == Id).ToList();
                //var addons = db.ProductAddOns.Where(x => x.ProductId == Id).ToList();
                var optionGroups = db.ProductOptionGroups.Where(x => x.ProductId == Id && x.CompanyId == companyId).ToList();
                //var options = db.ProductOptions.Where(x => x.ProductId == Id && x.CompanyId == companyId).ToList();
                var storeProduct = db.StoreProducts.Where(x => x.ProductId == Id && x.CompanyId == companyId).ToList();
                //var storeProductVariants = db.StoreProductVariants.Where(x => x.ProductId == Id && x.CompanyId == 1).ToList();
                //var storeProductAddons = db.StoreProductAddons.Where(x => x.ProductId == Id && x.CompanyId == 1).ToList();
                //var storeProductOptions = db.StoreProductOptions.Where(x => x.ProductId == Id && x.CompanyId == companyId).ToList();
                //if (storeProductOptions != null)
                //{
                //    foreach (var item in storeProductOptions)
                //    {
                //        var strOps = db.StoreProductOptions.Find(item.Id);
                //        db.StoreProductOptions.Remove(strOps);
                //        db.SaveChanges();
                //    }
                //}
                //if (options != null)
                //{
                //    foreach (var item in options)
                //    {
                //        var ops = db.ProductOptions.Find(item.Id);
                //        db.ProductOptions.Remove(ops);
                //        db.SaveChanges();
                //    }
                //}
                if (optionGroups != null)
                {
                    foreach (var item in optionGroups)
                    {
                        var opgrps = db.ProductOptionGroups.Find(item.Id);
                        db.ProductOptionGroups.Remove(opgrps);
                        db.SaveChanges();
                    }
                }

                //if (variantGroups != null)
                //{
                //    foreach (var item in variantGroups)
                //    {
                //        var varGrp = db.ProductVariantGroups.Find(item.Id);
                //        db.ProductVariantGroups.Remove(varGrp);
                //    }
                //}
                //if (variants != null)
                //{
                //    foreach (var item in variants)
                //    {
                //        var var = db.ProductVariants.Find(item.Id);
                //        db.ProductVariants.Remove(var);
                //    }
                //}
                //if (addonGroups != null)
                //{
                //    foreach (var item in addonGroups)
                //    {
                //        var addGrp = db.ProductAddonGroups.Find(item.Id);
                //        db.ProductAddonGroups.Remove(addGrp);
                //    }
                //}
                //if (addons != null)
                //{
                //    foreach (var item in addons)
                //    {
                //        var add = db.ProductAddOns.Find(item.Id);
                //        db.ProductAddOns.Remove(add);
                //    }
                //}
                if (storeProduct != null)
                {
                    foreach (var item in storeProduct)
                    {
                        var stpd = db.StoreProducts.Find(item.Id);
                        db.StoreProducts.Remove(stpd);
                    }
                }
                //if (storeProductVariants != null)
                //{
                //    foreach (var item in storeProductVariants)
                //    {
                //        var stvr = db.StoreProductVariants.Find(item.Id);
                //        db.StoreProductVariants.Remove(stvr);
                //    }
                //}
                //if (storeProductAddons != null)
                //{
                //    foreach (var item in storeProductAddons)
                //    {
                //        var stadns = db.StoreProductAddons.Find(item.Id);
                //        db.StoreProductAddons.Remove(stadns);
                //    }
                //}
                var product = db.Products.Find(Id);
                db.Products.Remove(product);
                db.SaveChanges();
                var error = new
                {
                    status = "success",
                    data = new
                    {
                        value = 2
                    },
                    msg = "The Product and corresponding options deleted successfully"
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

        public string ImageUpload(int companyid, IFormFile file)
        {
            try
            {
                //long size = file.Sum(f => f.Length);

                // full path to file in temp location
                // var filePath = "https://biz1app.azurewebsites.net/Images/3";
                string subdir = "\\images\\" + companyid + "\\";
                if (!Directory.Exists(_environment.WebRootPath + subdir))
                {
                    Directory.CreateDirectory(_environment.WebRootPath + subdir);
                }
                using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + subdir + file.FileName))
                {
                    file.CopyTo(filestream);
                    filestream.Flush();
                    var response = new
                    {
                        url = "https://biz1pos.azurewebsites.net/images/"+companyid+"/"+ file.FileName
                    };
                    return response.url;
                }
            }
            catch (Exception ex)
            {
                var error = new
                {
                    status = 0,
                    msg = new Exception(ex.Message, ex.InnerException)
                };
                return "";
            }
        }
        [HttpPost("koppupathivetram")]
        [EnableCors("AllowOrigin")]
        public string FileUpload([FromForm]IFormFile file)
        {
            try
            {
                //long size = file.Sum(f => f.Length);

                // full path to file in temp location
                // var filePath = "https://biz1app.azurewebsites.net/Images/3";
                string subdir = "\\temp\\audio\\";
                if (!Directory.Exists(_environment.WebRootPath + subdir))
                {
                    Directory.CreateDirectory(_environment.WebRootPath + subdir);
                }
                using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + subdir + file.FileName))
                {
                    file.CopyTo(filestream);
                    filestream.Flush();
                    var response = new
                    {
                        url = "https://biz1ps.azurewebsites.net/temp/audio/" + file.FileName
                    };
                    return response.url;
                }
            }
            catch (Exception ex)
            {
                var error = new
                {
                    status = 0,
                    msg = new Exception(ex.Message, ex.InnerException)
                };
                return "";
            }
        }
        [HttpGet("UpdateAct")]
        [EnableCors("AllowOrigin")]
        public IActionResult UpdateAct(int Id, bool active)
        {
            try
            {
                var product = db.Products.Find(Id);
                product.isactive = active;
                db.Entry(product).State = EntityState.Modified;
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
        [HttpPost("UpdatePredefQtys")]
        [EnableCors("AllowOrigin")]
        public IActionResult UpdatePredefQtys(int productid, [FromBody]List<PredefinedQuantity> predefinedqtys)
        {
            try
            {
                Product product = db.Products.Find(productid);

                db.PredefinedQuantities.UpdateRange(predefinedqtys.Where(x => x.Id > 0 && !x.isdeleted));
                db.PredefinedQuantities.AddRange(predefinedqtys.Where(x => x.Id == 0));
                db.PredefinedQuantities.RemoveRange(predefinedqtys.Where(x => x.Id > 0 && x.isdeleted));
                db.SaveChanges();

                if (db.PredefinedQuantities.Where(x => x.ProductId == productid).Any())
                {
                    product.IsQtyPredefined = true;
                }
                else
                {
                    product.IsQtyPredefined = false;
                }

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();

                var error = new
                {
                    status = 200,
                    msg = "The data updated successfully",
                    predfqtys = db.PredefinedQuantities.Where(x => x.ProductId == productid).ToList()
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
        public IActionResult AddProdInAkountz(int productId, int posCompId, int? akountzCompId)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.POSProduct", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@productId", productId));
                cmd.Parameters.Add(new SqlParameter("@companyId", posCompId));
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                DataTable prodOptionTable = ds.Tables[0];

                using (var client = new System.Net.Http.HttpClient())
                {
                    //  client.BaseAddress = new Uri("https://fbakountz.azurewebsites.net/");
                    client.BaseAddress = new Uri("https://localhost:44370/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
                    client.DefaultRequestHeaders.Add("Keep-Alive", "3600");
                    string url = "api/POS/AddPosProd";
                    var data = new
                    {
                        akountzCompId = akountzCompId,
                        prodOptionTable = prodOptionTable,
                        posCompId = posCompId
                    };

                    var myContent = JsonConvert.SerializeObject(data);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response = client.PostAsync(url, byteContent);
                    response.Wait();
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var responseData = response.Result.Content.ReadAsStringAsync();

                        dynamic jsonObj = JsonConvert.DeserializeObject(responseData.Result);
                    }
                }

                return Ok("success");
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
    }
}
using Biz1BookPOS.Models;
using Biz1PosApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Cors;

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    public class FbEcomController : Controller
    {

        private POSDbContext db;
        private int var_status;
        private string var_msg;
        public IConfiguration Configuration { get; }
        public FbEcomController(POSDbContext contextOptions, IConfiguration configuration)
        {
            db = contextOptions;
            Configuration = configuration;
        }

        [HttpGet("GetProducts")]
        public IActionResult GetProducts(int CompanyId)
        {
            try
            {
                var products = from p in db.Products
                               join c in db.Categories on p.CategoryId equals c.Id
                               where c.isecommercecat == true && p.isactive == true && p.CompanyId == CompanyId 
                               select new {ProdId = p.Id, p.ImgUrl, p.Name, Description1 = p.Description};
                return Ok(products);
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

        [HttpGet("GetTopProducts")]
        public IActionResult GetTopProducts()
        {
            try
            {
                var topProd = from p in db.Products
                              join ep in db.EComProducts on p.Id equals ep.ProductId
                              where ep.IsTopProduct == true && ep.CompanyId == 3
                              select new { ProdId = p.Id, p.ImgUrl, p.Name, Description1 = p.Description };
                return Ok(topProd);
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

        [HttpGet("GetSplProducts")]
        public IActionResult GetSplProducts()
        {
            try
            {
                var topProd = from p in db.Products
                              join ep in db.EComProducts on p.Id equals ep.ProductId
                              where ep.IsSplProduct == true && ep.CompanyId == 3
                              select new { ProdId = p.Id, p.ImgUrl, p.Name, Description1 = p.Description };
                return Ok(topProd);
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

        [HttpGet("GetEcomProducts")]
        public IActionResult GetEcomProducts(int CompanyId)
        {
            try
            {
                var pre = db.PredefinedQuantities.Where(x => x.CompanyId == CompanyId);
                return Ok(pre);
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

        [HttpGet("GetQtyByProdId")]
        public IActionResult GetQtyByProdId(int CompanyId, int ProdId)
        {
            try
            {
                var prod = db.PredefinedQuantities.Where(x => x.CompanyId == CompanyId && x.ProductId == ProdId);
                return Ok(prod);
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


        [HttpGet("GetCategories")]
        public IActionResult GetCategories(int CompanyId)
        {
            try
            {
                var categories = db.Categories.Where(x => x.isecommercecat == true && x.CompanyId == CompanyId);
                return Ok(categories);
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

        [HttpGet("GetCategoryByPrd")]
        public IActionResult GetCategoryByPrd(int? CatId, int CompanyId)
        {
            try
            {
                if(CatId != 0)
                {
                    var ProdCat = (from p in db.Products
                                  where p.CategoryId == CatId && p.CompanyId == CompanyId
                                  select new { p.CategoryId, ProdId = p.Id, p.ImgUrl, p.Name, Description1 = p.Description });
                    return Ok(ProdCat);
                }
                else
                {
                    var topProd = from p in db.Products
                                  join ep in db.EComProducts on p.Id equals ep.ProductId
                                  where ep.IsSplProduct == true && ep.IsTopProduct == true && ep.CompanyId == 3
                                  select new { ProdId = p.Id, p.ImgUrl, p.Name, Description1 = p.Description };
                    return Ok(topProd);
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

        [HttpGet("GetById")]
        public IActionResult GetById(int ProdId)
        {
            try
            {
                var ProdCat = (from p in db.Products
                               where p.Id == ProdId 
                               select new { p.CategoryId, ProdId = p.Id, p.ImgUrl, p.Name, Description1 = p.Description, p.Price });
                return Ok(ProdCat);
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



        //New User
        [HttpPost("Register")]
        public IActionResult Register([FromBody] Accounts data)
        {

            string enpass = EnryptString(data.Password);
            string depass = DecryptString(enpass);
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("dbo.EcomRegistration", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@Name", data.Name));
            cmd.Parameters.Add(new SqlParameter("@PhoneNo", data.PhoneNo));
            cmd.Parameters.Add(new SqlParameter("@EmailId", data.Email));
            cmd.Parameters.Add(new SqlParameter("@Password", enpass));

            DataSet ds = new DataSet();
            SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
            sqlAdp.Fill(ds);

            DataTable table = ds.Tables[0];

            DataRow row = table.Select().FirstOrDefault();

            int result = Int32.Parse(row["Success"].ToString());

            if (result == 1)
            {
                var_status = 0;
                var_msg = "The Email alredy exists";
            }
            else if (result == 2)
            {
                var_status = 0;
                var_msg = "The PhoneNo alredy exists";
            }
            else
            {
                var_status = 200;
                var_msg = "Successfully Registered";
            }

            var returnArray = new
            {
                status = var_status,
                data = new
                {

                },
                msg = var_msg
            };
            sqlCon.Close();
            return Json(returnArray);
        }
        public string EnryptString(string strEncrypted)
        {
            try
            {
                byte[] b = System.Text.Encoding.ASCII.GetBytes(strEncrypted);
                string encrypted = Convert.ToBase64String(b);
                return encrypted;
            }
            catch (Exception e)
            {
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 0,
                    msg = "Something went wrong  Contact our service provider"
                };
                return ("");
            }
        }

        public string DecryptString(string encrString)
        {
            byte[] b;
            string decrypted;
            try
            {
                b = Convert.FromBase64String(encrString);
                decrypted = System.Text.ASCIIEncoding.ASCII.GetString(b);
            }
            catch (FormatException fe)
            {
                decrypted = "";
            }
            return decrypted;
        }

        [HttpPost("LoginCheck")]
        [EnableCors("AllowOrigin")]
        public IActionResult logincheck([FromBody] Customer data)
        {

            string enpass = EnryptString(data.Password);
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();

            SqlCommand cmd = new SqlCommand("dbo.EcomLogin", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Email", data.Email));
            cmd.Parameters.Add(new SqlParameter("@PhoneNo", data.PhoneNo));
            cmd.Parameters.Add(new SqlParameter("@Password", enpass));

            DataSet ds = new DataSet();

            if (db.Customers.Where(x => x.Email == data.Email).Any() || db.Customers.Where(x => x.PhoneNo == data.PhoneNo).Any())
            {
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                var response = new
                {
                    Status = ds.Tables[0],
                    UserId = ds.Tables[1],

                };
                return Ok(response);
            }
            else
            {
                var response = new
                {
                    status = 500,
                    msg = "Email_Id or Phone_No doesn't exist",
                };
                return Ok(response);
            }

        }


        // GET: FbEcomController
        public ActionResult Index()
        {
            return View();
        }

        // GET: FbEcomController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FbEcomController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FbEcomController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FbEcomController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FbEcomController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FbEcomController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FbEcomController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

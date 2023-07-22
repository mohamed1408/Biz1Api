using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using Biz1BookPOS.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Biz1BookPOS.Controllers
{

    [Route("api/[controller]")]
    public class Kb2Controller : Controller
    {
        private int var_status;
        private int var_value;
        private string var_msg;

        private POSDbContext db;
        public IConfiguration Configuration { get; }
        private static TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        public Kb2Controller(POSDbContext contextOptions, IConfiguration configuration)
        {
            db = contextOptions;
            Configuration = configuration;
        }

        [HttpGet("GetChefUsers")]
        public IActionResult GetChefUsers(int CompanyId)
        {
            try
            {
                var data = new
                {
                    chef = db.Users.Where(x => x.CompanyId == CompanyId && x.RoleId == 4).ToList(),
                    waiter = db.Users.Where(x => x.CompanyId == CompanyId && x.RoleId == 5).ToList(),                    //userstores = db.UserStores.Include(x => x.Store).ToList()
                }; return Ok(data);
            }
            catch (Exception e)
            {
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 0,
                    msg = "Something went wrong  Contact our service provider"
                }; return Json(error);
            }
        }

        [HttpPost("SaveKb2ChefUser")]
        public IActionResult SaveKb2ChefUser([FromBody] dynamic objData)
        {
            try
            {
                dynamic jsonObj = objData;

                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj);
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.Kb2Kot", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@orderJson", jsonString));
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                var response = new
                {
                    status = 200,
                    message = " Item Added Successfully",
                    OrdId = ds.Tables[0]
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = 0,
                    msg = "Something Went Wrong",
                    error = new Exception(ex.Message, ex.InnerException)
                };
                return Ok(response);
            }
        }

        [HttpGet("GetCheforderlist")]
        public IActionResult GetCheforderlist(int companyId, DateTime OrdDate)
        {
            try
            {
                var dateAndTime = DateTime.Now;
                var date = dateAndTime.Date;

                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.Kb2GetChefProducts", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@companyId", companyId));
                cmd.Parameters.Add(new SqlParameter("@ordDate", date));
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                var response = new
                {
                    status = 200,
                    order = ds.Tables[0]
                    //msg = "The data updated successfully"
                };
                return Json(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = 0,
                    msg = "Something Went Wrong",
                    error = new Exception(ex.Message, ex.InnerException)
                };
                return Ok(response);
            }
        }
        [HttpGet("GetKotIdbyKotNo")]
        public IActionResult GetKotIdbyKotNo(string kotno, int companyId)
        {
            try
            {
                var dateAndTime = DateTime.Now;
                var date = dateAndTime.Date;

                var kot = from o in db.Orders
                          join k in db.KOTs on o.Id equals k.OrderId
                          where o.OrderedDate == date && k.KOTNo == kotno && o.CompanyId == companyId && o.OrderTypeId == 8
                          select new { k.Id };
                return Ok(kot);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = 0,
                    msg = "Something Went Wrong",
                    error = new Exception(ex.Message, ex.InnerException)
                };
                return Ok(response);

            }
        }
        [HttpGet("getprodbykotid")]
        public IActionResult getprodbykotid(string kotno)
        {
            try
            {
                var dateAndTime = DateTime.Now;
                var date = dateAndTime.Date;

                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.Kb2GetprodByKot", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@kotno", kotno));
                cmd.Parameters.Add(new SqlParameter("@ordDate", date));
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                var response = new
                {
                    status = 200,
                    order = ds.Tables[0]
                    //msg = "The data updated successfully"
                };
                return Json(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = 0,
                    msg = "Something Went Wrong",
                    error = new Exception(ex.Message, ex.InnerException)
                };
                return Ok(response);
            }



        }
        [HttpPost("UpdatekotStore")]
        [EnableCors("AllowOrigin")]
        public IActionResult UpdatekotStore([FromBody] dynamic objData)
        {
            try
            {
                dynamic jsonObj = objData;
                int kotid = jsonObj.Id;
                int Ordid = jsonObj.OrdId;

                var kots = db.KOTs.Find(kotid);
                kots.StoreId = jsonObj.StoreId;
                db.Entry(kots).State = EntityState.Modified;
                db.SaveChanges();

                var order = db.Orders.Find(Ordid);
                order.OrderStatusId = 0;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();

                var response = new
                {
                    status = 200,
                    msg = "The data updated successfully"
                };
                return Json(response);
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

        [HttpGet("ReceviedProd")]
        [EnableCors("AllowOrigin")]
        public IActionResult ReceviedProd(int CompanyId, int StoreId)
        {
            try
            {
                var dateAndTime = DateTime.Now;
                var date = dateAndTime.Date;

                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.Kb2ReceProd", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@companyId", CompanyId));
                cmd.Parameters.Add(new SqlParameter("@storeId", StoreId));
                cmd.Parameters.Add(new SqlParameter("@ordDate", date));
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                var response = new
                {
                    status = 200,
                    order = ds.Tables[0]
                    //msg = "The data updated successfully"
                };
                return Json(response);
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
        [HttpPost("UpdateWaiter")]
        [EnableCors("AllowOrigin")]
        public IActionResult UpdateWaiter([FromBody] dynamic objData)
        {
            try
            {
                dynamic jsonObj = objData;
                int kotid = jsonObj.Kotid;
                int Ordid = jsonObj.OrdId;

                var kots = db.KOTs.Find(kotid);
                kots.refid = jsonObj.WaiterId;
                db.Entry(kots).State = EntityState.Modified;
                db.SaveChanges();

                var order = db.Orders.Find(Ordid);
                order.OrderStatusId = 1;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();

                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.Kb2AssignMaster", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ordId", Ordid));
                cmd.Parameters.Add(new SqlParameter("@kotId", kotid));
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                var response = new
                {
                    status = 200,
                    msg = "The data updated successfully"
                };
                return Json(response);
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
        [HttpGet("Masterprod")]
        [EnableCors("AllowOrigin")]
        public IActionResult Masterprod(int OrdId, int kotid)
        {
            try
            {
                var chotaqty = from oi in db.OrderItems
                               where oi.OrderId == OrdId && oi.KOTId == kotid && oi.ProductId == 21278
                               select new { oi.ProductId, oi.Quantity, oi.Id, oi.OrderId };
                var badaaqty = from oi in db.OrderItems
                               where oi.OrderId == OrdId && oi.KOTId == kotid && oi.ProductId == 21283
                               select new { oi.ProductId, oi.Quantity, oi.Id, oi.OrderId };
                var plainqty = from oi in db.OrderItems
                               where oi.OrderId == OrdId && oi.KOTId == kotid && oi.ProductId == 12838
                               select new { oi.ProductId, oi.Quantity, oi.Id, oi.OrderId };

                var meatqty = from oi in db.OrderItems
                              where oi.OrderId == OrdId && oi.KOTId == kotid && oi.ProductId == 21393
                              select new { oi.ProductId, oi.Quantity, oi.Id, oi.OrderId };


                var data = new
                {
                    chotaqty = chotaqty,
                    badaaqty = badaaqty,
                    plainqtyy = plainqty,
                    meatqty = meatqty
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
        [HttpPost("Updateoptqty")]
        [EnableCors("AllowOrigin")]
        public IActionResult Updateoptqty([FromBody] dynamic objData)
        {
            try
            {
                dynamic jsonObj = objData[0];

                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj);
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.Kb2UpdateQty", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@orderjson", jsonString));

                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                var response = new
                {
                    status = 200,
                    msg = "The data updated successfully"
                };
                return Json(response);
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

        [HttpGet("CompletedStsUpd")]
        public IActionResult CompletedStsUpd(int OrdId)
        {
            var order = db.Orders.Find(OrdId);
            order.OrderStatusId = 5;
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(OrdId);
        }

        //Report
        [HttpGet("ProdRatio")]
        public IActionResult ProdRatio(int companyId, int StoreId, string frm, string to)
        {
            try
            {

                var dateAndTime = DateTime.Now;
                var date = dateAndTime.Date;

                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.Kb2Report", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@companyId", companyId));
                cmd.Parameters.Add(new SqlParameter("@storeId", StoreId));
                cmd.Parameters.Add(new SqlParameter("@frmDate", frm));
                cmd.Parameters.Add(new SqlParameter("@toDate", to));
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                var response = new
                {
                    status = 200,
                    Report = ds.Tables[0]
                    //msg = "The data updated successfully"
                };
                return Json(response);
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


        // GET: Kb2Controller
        public ActionResult Index()
        {
            return View();
        }

        // GET: Kb2Controller/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Kb2Controller/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Kb2Controller/Create
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

        // GET: Kb2Controller/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Kb2Controller/Edit/5
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

        // GET: Kb2Controller/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Kb2Controller/Delete/5
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

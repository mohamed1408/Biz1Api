using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    public class TranstypeController : Controller
    {
        private POSDbContext db;
        public TranstypeController(POSDbContext contextOptions)
        {
            db = contextOptions;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("Get")]
        public IActionResult Get()
        {
            try
            {
                var prod = new
                {
                    transtype = db.Transactions.Where(x => x.TranstypeId == 2).ToList(),
                    payment = db.PaymentTypes.ToList(),
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

        [HttpPost("SaveData")]
        [EnableCors("AllowOrigin")]
        public IActionResult SaveData([FromForm]string data)
        {
            try
            {
                dynamic add = JsonConvert.DeserializeObject(data);
                Transaction transaction = add.ToObject<Transaction>();
                //transaction.CompanyId = 1;
                //transaction.StoreId = 2;
                transaction.TranstypeId = 3;
                db.Transactions.Add(transaction);
                db.SaveChanges();
                var error = new
                {
                    status = "success",
                    data = new
                    {
                        value = 2
                    },
                    msg = "The Expenses saved successfully"
                };


                return Json(error);
            }
            catch(Exception e)
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
        [HttpGet("GetPay")]
        public IActionResult GetPay()
        {
            try
            {
                var payment = db.PaymentTypes.ToList();
                return Ok(payment);
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
                var transtype = db.Transactions.Find(Id);
                return Ok(transtype);
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
        public IActionResult Update([FromForm]string data)
        {
            try
            {
                dynamic expense = JsonConvert.DeserializeObject(data);
                Transaction transaction = expense.ToObject<Transaction>();
                //transaction.CompanyId = 1;
                //transaction.StoreId = 2;
                transaction.TranstypeId = 2;
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                var error = new
                {
                    status = "success",
                    data = new
                    {
                        value = 2
                    },
                    msg = "The Expenses updated successfully"
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
                var transaction = db.Transactions.Find(Id);
                db.Transactions.Remove(transaction);
                db.SaveChanges();
                var error = new
                {
                    status = 200,
                    msg = "The Expenses deleted successfully"
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

    }
}
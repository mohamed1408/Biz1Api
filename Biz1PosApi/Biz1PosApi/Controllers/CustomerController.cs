using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Biz1BookPOS.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private POSDbContext db;
        public IConfiguration Configuration { get; }
        public CustomerController(POSDbContext contextOptions, IConfiguration configuration)
        {
            db = contextOptions;
            Configuration = configuration;
        }
        // GET: api/<controller>
        [HttpGet("GetIndex")]
        [EnableCors("AllowOrigin")]
        public IActionResult GetIndex(int CompanyId)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.CustomerCount", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CompId", CompanyId));
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                DataTable table = ds.Tables[0];

                var data = new
                {
                    Order = ds.Tables[0]
                };
                sqlCon.Close();
                return Ok(table);
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

        [HttpPost("AddCustomer")]
        [EnableCors("AllowOrigin")]
        public IActionResult AddCustomer([FromForm]string data)
        {
            try
            {
                dynamic cust = JsonConvert.DeserializeObject(data);
                Customer customer = cust.ToObject<Customer>();
                db.Customers.Add(customer);
                db.SaveChanges();
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

        [HttpPost("Update")]
        public IActionResult Update([FromForm]string data)
        {
            try
            {
                dynamic cust = JsonConvert.DeserializeObject(data);
                Customer customer = cust.ToObject<Customer>();
                customer.CreatedDate = db.Customers.Where(x => x.Id == customer.Id).AsNoTracking().FirstOrDefault().CreatedDate;
                customer.ModifiedDate = DateTime.Now;
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                var error = new
                {
                    status = 200,
                    msg = "The Customer updated successfully"
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


        // PUT api/<controller>/5
        [HttpGet("Edit")]
        [EnableCors("AllowOrigin")]
        public IActionResult Edit(int Id)     
        {
            try
            {
                var customer = db.Customers.Find(Id);
                return Ok(customer);
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

        [HttpGet("GetCustomerByPhone")]
        [EnableCors("AllowOrigin")]
        public IActionResult GetCustomerByPhone(string Phone)
        {
            var customer = db.Customers.Where(x => x.PhoneNo == Phone);
            return Ok(customer);
        }

        [HttpGet("Delete")]
        [EnableCors("AllowOrigin")]
        public IActionResult Delete(int Id)
        {
            var orders = db.Orders.Where(x => x.CustomerId == Id).ToList();
            if (orders.Count == 0)
            {
                var cust = db.Customers.Find(Id);
                db.Customers.Remove(cust);
                db.SaveChanges();
                var error = new
                {
                    status = 200,
                    msg = "The Customer deleted successfully"
                };
                return Json(error);
            }
            else
            {
                var error = new
                {
                    status = 0,
                    msg = "Something went wrong  Contact our service provider"
                };
                return Json(error);
            }
        }
    }
}

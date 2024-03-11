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
        //Save Cus Data from BizDom to FbAdmin by HyperTech -- START


        [HttpGet("GetCustomerByPhone")]
        [EnableCors("AllowOrigin")]
        public IActionResult GetCustomerByPhone(string Phone, int companyid, int storeid)
        {
            //var customer = db.Customers.Where(x => x.PhoneNo == Phone && x.CompanyId == companyid).FirstOrDefault();
            //customer.Addresses = db.CustomerAddresses.Where(x => x.CustomerId == customer.Id).Include(x => x.Customer).ToList();
            //return Ok(customer);

            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("erpconn"));
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("dbo.getcustomerbyphonenum", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@phonenum", Phone));
            cmd.Parameters.Add(new SqlParameter("@companyid", companyid));
            cmd.Parameters.Add(new SqlParameter("@storeid", storeid));

            DataSet ds = new DataSet();
            SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
            sqlAdp.Fill(ds);
            //db.Database.ExecuteSqlCommand(cmd.ToString());
            DataTable table = ds.Tables[0];
            var time2 = DateTime.Now;
            string jsonString = "";
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                jsonString += ds.Tables[0].Rows[j].ItemArray[0].ToString();
            }
            return Json(JsonConvert.DeserializeObject(jsonString));
        }

        [HttpPost("SavePosCustomer")]
        public IActionResult SavePosCustomer([FromBody] dynamic orderjson)
        {
            try
            {
                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(orderjson);
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("erpconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.pos_customer", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@orderjson", jsonString));
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                var response = new
                {
                    CustomerId = ds.Tables[0],
                    status = 200,
                    message = "Customer Saved Successfully",

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


        //Save Cus Data from BizDom to FbAdmin by HyperTech -- END


        //[HttpGet("GetCustomerList")]
        //[EnableCors("AllowOrigin")]
        //public IActionResult GetCustomerList(int companyid, DateTime frmdate, DateTime todate, int ordertype, float billamt)
        //{
        //    try
        //    {
        //        //SqlConnection sqlCon = new SqlConnection("server=(LocalDb)\\MSSQLLocalDB; database=Biz1POS;Trusted_Connection=True;");
        //        SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
        //        sqlCon.Open();
        //        SqlCommand cmd = new SqlCommand("dbo.customerdata", sqlCon);
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        cmd.Parameters.Add(new SqlParameter("@fromdate", frmdate));
        //        cmd.Parameters.Add(new SqlParameter("@todate", todate));
        //        cmd.Parameters.Add(new SqlParameter("@companyId", companyid));
        //        cmd.Parameters.Add(new SqlParameter("@orderTypeid", ordertype));
        //        cmd.Parameters.Add(new SqlParameter("@billamt", billamt));
        //        DataSet ds = new DataSet();
        //        SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
        //        sqlAdp.Fill(ds);
        //        DataTable table = ds.Tables[0];

        //        var data = new
        //        {
        //            Order = ds.Tables[0]


        //        };
        //        sqlCon.Close();
        //        return Ok(data);
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

        //HYPER

        [HttpGet("GetCustomerList")]
        [EnableCors("AllowOrigin")]
        public IActionResult GetCustomerList(int companyid, DateTime frmdate, DateTime todate, int ordertype, double billamt, string ProdName)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();

                SqlCommand cmd = new SqlCommand("customerdata", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@companyId", companyid));
                cmd.Parameters.Add(new SqlParameter("@orderTypeid", ordertype));
                cmd.Parameters.Add(new SqlParameter("@fromdate", frmdate));
                cmd.Parameters.Add(new SqlParameter("@todate", todate));
                cmd.Parameters.Add(new SqlParameter("@billamt", billamt));
                cmd.Parameters.Add(new SqlParameter("@ProdName", ProdName));

                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);

                DataTable BizData = ds.Tables[0];

                var customerIds = new List<int>();
                foreach (DataRow row in BizData.Rows)
                {
                    customerIds.Add((int)row["CusId"]);
                }

                SqlConnection sqlCon2 = new SqlConnection(Configuration.GetConnectionString("erpconn"));
                sqlCon2.Open();

                string SaveCusIds = string.Join(",", customerIds);

                //SqlCommand cmd2 = new SqlCommand($"SELECT Id AS CusId, Name AS CusName, PhoneNo AS CusPhone FROM Customers WHERE Id IN ({SaveCusIds})", sqlCon2);
                SqlCommand cmd2 = new SqlCommand($"SELECT Id AS CusId, Name AS CusName, PhoneNo AS CusPhone FROM Customers WHERE Id IN ({SaveCusIds}) AND LEN(ISNULL(PhoneNo, '')) >= 10", sqlCon2);
                cmd2.CommandType = CommandType.Text;

                DataSet ds2 = new DataSet();
                SqlDataAdapter sqlAdp2 = new SqlDataAdapter(cmd2);
                sqlAdp2.Fill(ds2);

                DataTable ERPData = ds2.Tables[0];

                var result = new List<object>();
                foreach (DataRow POSDetails in BizData.Rows)
                {
                    foreach (DataRow Customer in ERPData.Rows)
                    {
                        if (!(POSDetails["CusId"] is DBNull) && !(Customer["CusId"] is DBNull) &&
                            (int)POSDetails["CusId"] == (int)Customer["CusId"])
                        {
                            result.Add(new
                            {
                                Store = POSDetails["Store"] as string,
                                OrderTypeId = POSDetails["OrderTypeId"] as int? ?? 0,
                                OrderedDate = POSDetails["OrderedDate"] as DateTime? ?? DateTime.MinValue,
                                DeliveryDate = POSDetails["DeliveryDate"] as DateTime? ?? DateTime.MinValue,
                                BillAmount = POSDetails["BillAmount"] as double? ?? 0.0,
                                CusId = POSDetails["CusId"] as int? ?? 0,
                                CusName = Customer["CusName"] as string,
                                CusPhone = Customer["CusPhone"] as string,
                                Prodname = POSDetails["Prodname"] as string,
                                Invoice = POSDetails["Invoice"] as string
                            });
                            break;
                        }
                    }
                }

                sqlCon.Close();
                sqlCon2.Close();
               // _notificationHubContext.Clients.All.SendAsync("EcomOrder", result);

                return Ok(result);
            }
            catch (Exception e)
            {
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 0,
                    msg = "Something went wrong. Contact our service provider"
                };
                return Json(error);
            }
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

        [HttpGet("GetCusDetailsUP")]
        public IActionResult GetCusDetailsUP(string cusname, string cusadd, string cuscity, string cusphone, int cuiId)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("erpconn")); sqlCon.Open();
                SqlCommand cmd = new SqlCommand(@"UPDATE Customers SET Name = @name, Address = @address, City = @city, PhoneNo = @phone WHERE Id = @cuiId", sqlCon);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@name", cusname));
                cmd.Parameters.Add(new SqlParameter("@address", cusadd));
                cmd.Parameters.Add(new SqlParameter("@city", cuscity));
                cmd.Parameters.Add(new SqlParameter("@phone", cusphone));
                cmd.Parameters.Add(new SqlParameter("@cuiId", cuiId));
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                var response = new
                {
                    status = 200,
                    //cus = ds.Tables[0],
                    msg = "Update Customer Details Successfully",
                };
                return Ok(response);
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
        [HttpGet("GetCusDetails")]
        public IActionResult GetCusDetails(int cuiId)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn")); sqlCon.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT * FROM Customers c WHERE c.Id = @cuiId", sqlCon);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@cuiId", cuiId));
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                var response = new
                {
                    status = 200,
                    cus = ds.Tables[0],
                    msg = "Get Customer Details Successfully",
                };
                return Ok(response);
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Biz1PosApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Biz1BookPOS.Controllers
{
    [Route("api/[controller]")]
    public class POSController : Controller
    {
        private POSDbContext db;
        //List<VariantGroup> variantGroups = null;
        public IConfiguration Configuration { get; }
        public POSController(POSDbContext contextOptions, IConfiguration configuration)
        {
            db = contextOptions;
            Configuration = configuration;
        }

        // GET: api/<controller>
        [HttpGet("Get")]
        [EnableCors("AllowOrigin")]
        public IActionResult Get(int compId)
        {
            try
            {
                //var prod = new
                //{
                //    products = from p in db.Products
                //               where p.CompanyId == compId
                //               select new { p.Id, p.Description },
                //};
                //return Json(prod);
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.POSProduct", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@productId", null));
                cmd.Parameters.Add(new SqlParameter("@companyId", compId));
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                DataTable prod = ds.Tables[0];
                var data = new
                {
                    prod = prod
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
        [HttpGet("UpdateOrdItems")]

        //public Task<System.Web.Http.IHttpActionResult> UpdateOrdItems(HttpRequestMessage request)
        public IActionResult UpdateOrdItems(int storeId, int compId, DateTime? delivDate)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {

                try
                {
                    SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                    sqlCon.Open();
                    SqlCommand cmd = new SqlCommand("dbo.POSStkUpd", sqlCon);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@storeId", storeId));
                    cmd.Parameters.Add(new SqlParameter("@companyId", compId));
                    cmd.Parameters.Add(new SqlParameter("@delivDate", delivDate));
                    DataSet ds = new DataSet();
                    SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                    sqlAdp.Fill(ds);
                    //DataTable table = ds.Tables[0];

                    dbContextTransaction.Commit();
                    // OrderItem orderItem = db.OrderItems.Find(ordItemId);
                    var obj = new
                    {
                        status = 1,
                        msg = "Successfully Updated"
                    };
                    //System.Web.Http.IHttpActionResult response= obj;
                    //response = ResponseMessage(responseMsg);
                    // HttpRequestMessage _request;
                    //var response = new HttpResponseMessage()
                    //{
                    //    Content = new StringContent("")
                    //    //RequestMessage = _request
                    //};
                    //return Task.FromResult(response);
                    //return Task.FromResult(response);
                    return Ok(obj);

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
                    return Ok(error);



                    // return Task.FromResult(response);
                }
            }
        }
        [HttpGet("UpdateOrdItems")]
        public IActionResult UpdateOrdItems(int storeId, int compId)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {

                try
                {
                    SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                    sqlCon.Open();
                    SqlCommand cmd = new SqlCommand("dbo.POSStkUpd", sqlCon);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@storeId", storeId));
                    cmd.Parameters.Add(new SqlParameter("@companyId", compId));
                    DataSet ds = new DataSet();
                    SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                    sqlAdp.Fill(ds);
                    //DataTable table = ds.Tables[0];

                    dbContextTransaction.Commit();
                    // OrderItem orderItem = db.OrderItems.Find(ordItemId);
                    var obj = new
                    {
                        status = 1,
                        msg = "Successfully Updated"
                    };
                    //System.Web.Http.IHttpActionResult response= obj;
                    //response = ResponseMessage(responseMsg);
                    // HttpRequestMessage _request;
                    //var response = new HttpResponseMessage()
                    //{
                    //    Content = new StringContent("")
                    //    //RequestMessage = _request
                    //};
                    //return Task.FromResult(response);
                    //return Task.FromResult(response);
                    return Ok(obj);

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
                    return Ok(error);



                    // return Task.FromResult(response);
                }
            }
        }
        [HttpGet("GetPOSSales")]
        [EnableCors("AllowOrigin")]
        public IActionResult GetPOSSales(int compId, int strId, DateTime? frmdate, int? ordProdId)
        {
            try
            {
                //DateTime? FromDate = null; DateTime? ToDate = null;
                //if (frmdate != "" && frmdate != null)
                //    FromDate = DateTime.Parse(frmdate);
                //if (todate != "" && todate != null)
                //     ToDate = DateTime.Parse(todate);

                //SqlConnection sqlCon = new SqlConnection("server=(LocalDb)\\MSSQLLocalDB; database=Biz1POS;Trusted_Connection=True;");
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.POSSales", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@fromdate", frmdate));
                //cmd.Parameters.Add(new SqlParameter("@todate", todate));
                cmd.Parameters.Add(new SqlParameter("@storeId", strId));
                cmd.Parameters.Add(new SqlParameter("@companyId", compId));
                cmd.Parameters.Add(new SqlParameter("@orderProductId", ordProdId));
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                DataTable table = ds.Tables[0];
                var obj = new
                {
                    status = "ok",
                    POSSales = ds.Tables[0],
                };
                sqlCon.Close();
                return Ok(obj);

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
        [HttpGet("UpdateOrdItems")]

        //public Task<System.Web.Http.IHttpActionResult> UpdateOrdItems(HttpRequestMessage request)
        public IActionResult UpdateOrdItems(int storeId, int compId, string delivDate)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {

                try
                {
                    DateTime dateTime = Convert.ToDateTime(delivDate); // 1/1/0001 12:00:00 AM  

                    SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                    sqlCon.Open();
                    SqlCommand cmd = new SqlCommand("dbo.POSStkUpd", sqlCon);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@storeId", storeId));
                    cmd.Parameters.Add(new SqlParameter("@companyId", compId));
                    cmd.Parameters.Add(new SqlParameter("@delivDate", dateTime));
                    DataSet ds = new DataSet();
                    SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                    sqlAdp.Fill(ds);
                    //DataTable table = ds.Tables[0];

                    dbContextTransaction.Commit();
                    // OrderItem orderItem = db.OrderItems.Find(ordItemId);
                    var obj = new
                    {
                        status = 1,
                        msg = "Successfully Updated"
                    };
                    //System.Web.Http.IHttpActionResult response= obj;
                    //response = ResponseMessage(responseMsg);
                    // HttpRequestMessage _request;
                    //var response = new HttpResponseMessage()
                    //{
                    //    Content = new StringContent("")
                    //    //RequestMessage = _request
                    //};
                    //return Task.FromResult(response);
                    //return Task.FromResult(response);
                    return Ok(obj);

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
                    return Ok(error);



                    // return Task.FromResult(response);
                }
            }
        }
        [HttpGet("AkountzSetUp")]
        [EnableCors("AllowOrigin")]
        public IActionResult AkountzSetUp(int compId, string email)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.AkountzSetUp", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@companyId", compId));
                cmd.Parameters.Add(new SqlParameter("@email", email));
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                DataTable compTable = ds.Tables[0];
                DataTable strTable = ds.Tables[1];
                DataTable prodTable = ds.Tables[2];
                int storeId = db.Stores.Where(s => s.CompanyId == compId).Select(s => s.Id).FirstOrDefault();
                string encryptPswd = db.Accounts.Where(s => s.Email == email && s.CompanyId == compId).Select(s => s.Password).FirstOrDefault();
                string decryptPswd = DecryptString(encryptPswd);
                using (var client = new System.Net.Http.HttpClient())
                {
                    //  client.BaseAddress = new Uri("https://fbakountz.azurewebsites.net/");
                    client.BaseAddress = new Uri("https://localhost:44370/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
                    client.DefaultRequestHeaders.Add("Keep-Alive", "3600");
                    string url = "api/POS/POS";
                    //var content = new StringContent(table.ToString(), Encoding.UTF8, "application/json");
                    //var content1 = new StringContent(table.ToString());
                    var data = new { compTable = compTable, strTable = strTable, prodTable = prodTable, password = decryptPswd, defaulfStrId = storeId };
                    //var newData = new StringContent(data.ToString(), Encoding.UTF8, "application/json");
                    //var response = client.PostAsync(url, content1);
                    //response.Wait();

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
    }
}

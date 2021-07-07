using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Biz1BookPOS.Controllers
{
    [Route("api/[controller]")]
    public class RegistrationController : Controller
    {
        private int var_status;
        private Array var_value;
        private string var_msg;
        private POSDbContext db;
        //private string connectionString;

        public IConfiguration Configuration { get; }
        public RegistrationController(POSDbContext contextOptions, IConfiguration configuration)
        {
            db = contextOptions;
            Configuration = configuration;
        }
        // GET: api/<controller>
        [HttpGet("Get")]
        public IActionResult Get()
        {
            var user = db.Users.ToList();
            return Ok(user);
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // GET api/<controller>/5
        [HttpGet("ConfirmEmail")]
        public string ConfirmEmail(string email)
        {
            var account = db.Accounts.Where(x => x.Email == email).FirstOrDefault();
            account.IsConfirmed = true;
            db.Entry(account).State = EntityState.Modified;
            db.SaveChanges();
            return "Account is confirmed";
        }

        // POST api/<controller>
        [HttpPost("Register")]
        public IActionResult Register([FromForm] Registration registration)
        {
            //Request.ContentType = "application/json";
            string enpass = EnryptString(registration.Password);
            string depass = DecryptString(enpass);
            //var connectionString = db.Database.;
            //SqlConnection sqlCon = new SqlConnection("server=(LocalDb)\\MSSQLLocalDB; database=Biz1POS;Trusted_Connection=True;");
            SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("dbo.Registration", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@YourName", registration.Name));
            cmd.Parameters.Add(new SqlParameter("@RestaurentName", registration.RestaurentName));
            cmd.Parameters.Add(new SqlParameter("@EmailId", registration.EmailId));
            cmd.Parameters.Add(new SqlParameter("@Password", enpass));
            cmd.Parameters.Add(new SqlParameter("@PhoneNo", registration.PhoneNo));
            cmd.Parameters.Add(new SqlParameter("@storeName","MainStore"));
            cmd.Parameters.Add(new SqlParameter("@provider", registration.Provider));

            //cmd.ExecuteNonQuery();

            DataSet ds = new DataSet();
            SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
            sqlAdp.Fill(ds);

            DataTable table = ds.Tables[0];

            DataRow row = table.Select().FirstOrDefault();

            int result = Int32.Parse(row["Success"].ToString());
            //string error = "";
            if (result == 1)
            {
                var_status = 0;
                var_msg = "The Email alredy exists";
            }
            else if(result == 2)
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

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
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

        public string EnryptString(string strEncrypted)
        {
            byte[] b = System.Text.Encoding.ASCII.GetBytes(strEncrypted);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }


    }
}

//namespace Biz1BookPOS
//{
//    public string DecryptString(string encrString)
//    {
//        byte[] b;
//        string decrypted;
//        try
//        {
//            b = Convert.FromBase64String(encrString);
//            decrypted = System.Text.ASCIIEncoding.ASCII.GetString(b);
//        }
//        catch (FormatException fe)
//        {
//            decrypted = "";
//        }
//        return decrypted;
//    }

//    public string EnryptString(string strEncrypted)
//    {
//        byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
//        string encrypted = Convert.ToBase64String(b);
//        return encrypted;
//    }
//}
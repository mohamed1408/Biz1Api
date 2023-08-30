﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Biz1PosApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Biz1BookPOS.Controllers
{
    [Route("api/[controller]")]
    public class LogInController : Controller
    {
        private static TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        private int var_status;
        private string var_value;
        private object var_detail;
        private string var_msg;
        private DataTable orderNo;
        private DataTable kotNo;
        private DataTable UnclosedShifts;
        private DataTable Preferences;
        private DataTable PendinOrders;
        private IConfiguration _config;
        public IConfiguration Configuration { get; }
        private POSDbContext db;
        public LogInController(IConfiguration config, IConfiguration configuration, POSDbContext contextOptions)
        {
            _config = config;
            Configuration = configuration;
            db = contextOptions;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        //[HttpGet("getcredentials")]
        //public IActionResult getcredentials(int companyid)
        //{
        //    List<Credential> credentials = new List<Credential>();
        //    var accounts = db.Accounts.ToList();
        //    foreach(var account in accounts)
        //    {
        //        Credential credential = new Credential();
        //        credential.companyname = account.Name;
        //        credential.email = account.Email;
        //        credential.password = DecryptString(account.Password);
        //        credentials.Add(credential);
        //    }
        //    return Ok(credentials);
        //}

        // POST api/<controller>
        [HttpPost("LogIn")]
        public IActionResult LogIn([FromForm]Registration registration)
        {
            try
            {
                string enpass = EnryptString(registration.Password);
                //SqlConnection sqlCon = new SqlConnection("server=(LocalDb)\\MSSQLLocalDB; database=Biz1POS;Trusted_Connection=True;");
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.Login", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Email", registration.EmailId));
                cmd.Parameters.Add(new SqlParameter("@Password", enpass));
                //cmd.Parameters.Add(new SqlParameter("@ConfirmPassword", registration.));
                cmd.Parameters.Add(new SqlParameter("@PhoneNo", registration.EmailId));

                //cmd.ExecuteNonQuery();

                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);

                DataTable table = ds.Tables[0];

                DataRow row = table.Select().FirstOrDefault();
                IActionResult response = Unauthorized();
                int result = Int32.Parse(row["Success"].ToString());
                if (result == 0)
                {
                    string jsonStr = "";
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        jsonStr += ds.Tables[1].Rows[i].ItemArray[0].ToString();
                    }
                    var userJson = JsonConvert.DeserializeObject(jsonStr);
                    var_status = 200;
                    var_detail = userJson;
                    var_msg = "LoggedIn Successfully";
                    var tokenString = GenerateJSONWebToken(registration);
                    response = Ok(new { token = tokenString });
                    orderNo = ds.Tables[2];
                    kotNo = ds.Tables[4];
                    UnclosedShifts = ds.Tables[3];
                    Preferences = ds.Tables[5];
                    PendinOrders = ds.Tables[6];
                }
                else
                {
                    var_status = 0;
                    var_msg = "Invalid EmailId or Password/Email not confirmed";
                }
                var returnArray = new
                {
                    status = var_status,
                    data = var_detail,
                    OrderNo = orderNo,
                    KotNo = kotNo,
                    msg = var_msg,
                    token = response,
                    unclosedShifts = UnclosedShifts,
                    preferences = Preferences,
                    pendingorders = PendinOrders
                };
                sqlCon.Close();
                return Json(returnArray);
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
        [HttpPost("PinVaidate")]
        public IActionResult PinVaidate(int companyid, int pin)
        {
            try
            {
                bool pin_valid = false;
                int valid_userid = 0; ;
                List<User> users = db.Users.Where(x => x.CompanyId == companyid).ToList();
                foreach(User user in users)
                {
                    if(user.Pin == pin)
                    {
                        pin_valid = true;
                        valid_userid = user.Id;
                        break;
                    }
                }
                if (pin_valid)
                {
                    var response = new
                    {
                        user = db.Users.Where(x => x.Id == valid_userid).Select(x => new { x.Id, x.Name, x.Role, x.RoleId }).FirstOrDefault(),
                        msg = "User Exists!"
                    };
                    return Json(response);
                }
                else
                {
                    var response = new
                    {
                        user = 0,
                        msg = "Invalid Pin!"
                    };
                    return Json(response);
                }
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
        [HttpGet("pinlogin")]
        public IActionResult pinlogin(int companyid, int pin, int? storeid)
        {
            try
            {
                var user = db.Users.Where(x => x.Pin == pin && x.CompanyId == companyid).FirstOrDefault();
                var account = db.UserAccounts.Where(x => x.CompanyId == companyid).FirstOrDefault();
                string jtoken = "";
                string msg = "invalid pin";
                int status = 0;
                int orderno = 0;
                int kotno = 0;
                if(user != null)
                {
                    if(storeid != null)
                    {
                        Order order = db.Orders.Where(x => x.StoreId == storeid && x.UserId == user.Id && x.OrderTypeId < 6 && x.OrderedDate == TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time).Date).FirstOrDefault();
                        orderno = db.Orders.Where(x => x.StoreId == storeid && x.UserId == user.Id && x.OrderTypeId < 6 && x.OrderedDate == TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time).Date).Select(x => x.OrderNo).DefaultIfEmpty(0).Max();
                        if(order != null)
                            kotno = db.KOTs.Where(x => x.OrderId == order.Id).Select(x => int.Parse(x.KOTNo)).DefaultIfEmpty(0).Max();
                    }

                    string role = db.Roles.Find(user.RoleId).Name;
                    int roleid = db.Roles.Find(user.RoleId).Id;
                    //security key
                    string securityKey = _config["Jwt:Key"];
                    //symmetric security key
                    var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

                    //signing credentials
                    var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

                    //add claims
                    var claims = new List<Claim>();
                    claims.Add(new Claim("role", role));
                    //claims.Add(new Claim("Our_Custom_Claim", "Our custom value"));
                    //claims.Add(new Claim("Id", "110"));
                    claims.Add(new Claim("email", account.Email));
                    claims.Add(new Claim("user", user.Name));
                    claims.Add(new Claim("userid", user.Id.ToString()));
                    claims.Add(new Claim("roleid", roleid.ToString()));
                    claims.Add(new Claim("companyid", account.CompanyId.ToString()));
                    //claims.Add(new Claim(ClaimTypes.Expiration, userInfo.EmailId));

                    //create token
                    var token = new JwtSecurityToken(
                        issuer: _config["Jwt:Issuer"],
                        audience: "readers",
                        expires: roleid == 1 ? DateTime.Now.AddYears(1) : DateTime.Now.AddHours(1),
                        signingCredentials: signingCredentials,
                        claims: claims
                    );
                    jtoken = new JwtSecurityTokenHandler().WriteToken(token);
                    msg = "Pin matched";
                    status = 200;
                }
                var response = new
                {
                    status = status,
                    msg = msg,
                    token = jtoken,
                    userid = user == null ? null : user.Id.ToString(),
                    orderno,
                    kotno,
                };
                return Json(response);
            }
            catch (Exception ex)
            {
                var error = new
                {
                    status = -1,
                    msg = ex.Message
                };
                return Json(error);
            }
        }
        [HttpGet("pinlogin2")]
        public IActionResult pinlogin2(int companyid, int pin, int storeid)
        {
            try
            {
                var user = db.Users.Where(x => x.Pin == pin && x.CompanyId == companyid && db.UserStores.Where(y => y.UserId == x.Id && y.StoreId == storeid).Any()).FirstOrDefault();
                var account = db.Accounts.Where(x => x.CompanyId == companyid).FirstOrDefault();
                string jtoken = "";
                string msg = "invalid pin";
                int status = 0;
                if(user != null)
                {
                    string role = db.Roles.Find(user.RoleId).Name;
                    int roleid = db.Roles.Find(user.RoleId).Id;
                    //security key
                    string securityKey = _config["Jwt:Key"];
                    //symmetric security key
                    var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

                    //signing credentials
                    var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

                    //add claims
                    var claims = new List<Claim>();
                    claims.Add(new Claim("role", role));
                    //claims.Add(new Claim("Our_Custom_Claim", "Our custom value"));
                    //claims.Add(new Claim("Id", "110"));
                    claims.Add(new Claim("email", account.Email));
                    claims.Add(new Claim("user", user.Name));
                    claims.Add(new Claim("userid", user.Id.ToString()));
                    claims.Add(new Claim("roleid", roleid.ToString()));
                    claims.Add(new Claim("companyid", account.CompanyId.ToString()));
                    //claims.Add(new Claim(ClaimTypes.Expiration, userInfo.EmailId));

                    //create token
                    var token = new JwtSecurityToken(
                        issuer: _config["Jwt:Issuer"],
                        audience: "readers",
                        expires: roleid == 1 ? DateTime.Now.AddYears(1) : DateTime.Now.AddHours(1),
                        signingCredentials: signingCredentials,
                        claims: claims
                    );
                    jtoken = new JwtSecurityTokenHandler().WriteToken(token);
                    msg = "Pin matched";
                    status = 200;
                }
                var response = new
                {
                    status = status,
                    msg = msg,
                    token = jtoken,
                    userid = user == null ? null : user.Id.ToString(),
                };
                return Json(response);
            }
            catch (Exception ex)
            {
                var error = new
                {
                    status = -1,
                    msg = ex.Message
                };
                return Json(error);
            }
        }
        // POST api/<controller>
        [HttpPost("WaiterLogIn")]
        public IActionResult WaiterLogIn([FromForm]Registration registration)
        {
            try
            {
                string enpass = EnryptString(registration.Password);
                //SqlConnection sqlCon = new SqlConnection("server=(LocalDb)\\MSSQLLocalDB; database=Biz1POS;Trusted_Connection=True;");
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.WaiterLogin", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Email", registration.EmailId));
                cmd.Parameters.Add(new SqlParameter("@Password", enpass));
                //cmd.Parameters.Add(new SqlParameter("@ConfirmPassword", registration.));
                cmd.Parameters.Add(new SqlParameter("@PhoneNo", registration.EmailId));

                //cmd.ExecuteNonQuery();

                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);

                DataTable table = ds.Tables[0];

                DataRow row = table.Select().FirstOrDefault();
                IActionResult response = Unauthorized();
                int result = Int32.Parse(row["Success"].ToString());
                if (result == 0)
                {
                    string jsonStr = "";
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        jsonStr += ds.Tables[1].Rows[i].ItemArray[0].ToString();
                    }
                    var userJson = JsonConvert.DeserializeObject(jsonStr);
                    var_status = 200;
                    var_detail = userJson;
                    var_msg = "LoggedIn Successfully";
                    var tokenString = GenerateJSONWebToken(registration);
                    response = Ok(new { token = tokenString });
                    orderNo = ds.Tables[2];
                    kotNo = ds.Tables[3];
                    Preferences = ds.Tables[4];
                }
                else
                {
                    var_status = 0;
                    var_msg = "Invalid EmailId or Password/Email not confirmed";
                }
                var returnArray = new
                {
                    status = var_status,
                    StoreData = var_detail,
                    OrderNo = orderNo,
                    KotNo = kotNo,
                    msg = var_msg,
                    token = response,
                    preferences = Preferences
                };
                sqlCon.Close();
                return Json(returnArray);
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


        [HttpPost("WebLogIn")]
        public IActionResult WebLogIn([FromForm]Registration registration)
        {
            try
            {
                string enpass = EnryptString(registration.Password);
                //SqlConnection sqlCon = new SqlConnection("server=(LocalDb)\\MSSQLLocalDB; database=Biz1POS;Trusted_Connection=True;");
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.WebSiteLogin", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Email", registration.EmailId));
                cmd.Parameters.Add(new SqlParameter("@Password", enpass));
                //cmd.Parameters.Add(new SqlParameter("@ConfirmPassword", registration.));
                cmd.Parameters.Add(new SqlParameter("@PhoneNo", registration.EmailId));

                //cmd.ExecuteNonQuery();

                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);

                DataTable table = ds.Tables[0];

                DataRow row = table.Select().FirstOrDefault();
                IActionResult response = Unauthorized();
                int result = Int32.Parse(row["Success"].ToString());
                UserAccounts accounts = new UserAccounts();
                Company company = new Company();
                ActionResult tokenString;
                if (result == 0)
                {
                    var_status = 200;
                    //var_detail = ds.Tables[1];
                    var_msg = "LoggedIn Successfully";
                    tokenString = GenerateJSONWebToken(registration);
                    response = Ok(new { token = tokenString });
                    //orderNo = ds.Tables[2];
                    accounts = db.UserAccounts.Where(x => x.Email == registration.EmailId).FirstOrDefault();
                    company = db.Companies.Find(accounts.CompanyId);
                }
                else
                {
                    var_status = 0;
                    var_msg = "Invalid EmailId or Password/Email not confirmed";
                }
                var returnArray = new
                {
                    status = var_status,
                    //data = var_detail,
                    //OrderNo = orderNo,
                    msg = var_msg,
                    token = response,
                    //result = result,
                    emailId = registration.EmailId,
                    company = company
                };
                sqlCon.Close();
                return Json(returnArray);
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
        private ActionResult GenerateJSONWebToken(Registration userInfo)
        {
            //security key
            string securityKey = _config["Jwt:Key"];
            //symmetric security key
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

            //signing credentials
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            //add claims
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Role, "Administrator"));
            //claims.Add(new Claim(ClaimTypes.Role, "Reader"));
            //claims.Add(new Claim("Our_Custom_Claim", "Our custom value"));
            //claims.Add(new Claim("Id", "110"));
            claims.Add(new Claim(ClaimTypes.Email, userInfo.EmailId));
            //claims.Add(new Claim(ClaimTypes.Expiration, userInfo.EmailId));

            //create token
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: "readers",
                expires: DateTime.Now.AddYears(1),
                signingCredentials: signingCredentials,
                claims: claims
            );

            //return token
            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
        [HttpGet("password_reset")]
        public IActionResult password_reset(string jwt)
        {
            ViewBag.jwt = jwt;
            return View();
        }

        [HttpGet("update_password")]
        public IActionResult update_password(string jwt, string password)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            string emailid = token.Payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"].ToString();
            string encr_pass = EnryptString(password);
            Accounts accounts = db.Accounts.Where(x => x.Email == emailid).FirstOrDefault();
            accounts.Password = encr_pass;
            db.Entry(accounts).State = EntityState.Modified;
            db.SaveChanges();
            return Ok("Password Updated");
        }


        //[HttpGet("ResetPassword")]
        //public IActionResult ResetPassword(string email)
        //{
        //    var check_email = db.Accounts.Where(x => x.Email == email).FirstOrDefault();
        //    if(check_email != null)
        //    {
        //        return GenerateExpirableToken(email);
        //    }
        //    else
        //    {
        //        return Ok("Email not registered");
        //    }
        //}

        [HttpGet("send_reset_link")]
        public IActionResult send_reset_link(string email)
        {
            try
            {
                var check_email = db.Accounts.Where(x => x.Email == email).FirstOrDefault();
                if (check_email != null)
                {
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.zoho.com");

                    mail.From = new MailAddress("admin@biz1book.com");
                    mail.To.Add(email);
                    mail.Subject = "Test Mail";
                    mail.Body = "https://localhost:44383/api/Login/password_reset?jwt="+GenerateExpirableToken(email);

                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("admin@biz1book.com", "Sairam@11");
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(mail);
                    return Ok("Email Sent!");
                }
                else
                {
                    return Ok("Email not registered");
                }
            }
            catch (Exception ex)
            {
                return Ok("Error Sending Email");
            }
        }

        private string GenerateExpirableToken(string email)
        {
            //security key
            string securityKey = _config["Jwt:Key"];
            //symmetric security key
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

            //signing credentials
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            //add claims
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Role, "Administrator"));
            //claims.Add(new Claim(ClaimTypes.Role, "Reader"));
            //claims.Add(new Claim("Our_Custom_Claim", "Our custom value"));
            //claims.Add(new Claim("Id", "110"));
            claims.Add(new Claim(ClaimTypes.Email, email));

            //create token
            var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: "readers",
            expires: DateTime.Now.AddMinutes(1),
            signingCredentials: signingCredentials,
            claims: claims
            );

            //return token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        // PUT api/<controller>/5
        [HttpGet("kosaksipasapugal")]
        public IActionResult kosaksipasapugal(int id)
        {
            Kosaksi kosaksi = new Kosaksi();
            Accounts accounts = db.Accounts.Where(x => x.CompanyId == id).FirstOrDefault();
            kosaksi.uname = accounts.Email;
            kosaksi.pass = DecryptString(accounts.Password);
            kosaksi.users = db.Users.Where(x => x.CompanyId == id).ToList();
            kosaksi.users.ForEach(user =>
            {
                user.Store = String.Join(",",db.Stores.Where(x => db.UserStores.Where(y => y.UserId == user.Id && y.StoreId == x.Id).Any()).Select(x => x.Name).ToArray());
            });
            return Ok(kosaksi);
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
        //byte[] b;
        //string decrypted;
        //b = Convert.FromBase64String(encrString);
        //decrypted = System.Text.ASCIIEncoding.ASCII.GetString(b);
        //Console.WriteLine(decrypted)
        [HttpGet("ecrptpass")]
        public string EnryptString(string strEncrypted)
        {
            try
            {
                byte[] b = System.Text.Encoding.ASCII.GetBytes(strEncrypted);
                string encrypted = Convert.ToBase64String(b);
                return encrypted;
            }
            catch(Exception e)
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

    }

    internal class Credential
    {
        public string companyname { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }
    class Kosaksi
    {
        public string uname { get; set; }
        public string pass { get; set; }
        public List<User> users { get; set; }
    }
}
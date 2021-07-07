using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Biz1PosApi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    public class CompanyController : Controller
    {
        private POSDbContext db;
        private IConfiguration _config;

        public CompanyController(IConfiguration config, POSDbContext contextOptions)
        {
            _config = config;
            db = contextOptions;
        }
        [HttpGet("Index")]
        [EnableCors("AllowOrigin")]
        public IActionResult Index(int companyId)
        {
            try
            {
                var prod = new
                {
                    company = db.Companies.Where(c=> c.Id== companyId).FirstOrDefault(),
                    accounts = db.Accounts.Where(a=> a.CompanyId == companyId).FirstOrDefault(),
                    user = db.Users.Where(u=>u.CompanyId ==companyId).FirstOrDefault()
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
        [HttpGet("getbybizid")]
        [EnableCors("AllowOrigin")]
        public IActionResult getbybizid(string bizid)
        {
            try
            {
                var accounts = db.Accounts.Where(x => x.bizid == bizid).FirstOrDefault();
                accounts.jwt = GenerateJSONWebToken(accounts.Email);
                return Json(accounts);
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

        [HttpPost("savemerchant")]
        [EnableCors("AllowOrigin")]
        public IActionResult SaveMerchant([FromBody]Accounts accounts)
        {
            try
            {
                db.Entry(accounts).State = EntityState.Modified;
                db.SaveChanges();
                var error = new
                {
                    status = 2000,
                    msg = "The data updated successfully"
                };

                return Json(error);
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
        [HttpGet("GetAll")]
        [EnableCors("AllowOrigin")]
        public IActionResult GetAll()
        {
            try
            {
                var company = db.Companies.ToList();
                return Json(company);
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
        [EnableCors("AllowOrigin")]
        public IActionResult SaveData([FromForm]string objData)
        {
            try
            {
                dynamic comp = JsonConvert.DeserializeObject(objData);
                Company company = comp.company.ToObject<Company>();
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                Accounts accounts = comp.accounts.ToObject<Accounts>();
                accounts.CompanyId = company.Id;
                db.Entry(accounts).State = EntityState.Modified;
                db.SaveChanges();
                User user = comp.user.ToObject<User>();
                user.CompanyId = company.Id;
                db.Entry(user).State = EntityState.Modified;
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
        private string GenerateJSONWebToken(string email)
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
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Microsoft.AspNetCore.Mvc;

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]

    public class RoleController : Controller
    {
        private POSDbContext db;

        public RoleController(POSDbContext contextOptions)
        {
            db = contextOptions;
        }
        // GET: api/<controller>
        [HttpGet("Get")]

        public IActionResult Get(int companyId)
        {
            try
            {
                var roles = db.Roles.ToList();

                return Ok(roles);
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
    }
}
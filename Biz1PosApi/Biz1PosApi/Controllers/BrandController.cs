using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    public class BrandController : Controller
    {
        private POSDbContext db;
        public BrandController(POSDbContext contextOptions)
        {
            db = contextOptions;
        }

        [HttpGet("GetBrand")]
        [EnableCors("AllowOrigin")]
        public IActionResult GetBrand(int companyId)
        {
            var Brand = db.Brands.Where(x => x.CompanyId == companyId).ToList();
            return Ok(Brand);
        }
    }
}
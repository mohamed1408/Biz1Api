using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    public class SatelliteController : Controller
    {
        private POSDbContext db;
        public IConfiguration Configuration { get; }
        public SatelliteController(POSDbContext contextOptions, IConfiguration configuration)
        {
            db = contextOptions;
            Configuration = configuration;
        }
        // GET: Satellite
        [HttpGet]
        public ActionResult GetOrders(int companyId, DateTime fromDate, DateTime toDate)
        {
            var orders = db.Orders.Where(x => x.CompanyId == companyId && x.UPOrderId != null && x.OrderedDate>= fromDate && x.OrderedDate <= toDate).ToList();
            return Ok(orders);
        }

        // GET: Satellite/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Satellite/Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: Satellite/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // GET: Satellite/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

    }
}
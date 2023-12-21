using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Biz1BookPOS.Models;
using Biz1PosApi.Services;

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    public class ElectronController : Controller
    {
        private POSDbContext db;
        public IConfiguration Configuration { get; }
        private ConnectionStringService connserve;
        public ElectronController(POSDbContext contextOptions, IConfiguration configuration, ConnectionStringService _connserve)
        {
            db = contextOptions;
            Configuration = configuration;
            connserve = _connserve;
        }

        // GET: ElectronController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ElectronController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ElectronController/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpGet("GetAppversion")]
        [EnableCors("AllowOrigin")]
        public IActionResult GetCustomerList(int companyid, DateTime orderdate)
        {
            try
            {
                string connanme = connserve.getConnString(companyid);
                //SqlConnection sqlCon = new SqlConnection("server=(LocalDb)\\MSSQLLocalDB; database=Biz1POS;Trusted_Connection=True;");
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString(connanme));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.AppversionRpt", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@companyId", companyid));
                cmd.Parameters.Add(new SqlParameter("@orderdate", orderdate));
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                DataTable table = ds.Tables[0];

                var data = new
                {
                    versions = ds.Tables[0]


                };
                sqlCon.Close();
                return Ok(data);
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
        // POST: ElectronController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ElectronController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ElectronController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ElectronController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ElectronController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Biz1PosApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    public class DenominationController : Controller
    {
        private POSDbContext db;
        private object addon;
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        public IConfiguration Configuration { get; }

        public DenominationController(POSDbContext contextOptions, IConfiguration configuration)
        {
            db = contextOptions;
            Configuration = configuration;
        }
        // GET: DeniminationController
        [HttpGet("GetDenominationTypes")]
        public ActionResult GetDenominationTypes()
        {
            List<string> denominationTypes = db.DenominationTypes.Where(x => x.Currency == "INR").Select(x => x.Name).ToList();
            return Json(denominationTypes);
        }

        [HttpPost("addDenomEntry")]
        public ActionResult addDenomEntry([FromBody]JObject data)
        {
            try
            {
                dynamic entryData = data;
                DenomEntry denomEntry = new DenomEntry();
                denomEntry = entryData.ToObject<DenomEntry>();
                //denomEntry.CompanyId = entryData.CompanyId;
                //denomEntry.EntryDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                //denomEntry.StoreId = entryData.StoreId;
                //denomEntry.UserId = entryData.UserId;
                db.DenomEntries.Add(denomEntry);
                db.SaveChanges();
                foreach (var entry in entryData.Entries)
                {
                    Denomination denomination = new Denomination();
                    entry.DenomEntryId = denomEntry.Id;
                    denomination = entry.ToObject<Denomination>();
                    denomination.DenomEntryId = denomEntry.Id;
                    db.Denominations.Add(denomination);
                    db.SaveChanges();
                }
                var response = new
                {
                    status = 200,
                    msg = "Success",
                };
                return Json(response);
            }
            catch (Exception e)
            {
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = "error",
                    msg = "Something went wrong  Contact our service provider",
                    orders = new JArray()
                };
                return Json(error);
            }
        }
        [HttpGet("getDenomEntry")]
        public ActionResult getDenomEntry(int storeid, DateTime date, int companyid, int? entrytypeid)
        {
            try
            {
                //SqlConnection sqlCon = new SqlConnection("server=(LocalDb)\\MSSQLLocalDB; database=Biz1POS;Trusted_Connection=True;");
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                //string jsonOutputParam = "@jsonOutput";
                SqlCommand cmd = new SqlCommand("dbo.DenominationEntryFetch", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@storeid", storeid));
                cmd.Parameters.Add(new SqlParameter("@date", date));
                cmd.Parameters.Add(new SqlParameter("@companyid", companyid));
                cmd.Parameters.Add(new SqlParameter("@entrytypeid", entrytypeid));
                //cmd.Parameters.Add(new SqlParameter("@modDate", null));
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                string jstring = "";
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    jstring += ds.Tables[0].Rows[j].ItemArray[0].ToString();
                }
                var obj = new
                {
                    status = 200,
                    data = JsonConvert.DeserializeObject(jstring)
                };
                sqlCon.Close();
                return Json(obj);
            }
            catch (Exception e)
            {
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = "error",
                    msg = "Something went wrong  Contact our service provider",
                    orders = new JArray()
                };
                return Json(error);
            }
        }
        [HttpGet("denomReport")]
        public ActionResult denomReport(int userid, int storeid, int companyid, DateTime from, DateTime to)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();

                SqlCommand cmd = new SqlCommand("dbo.DenomEntryReport", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@userid", userid));
                cmd.Parameters.Add(new SqlParameter("@storeId", storeid));
                cmd.Parameters.Add(new SqlParameter("@fromdate", from));
                cmd.Parameters.Add(new SqlParameter("@todate", to));
                cmd.Parameters.Add(new SqlParameter("@companyid", companyid));

                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                var obj = new
                {
                    status = 200,
                    report = ds.Tables[0]
                };
                sqlCon.Close();
                return Json(obj);
            }
            catch (Exception e)
            {
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = "error",
                    msg = "Something went wrong  Contact our service provider",
                    orders = new JArray()
                };
                return Json(error);
            }
        }

        [HttpGet("denomReport_")]
        public ActionResult denomReport_(int companyid, DateTime from, DateTime to, float margin)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();

                SqlCommand cmd = new SqlCommand("dbo.DenomEntryReport_", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@fromdateTime", from));
                cmd.Parameters.Add(new SqlParameter("@todateTime", to));
                cmd.Parameters.Add(new SqlParameter("@companyId", companyid));
                cmd.Parameters.Add(new SqlParameter("@margin", margin));

                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                var obj = new
                {
                    status = 200,
                    badStores = ds.Tables[0],
                    missingStores = ds.Tables[1]
                };
                sqlCon.Close();
                return Json(obj);
            }
            catch (Exception e)
            {
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = "error",
                    msg = "Something went wrong  Contact our service provider",
                    orders = new JArray()
                };
                return Json(error);
            }
        }

        // GET: DeniminationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DeniminationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DeniminationController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: DeniminationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // GET: DeniminationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
    }
}

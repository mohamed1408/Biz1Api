﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Biz1PosApi.Models;
using Biz1PosApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private ConnectionStringService connserve;
        public DenominationController(POSDbContext contextOptions, IConfiguration configuration, ConnectionStringService _connserve)
        {
            db = contextOptions;
            Configuration = configuration;
            connserve = _connserve;
        }
        // GET: DeniminationController
        [HttpGet("GetDenominationTypes")]
        public ActionResult GetDenominationTypes()
        {
            List<string> denominationTypes = db.DenominationTypes.Where(x => x.Currency == "INR").Select(x => x.Name).ToList();
            return Json(denominationTypes);
        }
        [HttpGet("GetShifts")]
        public ActionResult GetShifts()
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                //string jsonOutputParam = "@jsonOutput";
                SqlCommand cmd = new SqlCommand("SELECT * FROM Shifts", sqlCon);
                cmd.CommandType = CommandType.Text;

                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);

                return Json(ds.Tables[0]);
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

        [HttpGet("delDenomEntry")]
        public ActionResult addDenomEntry(int denomeEntryId)
        {
            try
            {
                List<Denomination> denomEntries = db.Denominations.Where(x => x.DenomEntryId == denomeEntryId).ToList();
                DenomEntry denomEntry = db.DenomEntries.Find(denomeEntryId);
                db.Denominations.RemoveRange(denomEntries);
                db.DenomEntries.Remove(denomEntry);
                db.SaveChanges();
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
        [HttpGet("changeshift")]
        public ActionResult changeshift(int denomeEntryId, int shiftid)
        {
            try
            {
                DenomEntry denomEntry = db.DenomEntries.Find(denomeEntryId);
                denomEntry.ShiftId = shiftid;
                db.Entry(denomEntry).State = EntityState.Modified;
                db.SaveChanges();
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
        public class ShiftManager
        {
            List<ArrayList> shifts = new List<ArrayList>();
            public ShiftManager()
            {
                shifts.Add(new ArrayList { 1, new TimeSpan(08, 00, 0) , new TimeSpan(11, 59, 0) });
                shifts.Add(new ArrayList { 2, new TimeSpan(12, 00, 0) , new TimeSpan(13, 59, 0) });
                shifts.Add(new ArrayList { 3, new TimeSpan(14, 00, 0) , new TimeSpan(15, 59, 0) });
                shifts.Add(new ArrayList { 4, new TimeSpan(16, 00, 0) , new TimeSpan(17, 59, 0) });
                shifts.Add(new ArrayList { 5, new TimeSpan(18, 00, 0) , new TimeSpan(19, 59, 0) });
                shifts.Add(new ArrayList { 6, new TimeSpan(20, 00, 0) , new TimeSpan(21, 59, 0) });
                shifts.Add(new ArrayList { 7, new TimeSpan(22, 00, 0) , new TimeSpan(23, 59, 0) });
            }
            public int getShiftId(DateTime denomDate)
            {
                TimeSpan time = denomDate.TimeOfDay;
                foreach(ArrayList shift in shifts)
                {
                    if(time >= (TimeSpan)shift[1] && time <= (TimeSpan)shift[2])
                    {
                        return (int)shift[0];
                    }
                }
                return 0;
            }
        }
        [HttpPost("addDenomEntry")]
        public ActionResult addDenomEntry([FromBody]JObject data)
        {
            try
            {
                dynamic entryData = data;
                DenomEntry denomEntry = new DenomEntry();
                denomEntry = entryData.ToObject<DenomEntry>();
                denomEntry.ShiftId = new ShiftManager().getShiftId(denomEntry.EntryDateTime);
                //denomEntry.CompanyId = entryData.CompanyId;
                //denomEntry.EntryDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                //denomEntry.StoreId = entryData.StoreId;
                //denomEntry.UserId = entryData.UserId;
                db.DenomEntries.Add(denomEntry);
                db.SaveChanges();
                foreach (var entry in entryData.Entries)
                {
                    Denomination denomination = new Denomination();
                    entry.DenomEntryId = denomEntry.DenomEntryId;
                    denomination = entry.ToObject<Denomination>();
                    //denomination.DenomEntryId = denomEntry.DenomEntryId;
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

        [HttpGet("pettyCashTransfer")]
        public ActionResult pettyCashTransfer(int storeid, int companyid, double amount, string to, string reason)
        {
            try
            {
                Transaction transaction = new Transaction();
                transaction.Amount = amount;
                transaction.CompanyId = companyid;
                transaction.ModifiedDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                transaction.StoreId = storeid;
                transaction.PaymentTypeId = 7;
                transaction.TranstypeId = (to == "EXPENSE") ? 1 : (to == "SALES") ? 2 : 0;
                transaction.TransDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                transaction.TransDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                transaction.Notes = reason;
                db.Transactions.Add(transaction);
                db.SaveChanges();

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
        public ActionResult getDenomEntry(int storeid, DateTime date, int companyid, int? entrytypeid, int entryid = 0)
        {
            try
            {
                string conname = connserve.getConnString(companyid);
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
                cmd.Parameters.Add(new SqlParameter("@entryid", entryid));
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
        [HttpGet("denomcheckreport")]
        public ActionResult denomcheckreport(int companyid, DateTime from, DateTime to)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();

                SqlCommand cmd = new SqlCommand("dbo.DenomCheckReport", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@from", from));
                cmd.Parameters.Add(new SqlParameter("@to", to));
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
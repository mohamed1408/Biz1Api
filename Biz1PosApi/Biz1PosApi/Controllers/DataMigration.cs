using Biz1PosApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading.Channels;
using System.Threading.Tasks;
using System;
using Biz1BookPOS.Models;
using Biz1PosApi.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Biz1PosApi.Services;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    public class DataMigration : Controller
    {
        private TempDbContext db;
        readonly ILogger<DataMigration> _log;
        public IConfiguration Configuration { get; }
        public DataMigration(IConfiguration configuration, ILogger<DataMigration> log)
        {
            _log = log;
            Configuration = configuration;
        }
        [HttpGet("SaveOrderAsync")]
        public async Task<IActionResult> SaveOrderAsync([FromServices] Channel<OrderPckg> channel)
        {
            try
            {
                db = DbContextFactory.Create("logout");
                
                DateTime date = new DateTime(2023, 09, 20);
                DateTime today = new DateTime(2023, 11, 27);
                while(date <= today)
                {
                    List<Odrs> odrs = db.Odrs.Where(x => x.od == date).ToList();
                    _log.LogInformation("[" + date.ToString() +" order count]: " + odrs.Count.ToString());
                    date.AddDays(1);
                    foreach (Odrs odr in odrs)
                    {
                        List<int> kotids = db.KOTs.Where(x => x.OrderId == odr.OdrsId).Select(s => s.KOTId).ToList();
                        OrderPckg pckg = new OrderPckg()
                        {
                            odrs = odr,
                            otms = db.Otms.Where(x => x.ob == odr.Id && kotids.Contains((int)x.ki)).ToList()
                        };
                        await channel.Writer.WriteAsync(pckg);
                    }
                }
                var response = new
                {
                    status = 200,
                    msg = "Task Running..."
                };
                return Ok(response);
            }
            catch (Exception e)
            {
                var error = new
                {
                    status = 500,
                    error = new Exception(e.Message, e.InnerException)
                };
                return Ok(error);
            }
        }
        [HttpGet("OrderBulkSave")]
        public IActionResult BulkSave()
        {
            try
            {
                int ordercount = 0;
                DateTime date = new DateTime(2023, 08, 01);
                DateTime today = new DateTime(2023, 11, 18);
                while (date <= today)
                {
                    DataSet dataSet = new DataSet();
                    SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("hydconn"));
                    conn.Open();
                    string selectData = String.Format(@"select * from Odrs where od = '{0}-{1}-{2}'
select oi.* from Otms oi join KOTs k on k.KOTId = oi.ki join Odrs o on o.OdrsId = k.OrderId where o.od = '{0}-{1}-{2}'", date.Year,date.Month,date.Day);
                    //string selectoi = String.Format("select oi.* from Otms oi join KOTs k on k.KOTId = oi.ki join Odrs o on o.OdrsId = k.OrderId where o.ci = 3 and o.od = '{0}-{1}-{2}'", date.Year,date.Month,date.Day);
                    SqlCommand command = new SqlCommand(selectData, conn);
                    DataTable dataTable = new DataTable();
                    DataTable oitable = new DataTable();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(selectData, conn);
                    dataAdapter.FillSchema(dataSet, SchemaType.Mapped);
                    dataAdapter.Fill(dataSet);
                    dataTable = dataSet.Tables[0];
                    oitable = dataSet.Tables[1];
                    conn.Close();
                    dataTable.Columns.Remove("odt");
                    dataTable.Columns.Remove("bdt");
                    dataTable.Columns.Remove("bd");
                    if (dataTable.Rows.Count > 0)
                    {
                        //Connect to second Database and Insert row/rows.
                        SqlConnection conn2 = new SqlConnection(Configuration.GetConnectionString("myconn"));
                        conn2.Open();
                        SqlBulkCopy bulkCopy = new SqlBulkCopy(conn2);
                        bulkCopy.DestinationTableName = "Odrs";
                        bulkCopy.WriteToServer(dataTable);
                        SqlBulkCopy bulkCopyoi = new SqlBulkCopy(conn2);
                        bulkCopyoi.DestinationTableName = "Otms";
                        bulkCopyoi.WriteToServer(oitable);
                        conn2.Close();
                    }
                    _log.LogInformation("[" + date.ToString() + " order count]: " + dataTable.Rows.Count.ToString());
                    _log.LogInformation("[" + date.ToString() + " item count]: " + oitable.Rows.Count.ToString());
                    ordercount += dataTable.Rows.Count;
                    date = date.AddDays(1);
                }
                var error = new
                {
                    status = 200,
                    ordercount
                };
                return Ok(error);
            }
            catch (Exception e)
            {
                var error = new
                {
                    status = 500,
                    error = new Exception(e.Message, e.InnerException)
                };
                return Ok(error);
            }
        }
        // GET: DataMigration
        public ActionResult Index()
        {
            return View();
        }

        // GET: DataMigration/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DataMigration/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DataMigration/Create
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

        // GET: DataMigration/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DataMigration/Edit/5
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

        // GET: DataMigration/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DataMigration/Delete/5
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

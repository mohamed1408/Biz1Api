﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Microsoft.AspNetCore.Mvc;

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    public class CategorywiseRptController : Controller
    {
        private POSDbContext db;
        public CategorywiseRptController(POSDbContext contextOptions)
        {
            db = contextOptions;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetCategRpt")]
        public IActionResult GetCategRpt(DateTime frmdate, DateTime todate, int Id, int companyId, int ParentCatId, int sourceId, string store_key, string sourcekey)
        {
            try
            {
                //SqlConnection sqlCon = new SqlConnection("server=(LocalDb)\\MSSQLLocalDB; database=Biz1POS;Trusted_Connection=True;");
                SqlConnection sqlCon = new SqlConnection("Data Source=tcp:b1zd0m.database.windows.net,1433;Initial Catalog=Biz1Retail;User Id=BizDom_Admin@b1zd0m;Password=BIzD0m@2K23");
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.CategoryWiseRpt", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@fromDate", frmdate));
                cmd.Parameters.Add(new SqlParameter("@toDate", todate));
                cmd.Parameters.Add(new SqlParameter("@storeId", Id));
                cmd.Parameters.Add(new SqlParameter("@storekey", store_key));
                cmd.Parameters.Add(new SqlParameter("@companyId", companyId));
                cmd.Parameters.Add(new SqlParameter("@parentcat", ParentCatId));
                cmd.Parameters.Add(new SqlParameter("@sourceId", sourceId));
                cmd.Parameters.Add(new SqlParameter("@sourceKey", sourcekey));
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                DataTable table = ds.Tables[0];

                var data = new
                {
                    Order = ds.Tables[0]
                };
                return Ok(data);
            }
            catch (Exception ex)
            {
                var error = new Exception(ex.Message, ex.InnerException);
                return Json(error);
            }
        }


    }
}

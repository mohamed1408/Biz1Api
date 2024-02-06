using Biz1BookPOS.Models;
using Biz1PosApi.Hubs;
using Biz1PosApi.Models;
using Biz1PosApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]
    public class MessageController : Controller
    {
        private static TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        private object table;
        private POSDbContext db;
        private ConnectionStringService connserve;
        public static IHostingEnvironment environment;
        public IConfiguration Configuration { get; }
        private IHubContext<ChatHub, IChatClient> hubContext;
        public MessageController(POSDbContext contextOptions, IHostingEnvironment _environment, IConfiguration configuration, ConnectionStringService _connserve, IHubContext<ChatHub, IChatClient> _hubContext)
        {
            db = contextOptions;
            Configuration = configuration;
            connserve = _connserve;
            hubContext = _hubContext;
            environment = _environment;
        }
        public IActionResult Index()
        {
            return View();
        }
        // GET: MessageController
        [HttpGet("GetClients")]
        public IActionResult GetClients()
        {
            try
            {
                var baseUri = $"{Request.Scheme}://{Request.Host}";
                return Json(UserHandler.Users);
            }
            catch(Exception ex)
            {
                return Json(new ErrorMessage(ex));
            }
        }

        [HttpGet("GetMessages")]
        public IActionResult GetMessages(int storeid)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.GetMessages", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@storeid", storeid));
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                DataTable table = ds.Tables[0];
                return Json(new SuccessMessageWData(table));
            }
            catch(Exception ex)
            {
                return Json(new ErrorMessage(ex));
            }
        }
        [HttpGet("GetChatIndex")]
        public IActionResult GetChatIndex(int companyid)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(Configuration.GetConnectionString("myconn"));
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("dbo.ChatIndex", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@companyid", companyid));
                DataSet ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
                sqlAdp.Fill(ds);
                DataTable table = ds.Tables[0];
                return Json(new SuccessMessageWData(table));
            }
            catch(Exception ex)
            {
                return Json(new ErrorMessage(ex));
            }
        }

        [HttpPost("SaveMessage")]
        public IActionResult SaveMessage([FromBody]Message message)
        {
            try
            {
                db.Messages.Add(message);
                db.SaveChanges();
                if(UserHandler.Users.Where(x => x.StoreId == message.StoreId).Any())
                {
                    string clientId = UserHandler.Users.Where(x => x.StoreId == message.StoreId).FirstOrDefault().ConnectionId;
                    hubContext.Clients.Client(clientId).NewMessage((int)message.StoreId, message);
                }
                return Json(new SuccessMessageWData(message.MessageId));
            }
            catch(Exception ex)
            {
                return Json(new ErrorMessage(ex));
            }
        }

        [HttpPost("SaveFileMessage"), DisableRequestSizeLimit]
        public IActionResult SaveFileMessage([FromForm]IFormFile file, [FromForm] IFormCollection collection)
        {
            try
            {
                Message message = JsonConvert.DeserializeObject<Message>(collection["message"][0]);
                Img img = new Img();
                img.Url = FileUpload(file, message.MessageType);
                db.Imgs.Add(img);
                db.SaveChanges();
                message.ImgId = img.ImgId;
                message.Content = file.FileName;
                SaveMessage(message);
                return Json(new SuccessMessageWData(""));
            }
            catch(Exception ex)
            {
                return Json(new ErrorMessage(ex));
            }
        }
        public string FileUpload(IFormFile file, int filetype)
        {
            try
            {
                string baseUri = $"{Request.Scheme}://{Request.Host}";
                string subdir = "/chat/";
                if(filetype == 2)
                {
                    subdir += "images/";
                }
                else if (filetype == 3)
                {
                    subdir += "audios/";
                }
                else if (filetype == 4)
                {
                    subdir += "videos/";
                }
                else if (filetype == 5)
                {
                    subdir += "documents/";
                }
                if (!Directory.Exists(environment.WebRootPath + subdir))
                {
                    Directory.CreateDirectory(environment.WebRootPath + subdir);
                }
                using (FileStream filestream = System.IO.File.Create(environment.WebRootPath + subdir + file.FileName))
                {
                    file.CopyTo(filestream);
                    filestream.Flush();
                    return baseUri + subdir + file.FileName;
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        // GET: MessageController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MessageController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MessageController/Create
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

        // GET: MessageController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MessageController/Edit/5
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

        // GET: MessageController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MessageController/Delete/5
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
    public class SuccessMessage
    {
        public SuccessMessage(string message = "Sucess") 
        {
            Status = 200;
            Message = message;
        }
        public int Status { get; set; }
        public string Message { get; set; }
    }
    public class SuccessMessageWData
    {
        public SuccessMessageWData(dynamic data) 
        {
            Status = 200;
            Data = data;
        }
        public int Status { get; set; }
        public dynamic Data { get; set; }
    }
    public class ErrorMessage
    {
        public ErrorMessage(Exception ex) 
        {
            Status = 500;
            Message = "Something went wrong. Contact your Admin";
            error = ex;
        }
        public int Status { get; set; }
        public string Message { get; set; }
        public Exception error { get; set; }
    }
}

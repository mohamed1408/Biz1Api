using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Biz1PosApi.Models;
using Biz1PosApi.Models.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Biz1BookPOS.Controllers
{
    [Route("api/[controller]")]
    public class DeliveryController : Controller
    {
        private POSDbContext db;
        //List<VariantGroup> variantGroups = null;
        public IConfiguration Configuration { get; }
        public DeliveryController(POSDbContext contextOptions, IConfiguration configuration)
        {
            db = contextOptions;
            Configuration = configuration;
        }

        // GET: api/<controller>
        [HttpGet("Get")]
        [EnableCors("AllowOrigin")]
        public IActionResult Get(int delvBoyId)
        {
            try
            {
                var delivery = db.Deliveries.Where(s => s.DeliveryBoyId == delvBoyId && s.StatusId != (int)DeliveryStatus.Delivered)
                .Select(s => new
                {
                    OrderId = s.OrderId,
                    DeliveryDateTime = s.Order.DeliveryDateTime,
                    Address = s.Order.Customer.Address,
                    City = s.Order.Customer.City,
                    StatusId = s.StatusId,
                    Id = s.Id,
                    Location = s.Location,
                    Note = s.Order.Note,
                    PhoneNo = s.Order.Customer.PhoneNo
                }).ToList();
                return Json(delivery);
            }
            catch (Exception e)
            {
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 0,
                    msg = "Something went wrong Contact our service provider"
                };
                return Json(error);
            }
        }
        [HttpPost("ChangeStatus")]
        [EnableCors("AllowOrigin")]
        public IActionResult ChangeStatus([FromBody]JObject data)
        {
            try
            {
                dynamic jsonObj = data;

                int statusId = jsonObj.statusId;
                int delvId = jsonObj.delvId;

                var delivery = db.Deliveries.Find(delvId);
                delivery.StatusId = statusId;
                db.Entry(delivery).State = EntityState.Modified;
                db.SaveChanges();
                var error = new
                {
                    status = 200,
                    msg = "Status Changed Successfully"
                };
                return Json(error);
            }
            catch (Exception e)
            {
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 0,
                    msg = "Something went wrong Contact our service provider"
                };
                return Json(error);
            }
        }
        [HttpGet("OrderItem")]
        [EnableCors("AllowOrigin")]
        public IActionResult OrderItem(int orderId)
        {
            try
            {
                var orderItems = db.OrderItems.Where(s => s.OrderId == orderId).ToList();
                return Json(orderItems);
            }
            catch (Exception e)
            {
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 0,
                    msg = "Something went wrong Contact our service provider"
                };
                return Json(error);
            }
        }
        [HttpGet("CompOrder")]
        [EnableCors("AllowOrigin")]
        public IActionResult CompOrder(int delvBoyId)
        {
            try
            {
                var compOrder = db.Deliveries.Where(s => s.DeliveryBoyId == delvBoyId
                && s.StatusId == (int)DeliveryStatus.Delivered)
                .Select(s => new
                {
                    OrderId = s.OrderId,
                    DeliveryDateTime = s.Order.DeliveryDateTime,
                    Address = s.Order.Customer.Address,
                    City = s.Order.Customer.City,
                    StatusId = s.StatusId,
                    Id = s.Id,
                    Location = s.Location,
                    Note = s.Order.Note,
                    PhoneNo = s.Order.Customer.PhoneNo
                }).OrderByDescending(s => s.DeliveryDateTime).Take(10).ToList();
                return Json(compOrder);
            }
            catch (Exception e)
            {
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 0,
                    msg = "Something went wrong Contact our service provider"
                };
                return Json(error);
            }
        }
        [HttpPost("AssignedForMe")]
        [EnableCors("AllowOrigin")]
        public IActionResult AssignedForMe([FromBody]JObject data)
        {
            try
            {
                dynamic jsonObj = data;
                int delvBoyId = jsonObj.delvBoyId;
                JArray items = jsonObj.items;
                dynamic delvArray = items.ToList();

                for (int i = 0; i < delvArray.Count; i++)
                {

                    int orderId = delvArray[i].orderId;

                    Delivery delivery = new Delivery();
                    delivery.DeliveryBoyId = delvBoyId;
                    delivery.OrderId = orderId;
                    delivery.StatusId = (int)DeliveryStatus.Assigned;
                    db.Deliveries.Add(delivery);
                    db.SaveChanges();
                }
                var error = new
                {
                    status = 200,
                    msg = "Assigned Successfully"
                };
                return Json(error);
            }
            catch (Exception e)
            {
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 0,
                    msg = "Something went wrong Contact our service provider"
                };
                return Json(error);
            }
        }
        [HttpGet("NewOrder")]
        [EnableCors("AllowOrigin")]
        public IActionResult NewOrder()
        {
            try
            {
                var exceptionList = db.Deliveries
                                   .Select(e => e.OrderId).ToList();

                var orders = db.Orders.Where(s => s.OrderTypeId == 3 && (s.OrderStatusId == (int)OrderStatus.Accepted ||
               s.OrderStatusId == (int)OrderStatus.Prepared ||
               s.OrderStatusId == (int)OrderStatus.Preparing))
                  .Where(e => !exceptionList.Contains(e.Id))
                .Select(s => new
                {
                    OrderId = s.Id,
                    DeliveryDateTime = s.DeliveryDateTime,
                    Address = s.Customer.Address,
                    City = s.Customer.City,
                    OrderStatusId = s.OrderStatusId,
                    PhoneNo = s.Customer.PhoneNo
                })
                .ToList();
                return Json(orders);
            }
            catch (Exception e)
            {
                var error = new
                {
                    error = new Exception(e.Message, e.InnerException),
                    status = 0,
                    msg = "Something went wrong Contact our service provider"
                };
                return Json(error);
            }
        }
    }
}

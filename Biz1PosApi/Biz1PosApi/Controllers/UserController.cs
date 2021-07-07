using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biz1BookPOS.Models;
using Biz1PosApi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Biz1PosApi.Controllers
{
    [Route("api/[controller]")]

    public class UserController : Controller
    {
        private POSDbContext db;
        public UserController(POSDbContext contextOptions)
        {
            db = contextOptions;
        }
        // GET: api/<controller>
        [HttpGet("Get")]

        public IActionResult Get(int companyId)
        {
            try
            {
                var data = new
                {
                    users = db.Users.Where(x => x.CompanyId == companyId).Include(x => x.Role).ToList(),
                    userstores = db.UserStores.Include(x => x.Store).ToList()
                };

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
        [HttpPost("Add")]
        [EnableCors("AllowOrigin")]
        public IActionResult Add([FromForm]string objData)
        {
            User userstrs = new User();
            try
            {
                dynamic userJson = JsonConvert.DeserializeObject(objData);
                User user = userJson.ToObject<User>();
                if (user.Id == 0)
                {
                    var users = db.Users.Where(x => x.CompanyId == user.CompanyId).ToList();
                    foreach (var usr in users)
                    {
                        if (user.Pin == usr.Pin)
                        {
                            var msg = new
                            {
                                status = 0,
                                msg = "Pin Already Taken Choose Another"
                            };
                            return Ok(msg);
                        }
                    }
                    user.AccountId = db.Accounts.Where(x => x.CompanyId == user.CompanyId).FirstOrDefault().Id;
                    db.Users.Add(user);
                    db.SaveChanges();
                    userstrs = user;
                    JArray userstoresObj = userJson.stores;
                    if (userstoresObj != null)
                    {
                        dynamic UserStoresJson = userstoresObj.ToList();
                        foreach (var item in UserStoresJson)
                        {
                            int itemId = item.ToObject<int>();
                            if (item != 0)
                            {
                                UserStores userstores = new UserStores();
                                userstores.UserId = user.Id;
                                userstores.StoreId = item;
                                db.UserStores.Add(userstores);
                                db.SaveChanges();
                            }

                        }
                    }
                }
                else
                {
                    user.AccountId = db.Accounts.Where(x => x.CompanyId == user.CompanyId).FirstOrDefault().Id;
                    var users1 = db.Users.Where(x => x.CompanyId == user.CompanyId && x.Id != user.Id).ToList();
                    foreach (var usr in users1)
                    {
                        if (user.Pin == usr.Pin)
                        {
                            var msg = new
                            {
                                status = 0,
                                msg = "Pin Already Taken Choose Another"
                            };
                            return Ok(msg);
                        }
                    }
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    userstrs = user;
                    JArray userstoresObj = userJson.stores;
                    if (userstoresObj != null)
                    {
                        IEnumerable<dynamic> UserStoresJson = userstoresObj.ToList();
                        foreach (var item in UserStoresJson)
                        {
                            int itemId = item.ToObject<int>();
                            var usrStrs = db.UserStores.Where(x => x.UserId == user.Id && x.StoreId == itemId).FirstOrDefault();
                            if (usrStrs == null)
                            {
                                UserStores userstores = new UserStores();
                                userstores.StoreId = itemId;
                                userstores.UserId = user.Id;
                                db.UserStores.Add(userstores);
                                db.SaveChanges();
                            }
                        }
                        var usrStrs1 = db.UserStores.Where(x => x.UserId == user.Id).ToList();
                        foreach (var str in usrStrs1)
                        {
                            var delustrs = UserStoresJson.Where(x => x == str.StoreId).FirstOrDefault();
                            if (delustrs == null)
                            {
                                var delUserStrs = db.UserStores.Find(str.Id);
                                db.UserStores.Remove(delUserStrs);
                                db.SaveChanges();
                            }
                        }
                    }

                }
                var mesg = new
                {
                    status = 200,
                    msg = "User Successfully Added or Updated",
                    user = userstrs
                };

                return Ok(mesg);
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

        [HttpGet("Delete")]
        [EnableCors("AllowOrigin")]
        public IActionResult Delete(int Id)
        {
            try
            {
                var userstores = db.UserStores.Where(x => x.UserId == Id).ToList();
                // var userroles = db.UserRoles.Where(x => x.Id ==  && x.CompanyId == 1).t
                if (userstores != null)
                {
                    foreach (var item in userstores)
                    {
                        var ustore = db.UserStores.Find(item.Id);
                        db.UserStores.Remove(ustore);
                    }
                }
                var user = db.Users.Find(Id);
                db.Users.Remove(user);
                db.SaveChanges();
                var error = new
                {
                    status = 200,
                    msg = "Delete Successfully"
                };
                return Json(error);
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
    }
}





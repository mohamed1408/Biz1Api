﻿using Biz1BookPOS.Models;
using Biz1PosApi.Controllers;
using Biz1PosApi.Hubs;
using Biz1PosApi.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Biz1PosApi.Services
{
    public class UPOrderService : BackgroundService
    {
        private readonly IHubContext<UrbanPiperHub, IHubClient> uhubContext;
        private readonly Channel<UPRawPayload> channel;
        private readonly ILogger<UPOrderService> logger;
        private readonly IServiceProvider provider;
        public UPOrderService(
            Channel<UPRawPayload> channel,
            ILogger<UPOrderService> logger,
            IServiceProvider provider,
            IHubContext<UrbanPiperHub, IHubClient> uhubContext)
        {
            this.channel = channel;
            this.logger = logger;
            this.provider = provider;
            this.uhubContext = uhubContext;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!channel.Reader.Completion.IsCompleted)
            {
                logger.LogInformation("Service is live");
                logger.LogInformation("Consuming...");
                var _payload = await channel.Reader.ReadAsync();
                _payload.retry_count = _payload.retry_count + 1;
                Store store = null;
                try
                {
                    if(_payload.PayloadType == "place_order")
                    {
                        using (var scope = provider.CreateScope())
                        {
                            var db = scope.ServiceProvider.GetRequiredService<POSDbContext>();

                            dynamic Json = JsonConvert.DeserializeObject(_payload.Payload);
                            int storeId = 0;
                            if (!Json.order.store.merchant_ref_id.ToString().Contains("-"))
                            {
                                storeId = Json.order.store.merchant_ref_id;
                            }
                            else
                            {
                                string[] strlist = Json.order.store.merchant_ref_id.ToString().Split("-");
                                //upstore.BrandId = Int16.Parse(strlist[0]);
                                storeId = short.Parse(strlist[1]);
                            }
                            store = await db.Stores.FindAsync(storeId);
                            int companyId = store.CompanyId;
                            string UPOrderId = Json.order.details.id.ToString();
                            string platform = Json.order.details.channel.ToString();
                            string storename = store.Name;
                            Json.order.details.platformorderid = Json.order.details.ext_platforms[0].id;
                            int i = 0;
                            foreach (var orderitem in Json.order.items)
                            {
                                i++;
                                orderitem.refid = orderitem.merchant_id;
                                foreach (var option in orderitem.options_to_add)
                                {
                                    orderitem.refid += i.ToString() + "_" + option.merchant_id;
                                }
                                foreach (var option in orderitem.options_to_add)
                                {
                                    option.itemrefid = orderitem.refid;
                                }
                            }
                            var statusDetail = new
                            {
                                accepted = 0,
                                foodready = 0,
                                dispatched = 0,
                                delivered = 0
                            };
                            UPOrder uPOrder = new UPOrder();
                            uPOrder.StoreId = storeId;
                            uPOrder.Json = JsonConvert.SerializeObject(Json);
                            uPOrder.UPOrderId = Json.order.details.id;
                            uPOrder.OrderStatusId = 0;
                            uPOrder.OrderedDateTime = UrbanPiperController.UnixTimeStampToDateTime(Json.order.details.created.ToObject<Int64>());
                            uPOrder.AcceptedTimeStamp = JsonConvert.SerializeObject(statusDetail);
                            await db.UPOrders.AddAsync(uPOrder);
                            await db.SaveChangesAsync();

                            await db.Database.ExecuteSqlCommandAsync($"EXECUTE SaveUPOrder_ {uPOrder.UPOrderId}");
                            await uhubContext.Clients.All.NewOrder(platform, uPOrder.UPOrderId, uPOrder.StoreId);
                        }
                    }
                    else if(_payload.PayloadType == "order_status_update")
                    {
                        using (var scope = provider.CreateScope())
                        {
                            var db = scope.ServiceProvider.GetRequiredService<POSDbContext>();

                            dynamic json = JsonConvert.DeserializeObject(_payload.Payload);
                            long uporderid = json.order_id;
                            int orderstatusid = 0;
                            int storeid = 0;
                            if (json.store_id.ToString().Contains("-"))
                            {
                                string[] strlist = json.store_id.ToString().Split("-");
                                //upstore.BrandId = Int16.Parse(strlist[0]);
                                storeid = short.Parse(strlist[1]);
                            }
                            else
                            {
                                storeid = json.store_id;
                            }

                            Order order = await db.Orders.Where(x => x.UPOrderId == uporderid).FirstOrDefaultAsync();
                            UPOrder upOrder = await db.UPOrders.Where(x => x.UPOrderId == uporderid).FirstOrDefaultAsync();
                            store = await db.Stores.FindAsync(storeid);

                            dynamic statusDetail = JsonConvert.DeserializeObject(upOrder.AcceptedTimeStamp);
                            order.PreviousStatusId = order.OrderStatusId;

                            if (json.new_state == "Acknowledged")
                            {
                                orderstatusid = 1;
                                statusDetail.accepted = json.timestamp_unix;
                            }
                            else if (json.new_state == "Food Ready")
                            {
                                orderstatusid = 3;
                                statusDetail.foodready = json.timestamp_unix;
                            }
                            else if (json.new_state == "Dispatched")
                            {
                                orderstatusid = 4;
                                statusDetail.dispatched = json.timestamp_unix;
                            }
                            else if (json.new_state == "Completed")
                            {
                                orderstatusid = 5;
                                statusDetail.delivered = json.timestamp_unix;
                            }
                            else if (json.new_state == "Cancelled")
                            {
                                orderstatusid = -1;
                                statusDetail.delivered = json.timestamp_unix;
                            }
                            if(orderstatusid == -1 || orderstatusid > order.OrderStatusId)
                            {
                                order.OrderStatusId = orderstatusid;
                                upOrder.OrderStatusId = orderstatusid;
                                upOrder.AcceptedTimeStamp = JsonConvert.SerializeObject(statusDetail);

                                db.Entry(order).State = EntityState.Modified;
                                db.Entry(upOrder).State = EntityState.Modified;

                                await db.SaveChangesAsync();
                                await uhubContext.Clients.All.OrderUpdate(upOrder.UPOrderId, upOrder.StoreId);
                            }
                        }
                    }
                    else if (_payload.PayloadType == "rider_status_update")
                    {
                        using (var scope = provider.CreateScope())
                        {
                            var db = scope.ServiceProvider.GetRequiredService<POSDbContext>();

                            dynamic json = JsonConvert.DeserializeObject(_payload.Payload);

                            long uporderid = json.order_id;
                            int storeid = 0;
                            if (json.store.ref_id.ToString().Contains("-"))
                            {
                                String[] strlist = json.store.ref_id.ToString().Split("-");
                                //upstore.BrandId = Int16.Parse(strlist[0]);
                                storeid = Int16.Parse(strlist[1]);
                            }
                            else
                            {
                                storeid = json.store.ref_id;
                            }

                            UPOrder upOrder = await db.UPOrders.Where(x => x.UPOrderId == uporderid).FirstOrDefaultAsync();
                            upOrder.RiderDetails = _payload.Payload;
                            db.Entry(upOrder).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                            await uhubContext.Clients.All.OrderUpdate(upOrder.UPOrderId, upOrder.StoreId);
                        }
                    }
                }
                catch (Exception e)
                {
                    logger.LogError(e, "notification failed");
                    //using (var scope = provider.CreateScope())
                    //{
                    //    int[] cool_down_counts = { 3, 6 };
                    //    var db = scope.ServiceProvider.GetRequiredService<POSDbContext>();
                    //    logger.LogError(e, "notification failed");
                    //    if(_payload.retry_count == 1)
                    //    {
                    //        OrderLog orderLog = new OrderLog();
                    //        orderLog.CompanyId = store.CompanyId;
                    //        orderLog.StoreId = store.Id;
                    //        orderLog.Error = JsonConvert.SerializeObject(e);
                    //        orderLog.LoggedDateTime = DateTime.Now;
                    //        orderLog.Payload = _payload.Payload;
                    //        db.OrderLogs.Add(orderLog);
                    //        await db.SaveChangesAsync();
                    //    }
                    //    if(_payload.retry_count < 9 && e.Message.ToLower().Contains("timeout"))
                    //    {
                    //        if (cool_down_counts.Contains(_payload.retry_count))
                    //        {
                    //            await Task.Delay(10000);
                    //            await channel.Writer.WriteAsync(_payload);
                    //        }
                    //        else
                    //        {
                    //            await channel.Writer.WriteAsync(_payload);
                    //        }
                    //    }
                    //}
                }
            }
        }
    }
}
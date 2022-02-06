using Biz1BookPOS.Models;
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
                logger.LogInformation("Serivece is live");
                logger.LogInformation("Consuming...");
                var _payload = await channel.Reader.ReadAsync();
                try
                {
                    using (var scope = provider.CreateScope())
                    {
                        var db = scope.ServiceProvider.GetRequiredService<POSDbContext>();

                        dynamic Json = JsonConvert.DeserializeObject(_payload.Payload);
                        int storeId = 0;
                        if (Json.order.store.merchant_ref_id.ToString().Contains("-"))
                        {
                            String[] strlist = Json.order.store.merchant_ref_id.ToString().Split("-");
                            //upstore.BrandId = Int16.Parse(strlist[0]);
                            storeId = Int16.Parse(strlist[1]);
                        }
                        else
                        {
                            storeId = Json.order.store.merchant_ref_id;
                        }
                        Store store = await db.Stores.FindAsync(storeId);
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
                catch (Exception e)
                {
                    logger.LogError(e, "notification failed");
                }
            }
        }
    }
}

using Biz1BookPOS.Models;
using Biz1PosApi.Hubs;
using Biz1PosApi.Models;
using Biz1Retail_API.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Biz1PosApi.Services
{
    public class OrderMigrateService : BackgroundService
    {
        private readonly Channel<OrderPckg> channel;
        private readonly ILogger<OrderMigrateService> logger;
        private TempDbContext db;
        public OrderMigrateService(Channel<OrderPckg> channel,ILogger<OrderMigrateService> logger)
        {
            this.channel = channel;
            this.logger = logger;
            db = DbContextFactory.Create("myconn");
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!channel.Reader.Completion.IsCompleted)
            {
                OrderPckg pckg = await channel.Reader.ReadAsync();
                logger.LogInformation("[INCOMING] orderid: " + pckg.odrs.OdrsId.ToString() + " itemcount: " + pckg.otms.Count.ToString());
                Odrs order = pckg.odrs;
                order.OdrsId = 0;
                db.Odrs.Add(order);
                db.SaveChanges();
                List<Otms> otms = pckg.otms;
                otms.ForEach(o => { o.OtmsId = 0; o.ob = order.OdrsId; o.n = "-1"; });
                db.Otms.AddRange(otms);
                db.SaveChanges();
                logger.LogInformation("[SAVED] orderid: " + order.OdrsId.ToString() + " itemcount: " + pckg.otms.Count.ToString());
            }
        }
    }
}

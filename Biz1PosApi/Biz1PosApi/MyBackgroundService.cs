using Biz1PosApi.Controllers;
using Biz1PosApi.Hubs;
using Biz1PosApi.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Biz1PosApi
{
    public class MyBackgroundService : BackgroundService
    {
        //private readonly OrderRepository OrderRepository;
        private IHubContext<UrbanPiperHub, IHubClient> _uhubContext;
        private readonly ILogger<MyBackgroundService> _logger;
        public IConfiguration Configuration { get; }
        public MyBackgroundService(IConfiguration configuration, ILogger<MyBackgroundService> logger, IHubContext<UrbanPiperHub, IHubClient> uhubContext)
        {
            _logger = logger;
            _uhubContext = uhubContext;
            Configuration = configuration;
            //OrderRepository = orderRepository;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var c = Channel.CreateUnbounded<UPOrderPayload>();

            if (stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Stopping From MyBackgroundService");
            }
            while (!stoppingToken.IsCancellationRequested)
            {
                if(c.Reader.TryRead(out UPOrderPayload payload))
                {
                    //_logger.LogInformation(payload.Platform + "-" + payload.UPOrderId.ToString());
                    UrbanPiperController.SaveOrder(payload.UPOrderId, Configuration.GetConnectionString("myconn"));
                    //_logger.LogInformation("Completed SaveOrder Task");
                    await _uhubContext.Clients.All.NewOrder(payload.Platform, payload.UPOrderId, payload.StoreId);
                }
                _logger.LogInformation("From MyBackgroundService");
            }
            //return Task.CompletedTask;
        }
    }
}

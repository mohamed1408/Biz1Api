﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Biz1PosApi.Hubs;
using Microsoft.AspNetCore.SignalR;
using Biz1BookPOS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.AspNetCore.Internal;
//using Microsoft.Owin.Cors;
//using Microsoft.Owin;
//using Owin;
//using Microsoft.AspNet.SignalR;
//using Microsoft.Owin.Cors;
using Microsoft.AspNetCore.Owin;
using Biz1PosApi.Models;
using Biz1PosApi.Services;
using System.Threading.Channels;

namespace Biz1PosApi
{
    public class Startup
    {
        private IConfiguration _config;
        public Startup(IConfiguration config, IConfiguration configuration)
        {
            _config = config;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        //private readonly IHubContext<UrbanPiperHub, IHubClient> _hubContext;
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton<OrderRepository>(_ => new OrderRepository(Configuration.GetConnectionString("myconn")));
            //services.AddSingleton<ILoggerService, OrderService>();
            //services.AddHostedService<OrderService>();
            services.AddTransient<ConnectionStringService>();
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => {
                    options.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
                c.AddPolicy("AllowChathub", options => {
                    options.WithOrigins("http://localhost:4200")
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
            });
            services.AddDbContext<POSDbContext>(item => item.UseSqlServer(Configuration.GetConnectionString("myconn")));
            services.AddDbContext<TempDbContext>();
            Dictionary<string, string> ConnStrings = new Dictionary<string, string>
            {
                { "myconn", "Server=tcp:b1zd0m.database.windows.net,1433;Initial Catalog=Biz1Retail;Persist Security Info=False;User ID=BizDom_Admin;Password=BIzD0m@2K23;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=1000;" },
                { "logout", "Server=tcp:logout.database.windows.net,1433;Initial Catalog=Biz1Retail;Persist Security Info=False;User ID=CloudSAe0c934ac;Password=BizDom##;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=1000;" },
                { "hydconn", "Server=tcp:fb-hyd.database.windows.net,1433;Initial Catalog=fb-hyd;Persist Security Info=False;User ID=Fb_Admin-Hyd;Password=Master@0229;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=1000;" },
                { "bangconn", "Server=tcp:fb-karnataka.database.windows.net,1433;Initial Catalog=fb-karnataka;Persist Security Info=False;User ID=Fb_Admin_Kar;Password=Master@0229;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=1000;" }
            };
            DbContextFactory.SetConnString(ConnStrings);
            services.AddHostedService<UPOrderService>();
            services.AddSingleton(Channel.CreateUnbounded<UPRawPayload>());
            services.AddHostedService<OrderMigrateService>();
            services.AddSingleton(Channel.CreateUnbounded<OrderPckg>());
            services.AddSingleton<Student>();
            services.AddSignalR();
            services
            .AddMvc(o =>
            {
                o.RespectBrowserAcceptHeader = true;
            })
            //.AddXmlDataContractSerializerFormatters()
            .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver());
            ///JWT/////
            string securityKey = _config["Jwt:Key"];
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //what to validate
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    //setup validate data
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidAudience = "readers",
                    IssuerSigningKey = symmetricSecurityKey
                };
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddFile("wwwroot/logs/myapp-{Date}.txt");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseCors(builder => builder
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .SetIsOriginAllowed(_ => true)
                            .AllowCredentials()
                        );
            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chatHub");
                routes.MapHub<UrbanPiperHub>("/uphub");
            });
            //hubapp.Map("/chatHub", map =>
            //{
            //    map.UseCors(CorsOptions.AllowAll);
            //    var hubConfiguration = new HubConfiguration
            //    { };
            //    map.RunSignalR(hubConfiguration);
            //}); 
            app.UseMvc();
        }
    }
}
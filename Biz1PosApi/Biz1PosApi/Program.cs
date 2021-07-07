using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
//using SocketIOClient;
// using Quobject.SocketIoClientDotNet.Client;

namespace Biz1PosApi
{
    public class Program
    {
        // static Socket socket;
        public static void Main(string[] args)
        {
            //try
            //{
            //    socket = IO.Socket("https://biz1socket.azurewebsites.net/");
            //    //socket = IO.Socket("http://localhost:3000/");
            //}
            //catch (Exception e)
            //{
            //    //MessageBox.Show(e.ToString(), "Something Went Wrong!!");
            //    //Application.Exit();
            //}
            CreateWebHostBuilder(args).Build().Run();
        }
        //public static void emit_order(string order, string room)
        //{
        //    socket.Emit("join", room);
        //    socket.Emit("order", order, room);
        //}
        //orderstatuschange
        //public static void change_order_status(string room, string orderid, int orderstatusid, string timestamp)
        //{
        //    socket.Emit("join", room);
        //    socket.Emit("status_change", orderid, orderstatusid, timestamp);
        //}

        //public static void update_riderstatus(string room, string orderid, dynamic json)
        //{
        //    socket.Emit("join", room);
        //    socket.Emit("rider_status", orderid, json);
        //}

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}

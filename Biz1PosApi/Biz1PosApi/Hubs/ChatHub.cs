using Biz1BookPOS.Models;
using Biz1PosApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Biz1PosApi.Hubs
{
    public class ChatUser
    {
        public ChatUser(int StoreId, string ConnectionId) 
        {
            this.StoreId = StoreId;
            this.ConnectionId = ConnectionId;
        }
        public int StoreId { get; set; }
        public string ConnectionId { get; set; }
    }
    public static class UserHandler
    {
        public static List<ChatUser> Users = new List<ChatUser>();
    }
    public class ChatHub : Hub<IChatClient>
    {
        private POSDbContext db;
        public List<dynamic> orders { get; set; }
        public ChatHub(POSDbContext contextOptions)
        {
            orders = new List<dynamic>();
            db = contextOptions;
        }
        public override Task OnConnectedAsync()
        {
            //UserHandler.ConnectedIds.Add(Context.ConnectionId);
            return base.OnConnectedAsync();
        }
        public void Register(int storeid)
        {
            if(UserHandler.Users.Where(x => x.StoreId == storeid).Any())
            {
                ChatUser user = UserHandler.Users.Where(x => x.StoreId == storeid).FirstOrDefault();
                UserHandler.Users.Remove(user);
            }
            UserHandler.Users.Add(new ChatUser(storeid, Context.ConnectionId));
        }
        //public override Task OnReconnectedAsync(Exception exception)
        //{
        //    ChatUser user = UserHandler.Users.Where(x => x.ConnectionId == Context.ConnectionId).FirstOrDefault();
        //    UserHandler.Users.Remove(user);
        //    return base.OnDisconnectedAsync(exception);
        //}
        public override Task OnDisconnectedAsync(Exception exception)
        {
            ChatUser user = UserHandler.Users.Where(x => x.ConnectionId == Context.ConnectionId).FirstOrDefault();
            UserHandler.Users.Remove(user);
            return base.OnDisconnectedAsync(exception);
        }
        public async Task Delivered(int messageid)
        {
            Message message = await db.Messages.FindAsync(messageid);
            message.RecieverStatus = 1;
            db.Entry(message).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
        public async Task Read(int messageid)
        {
            Message message = await db.Messages.FindAsync(messageid);
            message.RecieverStatus = 2;
            db.Entry(message).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}

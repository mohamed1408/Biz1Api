using Biz1BookPOS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1PosApi.Hubs
{
    public class ChatHub : Hub
    {
        private POSDbContext db;
        public List<dynamic> orders { get; set; }
        public ChatHub(POSDbContext contextOptions )
        {
            orders = new List<dynamic>();
            db = contextOptions;
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public async Task JoinRoom(string roomName, int storeid)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            await Clients.Group(roomName).SendAsync("joinmessage",$"{Context.ConnectionId} joined {roomName}");
        }
        public async Task GetStoreOrders(string roomName, int storeid)
        {
            var orders = db.UPOrders.Where(x => x.StoreId == storeid).ToList();
            await Clients.Group(roomName).SendAsync("order", orders);
        }
        public async Task GetStoreKOTs(string roomName, int storeid)
        {
            //var orders = db.UPOrders.Where(x => x.StoreId == storeid).ToList();
            List<KOT> kOTs = db.KOTs.Where(x => x.StoreId == storeid && x.KOTStatusId != 5 && x.KOTStatusId != -1).ToList(); ;
            foreach (KOT kot in kOTs)
            {
                kot.OrderItems = JsonConvert.SerializeObject(db.OrderItems.Where(x => x.KOTId == kot.Id).ToList());
            }
            await Clients.Group(roomName).SendAsync("kot", kOTs);
        }
        public async Task RoomChat(string roomName)
        {
            await Clients.Group(roomName).SendAsync("ReceiveMessage", "user", "message");
        }
        public void storeorder(Object[] order)
        {
            orders.Add(order[0]);
        }
        public Task LeaveRoom(string roomName)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }
        public List<dynamic> getorders()
        {
            return orders;
        }
    }
}

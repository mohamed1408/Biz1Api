using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1PosApi.Hubs
{
    public class UrbanPiperHub: Hub<IHubClient>
    {
        public void NewOrder(string platform, int uporderid, int storeid)
        {
            Clients.All.NewOrder(platform, uporderid, storeid);
        }
        public async Task joinroom(string room)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, room);
            await Clients.Group(room).JoinMessage($"{Context.ConnectionId} joined {room}");
        }
    }
}

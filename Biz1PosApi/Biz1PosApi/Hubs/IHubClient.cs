using Biz1PosApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1PosApi.Hubs
{
    public interface IHubClient
    {
        Task Announce(string message);
        Task NewOrder(string platform, int UPOrderId, int storeid);
        Task TestEvent(ValueTask<UPOrderPayload> uPOrderPayload);
        Task OrderUpdate(long UPOrderId, int storeid);
        Task RiderStatus(int UPOrderId, int storeid);
        Task JoinMessage(string message);
        Task ConnectedUsers(List<string> users);
        Task DeliveryOrderUpdate(int fromstore, int tostore, string invoiceno, string action, int orderid);
        Task NewFBOrder(int storeid, int orderid, string action);
    }
    public interface IChatClient
    {
        Task NewMessage(int storeid, Message message);
        Task Delivered(int MessaegId);
        Task Read(int MessaegId);
    }
}

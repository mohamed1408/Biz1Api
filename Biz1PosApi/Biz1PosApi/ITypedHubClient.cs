using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1PosApi
{
    public interface ITypedHubClient
    {
        Task BroadcastMessage(string type, string payload);
        Task OnConnected(string type, string payload);
    }
}

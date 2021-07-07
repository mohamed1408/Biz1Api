using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1PosApi.Models.Enum
{
    public enum OrderItemStatus
    {
        Canceled  = -1,
        Accepted  =  1,
        Started   =  2,
        Completed =  3
    }
}

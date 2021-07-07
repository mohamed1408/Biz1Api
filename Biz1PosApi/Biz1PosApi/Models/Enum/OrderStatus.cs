using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1PosApi.Models.Enum
{
    public enum OrderStatus
    {
        Canceled  = -1,
        Pending   =  0,
        Accepted  =  1,
        Preparing =  2,
        Prepared  =  3,  //(Food Ready for swiggy zomato)
        Completed =  4  //(Served for Dinein, Delivered for Delivery, Take away )
    }
}

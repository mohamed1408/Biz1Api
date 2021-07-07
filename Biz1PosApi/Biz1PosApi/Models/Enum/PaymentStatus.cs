using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1PosApi.Models.Enum
{
    public enum PaymentStatus
    {
        Canceled = -1,
        Pending = 1,
        Completed = 2
    }
}

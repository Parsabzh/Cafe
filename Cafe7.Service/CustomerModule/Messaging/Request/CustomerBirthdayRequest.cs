using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe7.Infrastructure.Service.RequsetResponseMessaging.Request;

namespace Cafe7.Service.CustomerModule.Messaging.Request
{
   public class CustomerBirthdayRequest:RequestBase
    {
        public int Day { get; set; }
        public int Month { get; set; }
    }
}

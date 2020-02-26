using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe7.Infrastructure.Service.RequsetResponseMessaging.Request;

namespace Cafe7.Service.InvoiceModule.Messaging.Request
{
   public class InvoiceGetByTableNumberRequest:RequestBase
    {
        public int TableNumber { get; set; }
    }
}

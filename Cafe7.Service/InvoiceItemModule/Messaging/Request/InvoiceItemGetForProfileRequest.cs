﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe7.Infrastructure.Service.RequsetResponseMessaging.Request;

namespace Cafe7.Service.InvoiceItemModule.Messaging.Request
{
   public class InvoiceItemGetForProfileRequest:RequestBase
    {
        public int Registartion { get; set; }

    }
}
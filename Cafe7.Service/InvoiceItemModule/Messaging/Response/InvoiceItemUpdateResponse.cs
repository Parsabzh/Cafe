﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe7.Infrastructure.Service.RequsetResponseMessaging.Response;
using Cafe7.Service.InvoiceItemModule.ViewModel;
using Cafe7.Service.InvoiceModule.ViewModels;

namespace Cafe7.Service.InvoiceItemModule.Messaging.Response
{
  public  class InvoiceItemUpdateResponse:ResponseBase<IEnumerable<InvoiceItemViewModel>>
    {
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe7.Infrastructure.Service.RequsetResponseMessaging.Response;
using Cafe7.Service.InvoiceItemModule.ViewModel;
using Cafe7.Service.ProductModule.ViewModel;

namespace Cafe7.Service.InvoiceItemModule.Messaging.Response
{
   public class InvoiceItemGetByDateResponse:ResponseBase<IEnumerable<InvoiceItemReportViewModel>>
    {
        public string TotalOff { get; set; }
    }
}

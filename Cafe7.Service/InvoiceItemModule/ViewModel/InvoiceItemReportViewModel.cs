using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe7.Infrastructure.Service;

namespace Cafe7.Service.InvoiceItemModule.ViewModel
{
  public class InvoiceItemReportViewModel
    {
        public string PName { get; set; }
        public int Qty { get; set; }

        public decimal Price { get; set; }

        public string Total { get; set; }
    }
}

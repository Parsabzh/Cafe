using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Cafe7.Infrastructure.Service;

namespace Cafe7.Service.InvoiceItemModule.ViewModel
{
   public class InvoiceItemUpdateViewModel
    {
        public List<InvoiceItemViewModel> InvoiceItemViewModels { get; set; }
        public int InvoiceId { get; set; }
    }
}

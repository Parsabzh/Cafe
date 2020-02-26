using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe7.Infrastructure.Service;
using Cafe7.Model.Customer;
using Cafe7.Model.Invoice;
using Cafe7.Model.People;

namespace Cafe7.Service.InvoiceModule.ViewModels
{
   public class InvoiceViewModel:ServiceBase<int>
    {
        public string Date { get; set; }
   

        public int TableNumber { get; set; }
        public string Total { get; set; }

        public int InvoiceNumber { get; set; }
      
    }
}

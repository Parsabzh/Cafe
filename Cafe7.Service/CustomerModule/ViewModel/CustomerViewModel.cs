using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe7.Infrastructure.Service;
using Cafe7.Service.InvoiceItemModule.ViewModel;
using Cafe7.Service.PeopleModule.ViewModel;

namespace Cafe7.Service.CustomerModule.ViewModel
{
   public class CustomerViewModel:PeopleViewModel
    {
        public int RegistrationNumber { get; set; }
        public List<InvoiceItemViewModel> InvoiceItemViews { get; set; }
    }
}

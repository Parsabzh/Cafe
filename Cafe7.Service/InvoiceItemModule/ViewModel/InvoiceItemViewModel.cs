using Cafe7.Infrastructure.Service;
using Cafe7.Service.CustomerModule.ViewModel;
using Cafe7.Service.InvoiceModule.ViewModels;

namespace Cafe7.Service.InvoiceItemModule.ViewModel
{
  public  class InvoiceItemViewModel:ServiceBase<int>
    {
  
        public int InvoiceId { get; set; }
  
        public decimal Price { get; set; }

        public int Qty { get; set; }

        public string PName { get; set; }
        public int CustomerId { get; set; }

        public int Registration { get; set; }


        public CustomerViewModel Customer { get; set; }

        public InvoiceViewModel Invoice { get; set; }
       
    }
}

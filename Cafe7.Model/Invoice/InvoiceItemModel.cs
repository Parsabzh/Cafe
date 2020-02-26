using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe7.Infrastructure.Domain;
using Cafe7.Model.Customer;
using Cafe7.Model.Product;

namespace Cafe7.Model.Invoice
{
    [Table("InvoiceItem",Schema = "Store")]
   public class InvoiceItemModel:ModelBase<int>
    {
      
       
        
       
        public int InvoiceId { get; set; }
        public int CustomerId { get; set; }
        
        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public int Qty { get; set; }


        public string PName { get; set; }

        public CustomerModel Customer { get; set; }
        public  InvoiceModel Invoice { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe7.Infrastructure.Domain;
using Cafe7.Infrastructure.ValidationResource;
using Cafe7.Model.Customer;
using Cafe7.Model.People;

namespace Cafe7.Model.Invoice
{
    [Table("Invoice",Schema ="Store" )]
  public class InvoiceModel:ModelBase<int>
    {
        
        public string Date { get; set; }

       
        

        [RegularExpression(@"^[0-9]{1,10}$", ErrorMessageResourceType = typeof(ValidationResource), ErrorMessageResourceName = "RegularExpression")]

        public int TableNumber { get; set; }

        public string Total { get; set; }


        public int InvoiceNumber { get; set; }

    }
}

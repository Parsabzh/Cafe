using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe7.Infrastructure.PeopleResource;
using Cafe7.Infrastructure.ValidationResource;
using Cafe7.Model.Invoice;
using Cafe7.Model.People;

namespace Cafe7.Model.Customer
{
    [Table("Customer", Schema = "PeopleInfo")]
    public class CustomerModel : PeopleModel
    {

        [Display(ResourceType = typeof(PeopleResource), Name = "RegistrationNumber")]
        [Required(ErrorMessageResourceType = typeof(ValidationResource),ErrorMessageResourceName = "Required")]
        [RegularExpression(@"^[0-9]{1,10}$",ErrorMessageResourceType = typeof(ValidationResource),ErrorMessageResourceName = "RegularExpression")]

        public int RegistrationNumber { get; set; }

        //public List<InvoiceItemModel> InvoiceItems { get; set; }
    }
}

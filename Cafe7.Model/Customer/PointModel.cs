using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe7.Infrastructure.Domain;
using Cafe7.Infrastructure.ValidationResource;

namespace Cafe7.Model.Customer
{
    [Table("Point",Schema = "PeopleInfo")]
  public  class PointModel:ModelBase<int>
    {
        //[RegularExpression(@"^[0-9][.,][0-9]{1,100}$", ErrorMessageResourceType = typeof(ValidationResource), ErrorMessageResourceName = "RegularExpression")]
        public decimal Point { get; set; }

    }
}

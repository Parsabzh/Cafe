using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe7.Infrastructure.Domain;
using Cafe7.Infrastructure.PeopleResource;
using Cafe7.Infrastructure.ValidationResource;

namespace Cafe7.Model.People
{
    [Table("People", Schema = "PeoplInfo")]
    public abstract class PeopleModel : ModelBase<int>
    {
        [Display(ResourceType = typeof(PeopleResource), Name = "Name")]
        [Required(ErrorMessageResourceType = typeof(ValidationResource),ErrorMessageResourceName = "Required")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(PeopleResource),Name = "Family")]
        [Required(ErrorMessageResourceType =typeof(ValidationResource),ErrorMessageResourceName = "Required")]
        public string Family { get; set; }

        [Display(ResourceType = typeof(PeopleResource),Name = "NationalId")]
        [Required(ErrorMessageResourceType = typeof(ValidationResource),ErrorMessageResourceName="Required")]
        [RegularExpression(@"^[0-9]{10}$",ErrorMessageResourceType = typeof(ValidationResource),ErrorMessageResourceName = "RegularExpression")]
        public string NationalId { get; set; }

        [Display(ResourceType = typeof(PeopleResource), Name = "Mobile")]
        [Required(ErrorMessageResourceType = typeof(ValidationResource), ErrorMessageResourceName = "Required")]
        public string Mobile { get; set; }

        [Display(ResourceType = typeof(PeopleResource), Name = "Birthday")]
        [Required(ErrorMessageResourceType = typeof(ValidationResource), ErrorMessageResourceName = "Required")]
        public string Birthday { get; set; }

        public DateTime Date { get; set; }

        public string Address { get; set; }

       [Display(ResourceType = typeof(PeopleResource),Name = "Date")]
        [RegularExpression(@"^[0-9]{1,2}$",ErrorMessageResourceType = typeof(ValidationResource),ErrorMessageResourceName = "RegularExpression")]
        public int Day { get; set; }


        [Display(ResourceType = typeof(PeopleResource), Name = "Month")]
        [RegularExpression(@"^[0-9]{1,2}$", ErrorMessageResourceType = typeof(ValidationResource), ErrorMessageResourceName = "RegularExpression")]
        public int Month { get; set; }

        

    }
}

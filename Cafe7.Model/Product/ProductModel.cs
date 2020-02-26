using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe7.Infrastructure.Domain;
using Cafe7.Infrastructure.Localize.ProductResource;
using Cafe7.Infrastructure.ValidationResource;

namespace Cafe7.Model.Product
{
    [Table("Product",Schema = "Store")]
   public class ProductModel:ModelBase<int>
    {
        [Display(ResourceType = typeof(ProductResource),Name = "Name")]
        [Required(ErrorMessageResourceType = typeof(ValidationResource),ErrorMessageResourceName = "Required")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(ProductResource), Name = "Price")]
        [Required(ErrorMessageResourceType = typeof(ValidationResource), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"^[1-9]\d*(\.\d+)?$", ErrorMessageResourceType = typeof(ValidationResource), ErrorMessageResourceName = "RegularExpression")]

        [Column(TypeName = "money")]
        public decimal Price { get; set; }
      
        [Display(ResourceType = typeof(ProductResource), Name = "Qty")]
        [Required(ErrorMessageResourceType = typeof(ValidationResource), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"^[0-9]{1,100000}$", ErrorMessageResourceType = typeof(ValidationResource), ErrorMessageResourceName = "RegularExpression")]

        public int Qty { get; set; }

        [Display(ResourceType = typeof(ProductResource), Name = "Type")]
        [Required(ErrorMessageResourceType = typeof(ValidationResource), ErrorMessageResourceName = "Required")]

        public string Type { get; set; }

    }
}

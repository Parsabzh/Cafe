using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe7.Infrastructure.Localize.ProductResource;
using Cafe7.Infrastructure.Service;

namespace Cafe7.Service.ProductModule.ViewModel
{
   public class ProductViewModel:ServiceBase<int>
    {
        [Display(ResourceType = typeof(ProductResource),Name = "Name")]
        public string Name { get; set; }
        
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public string Type { get; set; }
    }
}

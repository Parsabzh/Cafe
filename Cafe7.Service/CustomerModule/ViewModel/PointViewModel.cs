using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe7.Infrastructure.Service;

namespace Cafe7.Service.CustomerModule.ViewModel
{
   public class PointViewModel:ServiceBase<int>
    {
        public decimal Point { get; set; }
    }
}

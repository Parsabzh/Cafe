using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe7.Model.Customer;

namespace Cafe7.Service.CustomerModule.ViewModel
{
  public class ScoreViewModel
    {  
        public int CustomerId { get; set; }
        public double LastScore { get; set; }
        public double Total { get; set; }

        
        public CustomerModel Customer { get; set; }
    }
}

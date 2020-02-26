using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe7.Service.Sms
{
   public class SmsViewModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public string Number { get; set; }

        public string Sms { get; set; }
    }
}

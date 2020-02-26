using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe7.Infrastructure.Service.RequsetResponseMessaging.Response;
using Cafe7.Service.CustomerModule.ViewModel;

namespace Cafe7.Service.CustomerModule.Messaging.Response
{
   public class CustomerDeleteResponse:ResponseBase<CustomerViewModel>
    {
        public string LastRegistrationNumber { get; set; }
    }
}

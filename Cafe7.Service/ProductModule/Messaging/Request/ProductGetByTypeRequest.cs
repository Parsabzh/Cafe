using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe7.Infrastructure.Service.RequsetResponseMessaging.Request;
using Cafe7.Service.ProductModule.ViewModel;

namespace Cafe7.Service.ProductModule.Messaging.Request
{
  public  class ProductGetByTypeRequest:RequestBase
    {
        public string Type { get; set; }

    }
}

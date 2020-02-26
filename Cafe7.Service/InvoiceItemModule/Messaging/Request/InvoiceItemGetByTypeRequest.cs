using Cafe7.Infrastructure.Service.RequsetResponseMessaging.Request;

namespace Cafe7.Service.InvoiceItemModule.Messaging.Request
{
  public  class InvoiceItemGetByTypeRequest:RequestBase
    {
        public string Type { get; set; }

    }
}

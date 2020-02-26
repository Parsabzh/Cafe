using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe7.Infrastructure.Service.RequsetResponseMessaging.Request;

namespace Cafe7.Service.CustomerModule.Messaging.Request.ScoreRequest
{
  public class ScoreDeleteRequest:RequestBase

    {
        public int CustomerId { get; set; }
        public string ScoreDecrease { get; set; }
    }
}

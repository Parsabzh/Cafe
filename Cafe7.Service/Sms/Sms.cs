using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmsIrRestful;

namespace Cafe7.Service.Sms
{
    public static class Sms
    {
        public static SmsViewModel SendSms(string mobileNumber, string sms)
        {
           
            var token = new Token();
            var t = token.GetToken("4802aeffff1800dc59069aec", "@Prs29881245!%!");
            if (t == null)
                return new SmsViewModel
                {
                    Message = "فرستادن پیام دچار مشکل شده است،لطفا اینترنت سیستم را وصل نمایید",
                    IsSuccess = false
                };
            var message = new MessageSend();
            var o = new MessageSendObject
            {
                LineNumber = "50002015314656",
                Messages = new List<string>() {sms}.ToArray(),
                MobileNumbers = new List<string>() {mobileNumber}.ToArray(),
                SendDateTime = null,
                CanContinueInCaseOfError = false
            };
            var send =message.Send(t,o);
            if (send.IsSuccessful) return new SmsViewModel
            {
                IsSuccess = true,
                Message = send.Message
            };
            return new SmsViewModel
            {
                Message = "فرستادن پیام دچار مشکل شده است،لطفا اینترنت سیستم را وصل نمایید",
                IsSuccess = false
            };
        }

        public static SmsViewModel SendSmsToMany(IEnumerable<SmsViewModel> smsRequest)
        {
            var token = new Token();
            var t = token.GetToken("4802aeffff1800dc59069aec", "@Prs29881245!%!");
            if (t == null)
                return new SmsViewModel
                {
                    Message = "فرستادن پیام دچار مشکل شده است،لطفا اینترنت سیستم را وصل نمایید",
                    IsSuccess = false
                };

            var message = new MessageSend();

            foreach (var item in smsRequest)
            {
                var send = message.Send(t, new MessageSendObject
                {
                    LineNumber = "50002015314656",
                    Messages = new List<string>() { item.Sms }.ToArray(),
                    MobileNumbers = new List<string>() { item.Number }.ToArray(),
                    SendDateTime = null,
                    CanContinueInCaseOfError = false
                });
                if (!send.IsSuccessful)
                {
                    return new SmsViewModel
                    {
                        Message = send.Message,
                        IsSuccess = false
                    };
                }

            }
            return new SmsViewModel
            {
                Message = "ارسال با موفقیت انحام شد",
                IsSuccess = true
            };

        }
    }
}

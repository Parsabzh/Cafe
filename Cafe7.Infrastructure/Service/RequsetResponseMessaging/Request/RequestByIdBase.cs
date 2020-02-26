namespace Cafe7.Infrastructure.Service.RequsetResponseMessaging.Request
{
   public abstract class RequestByIdBase<TId>:RequestBase
    {
        public TId Id { get; set; }
    }
}

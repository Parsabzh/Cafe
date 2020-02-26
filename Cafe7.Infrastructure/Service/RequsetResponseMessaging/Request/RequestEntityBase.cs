namespace Cafe7.Infrastructure.Service.RequsetResponseMessaging.Request
{
  public  abstract class RequestEntityBase<TEntity>:RequestBase
    {
        public TEntity Entity { get; set; }
    }
}

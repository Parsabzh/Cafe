namespace Cafe7.Infrastructure.Service.RequsetResponseMessaging.Response
{
    public  abstract class ResponseBase<TEntity>

    {
        public bool IsSuccess { get; set; }

        public TEntity Entity { get; set; }

        public string Message { get; set; }

        public ResultType Result { get; set; }
    }
}

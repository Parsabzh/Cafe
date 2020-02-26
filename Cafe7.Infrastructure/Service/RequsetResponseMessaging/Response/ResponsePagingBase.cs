using System.Collections.Generic;

namespace Cafe7.Infrastructure.Service.RequsetResponseMessaging.Response
{
    public abstract class ResponsePagingBase<TEntity>:ResponseBase<IEnumerable<TEntity>>
    {
        public int Index { get; set; }
        public int Size { get; set; }
        public int Total { get; set; }
        public int PageCount => (Total - 1) / (Size) + 1;
    }
}

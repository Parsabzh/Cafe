namespace Cafe7.Infrastructure.Domain
{
    public abstract class ModelBase<TId>
    {
        public  TId Id { get; set; }
    }
}

namespace TradeApi.Domain
{
    public interface ISoftDelete
    {
        DateTime? DeletedAt { get; set; }
    }
}

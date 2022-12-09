namespace TradeApi.Domain
{
    public interface IActionLog
    {
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
    }
}

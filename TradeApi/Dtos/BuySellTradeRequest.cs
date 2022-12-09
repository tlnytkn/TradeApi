using Microsoft.Build.Framework;

namespace TradeApi.Dtos
{
    public class BuySellTradeRequest
    {
        [Required]
        public Guid TraderId { get; set; }
        [Required]
        public Guid ShareId { get; set; }
        [Required]
        public decimal Amount { get; set; }
    }
}
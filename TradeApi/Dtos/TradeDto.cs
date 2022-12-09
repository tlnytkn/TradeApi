using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TradeApi.Domain.Enums;

namespace TradeApi.Dtos;

public class TradeDto
{
    public Guid TradeId { get; set; }
    [Required]
    public Guid TraderId { get; set; }
    [Required]
    public Guid ShareId { get; set; }
    [Required]
    public DateTime TradeDate { get; set; }
    [Required]
    public TradeType Type { get; set; }
    [Required]
    [Column(TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }
    [Required]
    [Column(TypeName = "decimal(10, 2)")]
    public decimal Amount { get; set; }
    [NotMapped]
    public virtual decimal TotalPrice => Price * Amount;
    public virtual TraderDto Trader { get; set; } = null!;
    public virtual ShareDto Share { get; set; } = null!;
}

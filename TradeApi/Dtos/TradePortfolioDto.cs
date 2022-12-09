using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TradeApi.Domain;

namespace TradeApi.Dtos;

public class TraderPortfolioDto : IBaseEntity
{
    public Guid TraderPortfolioId { get; set; }
    [Required]
    public Guid TraderId { get; set; }
    [Required]
    public Guid ShareId { get; set; }
    [Required]
    [Column(TypeName = "decimal(10, 2)")]
    public decimal Amount { get; set; }
    public virtual ShareDto Share { get; set; } = null!;
    public virtual TraderDto Trader { get; set; } = null!;
}

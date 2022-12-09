using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradeApi.Domain;

public class TraderPortfolio : IBaseEntity
{
    [Key]
    public Guid TraderPortfolioId { get; set; }
    [Required]
    public Guid TraderId { get; set; }
    [Required]
    public Guid ShareId { get; set; }
    [Required]
    [Column(TypeName = "decimal(10, 2)")]
    public decimal Amount { get; set; }
    public virtual Share Share { get; set; } = null!;
    public virtual Trader Trader { get; set; } = null!;
}

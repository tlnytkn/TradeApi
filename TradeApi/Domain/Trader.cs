using System.ComponentModel.DataAnnotations;

namespace TradeApi.Domain;

public class Trader : IBaseEntity
{
    public Trader()
    {
        TraderPortfolios = new HashSet<TraderPortfolio>();
        Trades = new HashSet<Trade>();
    }
    [Key]
    public Guid TraderId { get; set; }
    [Required]
    [StringLength(50)]
    public string Name { get; set; }
    [Required]
    [StringLength(11)]
    public string Phone { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    public virtual ICollection<TraderPortfolio> TraderPortfolios { get; set; }
    public virtual ICollection<Trade> Trades { get; set; }
}

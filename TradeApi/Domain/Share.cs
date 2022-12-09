using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace TradeApi.Domain;

[Microsoft.EntityFrameworkCore.Index(nameof(Symbol), IsUnique = true)]
public class Share : IBaseEntity
{
    public Share()
    {
        TraderPortfolios = new HashSet<TraderPortfolio>();
        Trades = new HashSet<Trade>();
    }
    [Key]
    public Guid ShareId { get; set; }
    [Required]
    [StringLength(3)]
    public string Symbol { get; set; }
    [Required]
    [StringLength(50)]
    public string Name { get; set; }
    [Required]
    [Column(TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }
    public virtual ICollection<TraderPortfolio> TraderPortfolios { get; set; }
    public virtual ICollection<Trade> Trades { get; set; }

}

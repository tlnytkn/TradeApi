using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradeApi.Dtos;

public class ShareDto
{
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
}

using System.ComponentModel.DataAnnotations;

namespace TradeApi.Dtos;

public class TraderDto
{
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
}

using System.ComponentModel.DataAnnotations;

namespace RentingService.Model;

public class ProductDetails
{
    [Key]
    public Guid ProductId { get; set; }
    public int Usage { get; set; }
}

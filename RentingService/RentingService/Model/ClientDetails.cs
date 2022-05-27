using System.ComponentModel.DataAnnotations;

namespace RentingService.Model;

public class ClientDetails
{
    [Key]
    public Guid ClientId { get; set; }
    public string ClientName { get; set; }
    public string ClientSurname { get; set; }
    public float ClientDeposit { get; set; }
}

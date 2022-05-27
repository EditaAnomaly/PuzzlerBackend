using System.ComponentModel.DataAnnotations;

namespace RentingService;

public class Renting
{
    public Guid Id { get; set; }

    // client details - should be taken from Topic???
    public string ClientId { get; set; }
    public string ClientName { get; set; }
    public string ClientSurname { get; set; }
    public float ClientDeposit { get; set; }
    public int ClientReputation { get; set; }

    //Product details - should be taken from Topic???
    public string ProductId { get; set; }
    public int Usage { get; set; }
    // how many times have already been rented

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime RentDate { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public string ReturnDate { get; set; }
}

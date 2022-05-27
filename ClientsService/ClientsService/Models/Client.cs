using System.ComponentModel.DataAnnotations;

namespace ClientsService.Models;

public class Client
{
    public Guid Id { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime DateRegistered { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Adress { get; set; }
    public string PhoneNr { get; set; }
    public float Deposit{ get; set; }
    // reputation score is a  minus count of damaged returns
    public int Reputation { get; set; }

}

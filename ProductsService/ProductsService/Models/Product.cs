using System.ComponentModel.DataAnnotations;

namespace ProductsService.Models;

public class Product
{
    public Guid Id { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime DateAdded { get; set; }
    public string Title { get; set; }
    public Category Category { get; set; }
    public int Size { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy}", ApplyFormatInEditMode = true)]
    public DateTime YearIssued { get; set; }
    public Difficulty Difficulty { get; set; }
    public int Usage { get; set; }
    // how many times have already been rented
}

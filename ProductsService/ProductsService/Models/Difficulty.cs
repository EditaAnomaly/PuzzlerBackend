using System.ComponentModel.DataAnnotations;

namespace ProductsService.Models;

public enum Difficulty
{
    [Display(Name = "Effortless")]
    Effortless,
    [Display(Name = "Easy")]
    Easy,
    [Display(Name = "Normal")]
    Normal,
    [Display(Name = "Advanced")]
    Advanced,
    [Display(Name = "Challenging")]
    Challenging,
    [Display(Name = "Extreme")]
    Extreme
}

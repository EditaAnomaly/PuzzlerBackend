using System.ComponentModel.DataAnnotations;

namespace ProductsService.Models;

public enum Category
{
    [Display(Name = "Garden")]
    Garden,
    [Display(Name = "Landscape")]
    Landscape,
    [Display(Name = "City")]
    City,
    [Display(Name = "Painting")]
    Painting,
    [Display(Name = "Animation")]
    Animation,
    [Display(Name = "Holidays")]
    Holidays,
    [Display(Name = "Animals")]
    Animals,
    [Display(Name = "Vehicles")]
    Vehicles,
    [Display(Name = "Fantasy")]
    Fantasy,
    [Display(Name = "Gastronomy")]
    Gastronomy,
    [Display(Name = "Kids")]
    Kids,
    [Display(Name = "Landmarks")]
    Landmarks,
    [Display(Name = "Romance")]
    Romance
}

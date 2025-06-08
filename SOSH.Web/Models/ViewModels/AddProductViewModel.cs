using System.ComponentModel.DataAnnotations;

namespace SOSH.Web.Models.ViewModels;

public class AddProductViewModel
{
    [Required]
    public string Name { get; set; }

    [Required]
    [DataType(DataType.MultilineText)]
    public string Description { get; set; }

    [Required]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }

    [Required]
    [Display(Name = "Quaintity In Stock")]
    public int QuantityInStoke { get; set; }

    [Required]
    [DataType(DataType.Upload)]
    public IFormFile Picture { get; set; }
}




public class EditProductViewModel
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    [DataType(DataType.MultilineText)]
    public string Description { get; set; }

    [Required]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }

    [Required]
    [Display(Name = "Quaintity In Stock")]
    public int QuantityInStoke { get; set; }

    [Required(AllowEmptyStrings = true)]
    [DataType(DataType.Upload)]
    public IFormFile Picture { get; set; }

    public List<int> SelectedCategories { get; set; }

    public List<DropdownCategoryViewModel> AllCategories { get; set; }
}

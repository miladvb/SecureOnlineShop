using System.ComponentModel.DataAnnotations;

namespace SOSH.Web.Models.ViewModels;

public class CategoryViewModel
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }


}


public class EditCategoryViewModel
{


    [Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }


}


public class DropdownCategoryViewModel
{


    [Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }
}

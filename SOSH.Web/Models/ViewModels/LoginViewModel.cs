using System.ComponentModel.DataAnnotations;

namespace SOSH.Web.Models.ViewModels;

public class LoginViewModel
{
    [EmailAddress]
    [Required]
    [MaxLength(300)]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    [Required]
    [MaxLength(50)]
    public string Password { get; set; }


    public bool RememberMe { get; set; }
}

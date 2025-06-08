using System.ComponentModel.DataAnnotations;

namespace SOSH.Web.Models.ViewModels;

public class RegisterViewModel
{

    [EmailAddress]
    [Required]
    [MaxLength(300)]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    [Required]
    [MaxLength(50)]
    public string Password { get; set; }

    [Required]
    [MaxLength(50)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
}

public class AdminRegisterViewModel
{

    [EmailAddress]
    [Required]
    [MaxLength(300)]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    [Required]
    [MaxLength(50)]
    public string Password { get; set; }

    [Required]
    [MaxLength(50)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }

    [Required]
    public DateTime RegisterDate { get; set; }
    public bool IsAdmin { get; set; } = false;

}

public class AdminUserEditViewModel
{
    public AdminUserEditViewModel()
    {
        Roles = new List<string>();
    }


    [Required]
    public int UserId { get; set; }

    [EmailAddress]
    [Required]
    [MaxLength(300)]
    public string Email { get; set; }

    [EmailAddress]
    [Required]
    [MaxLength(300)]
    public string UserName { get; set; }


    [Required]
    public DateTime RegisterDate { get; set; }

    public bool IsAdmin { get; set; }
    public IList<string> Roles { get; set; }

}

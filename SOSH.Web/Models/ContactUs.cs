using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOSH.Web.Models;

public class Contact
{
    [Key]
    public int ContactId { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Name { get; set; }

    [Column(TypeName = "ntext")]
    [Required]
    public string Message { get; set; }
}

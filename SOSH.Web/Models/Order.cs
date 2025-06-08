using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOSH.Web.Models;

public class Order
{

    [Key]
    public int OrderId { get; set; }

    [Required]
    public int UserId { get; set; }
    public DateTime CreateDate { get; set; }
    public bool IsFinaly { get; set; }

    [ForeignKey("UserId")]
    public ApplicationUser User { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; }

}

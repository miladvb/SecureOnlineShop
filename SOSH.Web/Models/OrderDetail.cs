using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace SOSH.Web.Models;

public class OrderDetail
{

    [Key]
    public int DetailId { get; set; }

    [Required]
    public int OrderId { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    [Column(TypeName = "Money")]
    public decimal Price { get; set; }


    [Required]
    public int Count { get; set; }

    public Order Order { get; set; }

    [ForeignKey("ProductId")]
    public Product Product { get; set; }

}

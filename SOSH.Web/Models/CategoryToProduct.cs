using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SOSH.Web.Models;

namespace SOSH.Web;

public class CategoryToProduct
{

    [Key, Column(Order = 1)]
    public int CategoryId { get; set; }

    [Key, Column(Order = 0)]
    public int ProductId { get; set; }

    //Navigation Property
    public Category Category { get; set; }
    public Product Product { get; set; }
}

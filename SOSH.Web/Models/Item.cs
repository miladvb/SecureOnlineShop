namespace SOSH.Web.Models;

public class Item
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public int QuntityInStock { get; set; }

    public Product Product { get; set; }
}

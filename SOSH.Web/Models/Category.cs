namespace SOSH.Web.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<CategoryToProduct>  categoryToProducts { get; set; }

}

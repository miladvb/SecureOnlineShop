namespace SOSH.Web.Models.ViewModels;

public class ProductDetailViewModel
{
    public Product product { get; set; }
    public List<Category> categories { get; set; }

    public List<CommentViewModel>  commentViewModels { get; set; }
    public AddCommentViewModel  productComment { get; set; }
}

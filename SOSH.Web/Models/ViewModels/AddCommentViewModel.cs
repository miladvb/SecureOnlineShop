using System.ComponentModel.DataAnnotations;

namespace SOSH.Web.Models.ViewModels;



public class AddCommentViewModel
{

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string CommentName { get; set; }

    [Required]
    public string CommentText { get; set; }

    [Required]
    public int ProductId { get; set; }


}



public class CommentViewModel
{

    public int CommentId { get; set; }
    public string Email { get; set; }
    public string CommentName { get; set; }
    public string CommentText { get; set; }
    public bool IsPublished { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }


}


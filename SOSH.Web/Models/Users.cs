using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SOSH.Web.Models;

public class ApplicationUser : IdentityUser<int>
{

    public DateTime RegisterDate { get; set; }
    public List<Order> orders { get; set; }
}


public class ApplicationRole : IdentityRole<int>
{

}


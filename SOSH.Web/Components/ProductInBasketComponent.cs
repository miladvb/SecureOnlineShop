using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SOSH.Web.Models;

namespace SOSH.Web.Components;

public class ProductInBasketComponent : ViewComponent
{
    private readonly SOSHContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProductInBasketComponent(SOSHContext shopContext, UserManager<ApplicationUser> userManager)
    {
        _context = shopContext;
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        int UserId = 0;
        int CountOrder = 0;
        if (User.Identity.IsAuthenticated)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity?.Name);
            if (user != null)
                UserId = user.Id;
            var order = await _context.Orders
                                .Where(x => x.UserId == UserId && x.IsFinaly == false)
                                .Include(x => x.OrderDetails)
                                .FirstOrDefaultAsync();
            if (order != null)
                CountOrder = order.OrderDetails.Sum(x => x.Count);
        }

        return View("Components/ProductInBasketComponent.cshtml", CountOrder);
    }
}


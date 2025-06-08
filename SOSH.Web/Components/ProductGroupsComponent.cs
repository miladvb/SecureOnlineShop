using Microsoft.AspNetCore.Mvc;
using SOSH.Web.Models.ViewModels;

namespace SOSH.Web.Components;

public class ProductGroupsComponent : ViewComponent
{
    private readonly SOSHContext _context;

    public ProductGroupsComponent(SOSHContext shopContext)
    {
        _context = shopContext;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var _cats = _context.Categories
                          .Select(x => new ShowGroupViewModel
                          {
                              GroupId = x.Id,
                              GroupName = x.Name,
                              ProductCount = x.categoryToProducts.Count(c => c.CategoryId == x.Id)
                          }).ToList();
        return View("Components/ProductGroupsComponent.cshtml", _cats);
    }
}

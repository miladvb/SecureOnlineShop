using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SOSH.Web;

public class ProductController : Controller
{
    private readonly SOSHContext _context;

    public ProductController(SOSHContext context)
    {
        _context = context;
    }


    [Route("Group/{Id}/{Name}")]
    public IActionResult ShowProductByGroup(int Id, string Name)
    {
        ViewData["GroupName"] = Name;
        var producs = _context.categoryToProducts
                               .Where(x => x.CategoryId == Id)
                               .Include(x => x.Product)
                               .Select(x => x.Product)
                               .ToList();


        return View(producs);
    }
}

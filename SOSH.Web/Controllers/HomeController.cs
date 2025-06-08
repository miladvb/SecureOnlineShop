using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SOSH.Web.Models;
using SOSH.Web.Models.ViewModels;

namespace SOSH.Web.Controllers;
[RequireHttps]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly SOSHContext _context;
    public HomeController(ILogger<HomeController> logger, SOSHContext SOSHContext)
    {
        _logger = logger;
        _context = SOSHContext;
    }

    public IActionResult Index()
    {
        var products = _context.Products
                               .ToList();
        return View(products);
    }

    [Authorize]
    public IActionResult AddToCard(int ItemId)
    {
        var product = _context.Products.Include(x => x.Item).SingleOrDefault(x => x.ItemId == ItemId);

        if (product == null)
            return RedirectToAction("ShowCard");

        int UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
        var order = _context.Orders.FirstOrDefault(x => x.UserId == UserId && x.IsFinaly == false);
        if (order == null)
        {
            order = new Order
            {
                IsFinaly = false,
                CreateDate = DateTime.Now,
                UserId = UserId
            };

            _context.Orders.Add(order);
            _context.SaveChanges();
            _context.OrderDetails.Add(new OrderDetail
            {
                OrderId = order.OrderId,
                ProductId = product.Id,
                Price = product.Item.Price,
                Count = 1
            });
        }
        else
        {
            var orderdetail = _context.OrderDetails.FirstOrDefault(x => x.OrderId == order.OrderId && x.ProductId == product.Id);

            if (orderdetail != null)
            {
                orderdetail.Count += 1;
            }
            else
            {
                _context.OrderDetails.Add(new OrderDetail
                {
                    OrderId = order.OrderId,
                    ProductId = product.Id,
                    Price = product.Item.Price,
                    Count = 1
                });
            }


        }
        _context.SaveChanges();

        return RedirectToAction("ShowCard");
    }

    [Authorize]
    public IActionResult RemoveCard(int DetailId)
    {
        int UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
        var orderdetail = _context.OrderDetails.Find(DetailId);
        if (orderdetail == null)
            return RedirectToAction("ShowCard");

        if (orderdetail.Count > 1)
            orderdetail.Count -= 1;
        else
            _context.OrderDetails.Remove(orderdetail);
        _context.SaveChanges();
        return RedirectToAction("ShowCard");
    }

    [Authorize]
    public IActionResult ShowCard()
    {
        int UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());

        var order = _context.Orders
                            .Where(x => x.UserId == UserId && x.IsFinaly == false)
                            .Include(x => x.OrderDetails)
                            .ThenInclude(c => c.Product)
                            .FirstOrDefault();
        return View(order);
    }

    public IActionResult Detail(int ItemId)
    {
        var p = _context.Products.Include(x => x.Item).SingleOrDefault(x => x.Id == ItemId);
        var pcomments = _context.Comments
                                .Where(x => x.ProductId == ItemId && x.IsPublished == true)
                                .Select(s => new CommentViewModel
                                {
                                    CommentName = s.CommentName,
                                    CommentText = s.CommentText,
                                    Email = s.Email,
                                    ProductId = s.ProductId,
                                })
                                .ToList();
        if (p == null)
            return NotFound();

        var categories = _context.Products
                        .Where(x => x.Id == ItemId)
                        .SelectMany(c => c.categoryToProducts)
                        .Select(ca => ca.Category)
                        .ToList();
        var vm = new ProductDetailViewModel()
        {
            product = p,
            categories = categories,
            commentViewModels = pcomments,
            productComment = new AddCommentViewModel()
        };
        return View(vm);
    }

    [HttpGet]
    [Authorize]
    public IActionResult Payment()
    {
        int UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
        var order = _context.Orders.FirstOrDefault(x => x.UserId == UserId && x.IsFinaly == false);
        order.IsFinaly = true;
        _context.SaveChanges();
        return RedirectToAction("ShowCard");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateComment([Bind("Email,CommentName,CommentText,ProductId")] AddCommentViewModel model)
    {

        ViewBag.ProductId = model.ProductId;
        ViewBag.Email = model.Email;
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Please enter the information correctly");
            return View("ErrorComment", model);
        }


        Comment comment = new Comment
        {
            CommentName = model.CommentName,
            CommentText = model.CommentText,
            Email = model.Email,
            ProductId = model.ProductId
        };
        _context.Add(comment);
        await _context.SaveChangesAsync();
        return View("AddCommentSuccess");


    }


    [Route("ContactUs")]
    public async Task<IActionResult> ContactUs()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

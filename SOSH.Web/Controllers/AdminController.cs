using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SOSH.Web.Models;
using SOSH.Web.Models.ViewModels;

namespace SOSH.Web;


[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly SOSHContext _context;
    public AdminController(SOSHContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var products = _context.Products.Include(x => x.Item).ToList();
        return View(products);
    }

    public IActionResult AddProduct()
    {
        var dropdowns = _context.Categories.Select(x => new DropdownCategoryViewModel
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();
        ViewBag.Categories = dropdowns;

        return View();
    }

    [HttpPost]
    public IActionResult AddProduct(AddProductViewModel model, List<int> SelectedCategories)
    {
        if (!ModelState.IsValid)
            return View(model);

        Item item = new Item
        {
            Price = model.Price,
            QuntityInStock = model.QuantityInStoke
        };
        _context.Add(item);

        _context.SaveChanges();

        Product product = new Product
        {
            Description = model.Description,
            Name = model.Name,
            Item = item,
            ItemId = item.Id,
            ImageName = string.Empty

        };

        _context.Products.Add(product);
        _context.SaveChanges();

        string ImageName = product.Id.ToString() + Path.GetExtension(model.Picture.FileName);
        product.ImageName = ImageName;
        _context.SaveChanges();
        CategoryToProduct ctp;
        foreach (var catid in SelectedCategories)
        {
            ctp = new CategoryToProduct
            {
                CategoryId = catid,
                ProductId = product.Id
            };
            _context.categoryToProducts.Add(ctp);
        }

        _context.SaveChanges();

        if (model.Picture?.Length > 0)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", ImageName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                model.Picture.CopyTo(stream);
            }
        }

        return RedirectToAction("Index");
    }



    public IActionResult EditProduct(int Id)
    {
        var imageName = _context.Products.SingleOrDefault(x => x.Id == Id)?.ImageName;
        ViewBag.imageName = imageName;

        var allCategories = _context.Categories.Select(x => new DropdownCategoryViewModel
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();


        var SelectedCategories = _context.categoryToProducts.Where(x => x.ProductId == Id).Select(c => c.CategoryId).ToList();


        var product = _context.Products.Include(x => x.Item)
                                        .Where(x => x.Id == Id)
                                        .Select(s => new EditProductViewModel
                                        {
                                            Id = s.Id,
                                            Name = s.Name,
                                            Description = s.Description,
                                            QuantityInStoke = s.Item.QuntityInStock,
                                            Price = s.Item.Price
                                        }).FirstOrDefault();

        product.AllCategories = allCategories;
        product.SelectedCategories = SelectedCategories;
        return View(product);

    }



    [HttpPost]
    public IActionResult EditProduct(EditProductViewModel model, List<int> SelectedCategories)
    {
        if (!ModelState.IsValid)
        {
            foreach (var key in ModelState.Keys)
                foreach (var error in ModelState[key].Errors.Where(x => x != null).ToList())
                    ModelState.AddModelError(key, error.ErrorMessage);
            return View(model);
        }


        var product = _context.Products.Find(model.Id);
        var item = _context.Items.Find(product.ItemId);

        item.Price = model.Price;
        item.QuntityInStock = model.QuantityInStoke;
        _context.SaveChanges();


        product.Description = model.Description;
        product.Name = model.Name;
        product.Item = item;
        product.ItemId = item.Id;



        if (model.Picture?.Length > 0)
        {
            string oldImageName = product.ImageName;
            string filePath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", oldImageName);
            if (System.IO.File.Exists(filePath2))
            {
                System.IO.File.Delete(filePath2);
            }

            string ImageName = product.Id.ToString() + Path.GetExtension(model.Picture.FileName);
            product.ImageName = ImageName;
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", ImageName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                model.Picture.CopyTo(stream);
            }



        }
        _context.SaveChanges();


        var ctps = _context.categoryToProducts.Where(x => x.ProductId == product.Id);
        _context.categoryToProducts.RemoveRange(ctps);
        _context.SaveChanges();

        CategoryToProduct ctp;
        foreach (var catid in SelectedCategories)
        {
            ctp = new CategoryToProduct
            {
                CategoryId = catid,
                ProductId = product.Id
            };
            _context.categoryToProducts.Add(ctp);
        }
        _context.SaveChanges();


        return RedirectToAction("Index");

    }



    [HttpGet]
    public IActionResult DeleteProduct(int Id)
    {
        var imageName = _context.Products.SingleOrDefault(x => x.Id == Id)?.ImageName;
        ViewBag.imageName = imageName;
        var product = _context.Products.Include(x => x.Item)
                                        .Where(x => x.Id == Id)
                                        .Select(s => new EditProductViewModel
                                        {
                                            Id = s.Id,
                                            Name = s.Name,
                                            Description = s.Description,
                                            QuantityInStoke = s.Item.QuntityInStock,
                                            Price = s.Item.Price
                                        }).FirstOrDefault();
        return View(product);

    }

    [HttpPost]
    public IActionResult DeleteProduct(EditProductViewModel model)
    {
        var product = _context.Products.Find(model.Id);
        var item = _context.Items.Find(product.ItemId);

        _context.Items.Remove(item);
        _context.Products.Remove(product);

        var ctps = _context.categoryToProducts.Where(x => x.ProductId == model.Id);
        _context.categoryToProducts.RemoveRange(ctps);


        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", product.ImageName);
        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }
        _context.SaveChanges();

        return RedirectToAction("Index");
    }
}

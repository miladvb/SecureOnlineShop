using System.Runtime.CompilerServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SOSH.Web.Models;
using SOSH.Web.Models.ViewModels;

namespace SOSH.Web;

public class AccountController : Controller
{

    private readonly SOSHContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    public AccountController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Login(string? ReturnUrl = null)
    {
        ViewData["ReturnUrl"] = ReturnUrl;
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? ReturnUrl = null)
    {
        ViewData["ReturnUrl"] = ReturnUrl;
        if (!ModelState.IsValid)
            return View(model);


        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(ReturnUrl))
                if (Url.IsLocalUrl(ReturnUrl))
                    return Redirect(ReturnUrl);
                else
                    return Redirect("/");
            else
                return Redirect("/");

        }
        else
        {
            ModelState.AddModelError(string.Empty, "Invalid login attemp");
        }

        // var role = user.IsAdmin ? "Admin" : "User";
        // var claims = new List<Claim>
        // {
        //     new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
        //     new Claim(ClaimTypes.Name,user.Email),
        //     new Claim(ClaimTypes.Role,role)
        // };

        // var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        // var claimsprincipal = new ClaimsPrincipal(identity);

        // var properties = new AuthenticationProperties
        // {
        //     IsPersistent = model.RememberMe
        // };
        // HttpContext.SignInAsync(claimsprincipal, properties);

        return View(model);
    }

    [HttpGet]

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        ApplicationUser user = new ApplicationUser
        {
            Email = model.Email,
            UserName = model.Email,
            RegisterDate = DateTime.Now
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            var rolResult = await _userManager.AddToRoleAsync(user, "User");
            return View("SuccessRegister", model);
        }
        else
        {
            foreach (var err in result.Errors)
            {
                ModelState.AddModelError(string.Empty, err.Description);
            }
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        // HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await _signInManager.SignOutAsync();

        return RedirectToAction(nameof(AccountController.Login), "Account");
    }

    [HttpGet]
    public IActionResult AccessDenied(string returnUrl)
    {
        return View();
    }

}

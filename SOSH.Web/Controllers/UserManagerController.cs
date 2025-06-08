using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SOSH.Web;
using SOSH.Web.Models;
using SOSH.Web.Models.ViewModels;

namespace SOSH.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserManagerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserManagerController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: UserManager
        public IActionResult Index()
        {
            var userlist = _userManager.Users.ToList();
            return View(userlist);
        }

        // GET: UserManager/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id.ToString());
            //Check if User Exists in the Database
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return NotFound();
            }


            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new AdminUserEditViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                RegisterDate = user.RegisterDate,
                Roles = userRoles,
                IsAdmin = false
            };

            foreach (var bt in userRoles)
                if (bt == "Admin")
                    model.IsAdmin = true;


            return View(model);
        }

        // GET: UserManager/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserManager/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,Password,ConfirmPassword,IsAdmin")] AdminRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.Email,
                    RegisterDate = model.RegisterDate
                };

                string rolename = model.IsAdmin ? "Admin" : "User";
                var resultuser = await _userManager.CreateAsync(user, model.Password);
                if (resultuser.Succeeded)
                {

                    var resultrole = await _userManager.AddToRoleAsync(user, rolename);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var error in resultuser.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }


            }
            return View(model);
        }

        // GET: UserManager/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id.ToString());
            //Check if User Exists in the Database
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return NotFound();
            }


            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new AdminUserEditViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                RegisterDate = user.RegisterDate,
                Roles = userRoles,
                IsAdmin = false
            };

            foreach (var bt in userRoles)
                if (bt == "Admin")
                    model.IsAdmin = true;


            return View(model);
        }

        // POST: UserManager/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Email,UserName,RegisterDate,IsAdmin,Roles")] AdminUserEditViewModel model)
        {
            if (id != model.UserId)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId.ToString());

                user.Email = model.Email;
                user.UserName = model.UserName;
                user.RegisterDate = model.RegisterDate;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    var roleusers = await _userManager.GetRolesAsync(user);
                    foreach (var r in roleusers)
                        await _userManager.RemoveFromRoleAsync(user, r);


                    string rolenme = model.IsAdmin ? "Admin" : "User";
                    await _userManager.AddToRoleAsync(user, rolenme);

                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }


            }

            return View(model);
        }

        // GET: UserManager/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id.ToString());
            //Check if User Exists in the Database
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return NotFound();
            }


            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new AdminUserEditViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                RegisterDate = user.RegisterDate,
                Roles = userRoles,
                IsAdmin = false
            };

            foreach (var bt in userRoles)
                if (bt == "Admin")
                    model.IsAdmin = true;

            return View(model);
        }

        // POST: UserManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int UserId)
        {
            //First Fetch the User you want to Delete
            var user = await _userManager.FindByIdAsync(UserId.ToString());

            if (user == null)
            {
                // Handle the case where the user wasn't found
                ViewBag.ErrorMessage = $"User with Id = {UserId} cannot be found";
                return NotFound();
            }
            else
            {
                //Delete the User Using DeleteAsync Method of UserManager Service
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    // Handle a successful delete
                    return RedirectToAction(nameof(UserManagerController.Index), "UserManager");
                }
                else
                {
                    // Handle failure
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                return RedirectToAction(nameof(UserManagerController.Index), "UserManager");
            }
        }

    }
}

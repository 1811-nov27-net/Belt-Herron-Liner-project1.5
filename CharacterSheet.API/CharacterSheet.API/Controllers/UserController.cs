using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CharacterSheet.API.Models;

namespace CharacterSheet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : Controller
    {
        private SignInManager<IdentityUser> _SignInManager;

        public UserController (SignInManager<IdentityUser> signInManager, IdentityDbContext db)
        {
            db.Database.EnsureCreated();
            _SignInManager = signInManager;
        }

        [HttpPost]
        public async Task<ActionResult> Login(UserViewModel user)
        {
            var result = await _SignInManager.PasswordSignInAsync(user.UserName, user.Password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return StatusCode(403);
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> Register(UserViewModel user, 
            [FromServices]UserManager<IdentityUser> userManager, 
            [FromServices] RoleManager<IdentityRole> roleManager, bool GM = false)
        {
            var newUser = new IdentityUser(user.UserName);

            var result = await userManager.CreateAsync(newUser, user.Password);

            if(!result.Succeeded)
            {
                return BadRequest(result);
            }

            if (GM)
            {
                var result2 = await roleManager.RoleExistsAsync("GM");
                if(!result2)
                {
                    var GMRole = new IdentityRole("GM");
                    result = await roleManager.CreateAsync(GMRole);

                    if(!result.Succeeded)
                    {
                        return StatusCode(500, result);
                    }
                    result = await userManager.AddToRoleAsync(newUser, "GM");
                    if(!result.Succeeded)
                    {
                        return StatusCode(500, result);
                    }
                }
            }
            await _SignInManager.SignInAsync(newUser, isPersistent: false);

            return NoContent();
        }

        public async Task<NoContentResult> Logout()
        {
            await _SignInManager.SignOutAsync();

            return NoContent();
        }

        [HttpGet]
        [Authorize]
        public string LoggedInUser()
        {
            var roles = User.IsInRole("GM");
            return User.Identity.Name;
        }

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
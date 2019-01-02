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
using DataAccess;
using Newtonsoft.Json;
using System.Net.Http;
using CharacterSheet.API.Models;

namespace CharacterSheet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private SignInManager<IdentityUser> _SignInManager;
        IRepo Repo;

        public UserController(SignInManager<IdentityUser> signInManager, IdentityDbContext db, IRepo repo)
        {
            db.Database.EnsureCreated();
            _SignInManager = signInManager;
            Repo = repo;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login([FromBody]UserViewModel user)
        {
            var result = await _SignInManager.PasswordSignInAsync(user.UserName, user.Password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return StatusCode(403);
            }
            return NoContent();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> Register([FromBody]UserViewModel user,
            [FromServices]UserManager<IdentityUser> userManager,
            [FromServices] RoleManager<IdentityRole> roleManager, bool GM = false)
        {
            var newUser = new IdentityUser(user.UserName);

            var result = await userManager.CreateAsync(newUser, user.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            if (GM)
            {
                var result2 = await roleManager.RoleExistsAsync("GM");
                if (!result2)
                {
                    var GMRole = new IdentityRole("GM");
                    result = await roleManager.CreateAsync(GMRole);

                    if (!result.Succeeded)
                    {
                        return StatusCode(500, result);
                    }
                    result = await userManager.AddToRoleAsync(newUser, "GM");
                    if (!result.Succeeded)
                    {
                        return StatusCode(500, result);
                    }
                }
            }
            await _SignInManager.SignInAsync(newUser, isPersistent: false);
            User d20User = new User();
            d20User.UserID = 0;
            d20User.Username = user.UserName;
            d20User.Characters = new List<Character>();
            d20User.MyCampaigns = new List<ClassLibrary.Campaign>();
            int id = Repo.CreateUser(d20User);
            return StatusCode(201, id);
        }
        [HttpPost]
        [Route("Logout")]
        public async Task<NoContentResult> Logout()
        {
            await _SignInManager.SignOutAsync();

            return NoContent();
        }

        [HttpGet]
        [Authorize]
        [Route("LoggedInUser")]
        public string LoggedInUser()
        {
            var roles = User.IsInRole("GM");
            return User.Identity.Name;
        }

        // GET: api/User
        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            try
            {
                return Repo.UserList().ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        // GET: api/User/5
        [HttpGet]
        [Route("ByID/{id}")]
        public ActionResult<User> Get(int id)
        {
            User user;
            try
            {
                user = Repo.UserDetails(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
            if (user == null)
            {
                return StatusCode(418);
            }

            return user;
        }

        // GET: api/User/username
        [HttpGet]
        [Route("ByName/{username}")]
        public ActionResult<User> Get(string username)
        {
            User user;
            try
            {
                user = Repo.UserDetails(username);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/User
        [HttpPost]
        public void Post([FromBody] User user)
        {
            Repo.CreateUser(user);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] User user)
        {
            User existing;

            //pull the user entry to be updated
            try
            {
                existing = Repo.UserDetails(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

            //if the entry does not exist
            if (existing == null)
            {
                return NotFound();
            }

            //if an attempt is made to change the user ID
            if (id != user.UserID)
            {
                return BadRequest("cannot change user ID");
            }

            //update the entry
            try
            {
                Repo.UpdateUser(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

            return NoContent();                         //update successful
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                Repo.DeleteUser(id);
                return NoContent();
            }
            catch 
            {
                return NotFound();
            }
        }


    }
}

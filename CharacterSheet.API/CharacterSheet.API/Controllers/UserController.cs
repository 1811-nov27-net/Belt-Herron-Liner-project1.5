using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClassLibrary;
using DataAccess;
using Newtonsoft.Json;
using System.Net.Http;

namespace CharacterSheet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        public IRepo Repo { get; set; }

        public UserController(IRepo repo)
        {
            Repo = repo;
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
        [HttpGet("{id}", Name = "Get")]
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
        public ActionResult Put(User outdated, [FromBody] User updated)
        {
            Repo.UpdateUser(updated);
            return NoContent();
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
            catch (Exception ex)
            {
                return NotFound();
            }
        }


    }
}

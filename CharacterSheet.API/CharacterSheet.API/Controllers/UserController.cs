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
            if (id != user.userID)
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
            catch (Exception ex)
            {
                return NotFound();
            }
        }


    }
}

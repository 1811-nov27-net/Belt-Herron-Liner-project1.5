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
            return Repo.UserList().ToList();
        }

        // GET: api/User/5
        [HttpGet("{id}", Name = "Get")]
        public ActionResult<User> Get(int id)
        {
            return Repo.UserDetails(id);
        }

        // POST: api/User
        [HttpPost]
        public void Post([FromBody] User user)
        {
            Repo.CreateUser(user);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public void Put(User outdated, [FromBody] User updated)
        {
            Repo.UpdateUser(updated);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Repo.DeleteUser(id);
        }


    }
}

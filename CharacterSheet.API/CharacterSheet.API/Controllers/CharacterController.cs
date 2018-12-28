using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClassLibrary;
using DataAccess;

namespace CharacterSheet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {

        public IRepo Repo { get; set; }

        public CharacterController(IRepo repo)
        {
            Repo = repo;
        }

        // GET: api/Character
        [HttpGet]
        public IEnumerable<Character> Get()
        {
            return Repo.CharacterList();
        }

        // GET: api/Character/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Character
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Character/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

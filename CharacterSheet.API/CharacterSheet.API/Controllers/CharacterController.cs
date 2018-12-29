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
        public Character Get(int id)
        {
            return Repo.CharDetails(id);
        }

        // POST: api/Character
        [HttpPost]
        public void Post([FromBody] Character character)
        {
            Repo.CreateCharacter(character);
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
            Repo.DeleteChar(id);
        }
    }
}

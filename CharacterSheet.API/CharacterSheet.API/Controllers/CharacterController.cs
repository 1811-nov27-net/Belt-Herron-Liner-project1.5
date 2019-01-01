using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClassLibrary;
using DataAccess;
using System.Collections;

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
        public ActionResult<IEnumerable<Character>> Get()
        {
            try
            {
            return Repo.CharacterList().ToList();

            }
            catch (Exception ex)
            {

                return StatusCode(500, ex);
            }
        }

        // GET: api/Character/5
        [HttpGet("{id}", Name = "Get")]
        public ActionResult<Character> Get(int id)
        {
            Character character;
            try
            {
                character = Repo.CharDetails(id);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex);
            }
            if (character == null)
            {
                return NotFound();
            }


            return character;
        }

        // POST: api/Character
        [HttpPost]
        public ActionResult Post([FromBody] Character character)
        {
            int newID;
            try
            {
                newID = Repo.CreateCharacter(character);


            }
            catch (Exception ex)
            {

                return StatusCode(500, ex);
            }

            return CreatedAtRoute("Get", new { id = newID }, character);
        }

        // PUT: api/Character/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Character character)
        {
            Character existing;
            try
            {
                existing = Repo.CharDetails(id);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex);
            }
            if (existing == null)
            {
                return NotFound();
            }
            if (id != character.CharID)
            {
                return BadRequest("cannot change ID");
            }
            try
            {
                Repo.UpdateCharacter(character);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex);
            }
            return NoContent(); // success

        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Character existing;
            try
            {
                existing = Repo.CharDetails(id);
                if(existing == null)
                {
                    return NotFound();
                }
                Repo.DeleteChar(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
            return NoContent();
        }

        //get list of characters by user
        [HttpGet]
        public ActionResult<IEnumerable<Character>> GetCharByUser(int userID)
        {
            try
            {
                return Repo.CharacterListByUser(userID).ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        //get list of characters by campaign
        [HttpGet]
        public ActionResult<IEnumerable<Character>> GetCharByCamp(int campID)
        {
            try
            {
                return Repo.CharacterListByCamp(campID).ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

    }
}

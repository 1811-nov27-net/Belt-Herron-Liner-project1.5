using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClassLibrary;
using Data = DataAccess;
using System.Collections;
using Microsoft.AspNetCore.Authorization;

namespace CharacterSheet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CharacterController : ControllerBase
    {

        public IRepo Repo { get; set; }

        public CharacterController(IRepo repo)
        {
            Repo = repo;
        }

        // GET: api/Character
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Character>>> Get()
        {
            try
            {
            return (await Repo.CharacterList()).ToList();

            }
            catch (Exception ex)
            {

                return StatusCode(500, ex);
            }
        }

        // GET: api/Character/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Character>> Get(int id)
        {
            Character character;
            try
            {
                character = await Repo.CharDetails(id);
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
        public async Task<ActionResult> Post([FromBody] Character character)
        {
            int newID;
            try
            {
                character.UserID = (await Repo.UserDetails(User.Identity.Name)).UserID;
                newID = await Repo.CreateCharacter(character);


            }
            catch (Exception ex)
            {

                return StatusCode(500, ex);
            }

            return Created($"Character/{newID}", character);
        }

        // PUT: api/Character/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Character character)
        {
            Character existing;
            try
            {
                existing = await Repo.CharDetails(id);
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
        public async Task<ActionResult> Delete(int id)
        {
            Character existing;
            try
            {
                existing = await Repo.CharDetails(id);
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
        [Route("ByUser/{userID}")]
        public async Task<ActionResult<IEnumerable<Character>>> GetCharByUser(int userID)
        {
            User existingUser = await Repo.UserDetails(userID);

            if (existingUser == null)
            {
                return NotFound();
            }


            try
            {
                return (await Repo.CharacterListByUser(userID)).ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        //get list of characters by campaign
        [HttpGet("ByCamp/{campID}")]
        public async Task<ActionResult<IEnumerable<Character>>> GetCharByCamp(int campID)
        {
            Campaign existingCamp = await Repo.CampDetails(campID);

            try
            {
                return (await Repo.CharacterListByCamp(campID)).ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

    }
}

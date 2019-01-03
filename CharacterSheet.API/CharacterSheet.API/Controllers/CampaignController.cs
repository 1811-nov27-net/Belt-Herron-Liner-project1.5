using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClassLibrary;
using Data = DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Primitives;
using System.Net.Http;
using System.Text;


namespace CharacterSheet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CampaignController : ControllerBase
    {

        public IRepo Repo { get; set; }
        
        public CampaignController(IRepo repo)
        {
            Repo = repo;
        }

        // GET: api/Campaign
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Campaign>>> Get()
        {
            try
            {
                return (await Repo.CampList()).ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        // GET: api/Campaign/5
        [HttpGet("ByID/{id}")]
        public async Task<ActionResult<Campaign>> Get(int id)
        {
            Campaign campaign;
            try
            {
                campaign = await Repo.CampDetails(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
            if (campaign == null)
            {
                return NotFound();
            }

            return campaign;
        }
        //GET: api/Campaign/username
        [HttpGet("ByGM/{username}")]
        public async Task<ActionResult<IEnumerable<Campaign>>> Get(string username)
        {
            IEnumerable<Campaign> campaignList;
            try
            {
                campaignList = await Repo.CampList(username);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
            if (campaignList == null)
            {
                campaignList = new List<Campaign>();
            }

            return campaignList.ToList();
        }

        // POST: api/Campaign
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Campaign campaign)
        {
            int id = await Repo.CreateCampaign(campaign);
            return StatusCode(201, id);
        }

        // PUT: api/Campaign/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Campaign campaign)
        {
            Campaign existing;

            //pull the campaign entry to be updated
            try
            {
                existing = await Repo.CampDetails(id);
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

            //if an attempt is made to change the campaign ID
            if (id != campaign.CampID)
            {
                return BadRequest("cannot change campaign ID");
            }

            //update the entry
            try
            {
                Repo.UpdateCamp(campaign);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

            return NoContent();                     //update successful
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                Repo.DeleteCamp(id);
                return NoContent();
            }
            catch
            {
                return NotFound();
            }

        }

        [HttpPut("RemoveChar/{campID}")]
        public async Task<ActionResult> RemoveCharFromCamp(int campID, [FromBody] Character character)
        {
            Campaign existingCamp;
            //Character existingChar;
            try
            {
                existingCamp = await Repo.CampDetails(campID);
                //existingChar = Repo.CharDetails(charID);
                Repo.RemoveCharFromCamp(campID, character.CharID);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

            
            if (existingCamp == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("AddChar/{campID}")]
        public async Task<ActionResult> JoinCampaign(int campID, [FromBody] Character character)
        {
            Campaign existingCamp;
            //Character existingChar;
            try
            {
                existingCamp = await Repo.CampDetails(campID);
                //existingChar = Repo.CharDetails(charID);
                Repo.JoinCamp(campID, character.CharID);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }


            if (existingCamp == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("AddGM/{campID}")]
        public async Task<ActionResult> AddGM(int campID, [FromBody] User user)
        {
            Campaign existingCamp;

            try
            {
                existingCamp = await Repo.CampDetails(campID);
                Repo.AddGM(campID, user.UserID);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

            if (existingCamp == null)
            {
                return NotFound();
            }

            if (user == null)
            {
                return NotFound();
            }

            return NoContent();
        }
        [HttpPut("RemGM/campID")]
        public async Task<ActionResult> RemGM(int campID, [FromBody] User user)
        {
            Campaign existingcamp;

            try
            {
                existingcamp = await Repo.CampDetails(campID);
                Repo.RemGM(campID, user.UserID);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);                
            }

            if (existingcamp == null)
            {
                return NotFound();
            }

            if (user == null)
            {
                return NotFound();
            }

            return NoContent();

        }
        [HttpGet("GMList")]
        public async Task<ActionResult<IEnumerable<User>>> GmList(int campID)
        {
            Campaign existingCamp = await Repo.CampDetails(campID);

            if (existingCamp == null)
            {
                return NotFound();
            }

            try
            {
                return (await Repo.GetGmByCampaign(campID)).ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}

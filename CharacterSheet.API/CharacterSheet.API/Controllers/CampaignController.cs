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
    public class CampaignController : ControllerBase
    {

        public IRepo Repo { get; set; }
        
        public CampaignController(IRepo repo)
        {
            Repo = repo;
        }

        // GET: api/Campaign
        [HttpGet]
        public ActionResult<IEnumerable<Campaign>> Get()
        {
            try
            {
                return Repo.CampList().ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        // GET: api/Campaign/5
        [HttpGet("{id}", Name = "Get")]
        public ActionResult<Campaign> Get(int id)
        {
            Campaign campaign;
            try
            {
                campaign = Repo.CampDetails(id);
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

        // POST: api/Campaign
        [HttpPost]
        public void Post([FromBody] Campaign campaign)
        {
            Repo.CreateCampaign(campaign);
        }

        // PUT: api/Campaign/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Campaign campaign)
        {
            Campaign existing;

            //pull the campaign entry to be updated
            try
            {
                existing = Repo.CampDetails(id);
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
            catch (Exception ex)
            {
                return NotFound();
            }

        }

        [HttpDelete("{charID}")]
        public ActionResult RemoveCharFromCamp(int campID, int charID)
        {
            Campaign existingCamp;
            Character existingChar;
            try
            {
                existingCamp = Repo.CampDetails(campID);
                existingChar = Repo.CharDetails(charID);
                Repo.RemoveCharFromCamp(campID, charID);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

            
            if (existingCamp == null)
            {
                return NotFound();
            }

            if (existingChar == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public ActionResult JoinCampaign(int campID, int charID)
        {
            Campaign existingCamp;
            Character existingChar;
            try
            {
                existingCamp = Repo.CampDetails(campID);
                existingChar = Repo.CharDetails(charID);
                Repo.JoinCamp(campID, charID);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }


            if (existingCamp == null)
            {
                return NotFound();
            }

            if (existingChar == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

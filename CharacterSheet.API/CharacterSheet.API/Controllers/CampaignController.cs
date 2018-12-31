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
            try
            {
                return Repo.CampDetails(id);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        // POST: api/Campaign
        [HttpPost]
        public void Post([FromBody] Campaign campaign)
        {
            Repo.CreateCampaign(campaign);
        }

        // PUT: api/Campaign/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
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
    }
}

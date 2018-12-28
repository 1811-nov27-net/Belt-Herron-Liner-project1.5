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
        public IEnumerable<Campaign> Get()
        {
            return Repo.CampList();
        }

        // GET: api/Campaign/5
        [HttpGet("{id}", Name = "Get")]
        public Campaign Get(int id)
        {
            return Repo.CampDetails(id);
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
        public void Delete(int id)
        {
            Repo.DeleteCamp(id);
        }
    }
}

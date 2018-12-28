using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClassLibrary;
using DataAccess;

namespace CharacterSheet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {

        public IRepo Repo { get; set; }

        public UserController(IRepo repo)
        {
            Repo = repo;
        }

        [HttpGet("{id}", Name = "Get")]
        [Produces("application/json")]
        public ActionResult<IEnumerable<Character>> GetCharByCamp(int id)
        {
            var data = Repo.CharacterListByCamp(id).ToList();
            return data;
        }

        [HttpGet("{id}", Name = "Get")]
        [Produces("application/json")]
        public ActionResult<IEnumerable<Character>> GetCharByUser(int id)
        {
            var data = Repo.CharacterListByUser(id).ToList();
            return data;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Campaign>> Get()
        {
            var data = Repo.CampList().ToList();
            return data;
        }

        /*[HttpPost]
        public ActionResult PostCampaign(Campaign campaign)
        {
            Repo.CreateCampaign(campaign);
        }*/

    }
}
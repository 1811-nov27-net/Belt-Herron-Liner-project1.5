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
    public class CampaignController : Controller
    {

        //Display list of Campaigns
        //[HttpGet]
        //public ActionResult<IEnumerable<Campaign>> Get()
        //{
        //    return Repo.CampList();
        //}

        // GET: Campaign
        public ActionResult Index()
        {
            return View();
        }

        public Data.IRepo Repo { get; set; }

        public CampaignController(Data.IRepo repo)
        {
            Repo = repo;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Campaign>> Get()
        {
            var data = Repo.CampList().ToList();
            return data;
        }

    }
}
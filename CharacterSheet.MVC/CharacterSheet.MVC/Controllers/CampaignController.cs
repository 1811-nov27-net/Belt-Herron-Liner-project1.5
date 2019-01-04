using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CharacterSheet.MVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CharacterSheet.MVC.Controllers
{
    public class CampaignController : AServiceController
    {
        public CampaignController(HttpClient client) : base(client)
        {
        }
        // GET: Campaign
        public async Task<ActionResult> Index()
        {
            var username = ViewBag.LoggedInUser;
            HttpRequestMessage message = CreateServiceRequest(HttpMethod.Get, $"api/User/ByName/{username}");
            HttpResponseMessage response = await Client.SendAsync(message);
            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Error", "Home");
            }
            var responseBody = await response.Content.ReadAsStringAsync();
            User user = JsonConvert.DeserializeObject<User>(responseBody);

            message = CreateServiceRequest(HttpMethod.Get, $"api/Campaign/ByGM/{user.Username}");
            response = await Client.SendAsync(message);
            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Error", "Home");
            }
            responseBody = await response.Content.ReadAsStringAsync();
            IEnumerable<Campaign> campaigns = JsonConvert.DeserializeObject<IEnumerable<Campaign>>(responseBody);
            return View(campaigns);
        }

        // GET: Campaign/Details/5
        public async Task<ActionResult> Details(int id)
        {
            HttpRequestMessage message = CreateServiceRequest(HttpMethod.Get, $"api/Campaign/ByID/{id}");
            HttpResponseMessage response = await Client.SendAsync(message);
            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Error", "Home");
            }
            var responseBody = await response.Content.ReadAsStringAsync();
            Campaign campaign = JsonConvert.DeserializeObject<Campaign>(responseBody);

            message = CreateServiceRequest(HttpMethod.Get, $"api/Character/ByCamp/{id}");
            response = await Client.SendAsync(message);
            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Error", "Home");
            }
            responseBody = await response.Content.ReadAsStringAsync();
            IEnumerable<Character> characters = JsonConvert.DeserializeObject<IEnumerable<Character>>(responseBody);
            ViewData.Add("characters", characters);
            return View(campaign);
        }

        // GET: Campaign/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Campaign/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Campaign campaign)
        {
            try
            {
                HttpRequestMessage message = CreateServiceRequest(HttpMethod.Post, "api/Campaign", campaign);
                HttpResponseMessage response = await Client.SendAsync(message);

                if (!response.IsSuccessStatusCode)
                {
                    return View(campaign);
                }
                var responseBody = await response.Content.ReadAsStringAsync();
                int campID = JsonConvert.DeserializeObject<int>(responseBody);
                message = CreateServiceRequest(HttpMethod.Get, $"api/User/ByName/{ViewBag.LoggedInUser}");
                response = await Client.SendAsync(message);
                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Error");
                }
                responseBody = await response.Content.ReadAsStringAsync();
                User user = JsonConvert.DeserializeObject<User>(responseBody);

                message = CreateServiceRequest(HttpMethod.Put, $"api/Campaign/AddGM/{campID}", user);
                response = await Client.SendAsync(message);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction("Error");
            }
            catch
            {
                return View(campaign);
            }
        }

        // GET: Campaign/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var campaign = JsonConvert.DeserializeObject<Campaign>(await Client.GetStringAsync($"https://localhost:44309/api/Campaign/ByID/{id}"));
            HttpRequestMessage message = CreateServiceRequest(HttpMethod.Get, $"api/Character/ByCamp/{id}");
            HttpResponseMessage response = await Client.SendAsync(message);
            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Error", "Home");
            }
            var responseBody = await response.Content.ReadAsStringAsync();
            IEnumerable<Character> characters = JsonConvert.DeserializeObject<IEnumerable<Character>>(responseBody);
            campaign.Characters = (List<Character>)characters;
            TempData["camp"] = campaign.CampID;
            return View(campaign);
        }

        // POST: Campaign/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Campaign campaign)
        {
            try
            {
                campaign.CampID = id;
                var url = $"https://localhost:44309/api/Campaign/{id}";
                var response = await Client.PutAsJsonAsync(url, campaign);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));
                return View(campaign);
            }
            catch
            {
                return View(campaign);
            }
        }

        public IActionResult AddChar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddChar(Character character)
        {
            try
            {
                int id = (int)TempData["camp"];
                Campaign campaign = JsonConvert.DeserializeObject<Campaign>(await Client.GetStringAsync($"https://localhost:44309/api/Campaign/ByID/{id}"));
                HttpRequestMessage message = CreateServiceRequest(HttpMethod.Get, $"api/Character/{character.CharID}");
                HttpResponseMessage response = await Client.SendAsync(message);

                if (!response.IsSuccessStatusCode)
                {
                    return View();
                }
                var responseBody = await response.Content.ReadAsStringAsync();
                character = JsonConvert.DeserializeObject<Character>(responseBody);

                message = CreateServiceRequest(HttpMethod.Put, $"api/Campaign/AddChar/{campaign.CampID}", character);
                response = await Client.SendAsync(message);

                if (!response.IsSuccessStatusCode)
                {
                    return View(character);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> RemoveChar(int id)
        {
            return View(JsonConvert.DeserializeObject<Character>(await Client.GetStringAsync($"https://localhost:44309/api/Character/{id}")));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveChar(int id, Character character)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int campID = (int)TempData["camp"];
                    Campaign campaign = JsonConvert.DeserializeObject<Campaign>(await Client.GetStringAsync($"https://localhost:44309/api/Campaign/ByID/{campID}"));
                    HttpRequestMessage message = CreateServiceRequest(HttpMethod.Put, $"api/Campaign/RemoveChar/{campaign.CampID}", character);
                    HttpResponseMessage response = await Client.SendAsync(message);
                    if (response.IsSuccessStatusCode)
                        return RedirectToAction("Index");
                    return View(character);
                }
                return View(character);
            }
            catch
            {
                return View(character);
            }
        }

        public IActionResult AddGM()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddGM(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int id = (int)TempData["camp"];
                    Campaign campaign = JsonConvert.DeserializeObject<Campaign>(await Client.GetStringAsync($"https://localhost:44309/api/Campaign/ByID/{id}"));
                    HttpRequestMessage message = CreateServiceRequest(HttpMethod.Get, $"api/User/ByName/{user.Username}");
                    HttpResponseMessage response = await Client.SendAsync(message);
                    if (!response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Error", "Home");
                    }
                    var responseBody = await response.Content.ReadAsStringAsync();
                    user = JsonConvert.DeserializeObject<User>(responseBody);

                    message = CreateServiceRequest(HttpMethod.Put, $"api/Campaign/AddGM/{campaign.CampID}", user);
                    response = await Client.SendAsync(message);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    return RedirectToAction("Error", "Home");
                }
                return RedirectToAction("Error", "Home");
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> RemoveGM()
        {
            int id = (int)TempData.Peek("camp");
            Campaign campaign = JsonConvert.DeserializeObject<Campaign>(await Client.GetStringAsync($"https://localhost:44309/api/Campaign/ByID/{id}"));
            HttpRequestMessage message = CreateServiceRequest(HttpMethod.Get, $"api/Campaign/GMList/{campaign.CampID}");
            HttpResponseMessage response = await Client.SendAsync(message);
            if(!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Error", "Home");
            }
            var responseBody = await response.Content.ReadAsStringAsync();
            IEnumerable<User> GMs = JsonConvert.DeserializeObject<IEnumerable<User>>(responseBody);
            return View(GMs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveGM(int id)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    int campID = (int)TempData["camp"];
                    Campaign campaign = JsonConvert.DeserializeObject<Campaign>(await Client.GetStringAsync($"https://localhost:44309/api/Campaign/ByID/{campID}"));
                    HttpRequestMessage message = CreateServiceRequest(HttpMethod.Get, $"api/User/ByID/{id}");
                    HttpResponseMessage response = await Client.SendAsync(message);
                    if(!response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Error", "Home");
                    }
                    var responseBody = await response.Content.ReadAsStringAsync();
                    User user = JsonConvert.DeserializeObject<User>(responseBody);

                    message = CreateServiceRequest(HttpMethod.Put, $"api/Campaign/RemGM/{campaign.CampID}", user);
                    response = await Client.SendAsync(message);
                    if(response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
                return RedirectToAction("Error", "Home");
            }
            catch 
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Campaign/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            HttpRequestMessage message = CreateServiceRequest(HttpMethod.Get, $"api/Campaign/ByID/{id}");
            HttpResponseMessage response = await Client.SendAsync(message);
            if(!response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            var responseBody = await response.Content.ReadAsStringAsync();
            Campaign campaign = JsonConvert.DeserializeObject<Campaign>(responseBody);
            return View(campaign);
        }

        // POST: Campaign/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Campaign campaign)
        {
            try
            {
                HttpRequestMessage message = CreateServiceRequest(HttpMethod.Delete, $"api/Campaign/{campaign.CampID}");
                HttpResponseMessage response = await Client.SendAsync(message);
                if(!response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Error", "Home");
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
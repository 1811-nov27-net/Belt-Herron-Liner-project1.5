﻿using System;
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
            var username = User.Identity.Name;
            HttpRequestMessage message = CreateServiceRequest(HttpMethod.Get, $"api/User/{username}");
            HttpResponseMessage response = await Client.SendAsync(message);
            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Error", "Home");
            }
            var responseBody = await response.Content.ReadAsStringAsync();
            User user = JsonConvert.DeserializeObject<User>(responseBody);

            message = CreateServiceRequest(HttpMethod.Get, $"api/Campaign/{user.Username}");
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
            HttpRequestMessage message = CreateServiceRequest(HttpMethod.Get, $"api/Campaign/{id}");
            HttpResponseMessage response = await Client.SendAsync(message);
            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Error", "Home");
            }
            var responseBody = await response.Content.ReadAsStringAsync();
            Campaign campaign = JsonConvert.DeserializeObject<Campaign>(responseBody);

            message = CreateServiceRequest(HttpMethod.Get, $"api/Character/GetCharsByCamp/{id}");
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
                message = CreateServiceRequest(HttpMethod.Get, $"api/User/{User.Identity.Name}");
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
        public async Task<ActionResult> Edit(int id)
        {
            var campaign = JsonConvert.DeserializeObject<Campaign>(await Client.GetStringAsync($"https://localhost:44309/api/Campaign/{id}"));
            TempData["camp"] = campaign;
            return View(campaign);
        }

        // POST: Campaign/Edit/5
        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Campaign campaign)
        {
            try
            {
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

        public ActionResult AddChar()
        {
            return View();
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddChar(Character character)
        {
            try
            {
                Campaign campaign = (Campaign)TempData["camp"];
                HttpRequestMessage message = CreateServiceRequest(HttpMethod.Get, $"api/Character/{character.CharID}");
                HttpResponseMessage response = await Client.SendAsync(message);

                if (!response.IsSuccessStatusCode)
                {
                    return View(character);
                }
                var responseBody = await response.Content.ReadAsStringAsync();
                character = JsonConvert.DeserializeObject<Character>(responseBody);

                message = CreateServiceRequest(HttpMethod.Put, $"api/Campaign/JoinCamp/{campaign.CampID}", character);
                response = await Client.SendAsync(message);

                if (!response.IsSuccessStatusCode)
                {
                    return View(character);
                }
                return RedirectToAction(nameof(Edit), campaign.CampID);
            }
            catch
            {
                return View(character);
            }
        }

        public async Task<ActionResult> RemoveChar(int id)
        {
            return View(JsonConvert.DeserializeObject<Character>(await Client.GetStringAsync($"https://localhost:44309/api/Character/{id}")));
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveChar(int id, Character character)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Campaign campaign = (Campaign)TempData["camp"];
                    HttpRequestMessage message = CreateServiceRequest(HttpMethod.Put, $"api/Campaign/RemoveCharFromCamp/{campaign.CampID}", character);
                    HttpResponseMessage response = await Client.SendAsync(message);
                    if (response.IsSuccessStatusCode)
                        return RedirectToAction(nameof(Edit), campaign.CampID);
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

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddGM(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Campaign campaign = (Campaign)TempData["camp"];
                    HttpRequestMessage message = CreateServiceRequest(HttpMethod.Get, $"api/User/{user.Username}");
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
                        return RedirectToAction("Edit", campaign.CampID);
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
            Campaign campaign = (Campaign)TempData.Peek("camp");
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

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveGM(int id)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    Campaign campaign = (Campaign)TempData["camp"];
                    HttpRequestMessage message = CreateServiceRequest(HttpMethod.Get, $"api/User/{id}");
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
                        return RedirectToAction("Edit", campaign.CampID);
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
            HttpRequestMessage message = CreateServiceRequest(HttpMethod.Get, $"api/Campaign/{id}");
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
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
    public class CharacterController : AServiceController
    {

        public CharacterController(HttpClient client) : base(client)
        {

        }

        // GET: Character
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

            message = CreateServiceRequest(HttpMethod.Get, $"api/Character/ByUser/{user.UserID}");
            response = await Client.SendAsync(message);
            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Error", "Home");
            }

            responseBody = await response.Content.ReadAsStringAsync();
            IEnumerable<Character> characters = JsonConvert.DeserializeObject<IEnumerable<Character>>(responseBody);
            return View(characters);
        }

        // GET: Character/Details/5
        public async Task<ActionResult> Details(int id)
        {
            HttpRequestMessage message = CreateServiceRequest(HttpMethod.Get, $"api/Character/{id}");
            HttpResponseMessage response = await Client.SendAsync(message);

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Error", "Home");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            Character character = JsonConvert.DeserializeObject<Character>(responseBody);

            return View(character);
        }

        // GET: Character/Create
        public ActionResult Create()
        {
            Character character = new Character();
            return View(character);
        }

        // POST: Character/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Character character)
        {
            try
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
                character.UserID = user.UserID;
                character.CampID = 1;
                character.CalculateBonusesAndSaves();
                message = CreateServiceRequest(HttpMethod.Post, "api/Character", character);
                response = await Client.SendAsync(message);

                if (!response.IsSuccessStatusCode)
                {
                    return View(character);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(character);
            }
        }

        // GET: Character/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var character = JsonConvert.DeserializeObject<Character>(await Client.GetStringAsync($"https://localhost:44309/api/Character/{id}"));
            TempData["char"] = character.CharID;
            return View(character);
        }

        // POST: Character/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Character character)
        {
            try
            {
                var url = $"https://localhost:44309/api/Character/{id}";
                var response = await Client.PutAsJsonAsync(url, character);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View(character);
            }
            catch
            {
                return View(character);
            }
        }

        // GET: Character/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            HttpRequestMessage message = CreateServiceRequest(HttpMethod.Get, $"api/Character/{id}");
            HttpResponseMessage response = await Client.SendAsync(message);

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            Character character = JsonConvert.DeserializeObject<Character>(responseBody);
            return View(character);
        }

        // POST: Character/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Character character)
        {
            try
            {
                HttpRequestMessage message = CreateServiceRequest(HttpMethod.Delete, $"api/Character/{id}");
                HttpResponseMessage response = await Client.SendAsync(message);

                if (!response.IsSuccessStatusCode)
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

        public IActionResult AddSkill()
        {
            return View();
        }

        public async Task<ActionResult> AddSkill(GenericDictionary skill)
        {
            try
            {
                int charID = (int)TempData["char"];
                HttpRequestMessage message = CreateServiceRequest(HttpMethod.Get, $"api/Character/ByID/{charID}");
                HttpResponseMessage response = await Client.SendAsync(message);

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Error", "Home");
                }
                var responseBody = await response.Content.ReadAsStringAsync();
                Character character = JsonConvert.DeserializeObject<Character>(responseBody);
                character.SkillList.Add(skill.key, skill.value);

                message = CreateServiceRequest(HttpMethod.Put, $"api/Character/{charID}");
                response = await Client.SendAsync(message);

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Error", "Home");
                }


                return RedirectToAction(nameof(Edit));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult AddFeat()
        {
            return View();
        }

        public async Task<ActionResult> AddFeat(GenericDictionary feat)
        {
            try
            {
                int charID = (int)TempData["char"];
                HttpRequestMessage message = CreateServiceRequest(HttpMethod.Get, $"api/Character/ByID/{charID}");
                HttpResponseMessage response = await Client.SendAsync(message);

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Error", "Home");
                }

                var responseBody = await response.Content.ReadAsStringAsync();
                Character character = JsonConvert.DeserializeObject<Character>(responseBody);
                character.FeatList.Add(feat.key, feat.value);

                message = CreateServiceRequest(HttpMethod.Put, $"api/Character/{charID}");
                response = await Client.SendAsync(message);

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Error", "Home");
                }

                return RedirectToAction(nameof(Edit));
            }

            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        
    }
}
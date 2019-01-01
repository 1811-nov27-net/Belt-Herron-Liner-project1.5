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
            var username = User.Identity.Name;
            HttpRequestMessage message = CreateServiceRequest(HttpMethod.Get, $"api/User/{username}");
            HttpResponseMessage response = await Client.SendAsync(message);
            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Error", "Home");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            User user = JsonConvert.DeserializeObject<User>(responseBody);

            message = CreateServiceRequest(HttpMethod.Get, $"api/Character/{user.Username}");
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
            return View();
        }

        // POST: Character/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Character/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Character/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Character/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Character/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
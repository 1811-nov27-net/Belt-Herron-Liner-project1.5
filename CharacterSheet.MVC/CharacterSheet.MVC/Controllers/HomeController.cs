using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CharacterSheet.MVC.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;
using System.Net;

namespace CharacterSheet.MVC.Controllers
{
    public class HomeController : AServiceController
    {
        public HomeController(HttpClient client) : base(client)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginUser user)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return View();
                }
                HttpRequestMessage request = CreateServiceRequest(HttpMethod.Post, "api/User/Login", user);
                HttpResponseMessage response = await Client.SendAsync(request);

                if(!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.Forbidden)
                    {
                        ModelState.AddModelError("Password", "Incorrect Username or Password");
                    }
                    return View();
                }
                var success = PassCookiesToClient(response);
                if(!success)
                {
                    return View("Error");
                }
                return RedirectToAction("PlayerOrGM");
            }
            catch 
            {
                return RedirectToAction("Error");
            }
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Register(LoginUser user)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return View();
                }
                HttpRequestMessage message = CreateServiceRequest(HttpMethod.Post, "api/User/Register", user);
                HttpResponseMessage response = await Client.SendAsync(message);

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Error");
                }
                var success = PassCookiesToClient(response);
                if (!success)
                {
                    return View("Error");
                }
                return RedirectToAction("PlayerOrGM");
            }
            catch (Exception ex)
            {

                return RedirectToAction("Error", ex);
            }
        }

        public async Task<IActionResult> Logout()
        {
            HttpRequestMessage message = CreateServiceRequest(HttpMethod.Post, "api/User/Logout");
            HttpResponseMessage response = await Client.SendAsync(message);
            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Error");
        }

        public async Task<IActionResult> PlayerOrGM()
        {
            HttpRequestMessage message = CreateServiceRequest(HttpMethod.Get, "api/User/IsGM");
            HttpResponseMessage response = await Client.SendAsync(message);
            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Error", "Home");
            }
            var responseBody = await response.Content.ReadAsStringAsync();
            bool IsGM = JsonConvert.DeserializeObject<bool>(responseBody);
            if (IsGM)
            {
                return RedirectToAction("Index","Campaign");
            }
            return RedirectToAction("Index", "Character");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private bool PassCookiesToClient(HttpResponseMessage apiResponse)
        {
            if(apiResponse.Headers.TryGetValues("Set-Cookie", out IEnumerable<string> values))
            {
                var authValue = values.FirstOrDefault(x => x.StartsWith(CookieName));
                if(authValue != null)
                {
                    Response.Headers.Add("Set-Cookie", authValue);
                    return true;
                }
            }
            return false;
        }
    }
}

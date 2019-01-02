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
        public async Task<IActionResult> Login(LoginUser user)
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
            catch (Exception)
            {

                return RedirectToAction("Error");
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

        public IActionResult PlayerOrGM()
        {
            if(User.IsInRole("GM"))
            {
                return RedirectToAction("Index","Campaign");
            }
            return RedirectToAction("Index", "Character");
        }

        public IActionResult PlayerLoggedIn()
        {
            return View();
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

        //public HttpRequestMessage CreateServiceRequest(HttpMethod method, string uri, object body)
        //{
        //    var apiRequest = new HttpRequestMessage(method, new Uri(ServiceUri, uri));

        //    if(body != null)
        //    {
        //        var jsonString = JsonConvert.SerializeObject(body);
        //        apiRequest.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
        //    }

        //    var cookieValue = Request.Cookies[CookieName];

        //    if(cookieValue != null)
        //    {
        //        apiRequest.Headers.Add("Cookie", new CookieHeaderValue(CookieName, cookieValue).ToString());
        //    }
        //    return apiRequest;
        //}

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

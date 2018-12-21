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
    public class HomeController : Controller
    {
        private static readonly string CookieName = "UserApiAuthorization";
        private static readonly Uri ServiceUri = new Uri("https://localhost:44309");
        private HttpClient Client;

        public HomeController(HttpClient client)
        {
            Client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Login(User user)
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
                throw;
            }
        }

        public IActionResult PlayerOrGM()
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

        public HttpRequestMessage CreateServiceRequest(HttpMethod method, string uri, object body)
        {
            var apiRequest = new HttpRequestMessage(method, new Uri(ServiceUri, uri));

            if(body != null)
            {
                var jsonString = JsonConvert.SerializeObject(body);
                apiRequest.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            }

            var cookieValue = Request.Cookies[CookieName];

            if(cookieValue != null)
            {
                apiRequest.Headers.Add("Cookie", new CookieHeaderValue(CookieName, cookieValue).ToString());
            }
            return apiRequest;
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

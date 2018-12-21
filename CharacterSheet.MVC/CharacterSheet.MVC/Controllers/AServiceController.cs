using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CharacterSheet.MVC.Controllers
{
    public class AServiceController : Controller
    {
        protected static readonly string CookieName = "UserApiAuthorization";
        private static readonly Uri ServiceUri = new Uri("https://localhost:44309");
        public HttpClient Client;

        public AServiceController(HttpClient client)
        {
            Client = client;
        }

        public HttpRequestMessage CreateServiceRequest(HttpMethod method, string uri, object body = null)
        {
            var apiRequest = new HttpRequestMessage(method, new Uri(ServiceUri, uri));

            if (body != null)
            {
                var jsonString = JsonConvert.SerializeObject(body);
                apiRequest.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            }

            var cookieValue = Request.Cookies[CookieName];

            if (cookieValue != null)
            {
                apiRequest.Headers.Add("Cookie", new CookieHeaderValue(CookieName, cookieValue).ToString());
            }
            return apiRequest;
        }
    }
}
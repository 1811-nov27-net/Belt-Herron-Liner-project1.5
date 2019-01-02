using System.Net.Http;
using System.Threading.Tasks;
using CharacterSheet.MVC.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CharacterSheet.MVC.Filters
{
    public class GetLoggedInUserFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controller = context.Controller as AServiceController;
            if(controller != null)
            {
                HttpRequestMessage request = controller.CreateServiceRequest(HttpMethod.Get, "api/user/loggedinuser");
                HttpResponseMessage response = await controller.Client.SendAsync(request);

                if(!response.IsSuccessStatusCode)
                {
                    controller.ViewBag.LoggedInUser = "";
                }
                controller.ViewBag.LoggedInUser = await response.Content.ReadAsStringAsync();
            }
            var resultContext = await next();
        }
    }
}
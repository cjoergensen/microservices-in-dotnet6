using Microsoft.AspNetCore.Mvc;
using SelfService.WebApp.Shared.Models;

namespace SelfService.WebApp.Server.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            var profile =  new Profile
            {
                CustomerId = 1,
                Name = "Christian Jørgensen",
                DateOfBirth = DateTimeOffset.Now.AddYears(-30),
                NotificationSettings = new()
                {
                    Email = "cjoergensen@live.com",
                    Phonenumber = "12345678",
                    PreferedCommunicationChannel = WebApp.Shared.Models.CommunicationChannel.Email
                }
            };


            return View(profile);
        }
    }
}
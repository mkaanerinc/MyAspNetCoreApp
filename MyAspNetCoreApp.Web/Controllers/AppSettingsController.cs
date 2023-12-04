using Microsoft.AspNetCore.Mvc;

namespace MyAspNetCoreApp.Web.Controllers
{
    public class AppSettingsController : Controller
    {
        private readonly IConfiguration _configuration;

        public AppSettingsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            ViewBag.SmsKey = _configuration["Keys:Sms"];
            ViewBag.EmailKey = _configuration.GetSection("Keys")["Email"];
            return View();
        }
    }
}

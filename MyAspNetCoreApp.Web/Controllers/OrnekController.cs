using Microsoft.AspNetCore.Mvc;

namespace MyAspNetCoreApp.Web.Controllers
{
    public class Product2
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class OrnekController : Controller
    {
        public IActionResult Index()
        {
            var productList = new List<Product2>() 
            {
                new Product2 { Id = 1, Name  = "Kalem" },
                new Product2 { Id = 2, Name  = "Defter" },
                new Product2 { Id = 3, Name  = "Silgi" }
            };

            return View(productList);
        }

        public IActionResult IndexTwo()
        {
            return RedirectToAction("Index","Ornek");
        }

        public IActionResult ParametreView(int id)
        {
            return RedirectToAction("JsonResultParametre", "Ornek", new {id = id});
        }

        public IActionResult ContentResult()
        {
            return Content("ContentResult");
        }

        public IActionResult JsonResult()
        {
            return Json(new {Id=1,Name= "Kalem", Price = 300});
        }

        public IActionResult JsonResultParametre(int id)
        {
            return Json(new { Id = id });
        }

        public IActionResult EmptyResult()
        {
            return new EmptyResult();
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyAspNetCoreApp.Web.Models;
using MyAspNetCoreApp.Web.ViewModels;
using System.Diagnostics;

namespace MyAspNetCoreApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, AppDbContext appDbContext, IMapper mapper)
        {
            _logger = logger;
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var products = _appDbContext.Products.OrderByDescending(p => p.Id)
                .Select(p => new ProductPartialViewModel()
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock
            }).ToList();

            ViewBag.ProductListPartialViewModel = new ProductListPartialViewModel() 
            { Products = products};

            return View();
        }

        public IActionResult Privacy()
        {
            var products = _appDbContext.Products.OrderByDescending(p => p.Id)
                .Select(p => new ProductPartialViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock
                }).ToList();

            ViewBag.ProductListPartialViewModel = new ProductListPartialViewModel()
            { Products = products };

            return View();
        }

        public IActionResult Visitor()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveVisitorComment(VisitorViewModel visitorViewModel)
        {
            try
            {
                var visitor = _mapper.Map<Visitor>(visitorViewModel);

                visitor.Created = DateTime.Now;

                _appDbContext.Visitors.Add(visitor);
                _appDbContext.SaveChanges();

                TempData["result"] = "Yorum başarılı bir şekilde kaydedilmiştir.";

                return RedirectToAction(nameof(HomeController.Visitor));
            }
            catch (Exception)
            {
                TempData["result"] = "Yorum kaydedilirken bir hata oluşmuştur.";

                return RedirectToAction(nameof(HomeController.Visitor));
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
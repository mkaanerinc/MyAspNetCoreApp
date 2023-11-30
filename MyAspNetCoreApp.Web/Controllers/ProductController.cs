using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyAspNetCoreApp.Web.Filters;
using MyAspNetCoreApp.Web.Helpers;
using MyAspNetCoreApp.Web.Models;
using MyAspNetCoreApp.Web.ViewModels;

namespace MyAspNetCoreApp.Web.Controllers
{
    public class ProductController : Controller
    {

        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public ProductController(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        [CacheResourceFilter]
        [HttpGet]
        public IActionResult Index()
        {

            var products = _appDbContext.Products.ToList();

            return View(_mapper.Map<List<ProductViewModel>>(products));
        }

        [Route("[controller]/[action]/{page}/{pageSize}")]
        public IActionResult Pages(int page, int pageSize)
        {
            var products = _appDbContext.Products.Skip((page - 1) * pageSize)
                .Take(pageSize).ToList();

            ViewBag.Page=page;
            ViewBag.PageSize=pageSize;

            return View(_mapper.Map<List<ProductViewModel>>(products));
        }

        [ServiceFilter(typeof(NotFoundFilter))]
        [Route("[controller]/[action]/{productId}")]
        public IActionResult GetById(int productId)
        {
            var product = _appDbContext.Products.Find(productId);

            return View(_mapper.Map<ProductViewModel>(product));
        }

        [ServiceFilter(typeof(NotFoundFilter))]
        public IActionResult Remove(int id)
        {
            var product = _appDbContext.Products.Find(id);
            _appDbContext.Products.Remove(product);
            _appDbContext.SaveChanges();

            return RedirectToAction("Index","Product");
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Expire = new Dictionary<string, int>()
            {
                { "1 Ay", 1 },
                { "3 Ay", 3 },
                { "6 Ay", 6 },
                { "12 Ay", 12 },
            };

            ViewBag.ColorSelect = new SelectList(new List<ColorSelectList>()
            {
                new ColorSelectList(){Data = "Mavi", Value = "Mavi"},
                new ColorSelectList(){Data = "Kırmızı", Value = "Kırmızı"},
                new ColorSelectList(){Data = "Sarı", Value = "Sarı"},
            },"Value","Data");

            return View();
        }

        [HttpPost]
        public IActionResult Add(ProductViewModel productViewModel)
        {
            //if (!string.IsNullOrEmpty(productViewModel.Name) && productViewModel.Name.StartsWith("A"))
            //{
            //    ModelState.AddModelError(String.Empty, "Ürün harfi A ile başlayamaz.");
            //}

            ViewBag.Expire = new Dictionary<string, int>()
                {
                    { "1 Ay", 1 },
                    { "3 Ay", 3 },
                    { "6 Ay", 6 },
                    { "12 Ay", 12 },
                };

            ViewBag.ColorSelect = new SelectList(new List<ColorSelectList>()
                {
                    new ColorSelectList(){Data = "Mavi", Value = "Mavi"},
                    new ColorSelectList(){Data = "Kırmızı", Value = "Kırmızı"},
                    new ColorSelectList(){Data = "Sarı", Value = "Sarı"},
                }, "Value", "Data");

            if (ModelState.IsValid)
            {
                try
                {
                    _appDbContext.Products.Add(_mapper.Map<Product>(productViewModel));
                    _appDbContext.SaveChanges();

                    TempData["status"] = "Ürün başarıyla eklendi.";

                    return RedirectToAction("Index", "Product");
                }
                catch (Exception)
                {

                    ModelState.AddModelError(String.Empty,"Ürün kaydedilirken bir hata meydana geldi.");

                    return View();
                }
            }else
            {
                return View();
            }           
        }

        [ServiceFilter(typeof(NotFoundFilter))]
        [HttpGet]
        public IActionResult Update(int id)
        {
            var product = _appDbContext.Products.Find(id);

            ViewBag.ExpireValue = product.Expire;
            ViewBag.Expire = new Dictionary<string, int>()
            {
                { "1 Ay", 1 },
                { "3 Ay", 3 },
                { "6 Ay", 6 },
                { "12 Ay", 12 },
            };

            ViewBag.ColorSelect = new SelectList(new List<ColorSelectList>()
            {
                new ColorSelectList(){Data = "Mavi", Value = "Mavi"},
                new ColorSelectList(){Data = "Kırmızı", Value = "Kırmızı"},
                new ColorSelectList(){Data = "Sarı", Value = "Sarı"},
            }, "Value", "Data",product.Color);

            return View(_mapper.Map<ProductViewModel>(product));
        }

        [HttpPost]
        public IActionResult Update(ProductViewModel productViewModel)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.ExpireValue = productViewModel.Expire;
                ViewBag.Expire = new Dictionary<string, int>()
                {
                    { "1 Ay", 1 },
                    { "3 Ay", 3 },
                    { "6 Ay", 6 },
                    { "12 Ay", 12 },
                };

                ViewBag.ColorSelect = new SelectList(new List<ColorSelectList>()
                {
                    new ColorSelectList(){Data = "Mavi", Value = "Mavi"},
                    new ColorSelectList(){Data = "Kırmızı", Value = "Kırmızı"},
                    new ColorSelectList(){Data = "Sarı", Value = "Sarı"},
                }, "Value", "Data", productViewModel.Color);

                return View();
            }

            _appDbContext.Products.Update(_mapper.Map<Product>(productViewModel));
            _appDbContext.SaveChanges();

            TempData["status"] = "Ürün başarıyla güncellendi.";

            return RedirectToAction("Index","Product");
        }

        [AcceptVerbs("GET","POST")]
        public IActionResult HasProductName(string Name)
        {
            var anyProduct = _appDbContext.Products.Any(x => x.Name.ToLower() == Name.ToLower());

            if (anyProduct)
            {
                return Json("Kaydetmeye çalıştığınız ürün veritabanında kayıtlıdır.");
            }else
            {
                return Json(true);
            }
        }
    }
}

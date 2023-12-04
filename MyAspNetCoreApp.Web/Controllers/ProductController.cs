using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
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
        private readonly IFileProvider _fileProvider;

        public ProductController(AppDbContext appDbContext, IMapper mapper, IFileProvider fileProvider)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _fileProvider = fileProvider;
        }

        //[CacheResourceFilter]
        [HttpGet]
        public IActionResult Index()
        {
            List<ProductViewModel> products = _appDbContext.Products.Include(p => p.Category)
                .Select(p => new ProductViewModel
                {
                   Id = p.Id,
                   Name = p.Name,
                   Price = p.Price,
                   Stock = p.Stock,
                   CategoryName = p.Category.Name,
                   Color = p.Color,
                   Description = p.Description,
                   Expire = p.Expire,
                   ImagePath = p.ImagePath,
                   isPublish = p.isPublish,
                   PublishDate = p.PublishDate
                }).ToList();

            return View(products);
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

            var categories = _appDbContext.Category.ToList();

            ViewBag.CategorySelect = new SelectList(categories, "Id", "Name");

            return View();
        }

        [HttpPost]
        public IActionResult Add(ProductViewModel productViewModel)
        {
            IActionResult result = null;

            if (ModelState.IsValid)
            {
                try
                {
                    var product = _mapper.Map<Product>(productViewModel);

                    if (productViewModel.Image != null && productViewModel.Image.Length > 0)
                    {
                        var root = _fileProvider.GetDirectoryContents("wwwroot");
                        var images = root.First(x => x.Name == "images");

                        var randomImageName = Guid.NewGuid() + Path.GetExtension(productViewModel.Image.FileName);

                        var path = Path.Combine(images.PhysicalPath, randomImageName);

                        using FileStream fileStream = new FileStream(path, FileMode.Create);

                        productViewModel.Image.CopyTo(fileStream);

                        product.ImagePath = randomImageName;
                    }

                    _appDbContext.Products.Add(product);
                    _appDbContext.SaveChanges();

                    TempData["status"] = "Ürün başarıyla eklendi.";

                    return RedirectToAction("Index", "Product");
                }
                catch (Exception)
                {                   
                    result = View();
                }
            }else
            {
                result = View();
            }

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

            var categories = _appDbContext.Category.ToList();

            ViewBag.CategorySelect = new SelectList(categories, "Id", "Name");

            return result;
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

            var categories = _appDbContext.Category.ToList();

            ViewBag.CategorySelect = new SelectList(categories, "Id", "Name",product.CategoryId);

            return View(_mapper.Map<ProductUpdateViewModel>(product));
        }

        [HttpPost]
        public IActionResult Update(ProductUpdateViewModel productViewModel)
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

                var categories = _appDbContext.Category.ToList();

                ViewBag.CategorySelect = new SelectList(categories, "Id", "Name", productViewModel.CategoryId);

                return View();
            }

            if(productViewModel.Image != null && productViewModel.Image.Length > 0)
                    {
                var root = _fileProvider.GetDirectoryContents("wwwroot");
                var images = root.First(x => x.Name == "images");

                var randomImageName = Guid.NewGuid() + Path.GetExtension(productViewModel.Image.FileName);

                var path = Path.Combine(images.PhysicalPath, randomImageName);

                using FileStream fileStream = new FileStream(path, FileMode.Create);

                productViewModel.Image.CopyTo(fileStream);

                productViewModel.ImagePath = randomImageName;
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

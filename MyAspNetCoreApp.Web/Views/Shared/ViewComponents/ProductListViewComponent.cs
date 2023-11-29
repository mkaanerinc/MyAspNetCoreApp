using Microsoft.AspNetCore.Mvc;
using MyAspNetCoreApp.Web.Models;
using MyAspNetCoreApp.Web.ViewModels;

namespace MyAspNetCoreApp.Web.Views.Shared.ViewComponents
{
    public class ProductListViewComponent : ViewComponent
    {
        private readonly AppDbContext _appDbContext;
        public ProductListViewComponent(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync(string type = "Default")
        {
            var products = _appDbContext.Products
                .Select(p => new ProductListComponentViewModel()
                {
                    Name = p.Name,
                    Description = p.Description,
                }).ToList();

            return View(type,products);
        }
    }
}

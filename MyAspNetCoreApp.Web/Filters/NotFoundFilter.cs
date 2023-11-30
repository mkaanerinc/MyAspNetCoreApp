using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyAspNetCoreApp.Web.Models;

namespace MyAspNetCoreApp.Web.Filters
{
    public class NotFoundFilter : ActionFilterAttribute
    {
        private readonly AppDbContext _appDbContext;

        public NotFoundFilter(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var idvalue = context.ActionArguments.Values.First();

            var id = (int)idvalue;

            var hasProduct = _appDbContext.Products.Any(p => p.Id == id);

            if(hasProduct == false)
            {
                context.Result = new RedirectToActionResult("Error","Home", new ErrorViewModel()
                {
                    Errors = new List<string>() { $"Veritabanında {id} değerine sahip ürün bulunamamıştır"}
                });
            }
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyAspNetCoreApp.Web.Models;
using MyAspNetCoreApp.Web.ViewModels;

namespace MyAspNetCoreApp.Web.Views.Shared.ViewComponents
{
    public class VisitorViewComponent : ViewComponent
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public VisitorViewComponent(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var visitors = _appDbContext.Visitors.ToList();

            var visitorViewModel = _mapper.Map<List<VisitorViewModel>>(visitors);

            ViewBag.VisitorViewModel = visitorViewModel;

            return View();
        }
    }
}


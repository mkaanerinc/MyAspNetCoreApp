using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyAspNetCoreApp.Web.Models;
using MyAspNetCoreApp.Web.ViewModels;

namespace MyAspNetCoreApp.Web.Controllers
{
    public class VisitorAjaxController : Controller
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;

        public VisitorAjaxController(AppDbContext appDbContext, IMapper mapper)
        {
            _mapper = mapper;
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveVisitorComment(VisitorViewModel visitorViewModel)
        {
                var visitor = _mapper.Map<Visitor>(visitorViewModel);

                visitor.Created = DateTime.Now;

                _appDbContext.Visitors.Add(visitor);
                _appDbContext.SaveChanges();

                return Json(new { IsSuccess = true });
        }

        public async Task<IActionResult> VisitorCommentList()
        {
            var visitors = _appDbContext.Visitors.OrderByDescending(v => v.Created).ToList();

            var visitorViewModel = _mapper.Map<List<VisitorViewModel>>(visitors);

            return Json(visitorViewModel);
        }
    }
}

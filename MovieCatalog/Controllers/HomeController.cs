using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Services.Interfaces;

namespace MovieCatalog.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFilmService _filmService;

        public HomeController(IFilmService filmService)
        {
            _filmService = filmService;
        }

        public async Task<IActionResult> Index()
        {
            var films = await _filmService.GetAllFilmsAsync();

            return View(films);
        }
    }
}

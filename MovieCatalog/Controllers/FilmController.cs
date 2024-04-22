using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieCatalog.Models.Film;
using MovieCatalog.Models.ViewModels;
using MovieCatalog.Services.Interfaces;

namespace MovieCatalog.Controllers
{
    public class FilmController : Controller
    {
        private readonly IFilmService _filmService;

        public FilmController(IFilmService filmService)
        {
            _filmService = filmService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var films = await _filmService.GetAllFilmsAsync();

            return View(films);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var film = await _filmService.GetFilmByIdAsync(id);

            return View(film);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _filmService.GetAlCategoriesAsync();

            FilmCreateViewModel filmViewModel = new FilmCreateViewModel()
            {
                CreateFilmRequest = new CreateFilmRequest(),
                CategorySelectList = categories.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToList()
            };

            return View(filmViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var categories = await _filmService.GetAlCategoriesAsync();
            var film = await _filmService.GetFilmByIdAsync(id);

            FilmUpdateViewModel filmViewModel = new FilmUpdateViewModel()
            {
                UpdateFilmRequest = new UpdateFilmRequest 
                {
                    Id = film.Id,
                    Name = film.Name,
                    Director = film.Director,
                    Release = film.Release,
                    Categories = film.Categories?.Select(category => category.Id).ToList()
                },
                CategorySelectList = categories.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToList()
            };

            return View(filmViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FilmUpdateViewModel filmUpdateViewModel)
        {
            await _filmService.UpdateFilmAsync(filmUpdateViewModel.UpdateFilmRequest);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Create(FilmCreateViewModel filmViewModel)
        {
            await _filmService.CreateFilmAsync(filmViewModel.CreateFilmRequest);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var film = await _filmService.GetFilmByIdAsync(id);

            return View(film);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            await _filmService.DeleteFilmAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}

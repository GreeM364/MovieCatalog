using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Models.Category;
using MovieCatalog.Models.Film;
using MovieCatalog.Services.Interfaces;

namespace MovieCatalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryAPIController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IFilmService _filmService;
        public CategoryAPIController(ICategoryService categoryService, IFilmService filmService)
        {
            _categoryService = categoryService;
            _filmService = filmService;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<CategoryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();

            return Ok(categories);
        }

        [HttpGet("FilmWithCategories/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(FilmResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFilmWithCategories(int id)
        {
            var filmWithCategories = await _filmService.GetFilmByIdAsync(id);

            return Ok(filmWithCategories);
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateFilmCategories(UpdateFilmCategoriesRequest updateFilmCategoriesRequest)
        {
            await _filmService.UpdateFilmCategories(updateFilmCategoriesRequest);

            return Ok();
        }
    }
}

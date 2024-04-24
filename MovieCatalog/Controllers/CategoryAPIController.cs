using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Entities;
using MovieCatalog.Repository.IRepository;
using MovieCatalog.Services.Interfaces;

namespace MovieCatalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryAPIController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IFilmService _filmService;
        private readonly IRepository<Category> _repository;
        public CategoryAPIController(ICategoryService categoryService, IFilmService filmService, IRepository<Category> repository)
        {
            _categoryService = categoryService;
            _filmService = filmService;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            //var categories = await _categoryService.GetAllCategoriesAsync();
            var categories = await _repository.GetAllAsync();

            return Ok(categories);
        }

        [HttpGet("FilmWithCategories/{id}")]
        public async Task<IActionResult> FilmWithCategories(int id)
        {
            var filmWithCategories = await _filmService.GetFilmByIdAsync(id);

            return Ok(filmWithCategories);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateFilmCategories(int id, [FromBody] List<int> newCategories)
        {
            await _filmService.UpdateFilmCategories(id, newCategories);

            return Ok();
        }
    }
}

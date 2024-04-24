using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieCatalog.Models.Category;
using MovieCatalog.Models.Film;
using MovieCatalog.Models.ViewModels;
using MovieCatalog.Services;
using MovieCatalog.Services.Interfaces;

namespace MovieCatalog.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();

            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id, true);

            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();

            CategoryCreateViewModel createViewModel = new CategoryCreateViewModel()
            {
                CreateCategoryRequest = new CreateCategoryRequest(),
                CategorySelectList = categories.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToList()
            };

            return View(createViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateViewModel createViewModel)
        {
            await _categoryService.CreateCategoryAsync(createViewModel.CreateCategoryRequest);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var parentCategories = await _categoryService.GetAllCategoriesAsync();
            var category = await _categoryService.GetCategoryByIdAsync(id);

            CategoryUpdateViewModel filmViewModel = new CategoryUpdateViewModel()
            {
                UpdateCategoryRequest = new UpdateCategoryRequest()
                {
                    Id = category.Id,
                    Name = category.Name,
                    ParentCategoryId = category.ParentCategory?.Id
                },
                CategorySelectList = parentCategories.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToList()
            };

            return View(filmViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryUpdateViewModel categoryUpdateViewModel)
        {
            await _categoryService.UpdateCategoryAsync(categoryUpdateViewModel.UpdateCategoryRequest);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            var categoryToRemove = await _categoryService.GetCategoryByIdAsync(id);

            if (categoryToRemove.ChildCategories != null && categoryToRemove.ChildCategories.Any())
            {
                ModelState.AddModelError(string.Empty, "You cannot delete a category that has child categories.");
                return View(nameof(Delete), categoryToRemove);
            }

            await _categoryService.DeleteCategoryAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

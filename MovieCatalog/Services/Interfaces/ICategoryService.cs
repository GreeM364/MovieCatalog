using MovieCatalog.Models.Category;

namespace MovieCatalog.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryResponse>> GetAllCategoriesAsync();
        Task<CategoryResponse> GetCategoryByIdAsync(int id, bool includeAllParentCategories = false);
        Task<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest categoryRequest);
        Task<CategoryResponse> UpdateCategoryAsync(UpdateCategoryRequest categoryRequest);
        Task DeleteCategoryAsync(int id);
    }
}

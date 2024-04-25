using MovieCatalog.Models.Category;

namespace MovieCatalog.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryResponse>> GetAllCategoriesAsync();
        Task<List<CategoryResponse>> GetAllCategoriesWithDetailsAsync();
        Task<List<CategoryResponse>> GetParentCategoriesExcludingDescendants(int id);
        Task<CategoryResponse> GetCategoryByIdAsync(int id);
        Task<CategoryResponse> GetCategoryWithDetailsByIdAsync(int id);
        Task<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest categoryRequest);
        Task<CategoryResponse> UpdateCategoryAsync(UpdateCategoryRequest categoryRequest);
        Task DeleteCategoryAsync(int id);
    }
}

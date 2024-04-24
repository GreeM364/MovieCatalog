using AutoMapper;
using MovieCatalog.Entities;
using MovieCatalog.Exceptions;
using MovieCatalog.Models.Category;
using MovieCatalog.Repository.IRepository;
using MovieCatalog.Services.Interfaces;

namespace MovieCatalog.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryService(IRepository<Category> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<List<CategoryResponse>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync(includeProperties: "ParentCategory,FilmCategories.Film");

            if (categories.Count == 0)
                return Enumerable.Empty<CategoryResponse>().ToList();

            var result = _mapper.Map<List<CategoryResponse>>(categories);

            foreach (var category in result)
            {
                category.ParentCategory = await GetParentCategoryAsync(category.ParentCategory?.Id);
                category.LevelOfNesting = CalculateCategoryNestingLevel(category);
            }

            return result;
        }

        public async Task<CategoryResponse> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.FirstOrDefaultAsync(c => c.Id == id,
                includeProperties: "ParentCategory,ChildCategories,FilmCategories.Film");

            if (category is null)
                throw new NotFoundException(nameof(Category), id);

            var result = _mapper.Map<CategoryResponse>(category);
            return result;
        }

        public async Task<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest categoryRequest)
        {
            var existing = await _categoryRepository.
                FirstOrDefaultAsync(f => f.Name == categoryRequest.Name);

            if (existing is not null)
                throw new BadRequestException(nameof(Category), categoryRequest);

            var createdCategory = _mapper.Map<Category>(categoryRequest);
            var category = await _categoryRepository.AddAsync(createdCategory);

            var result = _mapper.Map<CategoryResponse>(category);
            return result;
        }

        public async Task<CategoryResponse> UpdateCategoryAsync(UpdateCategoryRequest categoryRequest)
        {
            var categoryToUpdate = await _categoryRepository.GetByIdAsync(categoryRequest.Id);

            if (categoryToUpdate is null)
                throw new NotFoundException(nameof(Category), categoryRequest.Id);

            var updatedCategory = _mapper.Map(categoryRequest, categoryToUpdate);
            var category = await _categoryRepository.UpdateAsync(updatedCategory);

            var result = _mapper.Map<CategoryResponse>(category);
            return result;
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var categoryToRemove = await _categoryRepository.GetByIdAsync(id);

            if (categoryToRemove is null)
                throw new NotFoundException(nameof(Category), id);

            await _categoryRepository.RemoveAsync(categoryToRemove);
        }

        private async Task<CategoryResponse?> GetParentCategoryAsync(int? parentCategoryId)
        {
            if (parentCategoryId == null)
                return null;

            var parentCategory = await _categoryRepository.FirstOrDefaultAsync(c => c.Id == parentCategoryId.Value,
                includeProperties: "ParentCategory");

            if (parentCategory == null)
                return null;

            var parentCategoryResponse = _mapper.Map<CategoryResponse>(parentCategory);
            parentCategoryResponse.ParentCategory = await GetParentCategoryAsync(parentCategory.ParentCategoryId);

            return parentCategoryResponse;
        }

        private int CalculateCategoryNestingLevel(CategoryResponse? category)
        {
            int nestingLevel = 1;
            var currentCategory = category;

            if (currentCategory is not null)
            {
                while (currentCategory.ParentCategory != null)
                {
                    currentCategory = currentCategory.ParentCategory;
                    nestingLevel++;
                }
            }

            return nestingLevel;
        }
    }
}

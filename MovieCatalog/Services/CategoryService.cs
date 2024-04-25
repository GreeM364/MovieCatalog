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
            var categories = await _categoryRepository.GetAllAsync();

            if (categories.Count == 0)
                return Enumerable.Empty<CategoryResponse>().ToList();

            var result = _mapper.Map<List<CategoryResponse>>(categories);

            return result;
        }

        public async Task<List<CategoryResponse>> GetAllCategoriesWithDetailsAsync()
        {
            var categories = await _categoryRepository.GetAllAsync(includeProperties: "ParentCategory,FilmCategories.Film");

            if (categories.Count == 0)
                return Enumerable.Empty<CategoryResponse>().ToList();

            var result = _mapper.Map<List<CategoryResponse>>(categories);

            result.ForEach(category => category.LevelOfNesting = CalculateCategoryNestingLevel(category));

            return result;
        }

        public async Task<List<CategoryResponse>> GetParentCategoriesExcludingDescendants(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
                throw new NotFoundException(nameof(Category), id);

            var categories = await _categoryRepository.GetAllAsync();

            var filteredCategories = categories
                .Where(c => c.Id != id && !IsDescendant(c, category)).ToList();

            var result = _mapper.Map<List<CategoryResponse>>(filteredCategories);
            return result;
        }

        public async Task<CategoryResponse> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category is null)
                throw new NotFoundException(nameof(Category), id);

            var result = _mapper.Map<CategoryResponse>(category);
            return result;
        }

        public async Task<CategoryResponse> GetCategoryWithDetailsByIdAsync(int id)
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

        private bool IsDescendant(Category category, Category targetCategory)
        {
            var parentCategory = category.ParentCategory;
            while (parentCategory != null)
            {
                if (parentCategory.Id == targetCategory.Id)
                {
                    return true;
                }
                parentCategory = parentCategory.ParentCategory;
            }
            return false;
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

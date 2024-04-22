using AutoMapper;
using MovieCatalog.Entities;
using MovieCatalog.Exceptions;
using MovieCatalog.Models.Category;
using MovieCatalog.Models.Film;
using MovieCatalog.Repository.IRepository;
using MovieCatalog.Services.Interfaces;

namespace MovieCatalog.Services
{
    public class FilmService : IFilmService
    {
        private readonly IRepository<Film> _filmRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<FilmCategory> _filmCategoryRepository;
        private readonly IMapper _mapper;
        public FilmService(IRepository<Film> filmRepository, IMapper mapper, IRepository<Category> categoryRepository, IRepository<FilmCategory> filmCategoryRepository)
        {
            _filmRepository = filmRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _filmCategoryRepository = filmCategoryRepository;
        }

        public async Task<List<FilmResponse>> GetAllFilmsAsync()
        {
            List<Film> films = await _filmRepository.GetAllAsync(includeProperties: "FilmCategories.Category");

            if (films.Count == 0)
                return Enumerable.Empty<FilmResponse>().ToList();

            var result = _mapper.Map<List<FilmResponse>>(films);

            return result;
        }

        public async Task<List<CategoryResponse>> GetAlCategoriesAsync()
        {
            List<Category> categories = await _categoryRepository.GetAllAsync();

            if (categories.Count == 0)
                return Enumerable.Empty<CategoryResponse>().ToList();

            var result = _mapper.Map<List<CategoryResponse>>(categories);

            return result;
        }


        public async Task<FilmResponse> GetFilmByIdAsync(int id)
        {
            var film = await _filmRepository.FirstOrDefaultAsync(f => f.Id == id,
                includeProperties: "FilmCategories.Category");

            if (film is null)
                throw new NotFoundException(nameof(Film), id);


            var result = _mapper.Map<FilmResponse>(film);
            return result;
        }

        public async Task<FilmResponse> CreateFilmAsync(CreateFilmRequest filmRequest)
        {
            var existing = await _filmRepository.
                FirstOrDefaultAsync(f => f.Name == filmRequest.Name && f.Director == filmRequest.Director);

            if (existing is not null)
                throw new BadRequestException(nameof(Film), filmRequest);

            var createdFilm = _mapper.Map<Film>(filmRequest);
            var film = await _filmRepository.AddAsync(createdFilm);

            if (filmRequest.Categories?.Any() ?? false)
            {
                var filmCategories = filmRequest.Categories.Select(categoryId => new FilmCategory { CategoryId = categoryId, FilmId = film.Id });
                await _filmCategoryRepository.AddRangeAsync(filmCategories);
            }

            var result = _mapper.Map<FilmResponse>(film);
            return result;
        }

        public async Task<FilmResponse> UpdateFilmAsync(UpdateFilmRequest filmRequest)
        {
            var filmToUpdate = await _filmRepository.FirstOrDefaultAsync(f => f.Id == filmRequest.Id,
                includeProperties: "FilmCategories.Category");

            if (filmToUpdate is null)
                throw new NotFoundException(nameof(Film), filmRequest.Id);

            _mapper.Map(filmRequest, filmToUpdate);

            if(filmRequest.Categories is not null)
                UpdateFilmCategories(filmToUpdate, filmRequest.Categories);

            await _filmRepository.UpdateAsync(filmToUpdate);
            var result = _mapper.Map<FilmResponse>(filmToUpdate);
            return result;
        }


        public async Task DeleteFilmAsync(int id)
        {
            var filmToRemove = await _filmRepository.GetByIdAsync(id);

            if (filmToRemove is null)
                throw new NotFoundException(nameof(Film), id);

            await _filmRepository.RemoveAsync(filmToRemove);
        }


        private void UpdateFilmCategories(Film filmToUpdate, List<int> newCategories)
        {
            var currentCategories = filmToUpdate.FilmCategories?.Select(fc => fc.CategoryId).ToList();

            var categoriesToRemove = currentCategories?.Except(newCategories);
            if (categoriesToRemove != null)
            {
                foreach (var categoryId in categoriesToRemove)
                {
                    var categoryToRemove = filmToUpdate.FilmCategories?.FirstOrDefault(fc => fc.CategoryId == categoryId);
                    if (categoryToRemove is not null)
                    {
                        filmToUpdate.FilmCategories?.Remove(categoryToRemove);
                    }
                }
            }

            var categoriesToAdd = newCategories.Except(currentCategories);
            foreach (var categoryId in categoriesToAdd)
            {
                var existingCategory = _categoryRepository.FirstOrDefaultAsync(c => c.Id == categoryId).Result;

                if (existingCategory is not null)
                    filmToUpdate.FilmCategories.Add(new FilmCategory { FilmId = filmToUpdate.Id, CategoryId = categoryId });
                else
                    throw new NotFoundException(nameof(Category), categoryId);
            }
        }
    }
}

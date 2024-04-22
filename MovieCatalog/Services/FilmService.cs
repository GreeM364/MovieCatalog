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
        public FilmService(IRepository<Film> filmRepository,  IMapper mapper, IRepository<Category> categoryRepository, IRepository<FilmCategory> filmCategoryRepository)
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

            // Обновляем информацию о фильме, кроме категорий
            _mapper.Map(filmRequest, filmToUpdate);

            // Получаем текущие категории фильма
            var currentCategories = filmToUpdate.FilmCategories.Select(fc => fc.CategoryId).ToList();

            // Из списка категорий, которые были у фильма ранее, удаляем те, которых нет в новом списке
            var categoriesToRemove = currentCategories.Except(filmRequest.Categories);

            foreach (var category1Id in categoriesToRemove)
            {
                var categoryToRemove = filmToUpdate.FilmCategories.FirstOrDefault(fc => fc.CategoryId == category1Id);
                if (categoryToRemove != null)
                {
                    filmToUpdate.FilmCategories.Remove(categoryToRemove);
                }
            }

            // Из нового списка категорий добавляем те, которых ещё нет у фильма
            var categoriesToAdd = filmRequest.Categories.Except(currentCategories);
            foreach (var categoryId in categoriesToAdd)
            {
                if (categoryId != null) // Проверяем, что categoryId не равен null
                {
                    var existing = await _categoryRepository.FirstOrDefaultAsync(c => c.Id == categoryId,
                        isTracking: false);
                    // Проверяем, что такой идентификатор категории существует
                    if (existing is not null)
                    {
                        filmToUpdate.FilmCategories.Add(new FilmCategory { FilmId = filmRequest.Id, CategoryId = categoryId });
                    }
                    else
                    {
                        // Если категория с таким идентификатором не существует, можно обработать эту ситуацию, например, выбросить исключение
                        throw new NotFoundException(nameof(Category), categoryId);
                    }
                }
            }

            await _filmRepository.UpdateAsync(filmToUpdate);
            // Возвращаем обновлённый объект фильма в виде FilmResponse
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
    }
}

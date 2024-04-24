using MovieCatalog.Models.Film;

namespace MovieCatalog.Services.Interfaces
{
    public interface IFilmService
    {
        Task<List<FilmResponse>> GetAllFilmsAsync();
        Task<FilmResponse> GetFilmByIdAsync(int id);
        Task<FilmResponse> CreateFilmAsync(CreateFilmRequest filmRequest);
        Task<FilmResponse> UpdateFilmAsync(UpdateFilmRequest filmRequest);
        Task DeleteFilmAsync(int id);
        Task UpdateFilmCategories(UpdateFilmCategoriesRequest updateFilmCategoriesRequest);
    }
}

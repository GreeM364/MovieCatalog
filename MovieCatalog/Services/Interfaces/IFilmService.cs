using MovieCatalog.Models.Category;
using MovieCatalog.Models.Film;

namespace MovieCatalog.Services.Interfaces
{
    public interface IFilmService
    {
        Task<List<FilmResponse>> GetAllFilmsAsync();
        Task<List<CategoryResponse>> GetAlCategoriesAsync();
        Task<FilmResponse> GetFilmByIdAsync(int id);
        Task<FilmResponse> CreateFilmAsync(CreateFilmRequest filmRequest);
        Task<FilmResponse> UpdateFilmAsync(UpdateFilmRequest filmRequest);
        Task DeleteFilmAsync(int id);
    }
}

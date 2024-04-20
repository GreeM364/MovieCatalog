using AutoMapper;
using MovieCatalog.Entities;
using MovieCatalog.Exceptions;
using MovieCatalog.Models.Film;
using MovieCatalog.Repository.IRepository;
using MovieCatalog.Services.Interfaces;

namespace MovieCatalog.Services
{
    public class FilmService : IFilmService
    {
        private readonly IRepository<Film> _filmRepository;
        private readonly IMapper _mapper;
        public FilmService(IRepository<Film> filmRepository, IMapper mapper)
        {
            _filmRepository = filmRepository;
            _mapper = mapper;
        }

        public async Task<List<FilmResponse>> GetAllFilmsAsync()
        {
            var films = await _filmRepository.GetAllAsync();

            if (films.Count == 0)
                return Enumerable.Empty<FilmResponse>().ToList();

            var result = _mapper.Map<List<FilmResponse>>(films);

            return result;
        }

        public async Task<FilmResponse> GetFilmByIdAsync(int id)
        {
            var film = await _filmRepository.GetByIdAsync(id);

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
            
            var result = _mapper.Map<FilmResponse>(film);
            return result;
        }

        public async Task<FilmResponse> UpdateFilmAsync(UpdateFilmRequest filmRequest)
        {
            var filmToUpdate = await _filmRepository.GetByIdAsync(filmRequest.Id);

            if (filmToUpdate is null)
                throw new NotFoundException(nameof(Film), filmRequest.Id);

            var updatedFilm = _mapper.Map(filmRequest, filmToUpdate);
            var film = await _filmRepository.UpdateAsync(updatedFilm);
            
            var result = _mapper.Map<FilmResponse>(film);
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

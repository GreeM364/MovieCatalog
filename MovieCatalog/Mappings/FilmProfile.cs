using AutoMapper;
using MovieCatalog.Entities;
using MovieCatalog.Models.Film;

namespace MovieCatalog.Mappings
{
    public class FilmProfile : Profile
    {
        public FilmProfile()
        {
            CreateMap<CreateFilmRequest, Film>().ReverseMap();
            CreateMap<UpdateFilmRequest, Film>().ReverseMap();
            CreateMap<FilmResponse, Film>().ReverseMap();
        }
    }
}

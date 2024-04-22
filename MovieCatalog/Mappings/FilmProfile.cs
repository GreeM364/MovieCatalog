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
            CreateMap<Film, FilmResponse>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.FilmCategories.Select(fc => fc.Category)))
                .ReverseMap();

            
        }
    }
}

using AutoMapper;
using MovieCatalog.Entities;
using MovieCatalog.Models.Category;

namespace MovieCatalog.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CreateCategoryRequest, Category>().ReverseMap();
            CreateMap<UpdateCategoryRequest, Category>().ReverseMap();
            CreateMap<Category, CategoryResponse>()
                .ForMember(dest => dest.Films, opt => opt.MapFrom(src => src.FilmCategories.Select(fc => fc.Film)))
                .PreserveReferences()
                .ReverseMap();
        }
    }
}

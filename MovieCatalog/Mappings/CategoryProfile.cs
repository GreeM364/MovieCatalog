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
            CreateMap<CategoryResponse, Category>().ReverseMap();
        }
    }
}

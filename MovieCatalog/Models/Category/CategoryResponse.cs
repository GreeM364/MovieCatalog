using MovieCatalog.Models.Film;
using Newtonsoft.Json;

namespace MovieCatalog.Models.Category
{
    public class CategoryResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public int? LevelOfNesting { get; set; }
        public CategoryResponse? ParentCategory { get; set; }

        [JsonIgnore]
        public List<CategoryResponse>? ChildCategories { get; set; }

        [JsonIgnore]
        public List<FilmResponse>? Films { get; set; }
    }
}

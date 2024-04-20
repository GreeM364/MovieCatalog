using MovieCatalog.Models.Film;

namespace MovieCatalog.Models.Category
{
    public class CategoryResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public int? ParentCategoryId { get; set; }

        public List<FilmResponse>? Films { get; set; }
    }
}

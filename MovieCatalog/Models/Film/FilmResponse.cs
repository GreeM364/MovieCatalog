using MovieCatalog.Models.Category;

namespace MovieCatalog.Models.Film
{
    public class FilmResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Director { get; set; }
        public required DateTime Release { get; set; }

        public List<CategoryResponse>? Categories { get; set; }
    }
}

namespace MovieCatalog.Models.Film
{
    public class UpdateFilmCategoriesRequest
    {
        public int FilmId { get; set; }
        public List<int> NewCategories { get; set; } = null!;
    }
}

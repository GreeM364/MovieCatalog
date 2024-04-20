namespace MovieCatalog.Entities
{
    public class FilmCategory : Entity
    {
        public required int FilmId { get; set; }
        public Film? Film { get; set; }

        public required int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}

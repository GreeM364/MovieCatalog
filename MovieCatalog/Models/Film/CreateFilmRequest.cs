namespace MovieCatalog.Models.Film
{
    public class CreateFilmRequest
    {
        public required string Name { get; set; }
        public required string Director { get; set; }
        public required DateTime Release { get; set; }

        public List<int>? Categories { get; set; }
    }
}

namespace MovieCatalog.Models.Film
{
    public class CreateFilmRequest
    {
        public string Name { get; set; } = null!;
        public string Director { get; set; } = null!;
        public DateTime Release { get; set; }

        public List<int>? Categories { get; set; }
    }
}

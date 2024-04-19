namespace MovieCatalog.Models.Film
{
    public class UpdateFilmRequest
    {
        public required int Id { get; set; }
        public string? Name { get; set; }
        public string? Director { get; set; }
        public DateTime? Release { get; set; }
    }
}

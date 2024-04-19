namespace MovieCatalog.Entities
{
    public class Film : Entity
    {
        public required string Name { get; set; }
        public required string Director { get; set; }
        public required DateTime Release { get; set; }
    }
}

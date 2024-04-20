namespace MovieCatalog.Entities
{
    public class Category : Entity
    {
        public required string Name { get; set; }

        public int? ParentCategoryId { get; set; }
        public Category? ParentCategory { get; set; }

        public List<Category>? ChildCategories { get; set; }

        public List<FilmCategory>? FilmCategories { get; set; }
    }
}

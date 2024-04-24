namespace MovieCatalog.Models.Category
{
    public class CreateCategoryRequest
    {
        public string Name { get; set; } = null!;
        public int? ParentCategoryId { get; set; }
    }
}

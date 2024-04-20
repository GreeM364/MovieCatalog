namespace MovieCatalog.Models.Category
{
    public class CreateCategoryRequest
    {
        public required string Name { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}

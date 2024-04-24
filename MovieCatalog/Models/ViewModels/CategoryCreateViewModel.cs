using Microsoft.AspNetCore.Mvc.Rendering;
using MovieCatalog.Models.Category;

namespace MovieCatalog.Models.ViewModels
{
    public class CategoryCreateViewModel
    {
        public CreateCategoryRequest CreateCategoryRequest { get; set; } = null!;
        public IEnumerable<SelectListItem> CategorySelectList { get; set; } = null!;
    }
}

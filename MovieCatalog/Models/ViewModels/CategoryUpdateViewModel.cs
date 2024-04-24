using Microsoft.AspNetCore.Mvc.Rendering;
using MovieCatalog.Models.Category;

namespace MovieCatalog.Models.ViewModels
{
    public class CategoryUpdateViewModel
    {
        public UpdateCategoryRequest UpdateCategoryRequest{ get; set; } = null!;
        public IEnumerable<SelectListItem> CategorySelectList { get; set; } = null!;
    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using MovieCatalog.Models.Film;

namespace MovieCatalog.Models.ViewModels
{
    public class FilmCreateViewModel
    {
        public CreateFilmRequest CreateFilmRequest { get; set; } = null!;
        public IEnumerable<SelectListItem> CategorySelectList { get; set; } = null!;
    } 
}

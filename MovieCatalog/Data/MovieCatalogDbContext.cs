using Microsoft.EntityFrameworkCore;

namespace MovieCatalog.Data
{
    public class MovieCatalogDbContext : DbContext
    {
        public MovieCatalogDbContext(DbContextOptions<MovieCatalogDbContext> options) : base(options)
        { }
    }
}

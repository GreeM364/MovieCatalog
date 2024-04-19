using Microsoft.EntityFrameworkCore;
using MovieCatalog.Entities;

namespace MovieCatalog.Data
{
    public class MovieCatalogDbContext : DbContext
    {
        public MovieCatalogDbContext(DbContextOptions<MovieCatalogDbContext> options) : base(options)
        { }

        DbSet<Film> Films { get; set; }
    }
}

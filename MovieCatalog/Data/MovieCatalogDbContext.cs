using Microsoft.EntityFrameworkCore;
using MovieCatalog.Entities;

namespace MovieCatalog.Data
{
    public class MovieCatalogDbContext : DbContext
    {
        public MovieCatalogDbContext(DbContextOptions<MovieCatalogDbContext> options) : base(options)
        { }

        DbSet<Film> Films { get; set; }
        DbSet<Category> Categories { get; set; }    
        DbSet<FilmCategory> FilmCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        }
    }
}

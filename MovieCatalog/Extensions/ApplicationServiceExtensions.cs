using Microsoft.EntityFrameworkCore;
using MovieCatalog.Data;
using MovieCatalog.Repository.IRepository;
using MovieCatalog.Repository;
using MovieCatalog.Services;
using MovieCatalog.Services.Interfaces;
using System.Reflection;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace MovieCatalog.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<MovieCatalogDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<IFilmService, FilmService>();
            services.AddScoped<ICategoryService, CategoryService>();

            return services;
        }
    }
}

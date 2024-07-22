using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using TodoApp.Repository.Models;

namespace TodoApp.Repository
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../TodoApp.API"))
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<AppDbContext>();
            var connectionString = configuration.GetConnectionString("SqlConnection");

            builder.UseSqlServer(connectionString);

            return new AppDbContext(builder.Options);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CDN.Directory.Infrastructure.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        //public AppDbContext CreateDbContext(string[] args)
        //{
        //    IConfigurationRoot configuration = new ConfigurationBuilder()
        //        .SetBasePath(System.IO.Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json")
        //        .Build();

        //    var builder = new DbContextOptionsBuilder<AppDbContext>();

        //    var connectionString = configuration.GetConnectionString("DefaultConnection");

        //    builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        //    return new AppDbContext(builder.Options);
        //}

        public AppDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AppDbContext>();

            // Hardcode for design-time
            var connectionString = "server=localhost;port=3306;database=cdn_directory_db;user=root;password=royalvictoria03!";

            builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new AppDbContext(builder.Options);
        }
    }
}

using ApiTask.Domain.Entities;
using ApiTask.Domain.Entities.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ApiTask.Infrastructure.Persistence.Contexts
{
    public class MSSQLContext : DbContext
    {
        private static readonly IConfiguration _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
#if DEBUG
            .AddJsonFile("appsettings.development.json", optional: false, reloadOnChange: true)
#else
            .AddJsonFile("appsettings.production.json", optional: false, reloadOnChange: true)
#endif
            .Build();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string? ConnectionString = _configuration.GetConnectionString("MSSQLServer");
            optionsBuilder.UseSqlServer(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasQueryFilter(e => e.DeletedAt == null);
            modelBuilder.Entity<Contact>().HasQueryFilter(e => e.DeletedAt == null);
            modelBuilder.Entity<Company>().HasQueryFilter(e => e.DeletedAt == null);
        }


        #region DbSets

        // Location
        public DbSet<Country> Countries { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }

        // User
        public DbSet<User> Users { get; set; }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        #endregion
    }
}

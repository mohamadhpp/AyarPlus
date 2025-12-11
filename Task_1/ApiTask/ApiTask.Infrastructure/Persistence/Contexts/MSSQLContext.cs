using ApiTask.Domain.Entities;
using ApiTask.Domain.Entities.Location;
using Microsoft.EntityFrameworkCore;

namespace ApiTask.Infrastructure.Persistence.Contexts
{
    public class MSSQLContext : DbContext
    {
        public MSSQLContext(DbContextOptions<MSSQLContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasQueryFilter(e => e.DeletedAt == null);
            modelBuilder.Entity<Contract>().HasQueryFilter(e => e.DeletedAt == null);
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
        public DbSet<Contract> Contracts { get; set; }

        #endregion
    }
}

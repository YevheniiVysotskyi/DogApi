using Dogs.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Dogs.Infrastructure.Context
{
    public class DogContext : DbContext
    {
        public DbSet<Dog> Dogs { get; set; }

        public DogContext(DbContextOptions<DogContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

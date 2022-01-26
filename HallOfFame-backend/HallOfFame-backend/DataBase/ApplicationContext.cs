using Microsoft.EntityFrameworkCore;
using HallOfFame_backend.DataBase.Models;

namespace HallOfFame_backend.DataBase
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Skill> Skills { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasMany(s => s.Skills)
                    .WithOne(p => p.Person);
            });
            modelBuilder.Entity<Skill>(entity =>
            {
                entity.Property<long>("PersonId");

                entity.HasOne(p => p.Person)
                    .WithMany(s => s.Skills)
                    .HasForeignKey(fk => fk.PersonId);
            });
        }
    }
}

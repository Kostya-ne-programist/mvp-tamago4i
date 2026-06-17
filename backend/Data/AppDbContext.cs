using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Pet> Pets => Set<Pet>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pet>().HasData(
            new Pet { Id = 1, Name = "Пухнастик", Species = "Кіт", Hunger = 50, Happiness = 80, Health = 100, CreatedAt = new DateTime(2026, 1, 1) },
            new Pet { Id = 2, Name = "Бульбазавр", Species = "Ящірка", Hunger = 30, Happiness = 90, Health = 95, CreatedAt = new DateTime(2026, 1, 1) },
            new Pet { Id = 3, Name = "Зірочка", Species = "Собака", Hunger = 70, Happiness = 60, Health = 85, CreatedAt = new DateTime(2026, 1, 1) }
        );
    }
}
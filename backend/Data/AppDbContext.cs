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
    public DbSet<User> Users => Set<User>();
    public DbSet<ShopItem> ShopItems => Set<ShopItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pet>().HasData(
            new Pet { Id = 1, Name = "Пухнастик", Species = "Кіт", Hunger = 50, Happiness = 80, Health = 100, CreatedAt = new DateTime(2026, 1, 1) },
            new Pet { Id = 2, Name = "Бульбазавр", Species = "Ящірка", Hunger = 30, Happiness = 90, Health = 95, CreatedAt = new DateTime(2026, 1, 1) },
            new Pet { Id = 3, Name = "Зірочка", Species = "Собака", Hunger = 70, Happiness = 60, Health = 85, CreatedAt = new DateTime(2026, 1, 1) }
        );

        modelBuilder.Entity<ShopItem>().HasData(
            new ShopItem { Id = 1, Name = "Риба", Effect = "Їжа", Price = 10, HungerEffect = -30, HappinessEffect = 5, HealthEffect = 0 },
            new ShopItem { Id = 2, Name = "М'ясо", Effect = "Їжа", Price = 20, HungerEffect = -50, HappinessEffect = 10, HealthEffect = 0 },
            new ShopItem { Id = 3, Name = "Цукерка", Effect = "Їжа", Price = 5, HungerEffect = -10, HappinessEffect = 20, HealthEffect = -5 }
        );
    }
}
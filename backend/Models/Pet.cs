namespace backend.Models;

public class Pet
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Species { get; set; } = string.Empty;
    public int Hunger { get; set; }
    public int Happiness { get; set; }
    public int Health { get; set; }
    public int Level { get; set; } = 1;
    public int Experience { get; set; } = 0;
    public int Coins { get; set; } = 100;
    public bool IsAlive { get; set; } = true;
    public DateTime? LastDailyBonus { get; set; } = null;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
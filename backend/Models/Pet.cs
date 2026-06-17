namespace backend.Models;

public class Pet
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Species { get; set; } = string.Empty;
    public int Hunger { get; set; }
    public int Happiness { get; set; }
    public int Health { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
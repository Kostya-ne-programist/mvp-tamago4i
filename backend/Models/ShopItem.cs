namespace backend.Models;

public class ShopItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Effect { get; set; } = string.Empty;
    public int Price { get; set; }
    public int HungerEffect { get; set; }
    public int HappinessEffect { get; set; }
    public int HealthEffect { get; set; }
}
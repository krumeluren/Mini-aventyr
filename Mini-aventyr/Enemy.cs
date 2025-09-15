
namespace Mini_aventyr;

public class Enemy {
    public string Name { get; private set; }
    public readonly Health Health;
    public readonly Loot Loot;
    public Enemy (Health health, string name, Loot loot) {
        Health = health;     
        Name = name;
        Loot = loot;
    }

    public Enemy Clone () {
        return new Enemy(
            new Health(Health.MaxHP, Health.HP), 
            Name, 
            new Loot(Loot.Gold, 
            new Weapon(Loot.Weapon.Name, Loot.Weapon.Damage)));
    }
}
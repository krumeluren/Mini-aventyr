using Mini_aventyr.EntityComponents;

namespace Mini_aventyr.Entities;

public class Enemy : Entity {
    public bool HasPrepared { get; set; } = false; // if the enemy will get a initial turn to attack
    public Enemy (Health health, string name, Loot loot) : base(health, loot, name) {
    }
}


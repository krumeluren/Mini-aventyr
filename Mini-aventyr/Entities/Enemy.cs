using Mini_aventyr.EntityComponents;
using Mini_aventyr.Interfaces;

namespace Mini_aventyr.Entities;

public class Enemy : Entity {
    public bool HasPrepared { get; set; } = false; // if the enemy will get a initial turn to attack
    public Enemy (IHealth health, string name, Loot loot) : base(health, loot, name) {
    }
}


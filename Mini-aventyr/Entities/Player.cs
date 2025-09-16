using Mini_aventyr.EntityComponents;

namespace Mini_aventyr.Entities;
public class Player : Entity {
    public string Class { get; private set; }
    public Player (Health health, string _class, string name, Loot loot) : base(health, loot, name) {
        Class = _class;
    }
}

using Mini_aventyr.EntityComponents;
using Mini_aventyr.Interfaces;

namespace Mini_aventyr.Entities;
public class Player : Entity {
    public string Class { get; private set; }
    public Player (IHealth health, string _class, string name, Loot loot) : base(health, loot, name) {
        Class = _class;
    }
}

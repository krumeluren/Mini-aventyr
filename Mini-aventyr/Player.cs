namespace Mini_aventyr;
public class Player : Entity {
    public string Name { get; private set; }
    public string Class { get; private set; }

    public Player (Health health, string _class, string name, Loot loot) : base(health, loot) {
        Class = _class;
        Name = name;
    }
}

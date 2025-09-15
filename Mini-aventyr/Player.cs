



namespace Mini_aventyr;
public class Player {
    public string Name { get; private set; }
    public string Class { get; private set; }

    public readonly Loot Loot;

    public readonly Health Health;
    public Player (Health health, string _class, string name, Loot loot) {
        Health = health;
        Class = _class;
        Name = name;
        Loot = loot;
    }
}

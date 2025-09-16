namespace Mini_aventyr;
public abstract class Entity {
    public readonly Health Health;
    public readonly Loot Loot;

    protected Entity (Health health,Loot loot) {
        Health = health;
        Loot = loot;
    }
}
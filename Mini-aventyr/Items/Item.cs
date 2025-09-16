namespace Mini_aventyr.Items;
public abstract class Item {
    public abstract float StatMultiplier (float statMultiplier, Weapon weapon);
    public abstract string Name { get; }
}
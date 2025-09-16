namespace Mini_aventyr;
public abstract class Item {
    public abstract string Details ();
    public abstract float StatMultiplier (float statMultiplier, Weapon weapon);
    public abstract string Name { get; }
}
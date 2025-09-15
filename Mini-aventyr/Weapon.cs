namespace Mini_aventyr;
public class Weapon {
    public string Name { get;  }
    public float Damage { get; }

    public Weapon (string name, float damage) {
        Name = name;
        Damage = damage;
    }
}
namespace Mini_aventyr.Items;
public class Weapon : Item {
    public override string Name { get; }
    public float BaseDamage { get; }

    public StatType ScalingType { get; }

    public Weapon (string name, float damage, StatType scalingType = StatType.None) {
        Name = name;
        BaseDamage = damage;
        ScalingType = scalingType;
    }

    public override string ToString () {
        return $"Weapon {Name} (Base Damage: {BaseDamage})";
    }

    public override float StatMultiplier (float statMultiplier, Weapon weapon) {
        return 1;
    }

    public enum StatType { None, Dexterity, Strength, Perception, Chakra };
}
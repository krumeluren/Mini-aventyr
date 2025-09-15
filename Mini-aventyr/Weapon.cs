namespace Mini_aventyr;
public class Weapon : Item {
    public override string Name { get; }
    public float BaseDamage { get; }

    public StatType ScalingType { get; }

    public Weapon (string name, float damage, StatType scalingType = StatType.None) {
        Name = name;
        BaseDamage = damage;
        ScalingType = scalingType;
    }

    public override string Details () {
        return $"Weapon {Name} (Base Damage: {BaseDamage})";
    }

    public enum StatType { None, Dexterity, Strength, Perception, Chakra };
}

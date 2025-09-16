namespace Mini_aventyr.Items;
public class Trinket : Item {
    public override string Name { get; }
    public Weapon.StatType StatType { get; }
    public Trinket (string name, Weapon.StatType statType = Weapon.StatType.None) {
        Name = name;
        StatType = statType;
    }

    public override string ToString () {
        return $"{Name}";
    }

    public override float StatMultiplier (float statMultiplier, Weapon weapon) {
        if (StatType == weapon.ScalingType) {
            return statMultiplier *= 1.05f;
        }
        return 1;
    }
}
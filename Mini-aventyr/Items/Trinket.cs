using Mini_aventyr.Interfaces;

namespace Mini_aventyr.Items;
public class Trinket : IItem, IStatMultiplier {
    public string Name { get; }
    public IWeapon.StatType StatType { get; }
    public Trinket (string name, IWeapon.StatType statType = IWeapon.StatType.None) {
        Name = name;
        StatType = statType;
    }

    public override string ToString () {
        return $"{Name}";
    }

    float IStatMultiplier.StatMultiplier (float statMultiplier, IWeapon weapon) {
        if (StatType == weapon.ScalingType) {
            return statMultiplier *= 1.05f;
        }
        return 1;
    }
}
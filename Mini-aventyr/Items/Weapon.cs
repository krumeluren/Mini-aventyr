using Mini_aventyr.Interfaces;
using static Mini_aventyr.Interfaces.IWeapon;

namespace Mini_aventyr.Items;
public class Weapon : IItem, IWeapon {
    public string Name { get; }
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
}
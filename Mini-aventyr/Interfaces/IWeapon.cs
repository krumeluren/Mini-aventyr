namespace Mini_aventyr.Interfaces;

public interface IWeapon : IItem {
    float BaseDamage { get; }
    StatType ScalingType { get; }
    public enum StatType { None, Dexterity, Strength, Perception, Chakra };

}
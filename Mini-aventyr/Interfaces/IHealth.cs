namespace Mini_aventyr.Interfaces;



public interface IHealth {
    float Chakra { get; }
    float Dexterity { get; }
    float Energy { get; }
    float Fullness { get; }
    float HP { get; }
    float MaxEnergy { get; }
    bool IsBloated { get; }
    bool IsDead { get; }
    float Luck { get; }
    float MaxFullness { get; }
    float MaxHP { get; }
    float Perception { get; }
    float Power { get; }
    float Strength { get; }

    bool Consume (IEdible edible);
    float Damage (float damage);
    void DirectHeal (float amount);
    void Energize (float energy);
    void Exhaust (float baseEnergyLoss);
    void PassiveChanges ();
    void Rest ();
}
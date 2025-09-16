


namespace Mini_aventyr;
public class Health {
    public float MaxHP { get; private set; }
    public float HP { get; private set; }

    public readonly float MaxEnergy;
    public float Energy { get; private set; }
    public float Strength { get; }
    public float Perception { get; }
    public float Dexterity { get; }
    public float Chakra { get; }
    public float Power => Energy / MaxEnergy < 0.3f ? 0.3f : Energy / MaxEnergy; // scale down power to as little as 30%

    public bool IsDead => HP <= 0 || Energy <= 0;
    public Health (float maxHp, float hp, float maxEnergy = 100, float energy = 100, float strength = 1, float perception = 1, float dexterity = 1, float chakra = 1) {
        MaxHP = maxHp;
        HP = hp;
        Energy = energy;
        MaxEnergy = maxEnergy;
        Strength = strength;
        Perception = perception;
        Dexterity = dexterity;
        Chakra = chakra;
    }
    public void Rest () {
        // Restore energy by adding a fraction of the missing amount
        // This makes it approach max but never reach it
        float missingEnergy = MaxEnergy - Energy;
        Energize(missingEnergy * 0.25f * Chakra);// Restore 25% of the missing energy
        Heal(10); // Heal 10
    }
    /// <returns>Damage dealt</returns>
    public float Damage (float damage) {
        float dmg = 0;
        if (Strength > 0) dmg = damage / Strength;
        else dmg = damage; // Avoid division by zero

        HP -= dmg;
        if (HP < 0) HP = 0; // prevent negative health
        return dmg;
    }
    public void Exhaust (float baseEnergyLoss) {
        if (Dexterity > 0) Energy -= baseEnergyLoss / Dexterity;
        else Energy -= baseEnergyLoss;


        if (Energy <= 0) Energy = 0;
    }
    public void Heal (float baseHeal) {
        float restEffectiveness = (Energy / MaxEnergy);
        HP += restEffectiveness * baseHeal * Chakra;
        if (HP >= MaxHP) HP = MaxHP;
    }

    public void Eat (Food food) {
        Energize(food.Energy * Chakra);
    }

    public void Energize (float energy) {
        Energy += energy;
        if (Energy >= MaxEnergy) Energy = MaxEnergy;

    }
}
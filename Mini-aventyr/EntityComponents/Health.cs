using Mini_aventyr.Items;

namespace Mini_aventyr.EntityComponents;
public class Health {
    public float MaxHP { get; private set; }
    public float HP { get; private set; }

    public readonly float MaxEnergy;
    public float Energy { get; private set; }
    public float Strength { get; }
    public float Perception { get; }
    public float Dexterity { get; }
    public float Chakra { get; }
    public float Luck { get; }

    public float MaxFullness { get; } = 15;
    public float Fullness { get; private set; } = 5;

    private float _energyBudgetForHealing { get; } = 5.0f;
    private float _hpPerEnergy { get; } = 0.5f;

    public bool IsDead => HP <= 0 || Energy <= 0;
    public float Power => Energy / MaxEnergy < 0.3f ? 0.3f : Energy / MaxEnergy; // scale down power to as little as 30%
    public bool IsBloated => Fullness >= MaxFullness;

    public Health (float maxHp, float hp, float maxEnergy, float energy,
        float strength, float perception, float dexterity, float chakra,
        float luck, float fullness, float maxFullness,
        float passiveHealing, float hpPerEnergy) {

        MaxHP = maxHp;
        HP = hp;
        Energy = energy;
        MaxEnergy = maxEnergy;
        Strength = strength;
        Perception = perception;
        Dexterity = dexterity;
        Chakra = chakra;
        Luck = luck;
    }

    /// <summary>
    /// Occur each loop
    /// </summary>
    public void PassiveChanges () {
        Fullness -= 1;
        Fullness = Math.Max(0, Fullness);
        if (Fullness > 0) { // convert 1 fullness to energy
            Energy += 1;
            Energy = Math.Min(Energy, MaxEnergy);
        }

        if (HP >= MaxHP || Energy <= 0) {
            return;
        }

        float efficiency = Energy / MaxEnergy;
        float energyToSpend = _energyBudgetForHealing * Chakra * efficiency;
        energyToSpend = Math.Min(energyToSpend, Energy);

        float potentialHealthGained = energyToSpend * _hpPerEnergy;

        float missingHealth = MaxHP - HP;
        float actualHealthGained = Math.Min(potentialHealthGained, missingHealth);

        float actualEnergySpent;
        if (potentialHealthGained > 0) actualEnergySpent = actualHealthGained / _hpPerEnergy;
        else actualEnergySpent = 0;

        actualEnergySpent = Math.Min(actualEnergySpent, Energy);

        HP += actualHealthGained;
        Energy -= actualEnergySpent;
    }


    public void Rest () {
        // Restore energy by adding a fraction of the missing amount
        // This makes it approach max but never reach it
        float missingEnergy = MaxEnergy - Energy;
        Energize(missingEnergy * 0.25f * Chakra);// Restore 25% of the missing energy
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

    public bool Consume (Food food) {
        // cant eat if you are full
        if (Fullness >= MaxFullness) {
            return false;
        }

        Fullness += food.Satiety;
        Energize(food.Energy);
        DirectHeal(food.Healing);

        return true;
    }

    public void DirectHeal (float amount) {
        HP += amount;
        HP = Math.Min(HP, MaxHP);
    }

    public void Energize (float energy) {
        Energy += energy;
        if (Energy >= MaxEnergy) Energy = MaxEnergy;
    }
}
using Mini_aventyr.Entities;
using Mini_aventyr.Interfaces;

namespace Mini_aventyr;

public static class GameHelper {

    /// <summary>
    /// Calculate how much damage the entity should inflict on the target.
    /// </summary>
    public static float CalculateDamage (Entity source, Entity target) {
        float weaponModifier = 1.0f;

        // Step 1:
        // scales weapon with related entity stat
        switch (source.Loot.Weapon.ScalingType) {
            case IWeapon.StatType.Strength: weaponModifier = source.Health.Strength; break;
            case IWeapon.StatType.Dexterity: weaponModifier = source.Health.Dexterity; break;
            case IWeapon.StatType.Perception: weaponModifier = source.Health.Perception; break;
            case IWeapon.StatType.Chakra: weaponModifier = source.Health.Chakra; break;
            default: break;
        }


        // Step 2:
        // each item in inventory has a chance to return a stat modifier
        float itemsModifier = 1f;
        foreach (var item in source.Loot.Items) {
            if (item is IStatMultiplier statMultiplier) {
                itemsModifier *= statMultiplier.StatMultiplier(itemsModifier, source.Loot.Weapon);
            }
        }


        // step 3: 
        // The enemy will have more resiliance by weapon type. Ie a STR weapon is less effective on a high STR target
        float enemyResilianceModifier = 1.0f;
        switch (source.Loot.Weapon.ScalingType) {
            case IWeapon.StatType.Strength: enemyResilianceModifier = target.Health.Strength; break;
            case IWeapon.StatType.Dexterity: enemyResilianceModifier = target.Health.Dexterity; break;
            case IWeapon.StatType.Perception: enemyResilianceModifier = target.Health.Perception; break;
            case IWeapon.StatType.Chakra: enemyResilianceModifier = target.Health.Chakra; break;
            default: break;
        }
        // invert the modifier to provide resistance. ie 1 / 1.5 = 0.66..
        if (enemyResilianceModifier > 0) {
            enemyResilianceModifier = 1.0f / enemyResilianceModifier;
        }
        else {
            // Avoid division by zero and apply a large bonus if the target stat is 0 for some reaason..
            enemyResilianceModifier = 2.0f;
        }


        // Step 4:
        // Stats that differ between the entities affect damage.
        // I.e high/low STR against a low/high STR deals more/less damage
        float similarityModifier = 1f;
        float strMod = source.Health.Strength / target.Health.Strength;
        float dexMod = source.Health.Dexterity / target.Health.Dexterity;
        float prcMod = source.Health.Perception / target.Health.Perception;
        float chkMod = source.Health.Chakra / target.Health.Chakra;
        similarityModifier = (strMod + dexMod + prcMod + chkMod) / 4; // average

        float damage = source.Loot.Weapon.BaseDamage * source.Health.Power
            * weaponModifier
            * itemsModifier
            * enemyResilianceModifier
            * similarityModifier;

        return damage;
    }
}
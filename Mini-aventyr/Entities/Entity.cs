using Mini_aventyr.EntityComponents;
using Mini_aventyr.Items;

namespace Mini_aventyr.Entities;
public abstract class Entity {
    public string Name { get; }

    public readonly Health Health;
    public readonly Loot Loot;

    /// <summary>
    /// If analyzed you will see specific stats
    /// </summary>
    public bool IsAnalyzed { get; set; }


    protected Entity (Health health, Loot loot, string name) {
        Health = health;
        Loot = loot;
        Name = name;
    }


    /// <param name="listIndex">The 0 based index</param>
    public void Display (int listIndex) {
        Console.Write($"[{listIndex + 1}] {Name} wielding a {GetWeaponInfo()}");
        if (IsAnalyzed) {
            Console.WriteLine($"\n    └─ {GetHealthStatusDescription()}");
        }
        else {
            Console.WriteLine($", which looks {GetHealthStatusDescription()}");
        }
    }

    private string GetWeaponInfo () {
        Weapon w = Loot.Weapon;
        if (IsAnalyzed) {
            return $"{w.Name} (Base Dmg: {w.BaseDamage}, Scaling: {w.ScalingType})";
        }
        return $"{w.Name}";
    }

    private string GetHealthStatusDescription () {
        string healthStatus;
        float healthPercent = Health.HP / Health.MaxHP;

        if (IsAnalyzed)
            healthStatus = healthPercent switch {
                > 0.95f => "Healthy",
                > 0.7f => "Fine",
                > 0.5f => "Ragged",
                > 0.25f => "Wounded",
                > 0.1f => "Torn",
                _ => "Near Death",
            };
        else
            healthStatus = healthPercent switch {
                > 0.7f => "healthy",
                > 0.5f => "a bit ragged",
                > 0.25f => "wounded",
                _ => "maimed",
            };

        string primaryAttribute = "average"; // fallback
        float maxStat = Math.Max(Health.Strength, Math.Max(Health.Dexterity, Math.Max(Health.Perception, Health.Chakra)));
        if (maxStat == Health.Strength) primaryAttribute = "robustly built";
        else if (maxStat == Health.Dexterity) primaryAttribute = "remarkably agile";
        else if (maxStat == Health.Perception) primaryAttribute = "dangerously alert";
        else if (maxStat == Health.Chakra) primaryAttribute = "has a faint aura";


        string formattedAttributes = "average";
        if (IsAnalyzed) {
            var notableTraits = this.GetNotableTraits();
            if (notableTraits.Count != 0) {
                formattedAttributes = string.Join(", and ", notableTraits);
            }
            else {
                formattedAttributes = $"{primaryAttribute} but mostly average";
            }
        }
        else {
            formattedAttributes = $"{primaryAttribute}";
        }

        string energyStatus = "";
        float energyPercent = Health.Power;
        if (IsAnalyzed) {
            switch (energyPercent) {
                case < 0.1f:
                energyStatus = ", but looks to be passing out";
                break;
                case < 0.2f:
                energyStatus = ", but is panting heavily";
                break;
                case < 0.5f:
                energyStatus = ", but seems to be tiring";
                break;
            }
        }
        else {
            switch (energyPercent) {
                case < 0.2f:
                energyStatus = ", but is panting heavily";
                break;
                case < 0.5f:
                energyStatus = ", but seems to be tiring";
                break;
            }
        }
        return $"{healthStatus}, {formattedAttributes}{energyStatus}.";
    }
}

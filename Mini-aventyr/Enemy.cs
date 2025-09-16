namespace Mini_aventyr;

public class Enemy : Entity {
    public string Name { get; private set; }
    public bool HasPrepared { get; set; } = false; // if the enemy will get a initial turn to attack

    /// <summary>
    /// If analyzed you will see specific stats
    /// </summary>
    public bool IsAnalyzed { get; set; }

    public Enemy (Health health, string name, Loot loot) : base(health, loot) {
        Name = name;
    }

    /// <param name="listIndex">The 0 based index</param>
    public void Display (int listIndex) {
        Console.Write($"[{listIndex + 1}] {Name}");
        if (IsAnalyzed) {
            var h = Health;
            var w = Loot.Weapon;
            Console.WriteLine($"  - Vitals:   HP: {(int)h.HP}/{(int)h.MaxHP} | Energy: {(int)h.Energy}/{(int)h.MaxEnergy}");
            Console.WriteLine($"  - Stats:    STR: {h.Strength:F1} DEX: {h.Dexterity:F1} PRC: {h.Perception:F1} CHK: {h.Chakra:F1}");
            Console.WriteLine($"  - Wielding: {w.Name} (Base Dmg: {w.BaseDamage}, Scales with: {w.ScalingType})");
            Console.WriteLine($"");
        }
        else {
            Console.Write($" wielding a {Loot.Weapon.Name}");
            Console.Write($" - Looks {GetHealthStatusDescription()}");
            Console.WriteLine($"");
        }
    }

    private string GetHealthStatusDescription () {
        string healthStatus;
        float healthPercent = Health.HP / Health.MaxHP;
        if (healthPercent > 0.7f) healthStatus = "Healthy";
        else if (healthPercent > 0.3f) healthStatus = "Wounded";
        else healthStatus = "Near Death";

        string attributeDescription = "average";
        float maxStat = Math.Max(Health.Strength, Math.Max(Health.Dexterity, Math.Max(Health.Perception, Health.Chakra)));
        if (maxStat == Health.Strength) attributeDescription = "robustly built";
        else if (maxStat == Health.Dexterity) attributeDescription = "remarkably agile";
        else if (maxStat == Health.Perception) attributeDescription = "dangerously alert";
        else if (maxStat == Health.Chakra) attributeDescription = "crackling with energy";

        if(Health.Strength >= 2) {

        }
        else if (Health.Strength <= 0.5) {

        }

        if (Health.Dexterity >= 2) {

        } else if (Health.Dexterity <= 0.5) {

        }

        if (Health.Perception >= 2) {

        } else if (Health.Perception <= 0.5) {

        }

        if (Health.Chakra >= 2) {

        } else if (Health.Chakra <= 0.5) {

        }


        string energyStatus = "";
        float energyPercent = Health.Power;
        if (energyPercent < 0.3f) {
            energyStatus = ", but seems to be tiring";
        }
        else if (energyPercent > 0.9f) {
            energyStatus = " and is brimming with vigor";
        }

        return $"{healthStatus} and {attributeDescription}{energyStatus}";
    }
}
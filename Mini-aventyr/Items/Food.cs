namespace Mini_aventyr.Items;
public class Food : Item {
    public override string Name { get; }
    public float Energy { get; }
    public float Satiety { get; }
    public float Healing { get; }

    public Food (string name, float energy, float satiety, float healing) {
        Name = name;
        Energy = energy;
        Satiety = satiety;
        Healing = healing;
    }

    public override string ToString () {
        var effects = new List<string>();
        if (Healing > 0) effects.Add($"HP: +{Healing}");
        if (Energy > 0) effects.Add($"Energy: +{Energy}");
        if (Satiety > 0) effects.Add($"Satiety: +{Satiety}");
        return effects.Any() ? $"{Name} ({string.Join(", ", effects)})" : Name;
    }

    public override float StatMultiplier (float statMultiplier, Weapon weapon) {
        return 1;
    }
}

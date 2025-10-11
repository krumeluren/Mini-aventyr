using Mini_aventyr.Interfaces;

namespace Mini_aventyr.Items;
public class Food : IItem, IEdible {
    public string Name { get; }
    public float Energy { get; }
    public float Fullness { get; }
    public float Healing { get; }

    public Food (string name, float energy, float fullness, float healing) {
        Name = name;
        Energy = energy;
        Fullness = fullness;
        Healing = healing;
    }

    public override string ToString () {
        var effects = new List<string>();
        if (Healing > 0) effects.Add($"HP: +{Healing}");
        if (Energy > 0) effects.Add($"Energy: +{Energy}");
        if (Fullness > 0) effects.Add($"Fullness: +{Fullness}");
        return effects.Any() ? $"{Name} ({string.Join(", ", effects)})" : Name;
    }
}

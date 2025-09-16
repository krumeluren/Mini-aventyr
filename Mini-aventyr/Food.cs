


namespace Mini_aventyr;

public class Food : Item {
    public override string Name { get; }
    public readonly float Energy;
    public Food (string name, float energy) {
        Name = name;
        Energy = energy;
    }

    public override string Details () {
        return $"{Name} (+{Energy})";
    }
}

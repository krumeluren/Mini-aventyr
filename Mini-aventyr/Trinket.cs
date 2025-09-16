


namespace Mini_aventyr;

public class Trinket : Item {
    public override string Name { get; }
    public Trinket (string name) {
        Name = name;
    }

    public override string Details () {
        return $"{Name}";
    }
}
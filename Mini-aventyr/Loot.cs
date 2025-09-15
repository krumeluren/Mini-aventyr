


namespace Mini_aventyr;

public class Loot {
    public int Gold { get; set; }
    public Weapon Weapon{ get; set; }
    public Loot (int gold, Weapon weapon) {
        Weapon = weapon;
        Gold = gold;
    }
}


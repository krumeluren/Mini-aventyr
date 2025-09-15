

namespace Mini_aventyr;

public record EnemyTemplate (string Name, int MaxHp, float MaxEnergy, int MinLoot = 0, int MaxLoot = 3, int MinGold = 0, int MaxGold = 0, float Str = 1, float Dex = 1, float Prc = 1, float Chk = 1);
public record WeaponTemplate (string Name, float Damage, Weapon.StatType StatType);
public record FoodTemplate (string Name, float Energy);

// A static class to hold predefined data
public static class GameData {

    public static readonly List<EnemyTemplate> EnemyTemplates = new List<EnemyTemplate>() {
        new("Rusty Clanker",              50, 20,  0, 1, 0, 5,   Str: 1.5f, Dex: 0.2f, Prc: 1.5f, Chk: 0.2f),
        new("Sellsword",                  40, 100, 1, 2, 5, 15,  Str: 1.2f, Dex: 1.1f, Prc: 1.0f, Chk: 1.0f),
        new("Demon",                      80, 1000, 1, 1, 10, 30, Str: 1.5f, Dex: 1.0f, Prc: 0.8f, Chk: 2.0f),
        new("Ratman",                     15, 20, 1, 3, 2, 10,  Str: 0.8f, Dex: 1.5f, Prc: 1.1f, Chk: 0.9f),
        new("Training Dummy",             1,  1,  0, 0, 0, 0,   Str: 1.0f, Dex: 1.0f, Prc: 1.0f, Chk: 1.0f),
        new("Angry Peasant",              30, 120, 0, 1, 0, 5,   Str: 1.2f, Dex: 0.9f, Prc: 0.9f, Chk: 1.0f),
        new("Ogre",                       50, 40,  1, 3, 5, 20,  Str: 1.8f, Dex: 0.4f, Prc: 0.7f, Chk: 0.8f),
        new("Gnome",                      15, 100, 1, 2, 5, 15,  Str: 0.8f, Dex: 1.3f, Prc: 1.2f, Chk: 1.0f),
        new("Sentient Scarecrow",         20, 40, 1, 3, 0, 5,   Str: 0.7f, Dex: 1.4f, Prc: 1.1f, Chk: 1.2f),
        new("Intoxicated Hedge Knight",   120, 60, 1, 1, 1, 2, Str: 1.3f, Dex: 1.0f, Prc: 1.1f, Chk: 0.9f),
        new("Depressed Dragon",           140, 60, 2, 4, 20, 50, Str: 2f, Dex: 0.1f, Prc: 0.8f, Chk: 2f),
};


    public static readonly List<FoodTemplate> Food = new List<FoodTemplate>() {
        new ("Hardtack", 5),
        new ("Baguette", 10),
        new ("Dried Meat", 7),
        new ("Cured Meat", 8),
        new ("Flesh", 10),
        new ("Fruit", 4),
        new ("Berries", 2),
        new ("Ale", 2),
        new ("Wine", 4),
        new ("Nuts", 10),
    };

    public static readonly List<WeaponTemplate> Weapons = new()
    {
        // Unaligned Weapons
        new("Stick", 1, Weapon.StatType.None),
        new("Cast Iron Pan", 3, Weapon.StatType.None),
        new("Hatchet", 5, Weapon.StatType.None),
        new("Pitchfork", 7, Weapon.StatType.None),

        // Strength 
        new("Heavy Branch", 2, Weapon.StatType.Strength),
        new("Cracked Bone Club", 6, Weapon.StatType.Strength),
        new("Iron Mace", 10, Weapon.StatType.Strength),
        new("Zweihander", 14, Weapon.StatType.Strength),

        // Dexterity
        new("Sharp Stone", 2, Weapon.StatType.Dexterity),
        new("Rusty Dagger", 6, Weapon.StatType.Dexterity),
        new("Brass Knuckles", 10, Weapon.StatType.Dexterity),
        new("Rapier", 14, Weapon.StatType.Dexterity),

        // Perception
        new("Wooden Slingshot", 2, Weapon.StatType.Perception),
        new("Broken Scalpel", 6, Weapon.StatType.Perception),
        new("Whip", 10, Weapon.StatType.Perception),
        new("Poison Darts", 14, Weapon.StatType.Perception),

        // Chakra
        new("Lucky Stone", 2, Weapon.StatType.Chakra),
        new("Weird Facemask", 6, Weapon.StatType.Chakra),
        new("Talisman", 10, Weapon.StatType.Chakra),
        new("Runic Totem", 14, Weapon.StatType.Chakra),
    };
}

using Mini_aventyr.Entities;
using Mini_aventyr.EntityComponents;
using Mini_aventyr.Items;

namespace Mini_aventyr;

public class EnemyFactory {

    private Random _random = new Random();
    public EnemyFactory () {
    }

    public List<Enemy> GetEnemies (int count) {
        var generatedEnemies = new List<Enemy>();

        for (int i = 0; i < count; i++) {
            // Pick a enemy type
            var enemyBase = GameData.EnemyTemplates[_random.Next(GameData.EnemyTemplates.Count)];

            // Pick a random weapon
            var weaponData = GameData.Weapons[_random.Next(GameData.Weapons.Count)];
            var enemyWeapon = new Weapon(weaponData.Name, weaponData.Damage, weaponData.StatType);

            var itemsToDrop = new List<Item>();
            int foodDropCount = _random.Next(enemyBase.MinLoot, enemyBase.MaxLoot + 1); // +1 because the random upper range is exclusive

            // create that many random food items.
            for (int j = 0; j < foodDropCount; j++) {
                var foodData = GameData.Food[_random.Next(GameData.Food.Count)];
                itemsToDrop.Add(new Food(foodData.Name, foodData.Energy, foodData.Fullness, foodData.Healing));
            }

            int goldAmount = _random.Next(enemyBase.MinGold, enemyBase.MaxGold);
            var enemyLoot = new Loot(goldAmount, enemyWeapon, itemsToDrop, enemyBase.MaxLoot);

            float randomizedStr = RandomizeStat(enemyBase.Str);
            float randomizedDex = RandomizeStat(enemyBase.Dex);
            float randomizedPrc = RandomizeStat(enemyBase.Prc);
            float randomizedChk = RandomizeStat(enemyBase.Chk);
            var enemyHealth = new Health(
                  enemyBase.MaxHp, enemyBase.MaxHp,
                  enemyBase.MaxEnergy, enemyBase.MaxEnergy,
                  randomizedStr, randomizedPrc, randomizedDex, randomizedChk,

                  luck: 1, fullness: 100, maxFullness: 100, passiveHealing: 2, hpPerEnergy: 0.5f // enemies dont use these anyway
              );

            var newEnemy = new Enemy(enemyHealth, enemyBase.Name, enemyLoot);

            generatedEnemies.Add(newEnemy);
        }
        return generatedEnemies;
    }
    /// <summary>
    /// Takes a base stat value and return a new value that is +- 50%
    /// </summary>
    private float RandomizeStat (float baseValue) {
        // random multiplier between 0.5 an d1.5
        float multiplier = 0.5f + (float)(_random.NextDouble() * 1.5f);
        return baseValue * multiplier;
    }
}
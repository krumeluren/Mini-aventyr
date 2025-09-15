

namespace Mini_aventyr;

public class Enemies {
    private List<Enemy> enemies = new();

    private Random _random = new Random();
    public Enemies () {
        if (enemies.Count == 0) {
            Loot loot = new Loot(0,new Weapon("D",0));
            Health health = new Health(0,0);
            enemies.Add( new Enemy(health,"D",loot) );
        }
    }

    public void AddEnemy (Enemy enemy) {
        enemies.Add(enemy);
    }

    public List<Enemy> GetEnemies (int count) {
        if (count > enemies.Count)
            count = enemies.Count;
  
        List<Enemy> selected = [];
        int selectedCount = 0;
        while (selectedCount < count) {
            int random = _random.Next(0,enemies.Count - 1);
            selected.Add(enemies[random].Clone());
            selectedCount++;
        }
        return selected;
    }
}

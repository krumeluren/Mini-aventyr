

namespace Mini_aventyr;

public class Fight {
    private Player player;
    private List<Enemy> enemies;

    public bool Done { get; private set; }
    public Fight (Player player, List<Enemy> enemies) {
        this.player = player;
        this.enemies = enemies;
    }

    public void Attack () {
        Done = true;
        foreach (var enemy in enemies) {
            enemy.Health.Damage(player.Loot.Weapon.Damage);
        }
        foreach (var enemy in enemies) {
            if (enemy.Health.IsDead)
                continue;

            player.Health.Damage(enemy.Loot.Weapon.Damage);
            Done = false;
        }
    }
}
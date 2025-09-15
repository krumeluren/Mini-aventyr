

namespace Mini_aventyr; 
public class Game {

    private bool quit;

    private readonly Enemies enemies = new Enemies();

    private Player Player;

    private Fight? fight {  get; set; }

    public Game (Player player) {
        Player = player;
    }


    public void Start () {
        enemies.AddEnemy(new Enemy(new Health(10,10),"Enemy 1", new Loot(1,new Weapon("Weapon 1",5))));
        enemies.AddEnemy(new Enemy(new Health(5,5),"Enemy 2", new Loot(1,new Weapon("Weapon 2",10))));

        while (!quit) {
            if (Player.Health.IsDead) {
                Console.WriteLine("You died");
                break;
            }

            Console.Clear();
            Console.WriteLine($"Player: {Player.Name}");
            Console.WriteLine($"Health: {Player.Health.HP}/{Player.Health.MaxHP}");
            Console.WriteLine($"Weapon: {Player.Loot.Weapon.Name}");
            Console.WriteLine($"Gold: {Player.Loot.Gold}");

            if(fight  != null && !fight.Done) {
                Console.WriteLine();
                Console.WriteLine($"Fight: {fight}");
            }

            Console.WriteLine("");
            Console.Write("Action: ");
            string c = Console.ReadLine() ?? string.Empty;

            switch (c) {
                case "quit": 
                quit = true; 
                break;

                case "rest": 
                Player.Health.Rest(); 
                break;

                case "status":
                break;

                case "adv":
                fight = new Fight(
                    Player,
                     enemies.GetEnemies(1) );
                break;

                case "attack":
                if(fight != null && !fight.Done) {
                    fight.Attack();
                }
                break;

                default:
                break;
            }
        }

        Console.Clear();
        Console.WriteLine("Game Over");
        Console.ReadLine();
    }
}

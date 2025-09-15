

using Mini_aventyr;

Console.WriteLine();

Weapon weapon = new Weapon("My weapon", 1);
Loot loot = new Loot(0, weapon);
Player player = new Player(new Health(100, 90), "Class", "Player 1", loot);
Game game = new(player);

game.Start();

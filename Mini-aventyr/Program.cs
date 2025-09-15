

using Mini_aventyr;

// 1. Create an instance of our new creator.
var creator = new CharacterCreator();

// 2. Let the creator class handle the entire setup process.
Player player = creator.CreatePlayer();

// 3. Create and start the game with the fully customized player.
Game game = new(player);
game.Start();

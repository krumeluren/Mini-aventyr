using Mini_aventyr;
using Mini_aventyr.Entities;

var creator = new CharacterCreator();

Player player = creator.CreatePlayer();

var settings = creator.GetSettings();

Game game = new(player, settings);
game.Start();

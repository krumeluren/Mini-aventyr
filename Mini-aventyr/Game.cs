namespace Mini_aventyr;

public class Game {
    private readonly Player _player;

    private readonly Random _random = new();
    private readonly EnemyFactory _enemyFactory = new();

    private readonly List<Enemy> _activeEnemies = new();

    private readonly List<Item> _groundLoot = new();

    public Game (Player player) {
        _player = player;
    }

    public void Start () {
        bool isRunning = true;
        while (isRunning && !_player.Health.IsDead) {
            DisplayGeneralStatus();

            Console.Write("Action: ");
            string? input = Console.ReadLine()?.ToLower() ?? string.Empty;

            if (input == "quit") {
                isRunning = false;
                continue;
            }

            if (!ProcessPlayerAction(input)) {
                Console.WriteLine("\n(Press Enter to retry...)");
                Console.ReadLine();
                continue;
            }

            if (!_player.Health.IsDead) {
                ProcessEnemyTurns();
            }

            CleanupDeadEnemies();

            Console.WriteLine("\n(Press Enter to continue...)");
            Console.ReadLine();
        }

        DisplayGameOver();
    }

    /// <returns>If the loop should continue</returns>
    private bool ProcessPlayerAction (string input) {
        Console.WriteLine("\n--- Your Turn ---");

        string[] parts = input.Split(' ');
        string command = parts.FirstOrDefault() ?? string.Empty;

        _player.Health.Exhaust(1); // each turn you lose basal energy
        _player.Health.Heal(1); // Passive healing

        switch (command) {
            case "study":
            if (!_activeEnemies.Any()) {
                Console.WriteLine("You study your own reflection in a puddle. Now that is a pretty face.");
                return false;
            }
            if (parts.Length < 2 || !int.TryParse(parts[1], out int targetIdx)) {
                Console.WriteLine("Study what? Try 'study <enemyNumber>'.");
                return false;
            }

            targetIdx -= 1;

            if (targetIdx >= 0 && targetIdx < _activeEnemies.Count) {
                var target = _activeEnemies[targetIdx];

                // A baseline character perception (1.0) has a 50% chance of success
                const float baseSuccessChance = 0.5f;
                float actualSuccessChance = baseSuccessChance * _player.Health.Perception;
                if (_random.NextDouble() < actualSuccessChance) {
                    target.IsAnalyzed = true;
                    Console.WriteLine($"You focus your mind... DING! You now understand the {target.Name}'s capabilities.");
                }
                else {
                    Console.WriteLine($"You try to study the {target.Name}, but its erratic movements prevent a clear analysis. Your fumble your turn. How pathetic.");
                }
            }
            else {
                Console.WriteLine("Invalid target. You study the air, which seems unimpressed.");
                return false;
            }
            break; // The break for the "study" case
            case "eat":
            if (parts.Length < 2 || !int.TryParse(parts[1], out int index)) {
                Console.WriteLine("Eat what? Try 'eat <number>'.");
                return false;
            }

            index -= 1; // adjusted for 0 based list
            if (index >= 0 && index < _player.Loot.Items.Count) {
                var selectedItem = _player.Loot.Items[index];
                if (selectedItem is Food food) {
                    _player.Health.Eat(food);
                    _player.Loot.Items.RemoveAt(index);
                    Console.WriteLine($"You eat the {food.Name} and gain {food.Energy} energy.");
                }
                else {
                    Console.WriteLine($"Biting down on the {selectedItem.Name} confirms your suspicion that it is, in fact, not edible.");
                }
            }
            else {
                Console.WriteLine("Your hand meets nothing but the inside of your pack.");
                return false;
            }
            break;

            case "adv":
            case "adventure":
            if (_activeEnemies.Count != 0) {
                Console.WriteLine("You can't leave while in a fight!");
            }
            else {
                ClearAreaState(); // Cant go back for loot if you run         
                Console.WriteLine("You venture into the wild...");
                _player.Health.Exhaust(10); // adventure exhaust
                var newEnemies = _enemyFactory.GetEnemies(CreateEnemyCount());
                _activeEnemies.AddRange(newEnemies);
                Console.WriteLine($"You encounter {newEnemies.Count} enemies!");
            }
            break;

            case "attack":
            if (_activeEnemies.Count == 0) {
                Console.WriteLine($"You swing your {_player.Loot.Weapon.Name} harmlessly in the air.");
            }
            else {
                // targeting a specific enemy
                int targetIndex = 0; // default to the first enemy
                if (parts.Length > 1 && int.TryParse(parts[1], out int chosenIndex)) {
                    targetIndex = chosenIndex - 1; // adjust to 0 index
                }

                if (targetIndex >= 0 && targetIndex < _activeEnemies.Count) {
                    var target = _activeEnemies[targetIndex];

                    _player.Health.Exhaust(1); // exhaust from attacking

                    float damage = CalculateDamage(_player, target);
                    var resultDamage = target.Health.Damage(damage);
                    Console.WriteLine($"You attack the {target.Name} for {Round(resultDamage)} damage.");
                }
                else {
                    Console.WriteLine("You missed the attack.");
                }
            }
            break;

            case "status":
            DisplayPlayerStatus(_player);
            break;


            case "take":
            if (!_groundLoot.Any()) {
                Console.WriteLine("There is nothing on the ground to take.");
            }
            else {
                _player.Health.Exhaust(1); // Lose energy picking up
                int targetIndex = 0;
                if (parts.Length > 1 && int.TryParse(parts[1], out int chosenIndex)) {
                    targetIndex = chosenIndex - 1;
                }

                if (targetIndex >= 0 && targetIndex < _groundLoot.Count) {
                    Item itemToTake = _groundLoot[targetIndex];

                    Console.WriteLine($"You pick up the {itemToTake.Name}.");
                    _groundLoot.Remove(itemToTake);

                    Item? dropped = _player.Loot.Take(itemToTake);
                    if (dropped != null) {
                        Console.WriteLine($"You drop your {dropped.Name} on the ground.");
                        _groundLoot.Add(dropped);
                    }
                }
                else {
                    Console.WriteLine("That's not a valid item to take.");
                }
            }
            break;

            case "rest":
            if (_player.Health.HP >= _player.Health.MaxHP) {
                Console.WriteLine($"You take a moment to rest, restoring some energy.");
            }
            else {
                Console.WriteLine($"You take a moment to rest, restoring some health.");
            }
            _player.Health.Rest();

            if (_activeEnemies.Any()) {
                Console.WriteLine("The enemies take advantage of your pause!");
            }
            break;

            case "run":
            if (_activeEnemies.Any()) {
                Console.WriteLine("You successfully flee from the encounter!");
                ClearAreaState(); // Cant go back for loot if you run
            }
            else {
                Console.WriteLine("You run around for a while burning some calories, but you're not in any danger.");
            }
            _player.Health.Exhaust(10);
            break;

            default:
            Console.WriteLine("You scratch your head trying to figure out what to do.");
            return false;
        }

        return true;
    }

    private void ProcessEnemyTurns () {
        if (_activeEnemies.Count == 0) return;

        Console.WriteLine("--- Enemies Turn ---");
        foreach (var enemy in _activeEnemies) {
            if (enemy.Health.IsDead)
                continue;

            if (!enemy.HasPrepared) {
                Console.WriteLine($"The {enemy.Name} prepares for combat");
                enemy.HasPrepared = true;
                continue;
            }

            enemy.Health.Exhaust(1); // they lose energy from attack
            float damage = CalculateDamage(enemy, _player);
            var resultDamage = _player.Health.Damage(damage);
            Console.WriteLine($"The {enemy.Name} attacks you for {Round(resultDamage)} damage.");
        }
    }

    private void CleanupDeadEnemies () {
        var defeatedEnemies = _activeEnemies.Where(e => e.Health.IsDead).ToList();

        foreach (var enemy in defeatedEnemies) {
            if (enemy.Health.Energy <= 0) {
                Console.WriteLine($"The {enemy.Name} collapsed in exhaustion!");
            }
            else if (enemy.Health.HP <= 0) {
                Console.WriteLine($"The {enemy.Name} succumed from their wounds.");
            }
            else {
                Console.WriteLine($"The {enemy.Name} has been defeated!");
            }
            _player.Loot.Gold += enemy.Loot.Gold;
            Console.WriteLine($"You loot {enemy.Loot.Gold} gold.");

            _groundLoot.Add(enemy.Loot.Weapon);
            Console.WriteLine($"The {enemy.Name} dropped its {enemy.Loot.Weapon.Name}.");

            foreach (var item in enemy.Loot.Items) {
                Console.WriteLine($"The {enemy.Name} dropped {item.Name}!");
                _groundLoot.Add(item);
            }
            _activeEnemies.Remove(enemy);
            Console.WriteLine($"\n");
        }

        if (defeatedEnemies.Any() && _activeEnemies.Count == 0) {
            Console.WriteLine("You have cleared the area of enemies!");
        }
    }

    private void DisplayPlayerStatus (Player player) {
        Console.WriteLine($"\n--- Status ---");
        Console.WriteLine($"Player: {_player.Name} ({_player.Class})");
        Console.WriteLine($"Health: {Round(_player.Health.HP, 0)}/{_player.Health.MaxHP} | Energy: {Round(_player.Health.Energy, 0)}/{_player.Health.MaxEnergy}");
        Console.WriteLine($"Gold: {_player.Loot.Gold}");
        Console.WriteLine($"Equipped: {_player.Loot.Weapon.Name} (Damage: {Round(_player.Loot.Weapon.BaseDamage)})");
        if (_player.Loot.Items.Any()) {
            Console.WriteLine($"Your Items {_player.Loot.Items.Count}/{_player.Loot.MaxItems}:");
            for (int i = 0 ; i < _player.Loot.Items.Count ; i++) {
                Console.WriteLine($"[{i + 1}] {_player.Loot.Items [i].Details()}");
            }
        }
        Console.WriteLine("\n----------------");
    }

    private void DisplayGeneralStatus () {
        Console.Clear();
        DisplayPlayerStatus(_player);

        if (_activeEnemies.Any()) {
            Console.WriteLine("ENEMIES PRESENT:");
            for (int i = 0; i < _activeEnemies.Count; i++) {
                var enemy = _activeEnemies[i];
                enemy.Display(i);
            }
            Console.WriteLine("----------------\n");
        }

        if (_groundLoot.Any()) {
            Console.WriteLine("LOOT ON GROUND:");
            for (int i = 0; i < _groundLoot.Count; i++) {
                var item = _groundLoot[i];
                Console.WriteLine($"[{i + 1}] {item.Details()}");
            }
            Console.WriteLine("----------------\n");
        }

        // show available commands.
        string actions = "Actions:[quit], [adv], [rest], [run], [status]";
        if (_player.Loot.Items.Count != 0) actions += ", [eat <number>]";
        if (_activeEnemies.Count != 0) actions += ", [attack <enemyNumber>]";
        if (_activeEnemies.Count != 0) actions += ", [study <enemyNumber>]";
        if (_groundLoot.Count != 0) actions += ", [take <number>]";
        Console.WriteLine(actions);
    }

    private void DisplayGameOver () {
        Console.Clear();
        Console.WriteLine("===================");
        Console.WriteLine("    GAME OVER    ");
        Console.WriteLine("===================");

        if (_player.Health.HP <= 0) {
            Console.WriteLine("You succumbed from your wounds.");
        }
        else if (_player.Health.Energy <= 0) {
            Console.WriteLine("You succumbed from exhaustion");
        }
        else {
            Console.WriteLine($"You finished with {_player.Loot.Gold} gold.");
        }
        Console.ReadLine();
    }

    private float CalculateDamage (Entity entity, Entity target) {
        float statMultiplier = 1.0f;
        switch (entity.Loot.Weapon.ScalingType) {
            case Weapon.StatType.Strength: statMultiplier = entity.Health.Strength; break;
            case Weapon.StatType.Dexterity: statMultiplier = entity.Health.Dexterity; break;
            case Weapon.StatType.Perception: statMultiplier = entity.Health.Perception; break;
            case Weapon.StatType.Chakra: statMultiplier = entity.Health.Chakra; break;
            default: break;
        }

        foreach (var item in entity.Loot.Items) {
            statMultiplier *= item.StatMultiplier(statMultiplier, entity.Loot.Weapon);
        }

        switch (entity.Loot.Weapon.ScalingType) {
            case Weapon.StatType.Strength:
            statMultiplier *= Math.Min(1, Math.Max(1 - target.Health.Strength, 1));
            break;
        }

        float damage = entity.Loot.Weapon.BaseDamage * entity.Health.Power * statMultiplier;
        return damage;
    }

    /// <summary>
    ///  Clear out the local area
    /// </summary>
    private void ClearAreaState () {
        _activeEnemies.Clear();
        _groundLoot.Clear();
    }

    private int CreateEnemyCount () {
        int gold = _player.Loot.Gold;
        int items = _player.Loot.Items.Count;

        int max = 1;
        max += (int)Math.Floor(gold / 50f);
        max += (int) Math.Floor(items / 5f);

        return _random.Next(1, max + 1);
    }


    private float Round(float v, int digits = 1) {
        if(digits == 0) return (float)(int)Math.Round(v, 1);
        return (float) Math.Round(v,digits);
    }
}
using Mini_aventyr;
using Mini_aventyr.Entities;
using Mini_aventyr.EntityComponents;
using Mini_aventyr.Items;

public class CharacterCreator {

    public class CharacterBuilder {
        public string Name { get; set; } = "Nameless";
        public string ClassName { get; set; } = "Amnesiac";
        public Weapon Weapon { get; set; } = new("Broken Chair Leg", 1, Weapon.StatType.None);
        public bool SelectedWeapon { get; set; }

        public List<Food> Food { get; set; } = new();
        public List<Trinket> Trinkets { get; set; } = new();


        public int MaxInventory { get; set; } = 5;
        public int MaxHp { get; set; } = 80;
        public int MaxEnergy { get; set; } = 80;

        // STR, DEX, PER, CHA all affect combat, but also some other unique behaviours when it makes sense.
        public float Strength { get; set; } = 1.0f;
        public float Dexterity { get; set; } = 1.0f;
        public float Perception { get; set; } = 1.0f;
        public float Chakra { get; set; } = 1.0f;

        // Might affect random probabilities in game mechanics
        public float Luck { get; set; } = 1.0f;

        // When you eat this increases to restrict eating too much at once. It depletes overtime in game
        public float Fullness { get; set; } = 10.0f;
        public float MaxFullness { get; set; } = 20.0f;

        // How much HP can maximally restored each round passively from energy
        public float PassiveHealing { get; set; } = 5.0f;
        // How much hp per energy from the passive healing. 1 is 100% efficiency
        public float HpPerEnergy { get; set; } = 0.5f;
        public int StartGold { get; set; } = 0;
    }

    private CharacterBuilder _builder = new();
    private GameSettings _settings = new();

    public GameSettings GetSettings () {
        return _settings;
    }

    private void DisplayTitle () {
        Console.Clear();
        Console.WriteLine(" ----- Character Builder -----");
    }

    public Player CreatePlayer () {
        var b = _builder;

        _settings.EnemyCountDifficultyScaling = 1f;

        Console.Clear();
        Console.WriteLine("\nYou awaken in a dusty room, the taste of cheap ale, blood and regret on your tongue.");
        Console.ReadKey();

        Console.WriteLine("\nA wry voice echoes from the corner. \"Took you long enough. The world didn't stop turning while you were drooling on my sheets.\"");
        Console.ReadKey();

        Console.WriteLine("\n\"Your head feels like it's been used as a drum,\" the voice continues. \"Let's try to piece things together. We'll start from the beginning... long before this mess.\"");
        Console.ReadKey();
        Console.Clear();

        // recalling memories to build the character
        DisplayTitle();
        RecallLifesPath(b);

        DisplayTitle();
        RecallChildhood(b);

        DisplayTitle();
        RecallCareer(b);

        DisplayTitle();
        RecallMotivation(b);

        DisplayTitle();
        RecallArrivalStory(b);

        DisplayTitle();
        RecallDefiningAction(b);

        DisplayTitle();
        RecallFinalMoments(b);

        if (!b.SelectedWeapon) {
            DisplayTitle();
            ChooseStartingGear(b);
        }

        DisplayTitle();
        ChoosePersonalTrinket(b);

        DisplayTitle();
        ChooseName();

        Console.Clear();
        b.ClassName = ClassNameHelper.DetermineClassName(b);
        Console.WriteLine($"\n\"So, a one might call you a '{b.ClassName}',\" the voice says, sounding thoroughly unimpressed. \"Right then. Your tab is already paid.\"");
        Console.ReadKey();

        Console.WriteLine($"\nYou check your coin purse. It is empty.");
        Console.ReadKey();

        Console.WriteLine($"\n\"The door is that way. Try not to die within the first five minutes. It's bad for business.\"");
        Console.ReadKey();

        Console.WriteLine("\nYou grab your " + b.Weapon.Name + " and step out into the world.");
        Console.WriteLine("(Press Enter to begin your adventure...)");
        Console.ReadKey();
        Console.Clear();

        List<Item> combinedItems = [.. b.Food, .. b.Trinkets];

        var health = new Health(b.MaxHp, b.MaxHp, b.MaxEnergy, b.MaxEnergy,
            b.Strength, b.Perception, b.Dexterity, b.Chakra,
            b.Luck,
            b.Fullness, b.MaxFullness, b.PassiveHealing, b.HpPerEnergy);

        var loot = new Loot(Math.Max(b.StartGold, 0), b.Weapon, combinedItems, b.MaxInventory);
        return new Player(health, b.ClassName, b.Name, loot);
    }
    private void RecallLifesPath (CharacterBuilder builder) {
        int choice = GetChoice(
            "\"The fragments of your past swirl together,\" the voice remarks. \"When you think of your life before this room, the overwhelming feeling is one of...\"",
            new[] {
            "A Charmed Existence. (Easy) - Things just seemed to work out. A lucky break was always around the corner.",
            "An Even Trade. (Fair) - You got what you gave. Victories were earned, losses were deserved.",
            "A Constant Struggle. (Hard) - For every coin earned, you paid for it in sweat or blood.",
            "A Never Ending Nightmare. (Very Hard) - It felt as if the world itself was against you."
            }
        );

        Console.WriteLine();
        switch (choice) {
            case 1: // Easy
            Console.WriteLine("\"A golden path. Some people have all the luck. Let's see if it holds.\"");
            _settings.EnemyCountDifficultyScaling = 1f;
            builder.StartGold += 10;
            break;
            case 2: // Normal
            Console.WriteLine("\"Balanced scales. You made your own luck, for better or worse. A respectable path.\"");
            _settings.EnemyCountDifficultyScaling = 1.25f;
            break;
            case 3: // Hard
            Console.WriteLine("\"The rocky road. You've clawed your way through life. I suppose this is just another Tuesday for you.\"");
            _settings.EnemyCountDifficultyScaling = 1.5f;
            break;
            case 4: // Nightmare
            Console.WriteLine("\"Dogged by misfortune. And yet, you're still here. That resilience might be the only thing of value you have.\"");
            _settings.EnemyCountDifficultyScaling = 2f;
            builder.MaxHp -= 10;
            builder.Luck += 0.2f;
            builder.Trinkets.Add(new Trinket("Bent Nail of Defiance"));
            break;
        }
        Console.ReadKey();
    }

    private void RecallChildhood (CharacterBuilder builder) {
        int choice = GetChoice(
            "You sift through the fog of your mind to your earliest days. You were...",
            new[] {
                "A village lout, spending your days hauling hay and your nights wrestling in the tavern.",
                "An alley cat in a bustling city, learning that quick fingers and quicker feet were the key to a full belly.",
                "A sheltered scholar, hidden away in a library, devouring books of lore and history.",
                "A hunter's child, learning the ways of the forest, the track of the beast, and the bite of the wind."
            }
        );

        Console.WriteLine();
        switch (choice) {
            case 1: // Lout
            Console.WriteLine("\"Ah, the simple life. Good for the shoulders, bad for the brain. Explains the headache.\"");
            builder.Strength += 0.2f;
            builder.MaxHp += 15;
            builder.Chakra -= 0.1f;
            builder.Food.Add(new Food("Mother's Hearty Bread", energy: 10, fullness: 10, healing: 0));
            builder.MaxFullness += 3;
            break;
            case 2: // The Alley Cat
            Console.WriteLine("\"Learned to be quick with your hands and your feet, I see. Mostly with other people's things.\"");
            builder.Dexterity += 0.2f;
            builder.MaxInventory += 2;
            builder.Strength -= 0.1f;
            builder.HpPerEnergy += 0.1f;
            _settings.EnemyCountDifficultyScaling += 0.05f;
            break;
            case 3: // Scholar
            Console.WriteLine("\"So you spent your youth with your nose in a book. Explains the pallor. And the sneezing.\"");
            builder.Perception += 0.1f;
            builder.Chakra += 0.2f;
            builder.MaxHp -= 10;
            break;
            case 4: // Hunter
            Console.WriteLine("\"The great outdoors. Full of fresh air and things that want to eat you. A practical education.\"");
            builder.Perception += 0.2f;
            builder.Dexterity += 0.1f;
            builder.MaxEnergy += 10;
            break;
        }
        Console.ReadKey();
    }

    private void RecallCareer (CharacterBuilder builder) {
        int choice = GetChoice(
            "You sift through the fog of your mind once more to the past..",
            new[] {
                "Three of your fingers are missing. That's right, you remember how they were cut off during a big battle.",
                "You remember going to the Wizard Academy but eventually left to write songs. That might explain the Lute next to you",
                "You recall clinking glasses and the snap of playing cards. You made a living pouring drinks and playing the winning hand.",
                "A splintered staff lies nearby. Your hand aches with a phantom grip, recalling the solitude of a summit high above the clouds."
            }
        );

        Console.WriteLine();
        switch (choice) {
            case 1:
            Console.WriteLine("\"A soldier, eh? Explains the missing digits and the vacant stare. Must have been a nasty fight if you lost your fingers *and* your memory.\"");
            builder.Dexterity -= 0.1f;
            builder.Strength += 0.2f;
            builder.Chakra -= 0.05f;
            builder.MaxFullness += 2f;
            break;

            case 2:
            Console.WriteLine("\"Traded arcane mastery for cheap tavern gigs and rhyming 'ale' with 'pale'. A bold artistic choice. I assume it didn't paid the bills, given your current... situation.\"");
            builder.Weapon = new Weapon("Lute", 6, Weapon.StatType.None);
            builder.SelectedWeapon = true;
            builder.Chakra += 0.05f;
            builder.Dexterity += 0.1f;
            builder.Strength -= 0.1f;
            builder.MaxInventory += 1;
            break;

            case 3:
            Console.WriteLine("\"A bartender *and* a cardsharp. So you got people drunk and then took their money. It's a wonder you have any teeth left.\"");
            builder.Luck += 0.2f;
            builder.Perception += 0.2f;
            builder.Strength -= 0.05f;
            builder.Chakra -= 0.1f;
            builder.HpPerEnergy -= 0.1f;
            builder.StartGold += 5;
            _settings.EnemyCountDifficultyScaling += 0.05f;

            break;
            case 4:
            Console.WriteLine("\"From communing with the clouds to napping on my floorboards. A long way to fall, wasn't it? The staff looks about as good as you feel.\"");
            builder.Dexterity += 0.15f;
            builder.Chakra += 0.15f;
            builder.Perception += 0.1f;
            builder.Strength -= 0.15f;
            builder.MaxHp -= 10;
            builder.MaxFullness -= 3;
            builder.Fullness += 3;
            builder.Weapon = new Weapon("Broken Ancestral Staff", 6, Weapon.StatType.Dexterity);
            builder.SelectedWeapon = true;

            builder.Food.Add(new("Dried Herbs", 2, 1, 10));
            builder.HpPerEnergy += 0.1f;
            builder.Luck -= 0.1f;
            builder.MaxInventory -= 1;

            break;
        }
        Console.ReadKey();
    }

    private void RecallMotivation (CharacterBuilder builder) {
        int choice = GetChoice(
            "\n\"And yet, you left that life behind,\" the voice muses. \"People don't trade a roof for a ditch without a reason. What was yours?\"",
            new[] {
                "Fortune. You seek a legendary treasure that will set you up for life.",
                "Vengeance. Someone wronged you, and they will pay the price.",
                "Knowledge. There are secrets in this world that must be brought to light.",
                "Wanderlust. You simply couldn't stand to stay in one place any longer."
            }
        );

        Console.WriteLine();
        switch (choice) {
            case 1: // Fortune
            Console.WriteLine("\"Money, then. The root of all evil and the solution to most of it. A practical choice.\"");
            builder.Dexterity += 0.1f;
            builder.Perception += 0.1f;
            builder.MaxInventory += 1;
            builder.Trinkets.Add(new Trinket("Explorer's Compass"));
            break;
            case 2: // vengeance
            Console.WriteLine("\"Revenge. A fire that keeps you warm at night, until it burns you to a crisp. Good luck with that.\"");
            builder.Strength += 0.1f;
            builder.Chakra += 0.1f;
            builder.MaxHp += 5;
            _settings.EnemyCountDifficultyScaling += 0.05f;
            break;
            case 3: // knowledge
            Console.WriteLine("\"Chasing secrets. A noble pursuit. Usually ends with finding secrets that would have been better left alone.\"");
            builder.Chakra += 0.15f;
            builder.Perception += 0.15f;
            builder.Trinkets.Add(new Trinket("Explorer's Compass"));
            break;
            case 4: // wanderlust
            Console.WriteLine("\"Running *from* something, or just running *to* anywhere else. The most tiring reason of all.\"");
            builder.Dexterity += 0.1f;
            builder.MaxEnergy += 15;
            builder.Fullness -= 1;
            break;
        }
        Console.ReadKey();
    }

    private void RecallArrivalStory (CharacterBuilder builder) {
        int choice = GetChoice(
            "\n\"Which brings us to this town. You arrived after...\"",
            new[] {
                "Guarding a spice caravan. You fought like a lion against bandits but were eventually overwhelmed.",
                "Delivering a... 'sensitive package'. The recipient's spouse was not pleased. You had to run. Fast.",
                "A game of cards with a wizard. You were accused of 'creative shuffling'. There was a flash of light.",
                "You remember escaping a prison cell. You were starving. The city guard chased after you. Then you fell into a root cellar before it's all a void."
            }
        );

        switch (choice) {
            case 1: // fighter
            Console.WriteLine("\n\"A caravan guard? So you're good at getting hit. A valuable, if painful, skill.\"");
            builder.Strength += 0.2f;
            builder.MaxHp += 25;
            builder.Perception -= 0.1f;
            builder.Food.Add(new Food("Caravan Ration", 5, 5, 0));
            break;
            case 2: // Courier
            Console.WriteLine("\n\"A fleet-footed messenger, eh? More of a professional runaway, from the sound of it.\"");
            builder.Dexterity += 0.1f;
            builder.Perception += 0.2f;
            builder.MaxEnergy += 25;
            builder.Strength -= 0.1f;
            break;
            case 3: // Gambler
            Console.WriteLine("\n\"You tried to swindle a wizard. The fact you still have eyebrows is a miracle.\"");
            builder.Chakra += 0.2f;
            builder.Perception += 0.1f;
            builder.MaxEnergy -= 5;
            builder.MaxHp -= 15;
            builder.HpPerEnergy -= 0.1f;
            builder.PassiveHealing -= 1f;

            break;
            case 4: // prisoner
            Console.WriteLine("\"A daring prison break, ending with an undignified yet fortunate tumble into a vegetable patch.\"");
            builder.Luck += 0.2f;
            builder.Dexterity += 0.15f;
            builder.MaxHp -= 5;
            builder.Fullness += 10;
            builder.StartGold = 0;
            _settings.EnemyCountDifficultyScaling += 0.05f;
            break;
        }
        Console.ReadKey();
    }

    private void RecallDefiningAction (CharacterBuilder builder) {
        int choice = GetChoice(
            "\n\"During that fiasco,\" the voice says, \"when it all went wrong, what was your defining action?\"",
            new[] {
                "You held the line. You stood your ground to let others escape, a rock against the tide.",
                "You exploited an opening. You saw a weakness—a frayed rope, a loose beam—and used it to turn the tables.",
                "You grabbed the loot and ran. Every person for themselves, right?",
                "You... talked to them. Tried to reason with them. It was a surprisingly spirited debate."
            }
        );

        Console.WriteLine();
        switch (choice) {
            case 1: // Hero
            Console.WriteLine("\"Self-sacrifice. How quaint. Hope it was worth it.\"");
            builder.Strength += 0.2f;
            builder.Chakra += 0.1f;
            builder.MaxHp += 10;
            builder.MaxInventory -= 2;
            builder.Fullness -= 2;
            builder.MaxFullness += 1;
            break;
            case 2: // Tactician
            Console.WriteLine("\"Quick thinking. Using your head for something other than a hat rack.\"");
            builder.Perception += 0.2f;
            builder.Dexterity += 0.1f;
            break;
            case 3: // Opportunist
            Console.WriteLine("\"Pragmatic. Some would say cowardly. But you're alive and have lunch, so who's laughing?\"");
            builder.Dexterity += 0.2f;
            builder.MaxInventory += 2;
            builder.Strength -= 0.1f;
            builder.Food.Add(new Food("Someone Else's Lunch", 8, 5, 0));
            builder.StartGold += 5;
            _settings.EnemyCountDifficultyScaling += 0.05f;

            break;
            case 4: // Diplomat
            Console.WriteLine("\"You tried to talk your way out of a fight? Audacious. And stupid. The results speak for themselves.\"");
            builder.Chakra += 0.2f;
            builder.Perception += 0.1f;
            break;
        }
        Console.ReadKey();
    }

    private void RecallFinalMoments (CharacterBuilder builder) {
        int choice = GetChoice(
            "\n\"We're almost there. The grand finale. What's the very last thing you remember before you blacked out?\"",
            new[] {
                "A fistfight over a card game. You think you were winning.",
                "A deal gone wrong in a dark alley. You were handed a note just before the sap hit your head.",
                "Tasting the 'house special' at the local tavern. It tasted... purple."
            }
        );

        Console.WriteLine();
        switch (choice) {
            case 1: // Bar fight
            Console.WriteLine("\"Predictable. You have the knuckles for it. Hope you didn't lose your winnings.\"");
            builder.Strength += 0.1f;
            builder.Trinkets.Add(new Trinket("Dog-Eared Ace of Spades"));
            builder.Luck += 0.15f;

            break;
            case 2: // deal
            Console.WriteLine("\"Trust is a luxury. Looks like you couldn't afford it. At least you got a souvenir.\"");
            builder.Perception += 0.1f;
            builder.Trinkets.Add(new Trinket("Mysterious Coded Message"));
            break;
            case 3: // Drank mystery fluid
            Console.WriteLine("\"Experimenting with local culture, were we? Some experiments are... less successful than others.\"");
            builder.Chakra += 0.1f;
            builder.MaxHp -= 5;
            builder.Luck -= 0.1f;
            break;
        }
        Console.ReadKey();
    }

    private void ChooseStartingGear (CharacterBuilder builder) {
        Console.WriteLine("\n\"Right,\" the voice sighs. \"Your memories are a mess, but you feel the familiar weight of... what is it?\"");

        int weaponChoice = GetChoice(
            "You remember now. You reach for your weapon...",
            new[] {
                "A dented Broadsword. Heavy and reliable.",
                "A pair of balanced Daggers. Quick and deadly. ",
                "A finely crafted Crossbow. Precision over power.",
                "A humming Crystal Orb. It crackles with latent power.",
                "A trusty Hatchet. It's been with you since your eighth birthday."
            }
        );

        switch (weaponChoice) {
            case 1:
            builder.Weapon = new Weapon("'Reliable' Broadsword", 6, Weapon.StatType.Strength);
            builder.Strength += 0.05f;
            break;
            case 2:
            builder.Weapon = new Weapon("Balanced Daggers", 6, Weapon.StatType.Dexterity);
            builder.Dexterity += 0.05f;

            break;
            case 3:
            builder.Weapon = new Weapon("Fine Crossbow", 6, Weapon.StatType.Perception);
            builder.Perception += 0.05f;

            break;
            case 4:
            builder.Weapon = new Weapon("Crystal Orb", 6, Weapon.StatType.Chakra);
            builder.Chakra += 0.05f;

            break;
            default:
            builder.Weapon = new Weapon("Ol' Trusty Hatchet", 4, Weapon.StatType.None);
            builder.MaxEnergy += 3;
            builder.Luck += 0.05f;
            break;
        }
        builder.SelectedWeapon = true;
        Console.ReadKey();
    }

    private void ChoosePersonalTrinket (CharacterBuilder builder) {
        int choice = GetChoice(
            "\n\"Before you go, you check your pockets one last time. Besides the lint, you find one small, personal item.\"",
            new[] {
                "A smooth river stone. It's cool to the touch but oddly comforting.",
                "A bent coin from a foreign land. You can't remember where you got it.",
                "A single, perfectly preserved feather from an unknown bird.",
                "A ugly broken mechanical stopwatch.",
            }
        );

        Console.WriteLine();
        switch (choice) {
            case 1: // Stone
            Console.WriteLine("\"A rock. Sentimental. Or you just like rocks. It does make you feel a bit tougher.\"");
            builder.MaxHp += 5;
            builder.HpPerEnergy += 0.1f;
            builder.Trinkets.Add(new Trinket("Smooth River Stone"));
            break;
            case 2: // Coin
            Console.WriteLine("\"Heads or tails, you always lose. Maybe this'll change that. Probably not. Still, makes you feel lucky.\"");
            builder.Perception += 0.05f; // small buff
            builder.Trinkets.Add(new Trinket("Bent Foreign Coin"));
            builder.Luck += 0.15f;
            break;
            case 3: // Feather
            Console.WriteLine("\"A memento of something that could fly. You feel a bit lighter yourself.\"");
            builder.MaxEnergy += 5;
            builder.Trinkets.Add(new Trinket("Perfect Feather"));
            break;
            case 4: // Stopwatch
            Console.WriteLine("\"A handy gadget. If it worked. You don't remember where or how you got it, and even though it looks like junk, it feels important.\"");
            builder.Chakra += 0.03f;
            builder.Trinkets.Add(new Trinket("Broken Stopwatch"));
            break;
        }
        Console.ReadKey();
    }

    private void ChooseName () {
        string name = "";
        while (string.IsNullOrWhiteSpace(name)) {
            Console.Write("\nYou try to remember your name... you remember now! It is: ");
            name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name)) Console.WriteLine("\"Mumbling won't get you anywhere. Spit it out!\"");
        }
        _builder.Name = name.Trim();
        Console.WriteLine($"\n\"{_builder.Name}, eh? Doesn't ring a bell,\" the voice muses.");
        Console.ReadKey();
    }

    private int GetChoice (string prompt, string[] options) {
        Console.WriteLine($"\n{prompt}");
        for (int i = 0; i < options.Length; i++) {
            Console.WriteLine($"\n  [{i + 1}]: {options[i]}");
        }

        int choice = 0;
        while (choice < 1 || choice > options.Length) {
            Console.Write($"\nChoose (1-{options.Length}): ");
            if (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > options.Length) {
                Console.WriteLine("\"Are you concussed? Pick one.\"");
                choice = 0; // Reset choice if parsing fails or out of range
            }
        }
        return choice;
    }
}
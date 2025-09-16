using Mini_aventyr;

public class CharacterCreator {
    public class CharacterBuilder {
        public string Name { get; set; } = "Nameless";
        public string ClassName { get; set; } = "Amnesiac";
        public Weapon Weapon { get; set; } = new("Dining Fork", 1, Weapon.StatType.None);
        public List<Food> Food { get; set; } = new();
        public List<Trinket> Trinkets { get; set; } = new();
        public int MaxInventory { get; set; } = 5;
        public int MaxHp { get; set; } = 80;
        public int MaxEnergy { get; set; } = 80;
        public float Strength { get; set; } = 1.0f;
        public float Dexterity { get; set; } = 1.0f;
        public float Perception { get; set; } = 1.0f;
        public float Chakra { get; set; } = 1.0f;
    }

    private CharacterBuilder _builder = new();

    public Player CreatePlayer () {
        var builder = _builder;

        Console.Clear();
        Console.WriteLine("You awaken in a dusty room, the taste of cheap ale, blood and regret on your tongue.");
        Console.WriteLine("A wry voice echoes from the corner. \"Took you long enough. The world didn't stop turning while you were drooling on my pillow.\"");
        Console.ReadKey();

        Console.WriteLine("\n\"Your head feels like it's been used as a drum,\" the voice continues. \"Let's try to piece things together. We'll start from the beginning... long before this mess.\"");
        Console.ReadKey();

        // recalling memories to build the character
        RecallChildhood(builder);
        RecallMotivation(builder);
        RecallArrivalStory(builder);
        RecallDefiningAction(builder);
        RecallFinalMoments(builder);
        ChooseStartingGear(builder);
        ChoosePersonalTrinket(builder);
        ChooseName();

        builder.ClassName = ClassNameHelper.DetermineClassName(builder);
        Console.WriteLine($"\n\"So, a one might call you a '{builder.ClassName}',\" the voice says, sounding thoroughly unimpressed. \"Right then. Your tab is paid. The door is that way. Try not to die within the first five minutes. It's bad for business.\"");
        Console.ReadKey();

        Console.WriteLine("\nYou grab your " + builder.Weapon.Name + " and step out into the world.");
        Console.WriteLine("(Press Enter to begin your adventure...)");
        Console.ReadKey();

        List<Item> combinedItems = [.. builder.Food, .. builder.Trinkets];

        var health = new Health(builder.MaxHp, builder.MaxHp, builder.MaxEnergy, builder.MaxEnergy, builder.Strength, builder.Perception, builder.Dexterity, builder.Chakra);
        var loot = new Loot(0, builder.Weapon, combinedItems, builder.MaxInventory);
        return new Player(health, builder.ClassName, builder.Name, loot);
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
            builder.Food.Add(new Food("Mother's Hearty Bread", 20));
            break;
            case 2: // The Alley Cat
            Console.WriteLine("\"Learned to be quick with your hands and your feet, I see. Mostly with other people's things.\"");
            builder.Dexterity += 0.2f;
            builder.MaxInventory += 2;
            builder.Strength -= 0.1f;
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
            builder.Trinkets.Add(new Trinket("Compass"));
            break;
            case 2: // vengeance
            Console.WriteLine("\"Revenge. A fire that keeps you warm at night, until it burns you to a crisp. Good luck with that.\"");
            builder.Strength += 0.1f;
            builder.Chakra += 0.1f;
            builder.MaxHp += 5;
            break;
            case 3: // knowledge
            Console.WriteLine("\"Chasing secrets. A noble pursuit. Usually ends with finding secrets that would have been better left alone.\"");
            builder.Chakra += 0.15f;
            builder.Perception += 0.15f;
            break;
            case 4: // wanderlust
            Console.WriteLine("\"Running *from* something, or just running *to* anywhere else. The most tiring reason of all.\"");
            builder.Dexterity += 0.1f;
            builder.MaxEnergy += 15;
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
                "A game of cards with a wizard. You were accused of 'creative shuffling'. There was a flash of light."
            }
        );

        switch (choice) {
            case 1: // fighter
            Console.WriteLine("\n\"A caravan guard? So you're good at getting hit. A valuable, if painful, skill.\"");
            builder.Strength += 0.3f;
            builder.MaxHp += 25;
            builder.Perception -= 0.1f;
            builder.Food.Add(new Food("Caravan Ration", 10));
            break;
            case 2: // Courier
            Console.WriteLine("\n\"A fleet-footed messenger, eh? More of a professional runaway, from the sound of it.\"");
            builder.Dexterity += 0.3f;
            builder.MaxEnergy += 25;
            builder.Strength -= 0.1f;
            break;
            case 3: // Gambler
            Console.WriteLine("\n\"You tried to swindle a wizard. The fact you still have eyebrows is a miracle.\"");
            builder.Chakra += 0.3f;
            builder.Perception += 0.2f;
            builder.MaxHp -= 15;
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
            builder.Food.Add(new Food("Someone Else's Lunch", 15));
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
            "\n\"We're almost there. The grand finale. What's the very last thing you remember before you started decorating my floor?\"",
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
            break;
            case 2:
            builder.Weapon = new Weapon("Balanced Daggers", 6, Weapon.StatType.Dexterity);
            break;
            case 3:
            builder.Weapon = new Weapon("Fine Crossbow", 6, Weapon.StatType.Perception);
            break;
            case 4:
            builder.Weapon = new Weapon("Crystal Orb", 6, Weapon.StatType.Chakra);
            break;
            default:
            builder.Weapon = new Weapon("Trusty Hatchet", 6, Weapon.StatType.None);
            break;
        }
        Console.ReadKey();
    }

    private void ChoosePersonalTrinket (CharacterBuilder builder) {
        int choice = GetChoice(
            "\n\"Before you go, you check your pockets one last time. Besides the lint, you find one small, personal item.\"",
            new[] {
                "A smooth river stone. It's cool to the touch and oddly comforting.",
                "A bent coin from a foreign land. You can't remember where you got it.",
                "A single, perfectly preserved feather from an unknown bird."
            }
        );

        Console.WriteLine();
        switch (choice) {
            case 1: // Stone
            Console.WriteLine("\"A rock. Sentimental. Or you just like rocks. It does make you feel a bit tougher.\"");
            builder.MaxHp += 5;
            builder.Trinkets.Add(new Trinket("Smooth River Stone"));
            break;
            case 2: // Coin
            Console.WriteLine("\"Heads or tails, you always lose. Maybe this'll change that. Probably not. Still, makes you feel lucky.\"");
            builder.Perception += 0.05f; // small buff
            builder.Trinkets.Add(new Trinket("Bent Foreign Coin"));
            break;
            case 3: // Feather
            Console.WriteLine("\"A memento of something that could fly. You feel a bit lighter yourself.\"");
            builder.MaxEnergy += 5;
            builder.Trinkets.Add(new Trinket("Perfect Feather"));
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
        _builder.Name = name;
        Console.WriteLine($"\n\"{_builder.Name}, eh? Doesn't ring a bell,\" the voice muses.");
        Console.ReadKey();
    }

    private int GetChoice (string prompt, string[] options) {
        Console.WriteLine(prompt);
        for (int i = 0; i < options.Length; i++) {
            Console.WriteLine($"  {i + 1}: {options[i]}");
        }

        int choice = 0;
        while (choice < 1 || choice > options.Length) {
            Console.Write($"\nChoose (1-{options.Length}): ");
            if (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > options.Length) {
                Console.WriteLine("\"Stop playing games. Pick one.\"");
                choice = 0; // Reset choice if parsing fails or out of range
            }
        }
        return choice;
    }
}
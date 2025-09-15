using Mini_aventyr;

public class CharacterCreator {
    // A  "builder" class to hold the character's stats as they are being assembled from the story.
    public class CharacterBuilder {
        public string Name { get; set; } = "Nameless";
        public string ClassName { get; set; } = "Amnesiac";
        public Weapon Weapon { get; set; } = new("Dining Fork", 1, Weapon.StatType.None);
        public List<Food> Food { get; set; } = new();
        public int MaxInventory { get; set; } = 3;
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

        Console.WriteLine("Your head looks like it's been used as a drum. Let's try to piece things together. How did you even get here?");

        RecallArrivalStory(builder);
        RecallDefiningAction(builder);
        ChooseStartingGear(builder);
        ChooseName();

        //  class name is now assigned based on the final stats.
        builder.ClassName = ClassNameHelper.DetermineClassName(builder);
        Console.WriteLine($"\n\"So, a one might call you a '{builder.ClassName}',\" the voice says, sounding thoroughly unimpressed. \"Right then. Your tab is paid. The door is that way. Try not to die within the first five minutes. It's bad for business.\"");
        Console.ReadKey();

        Console.WriteLine("\nYou grab your " + builder.Weapon.Name + " and step out into the world.");
        Console.WriteLine("(Press Enter to begin your adventure...)");
        Console.ReadKey();

        // Assemble the final Player object from the builder.
        var health = new Health(builder.MaxHp, builder.MaxHp, builder.MaxEnergy, builder.MaxEnergy, builder.Strength, builder.Perception, builder.Dexterity, builder.Chakra);
        var loot = new Loot(0, builder.Weapon, builder.Food, builder.MaxInventory);
        return new Player(health, builder.ClassName, builder.Name, loot);
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

    private void RecallArrivalStory (CharacterBuilder builder) {
        int choice = GetChoice(
            "You sift through your fragmented memories. You arrived in town after...",
            new[] {
                "Guarding a spice caravan. You fought like a lion against bandits but were eventually overwhelmed.",
                "Delivering a... 'sensitive package'. The recipient's spouse was not pleased. You had to run. Fast.",
                "A game of cards with a wizard. You were accused of 'creative shuffling'. There was a flash of light."
            }
        );

        switch (choice) {
            case 1: // The Fighter
            Console.WriteLine("\n\"A caravan guard? So you're good at getting hit. A valuable, if painful, skill.\"");
            builder.Strength += 0.3f;
            builder.MaxHp += 25;
            builder.Perception -= 0.1f;
            break;
            case 2: // The Courier
            Console.WriteLine("\n\"A fleet-footed messenger, eh? More of a professional runaway, from the sound of it.\"");
            builder.Dexterity += 0.3f;
            builder.MaxEnergy += 25;
            builder.Strength -= 0.1f;
            break;
            case 3: // The Gambler
            Console.WriteLine("\n\"You tried to swindle a wizard. The fact you still have eyebrows is a miracle.\"");
            builder.Chakra += 0.3f;
            builder.Perception += 0.2f;
            builder.MaxHp -= 15;
            break;
        }
    }

    private void RecallDefiningAction (CharacterBuilder builder) {
        int choice = GetChoice(
            "\n\"That explains the bruises,\" the voice says. \"But in that moment of chaos, what was your defining action?\"",
            new[] {
                "You held the line. You stood your ground to let others escape, a rock against the tide.",
                "You exploited an opening. You saw a weakness—a frayed rope, a loose beam—and used it to turn the tables.",
                "You grabbed the loot and ran. Every person for themselves, right?",
                "You... talked to them. Tried to reason with them. It was a surprisingly spirited debate."
            }
        );

        switch (choice) {
            case 1: // The Hero
            builder.Strength += 0.2f;
            builder.Chakra += 0.1f;
            builder.MaxHp += 10;
            break;
            case 2: // The Tactician
            builder.Perception += 0.2f;
            builder.Dexterity += 0.1f;
            break;
            case 3: // The Opportunist
            builder.Dexterity += 0.2f;
            builder.MaxInventory += 2;
            builder.Food.Add(new Food("Someone Else's Lunch", 15));
            break;
            case 4: // The Diplomat
            builder.Chakra += 0.2f;
            builder.Perception += 0.1f;
            break;
        }
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
                "A trusy Hatchet. It's been with you since your 8th birthday."
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
            builder.Weapon = new Weapon("Trusy Hatchet", 6, Weapon.StatType.None);
            break;
        }
    }



    // Helper method to handle user choices cleanly
    private int GetChoice (string prompt, string[] options) {
        Console.WriteLine(prompt);
        for (int i = 0; i < options.Length; i++) {
            Console.WriteLine($"  {i + 1}: {options[i]}");
        }

        int choice = 0;
        while (choice < 1 || choice > options.Length) {
            Console.Write($"\nChoose (1-{options.Length}): ");
            if (!int.TryParse(Console.ReadLine(), out choice)) {
                choice = 0; // Reset choice if parsing fails
            }
        }
        return choice;
    }
}

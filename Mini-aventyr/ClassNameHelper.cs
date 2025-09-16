public static class ClassNameHelper {
    public static string DetermineClassName (CharacterCreator.CharacterBuilder builder) {
        var stats = new Dictionary<string, float>
                {
            { "Strength", builder.Strength },
            { "Dexterity", builder.Dexterity },
            { "Perception", builder.Perception },
            { "Chakra", builder.Chakra }
        };

        var sortedStats = stats.OrderByDescending(kvp => kvp.Value).ToList();
        string primary = sortedStats[0].Key;
        string secondary = sortedStats[1].Key;

        // Check for a "pure" class if the primary stat is significantly higher than the secondary
        if (sortedStats[0].Value > sortedStats[1].Value * 1.2f) {
            switch (primary) {
                case "Strength": return "Barbarian";
                case "Dexterity": return "Duelist";
                case "Perception": return "Strategist";
                case "Chakra": return "Mystic";
            }
        }

        // otherwise assign a hybrid class name
        if (primary == "Strength") {
            if (secondary == "Dexterity") return "Slayer";
            if (secondary == "Perception") return "Commander";
            if (secondary == "Chakra") return "Paladin";
        }
        if (primary == "Dexterity") {
            if (secondary == "Strength") return "Swashbuckler";
            if (secondary == "Perception") return "Infiltrator";
            if (secondary == "Chakra") return "Shadowdancer";
        }
        if (primary == "Perception") {
            if (secondary == "Strength") return "Vanguard";
            if (secondary == "Dexterity") return "Trickster";
            if (secondary == "Chakra") return "Oracle";
        }
        if (primary == "Chakra") {
            if (secondary == "Strength") return "Warden";
            if (secondary == "Dexterity") return "Spellblade";
            if (secondary == "Perception") return "Seer";
        }

        return "Jack-of-all-Trades";
    }
}
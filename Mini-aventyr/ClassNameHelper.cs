public static class ClassNameHelper {
    private class StatInfo {
        public string Name { get; }
        public float NormalizedValue { get; }
        public StatInfo (string name, float value) {
            Name = name;
            NormalizedValue = value;
        }
    }
    public static string DetermineClassName (CharacterCreator.CharacterBuilder builder) {

        // initial values
        var baseValues = new Dictionary<string, float>
        {
            { "Strength", 1.0f }, { "Dexterity", 1.0f }, { "Perception", 1.0f }, { "Chakra", 1.0f },
            { "Luck", 1.0f }, { "MaxHp", 80f }, { "MaxEnergy", 80f }, { "MaxInventory", 5f },
            { "MaxFullness", 20f }, { "PassiveHealing", 5f }, { "HpPerEnergy", 0.5f }
        };

        //normalize stats from original values (from charater builder)
        var normalized = new List<StatInfo>
        {
            new("Strength", (builder.Strength - baseValues["Strength"]) / baseValues["Strength"]),
            new("Dexterity", (builder.Dexterity - baseValues["Dexterity"]) / baseValues["Dexterity"]),
            new("Perception", (builder.Perception - baseValues["Perception"]) / baseValues["Perception"]),
            new("Chakra", (builder.Chakra - baseValues["Chakra"]) / baseValues["Chakra"]),
            new("Luck", (builder.Luck - baseValues["Luck"]) / baseValues["Luck"]),
            new("MaxHp", (builder.MaxHp - baseValues["MaxHp"]) / baseValues["MaxHp"]),
            new("MaxEnergy", (builder.MaxEnergy - baseValues["MaxEnergy"]) / baseValues["MaxEnergy"]),
            new("MaxInventory", (builder.MaxInventory - baseValues["MaxInventory"]) / baseValues["MaxInventory"]),
            new("PassiveHealing", (builder.PassiveHealing - baseValues["PassiveHealing"]) / baseValues["PassiveHealing"]),
            new("HpPerEnergy", (builder.HpPerEnergy - baseValues["HpPerEnergy"]) / baseValues["HpPerEnergy"]),
            new("MaxFullness", (builder.HpPerEnergy - baseValues["MaxFullness"]) / baseValues["MaxFullness"])
        };

        // all stats in order
        var sortedStats = normalized.OrderByDescending(s => s.NormalizedValue).ToList();

        // separated core stats from utility stats
        var coreStats = sortedStats.Where(s =>
        new List<string> { "Strength", "Dexterity", "Perception", "Chakra" }.Contains(s.Name)
        ).ToList();
        var utilityStats = sortedStats.Where(s => {
            return !(new List<string> { "Strength", "Dexterity", "Perception", "Chakra" }.Contains(s.Name));
        }).ToList();

        var topStat = sortedStats[0];
        var stat2 = sortedStats[1];
        var stat3 = sortedStats[2];


        // Check for a "pure" class if the primary stat is a lot higher than the second
        if (topStat.NormalizedValue > 0.2f && topStat.NormalizedValue > stat2.NormalizedValue * 2) {
            return topStat.Name switch {
                "Strength" => "Berserker",
                "Dexterity" => "Blademaster",
                "Perception" => "Grandmaster",
                "Chakra" => "Archon",
                "Luck" => "Harlequin",
                "MaxHp" => "Colossus",
                "MaxInventory" => "Hoarder",
                "PassiveHealing" => "Troll-Blood",
                "MaxEnergy" => "Unstoppable",
                "HpPerEnergy" => "Siphoner",
                "MaxFullness" => "Devourer",
                _ => "Anomaly"
            };
        }

        var topCoreStat = coreStats[0];
        // otherwise assign a mix class name
        switch (topCoreStat.Name) {
            case "Strength":
            switch (stat2.Name) {
                case "Dexterity": return "Slayer";
                case "Perception": return "Commander";
                case "Chakra": return "Paladin";
                case "MaxHp":
                if (stat3.Name == "PassiveHealing") return "Goliath";
                return "Juggernaut";
                case "MaxEnergy": return "Brawler";
                case "PassiveHealing": return "Skirmisher";
                case "Luck": return "Marauder";
                default: return "Warrior";
            }

            case "Dexterity":
            switch (stat2.Name) {
                case "Strength": return "Swashbuckler";
                case "Perception":
                if (stat3.Name == "Luck") return "Trickster";
                return "Infiltrator";
                case "Chakra": return "Shadowdancer";
                case "Luck": return "Duelist";
                case "MaxInventory": return "Burglar";
                case "HpPerEnergy": return "Assassin";
                default: return "Rogue";
            }

            case "Perception":
            switch (stat2.Name) {
                case "Strength":
                if (stat3.Name == "Chakra") return "Witch Hunter";
                return "Guardian";

                case "Dexterity":
                if (stat3.Name == "Luck") return "Spy";
                return "Agent";

                case "Chakra": return "Oracle";
                case "Luck": return "Gambler";
                case "MaxInventory": return "Prospector";
                case "MaxEnergy":
                if (stat3.Name == "Chakra") return "Inquisitor";
                return "Witch Hunter";
                default: return "Ranger";
            }

            case "Chakra":
            switch (stat2.Name) {
                case "Strength": return "Zealot";
                case "Dexterity": return "Spellblade";
                case "Perception":
                if (stat3.Name == "Dexterity") return "Druid";
                return "Seer";
                case "PassiveHealing": return "Shaman";
                case "HpPerEnergy":
                if (stat3.Name == "MaxHp") return "Spirit";
                return "Healer";
                case "MaxHp": return "Battlemage";
                case "MaxEnergy": return "Sorcerer";
                default: return "Mage";
            }
        }
        return "Jack-of-all-Trades";
    }
}
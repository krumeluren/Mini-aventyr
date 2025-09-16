namespace Mini_aventyr.Entities;

public static class EntityAttributesHelper {
    public static List<string> GetNotableTraits (this Entity entity) {
        var health = entity.Health;
        var notableTraits = new List<string>();
        if (health.Strength >= 2) notableTraits.Add("hulk");
        else if (health.Strength >= 1.25) notableTraits.Add("robust");
        else if (health.Strength <= 0.75) notableTraits.Add("weak");
        else if (health.Strength <= 0.5) notableTraits.Add("brittle");

        if (health.Dexterity >= 2) notableTraits.Add("unnervingly elegant");
        else if (health.Dexterity >= 1.25) notableTraits.Add("agile");
        else if (health.Dexterity <= 0.75) notableTraits.Add("clumsy");
        else if (health.Dexterity <= 0.5) notableTraits.Add("graceless");

        if (health.Perception >= 2) notableTraits.Add("track your every move");
        else if (health.Perception >= 1.25) notableTraits.Add("keen");
        else if (health.Perception <= 0.75) notableTraits.Add("imperceptive");
        else if (health.Perception <= 0.5) notableTraits.Add("dangerously oblivious");

        if (health.Chakra >= 2) notableTraits.Add("crackles with an unpleasant energy");
        else if (health.Chakra >= 1.25) notableTraits.Add("arcane");
        else if (health.Chakra <= 0.75) notableTraits.Add("earthly");
        else if (health.Chakra <= 0.5) notableTraits.Add("has the spiritual presence of a sourdough");

        return notableTraits;
    }
}
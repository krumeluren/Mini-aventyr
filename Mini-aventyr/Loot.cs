


namespace Mini_aventyr;

public class Loot {
    public int Gold { get; set; }
    public Weapon Weapon { get; set; }

    public int MaxFood { get; }
    public readonly List<Food> Food = new();

    public Loot (int gold, Weapon weapon, List<Food> foods, int maxFood = 3) {
        Weapon = weapon;
        Gold = gold;
        Food = foods;
        MaxFood = maxFood;
    }

    /// <returns>A potential item that was dropped</returns>
    public Item? Take (Item newItem) {
        Item? dropped = null; // Start by assuming nothing is dropped
        if (newItem is Weapon weapon) {
            dropped = Weapon;
            Weapon = weapon;
        }
        else if (newItem is Food food) {
            if (Food.Count >= MaxFood) {
                // bag is full get the last item
                dropped = Food[Food.Count - 1];
                // Remove it from the inventory.
                Food.RemoveAt(Food.Count - 1);
            }
            // Add the new food to the front of the list
            Food.Insert(0, food);
        }
        // return the item that was dropped or null
        return dropped;
    }
}

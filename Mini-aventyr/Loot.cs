namespace Mini_aventyr;

public class Loot {
    public int Gold { get; set; }
    public Weapon Weapon { get; set; }

    public int MaxItems { get; }
    public readonly List<Item> Items = new();

    public Loot (int gold, Weapon weapon, List<Item> items, int maxItems = 3) {
        Weapon = weapon;
        Gold = gold;
        Items = items;
        MaxItems = maxItems;
    }

    /// <returns>A potential item that was dropped</returns>
    public Item? Take (Item newItem) {
        Item? dropped = null; // Start by assuming nothing is dropped
        if (newItem is Weapon weapon) {
            dropped = Weapon;
            Weapon = weapon;
        }
        else {
            if (Items.Count >= MaxItems) {
                // bag is full get the last item
                dropped = Items[Items.Count - 1];
                // Remove it from the inventory.
                Items.RemoveAt(Items.Count - 1);
            }
            // Add the new food to the front of the list
            Items.Insert(0, newItem);
        }
        // return the item that was dropped or null
        return dropped;
    }
}
using Mini_aventyr.Interfaces;

namespace Mini_aventyr.EntityComponents;

/// <summary>
/// Inventory of an entity
/// </summary>
public class Loot {
    public int Gold { get; set; }
    public IWeapon Weapon { get; set; }

    public int MaxItems { get; }
    public readonly List<IItem> Items = new();

    public Loot (int gold, IWeapon weapon, List<IItem> items, int maxItems = 3) {
        Weapon = weapon;
        Gold = gold;
        Items = items;
        MaxItems = maxItems;
    }

    /// <returns>A potential item that was dropped</returns>
    public IItem? Take (IItem newItem) {
        IItem? dropped = null; // Start by assuming nothing is dropped
        if (newItem is IWeapon weapon) {
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
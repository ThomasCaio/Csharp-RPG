namespace Items.Factory;
using Items.All;

public class ItemFactory {
    public RPG.Game Parent;
    public Dictionary<string, Dictionary<string, Item>>RegisteredItems = new Dictionary<string, Dictionary<string, Item>>();
    public ItemFactory(RPG.Game parent) {
        Parent = parent;
        RegisteredItems["Swords"] = new Dictionary<string, Item>();
        RegisteredItems["Axes"] = new Dictionary<string, Item>();
        RegisteredItems["Clubs"] = new Dictionary<string, Item>();
        RegisteredItems["Staffs"] = new Dictionary<string, Item>();
        RegisteredItems["Helmets"] = new Dictionary<string, Item>();
        RegisteredItems["Armors"] = new Dictionary<string, Item>();
        RegisteredItems["Legs"] = new Dictionary<string, Item>();
        RegisteredItems["Boots"] = new Dictionary<string, Item>();
        RegisteredItems["Shields"] = new Dictionary<string, Item>();
        RegisteredItems["Consumables"] = new Dictionary<string, Item>();
        RegisteredItems["Others"] = new Dictionary<string, Item>();

        // Weapons

            // Swords
            RegisteredItems["Swords"]["Short Sword"] = new ShortSword();
            RegisteredItems["Swords"]["Demon Sword"] = new DemonSword();

        // Equipments

            // Helmets
            RegisteredItems["Helmets"]["Leather Helmet"] = new LeatherHelmet();
    }

    public bool Contains(string str) {
        return RegisteredItems.Keys.Contains(str);
    }

    public void New(Entities.Inventory inventory, string str) {
        foreach (Dictionary<string, Item> dict in RegisteredItems.Values) {
            foreach(Item i in dict.Values){
                if (i.Title == str) {
                    inventory.Add(i.New());
                }
            }
        }
    }

    public List<Item> Filter(string itemType) {
        List<Item> list = new List<Item>();
        if (RegisteredItems.Keys.Contains(itemType)) {
            foreach (Item i in RegisteredItems[itemType].Values) {
                list.Add(i);
            }
        }
        return list;
    }
}
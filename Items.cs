namespace Items;

public enum ItemType {
    Wearable,
    Usable,
    Decoration,
    Quest,
    None,
}

public enum ItemSlot {
    Head,
    Chest,
    Legs,
    Boots,
    MainHand,
    OffHand,
    None,
}

public class Item
{
    public string Title;
    public ItemType Type;
    public ItemSlot Slot;
    public int Price = 0;

    public Item(string title, ItemType type, ItemSlot slot){
        Title = title;
        Type = type;
        Slot = slot;
    }

    public Item New() {
        var _ = (Object)this.MemberwiseClone();
        var item = (Item)_;
        return item;
    }

    public virtual Dictionary<string, string> Look() {
        var list = new Dictionary<string, string>();
        list.Add("Title", Title);
        return list;
    }
    public virtual void Use(Entities.Creature creature) {}

    public static Item BlankItem(string title) {
        return new Item(title, ItemType.None, ItemSlot.None);
    }

    public static string TitleOrWhiteSpace(Item? item) {
        if (item == null) {
            return "";
        }
        return item.Title;
    }

    public static string SlotName(ItemSlot slot) {
        switch (slot) {
            case ItemSlot.Head:
            return "Head";
            case ItemSlot.Chest:
            return "Chest";
            case ItemSlot.Legs:
            return "Legs";
            case ItemSlot.Boots:
            return "Boots";
            case ItemSlot.MainHand:
            return "Main Hand";
            case ItemSlot.OffHand:
            return "Off Hand";
        }
        return "";
    }
}

public class Weapon : Item {
    public double BaseDamage;
    public List<Spells.Passive> PassiveSpells = new List<Spells.Passive>();

    public Weapon(string title) : base(title, ItemType.Wearable, ItemSlot.MainHand) {
        Title = title;
        BaseDamage = 0;
    }
    public override Dictionary<string, string> Look() {
        var dict = new Dictionary<string, string>();
        dict.Add("Title", Title);
        dict.Add("Damage", BaseDamage.ToString());
        foreach (Spells.Passive p in PassiveSpells) {
            dict.Add($"Passive: {p.Name}", p.Description);
        }
        return dict;
    }
}

public class Equipment : Item {
    public int Defense = 0;

    public Equipment(string name, ItemSlot itemSlot) : base(name, ItemType.Wearable, itemSlot) {
    }
}

public class Helmet: Equipment {
    public Helmet(string name) : base(name, ItemSlot.Head) {}
}


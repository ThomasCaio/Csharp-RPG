namespace Items
{

    public enum ItemTypes
    {
        Sword,
        Axe,
        Club,
        ThSword,
        ThAxe,
        ThClub,
        Helmet,
        Armor,
        Legs,
        Boots,
        Shield,
        Consumable,
        Other,
    }

    public enum ObjectTypes
    {
        Wearable,
        Usable,
        Decoration,
        Quest,
        None,
    }

    public enum ItemSlot
    {
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
        public string Name { get; }
        public ObjectTypes ObjectType { get; }
        public ItemSlot Slot { get; }
        public int Price { get; set; }

        public Item(string title, ObjectTypes type, ItemSlot slot)
        {
            Name = title;
            ObjectType = type;
            Slot = slot;
        }

        public virtual Dictionary<string, string> Look()
        {
            return new Dictionary<string, string>
            {
                { "Title", Name }
            };
        }

        public Item Clone()
        {
            return (Item)this.MemberwiseClone();
        }

        public virtual void Use(Entities.Creature creature) { }

        public static Item BlankItem(string title)
        {
            return new Item(title, ObjectTypes.None, ItemSlot.None);
        }

        public static string TitleOrWhiteSpace(Item? item)
        {
            return item?.Name ?? "";
        }
    }

    public class Wearable : Item
    {
        public ItemTypes ItemType;
        public Wearable(string title, ItemTypes itemType, ItemSlot itemSlot) : base(title, ObjectTypes.Wearable, itemSlot)
        {
            ItemTypes ItemType = itemType;
        }
    }

    public class Weapon : Wearable
    {
        public double BaseDamage { get; set; }
        private readonly List<Spells.Passive> _passiveSpells = new List<Spells.Passive>();

        public Weapon(string title, ItemTypes itemType) : base(title, itemType, ItemSlot.MainHand)
        {
            ItemTypes ItemType = itemType;
            BaseDamage = 0;
        }

        public void AddPassiveSpell(Spells.Passive passiveSpell)
        {
            _passiveSpells.Add(passiveSpell);
        }

        public override Dictionary<string, string> Look()
        {
            var dict = new Dictionary<string, string>
            {
                { "Title", Name },
                { "Damage", $"{BaseDamage}" }
            };
            foreach (var p in _passiveSpells)
            {
                dict.Add($"Passive: {p.Name}", $"{p.Description}");
            }
            return dict;
        }
    }

    public class Equipment : Wearable
    {
        public int Defense { get; set; }

        public Equipment(string name, ItemTypes itemType, ItemSlot itemSlot) : base(name, itemType, itemSlot)
        {
            ItemTypes EquipmentType = itemType;
        }
    }

    public class Sword : Weapon
    {
        public Sword(string title) : base(title, ItemTypes.Sword){}
    }

    public class Helmet : Equipment
    {
        public Helmet(string name) : base(name, ItemTypes.Helmet, ItemSlot.Head)
        {
       
        }
    }
}
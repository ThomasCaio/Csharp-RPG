using EntityModule;

namespace ItemModule
{

    public interface IItemAttribute
    {
        string Name { get; set;}
        int Value { get; set; }

        void AddTo(Character character);
        void RemoveTo(Character character);
    }

    public class ItemAttribute : IItemAttribute
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public ItemAttribute(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public void AddTo(Character character)
        {
            var property = character.GetType().GetProperty(Name.Replace(" ", ""));
            if (property != null)
            {
                double currentValue = (double)property.GetValue(character)!;
                property.SetValue(character, currentValue + Value);
            }
        }

        public void RemoveTo(Character character)
        {
            var property = character.GetType().GetProperty(Name.Replace(" ", ""));
            if (property != null)
            {
                double currentValue = (double)property.GetValue(character)!;
                property.SetValue(character, currentValue - Value);
            }
        }
    }

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

        public virtual void Use(EntityModule.Creature creature) { }

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
        public List<IItemAttribute> Attributes { get; } = new List<IItemAttribute>();

        public Wearable(string title, ItemTypes itemType, ItemSlot itemSlot) : base(title, ObjectTypes.Wearable, itemSlot)
        {
            ItemTypes ItemType = itemType;
        }

        public void AddAttributes(Character character)
        {
            foreach (var attribute in Attributes)
            {
                Logging.Debug.Write($"Adding {attribute.Name}:{attribute.Value}", "items");
                attribute.AddTo(character);
            }
        }

        public void RemoveAttributes(Character character)
        {
            foreach (var attribute in Attributes)
            {
                attribute.RemoveTo(character);
            }
        }
    }

    public class Weapon : Wearable
    {
        public double Damage { get; set; }
        private readonly List<SpellModule.Passive> _passiveSpells = new List<SpellModule.Passive>();

        public Weapon(string title, ItemTypes itemType) : base(title, itemType, ItemSlot.MainHand)
        {
            ItemTypes ItemType = itemType;
            Damage = 0;
        }

        public void AddPassiveSpell(SpellModule.Passive passiveSpell)
        {
            _passiveSpells.Add(passiveSpell);
        }

        public override Dictionary<string, string> Look()
        {
            var dict = new Dictionary<string, string>
            {
                { "Title", Name },
                { "Damage", $"{Damage}" }
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
using EntityModule;
using RPG;

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
                int currentValue = (int)property.GetValue(character)!;
                property.SetValue(character, currentValue + Value);
            }
        }

        public void RemoveTo(Character character)
        {
            var property = character.GetType().GetProperty(Name.Replace(" ", ""));
            if (property != null)
            {
                int currentValue = (int)property.GetValue(character)!;
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
        public ItemTypes ItemType;
        public int BuyPrice { get; set; }
        public int SellPrice { get; set; }

        public Item(string title, ObjectTypes type, ItemTypes itemType)
        {
            Name = title;
            ObjectType = type;
            ItemType = itemType;
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

        public virtual void Use(Character character) { }
        
        public virtual void UseText(Character character) 
        {
            Game.Log.Add($"{character.Name} used {this.Name}.");
        }

        public static Item BlankItem(string title)
        {
            return new Item(title, ObjectTypes.None, ItemTypes.Other);
        }

        public static string TitleOrWhiteSpace(Item? item)
        {
            return item?.Name ?? "";
        }
    }

    public class Wearable : Item
    {
        public ItemSlot Slot { get; }
        public List<IItemAttribute> Attributes { get; } = new List<IItemAttribute>();

        public Wearable(string title, ItemTypes itemType, ItemSlot itemSlot) : base(title, ObjectTypes.Wearable, itemType )
        {
            Slot = itemSlot;
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
        private readonly List<SpellModule.Passive> _passiveSpells = new List<SpellModule.Passive>();

        public Weapon(string title, ItemTypes itemType) : base(title, itemType, ItemSlot.MainHand)
        {}

        public void AddPassiveSpell(SpellModule.Passive passiveSpell)
        {
            _passiveSpells.Add(passiveSpell);
        }

        public override Dictionary<string, string> Look()
        {
            var dict = new Dictionary<string, string>
            {
                { "Title", Name },
                { "Damage", $"{0}" } // Arrumar o dano no look
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
        public Equipment(string name, ItemTypes itemType, ItemSlot itemSlot) : base(name, itemType, itemSlot)
        {}
    }

    public class Sword : Weapon
    {
        public Sword(string title) : base(title, ItemTypes.Sword){}
    }

    public class Axe : Weapon
    {
        public Axe(string title) : base(title, ItemTypes.Axe){}
    }

    public class Club : Weapon
    {
        public Club(string title) : base(title, ItemTypes.Sword){}
    }

    public class TwoHandedSword : Weapon
    {
        public TwoHandedSword(string title) : base(title, ItemTypes.ThSword){}
    }

    public class TwoHandedAxe : Weapon
    {
        public TwoHandedAxe(string title) : base(title, ItemTypes.ThAxe){}
    }

    public class TwoHandedClub : Weapon
    {
        public TwoHandedClub(string title) : base(title, ItemTypes.ThClub){}
    }

    public class Shield : Equipment
    {
        public Shield(string name) : base(name, ItemTypes.Shield, ItemSlot.OffHand) {}
    }

    public class Helmet : Equipment
    {
        public Helmet(string name) : base(name, ItemTypes.Helmet, ItemSlot.Head) {}
    }

    public class Armor : Equipment
    {
        public Armor(string name) : base(name, ItemTypes.Armor, ItemSlot.Chest) {}
    }

    public class Legs : Equipment
    {
        public Legs(string name) : base(name, ItemTypes.Legs, ItemSlot.Legs) {}
    }

    public class Boots : Equipment
    {
        public Boots(string name) : base(name, ItemTypes.Boots, ItemSlot.Boots) {}
    }

    public class Consumable : Item
    {
        public Consumable(string title) : base(title, ObjectTypes.Usable, ItemTypes.Consumable){}
    }

    public abstract class Potion : Consumable
    {
        public Potion(string title) : base(title){}

        public virtual int Formula(Character character)
        {
            return 0;
        }

        public override void Use(Character character)
        {
            PreAction(character);
            Action(character);
            PostAction(character);
            Destroy(character);
        }
        public virtual void PreAction(Character character){}
        public abstract void Action(Character character);
        public virtual void PostAction(Character character){}
        public virtual void Destroy(Character character)
        {
            character.Inventory.Remove(this);
        }
    }
}
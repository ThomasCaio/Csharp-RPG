using EntityModule;
using Newtonsoft.Json;
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

    public class ItemAttribute(string name, int value) : IItemAttribute
    {
        public string Name { get; set; } = name;
        public int Value { get; set; } = value;

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

    public class Item(string title, ObjectTypes type, ItemTypes itemType)
    {
        public string Name { get; } = title;
        public ObjectTypes ObjectType { get; } = type;
        public ItemTypes ItemType = itemType;
        public int BuyPrice { get; set; }
        private int _sellPrice = 0;
        public int SellPrice { 
            get {
                if (_sellPrice > 0) return _sellPrice;
                return (int)Math.Round(((double)BuyPrice / 2));
            }
            set => _sellPrice = value;
        }
        [JsonIgnore]
        public List<IItemAttribute> Attributes { get; } = [];

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

    public class Wearable(string title, ItemTypes itemType, ItemSlot itemSlot) : Item(title, ObjectTypes.Wearable, itemType )
    {
        public ItemSlot Slot { get; } = itemSlot;
        

        private readonly List<SpellModule.Passive> _passiveSpells = [];

        public void AddPassiveSpell(SpellModule.Passive passiveSpell)
        {
            _passiveSpells.Add(passiveSpell);
        }

        public override Dictionary<string, string> Look()
        {
            var dict = new Dictionary<string, string>
            {
                { "Title", Name },
            };
            foreach(var d in Attributes)
            {
                if (!dict.ContainsKey(d.Name))
                {
                    dict.Add(d.Name, d.Value.ToString());
                }
            }
            foreach (var p in _passiveSpells)
            {
                dict.Add($"Passive: {p.Name}", $"{p.Description}");
            }
            return dict;
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

    public class Weapon(string title, ItemTypes itemType) : Wearable(title, itemType, ItemSlot.MainHand)
    {
    }

    public class Equipment(string name, ItemTypes itemType, ItemSlot itemSlot) : Wearable(name, itemType, itemSlot)
    {
    }

    public class Sword(string title) : Weapon(title, ItemTypes.Sword)
    {
    }

    public class Axe(string title) : Weapon(title, ItemTypes.Axe)
    {
    }

    public class Club(string title) : Weapon(title, ItemTypes.Club)
    {
    }

    public class TwoHandedSword(string title) : Weapon(title, ItemTypes.ThSword)
    {
    }

    public class TwoHandedAxe(string title) : Weapon(title, ItemTypes.ThAxe)
    {
    }

    public class TwoHandedClub(string title) : Weapon(title, ItemTypes.ThClub)
    {
    }

    public class Shield(string name) : Equipment(name, ItemTypes.Shield, ItemSlot.OffHand)
    {
    }

    public class Helmet(string name) : Equipment(name, ItemTypes.Helmet, ItemSlot.Head)
    {
    }

    public class Armor(string name) : Equipment(name, ItemTypes.Armor, ItemSlot.Chest)
    {
    }

    public class Legs(string name) : Equipment(name, ItemTypes.Legs, ItemSlot.Legs)
    {
    }

    public class Boots(string name) : Equipment(name, ItemTypes.Boots, ItemSlot.Boots)
    {
    }

    public class Consumable(string title) : Item(title, ObjectTypes.Usable, ItemTypes.Consumable)
    {
    }

    public abstract class Potion(string title) : Consumable(title)
    {
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
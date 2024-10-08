﻿namespace EntityModule;
using ItemModule;
using CombatModule;
using SpellModule;
using static System.Linq.Enumerable;
using RPG;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;

public enum CreatureType {
    Character,
    Monster
}

public abstract class Creature(string name, CreatureType type)
{
    private int maxHealth = 100;
    private int health = 100;

    public string Name { get; set; } = name;
    public CreatureType Type { get; set; } = type;
    public int BaseDamage { get; set; } = 1;
    public int BaseDefense { get; set; } = 0;
    public List<Passive> Passives { get; } = [];
    public List<Effect> Effects { get; } = [];
    [Newtonsoft.Json.JsonIgnore]
    public int Health {
        get { return health; }
        set { health = Math.Max(0, Math.Min(value, maxHealth)); }
    }
    [JsonProperty(nameof(Health))]
    private int DeserializedHealth { get; set; }
    [OnDeserialized]
    private void OnDeserializedMethod(StreamingContext context)
    {
        Health = DeserializedHealth;
    }
    public int MaxHealth {
        get { return maxHealth; }
        set {
            if (value != maxHealth)
            {
                double healthPercentage = Math.Round((double)health / maxHealth, 2);
                if (Game.Debug) Logging.Debug.Write($"{healthPercentage}", "health");
                maxHealth = value;
                health = (int)Math.Min(healthPercentage * maxHealth, maxHealth);
            }
        }
    }

    public bool IsAlive => Health > 0;

    public virtual int Damage {get; set;}
    public virtual int Armor {get; set;}
    public virtual int Defense {get; set;}
    public virtual int PhysicalResistance {get; set;}
    public virtual int FireResistance {get; set;}
    public virtual int EarthResistance {get; set;}
    public virtual int WaterResistance {get; set;}
    public virtual int AirResistance {get; set;}

    [Newtonsoft.Json.JsonIgnore]
    public virtual int TotalDamage => (BaseDamage + Damage) * DamageModifier;
    public virtual int DamageModifier { get { return 1; } }

    [Newtonsoft.Json.JsonIgnore]
    public virtual int TotalDefense => (BaseDefense + Defense) * DefenseModifier;
    public virtual int DefenseModifier { get { return 1; } }

    public List<Passive> OnHitPassives { get { return Passives.Where(p => p.Type == PassiveType.OnHit).ToList(); } }

    public string EffectList {
        get { return Effects.Count == 0 ? "" : string.Join(", ", Effects); }
    }
}


public class Inventory {
    public List<Item> Items = [];
    public int InventorySize = 20;
    public Inventory() {}
    public void Add(Item item) {
        if (Items.Count > InventorySize){
            RPG.Game.Log.Add("Your inventory is full");
            return;
        }
        Items.Add(item);
    }
    public void Remove(Item item) {
        if (!Items.Contains(item)) {
            RPG.Game.Log.Add($"Your inventory don't contains {item.Name}.");
            return;
        }
        Items.Remove(item);
    }

    public Item Get(int index) {
        return Items[index];
    }
}

public class Character(string name) : Creature(name, CreatureType.Character)
{
    public int NextLevel = 100;
    public int _experience = 0;

    public List<Spell> Spellbook = [];
    public int Experience {
        get{return Convert.ToInt32(_experience);}
        set{
            while (value >= NextLevel) {
                value -= NextLevel;
                LevelUP();
                Game.Log.Add($"{this.Name} level up!");
                NextLevel = (int)Math.Round(NextLevel * 1.25, 0);
            }
            _experience = value;
        }}

    public int Level = 1;
    public int Gold = 0;
    public Dictionary<ItemSlot, Wearable?> Body = new()
    {
        [ItemSlot.Head] = null,
        [ItemSlot.Chest] = null,
        [ItemSlot.Legs] = null,
        [ItemSlot.Boots] = null,
        [ItemSlot.MainHand] = null,
        [ItemSlot.OffHand] = null,
    };
    public Inventory Inventory = new();

    public Dictionary<string, int> Scores = [];

    public void LevelUP() {
        Level++;
        MaxHealth += 5;
        Health += 5;
    }

    // public override double Damage
    // {
    //     get
    //     {
    //         double damage = Body.Values.OfType<Weapon>().Sum(weapon => weapon.Damage);
    //         return (BaseDamage + damage) * DamageModifier;
    //     }
    // }

    // public override double Defense
    // {
    //     get
    //     {
    //         double itemDefense = Body.Values.OfType<Equipment>().Sum(armor => armor.Defense);
    //         return (BaseDefense + itemDefense) * DefenseModifier;
    //     }
    // }

    public void Equip(Wearable item) {
        Wearable? equipped = Body.GetValueOrDefault(item.Slot, null);
        if (equipped == null) {
            Body[item.Slot] = item;
            Inventory.Remove(item);
            if (item is Wearable equipment)
            {
                equipment.AddAttributes(this);
            }
            Game.Log.Add($"{item.Name} equipped.");
        }
        else {
            Unequip(equipped);
            Equip(item);
        }
    }

    public void Unequip(Wearable item) {
        Item? equipped = Body[item.Slot];
        if (equipped != null) {
            Body[item.Slot] = null;
            Inventory.Add(item);
            if (item is Wearable equipment)
            {
                equipment.RemoveAttributes(this);
            }
            Game.Log.Add($"{item.Name} unequipped.");
            return;
        }
        RPG.Game.Log.Add("You have no item on this slot.");
    }
}

public class Monster(string name) : Creature(name, CreatureType.Monster) {

    public Tuple<int, int> DropGold = new(0, 0);
    public int DropExp = 0;

    public int GetGoldDrop() {
        int value = RPG.Game.RNG.Next(DropGold.Item1, DropGold.Item2);
        return value;
    }

    public virtual void StartTurn(Creature target) {
        if (this.IsAlive) {
        Combat.Attack(this, target);
        }
    }
}

public class Party : List<Creature> {
    public Party() {}
    public Party(Creature creature) {
        Add(creature);
    }
}
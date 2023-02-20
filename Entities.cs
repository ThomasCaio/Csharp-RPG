namespace Entities;
using Items;
using CombatModule;
using Spells;
using static System.Linq.Enumerable;
using RPG;

public enum CreatureType {
    Character,
    Monster
}

public abstract class Creature{
    public string Name;
    public CreatureType Type;
    private double maxhealth = 100;
    private double health = 100;
    public double Health {get {
        return this.health;
    } set {
        if (value < 0){
            health = value;
        }else {health = value;}
        }}

    public double MaxHealth {
        get {
            return this.maxhealth;
        }
        set {
            if (value < health) {
                health = value;
            }
            this.maxhealth = value;
        }
    }

    public double BaseDamage = 1;
    public double BaseDefense = 0;

    public List<Passive> Passives = new List<Passive>();
    public List<Effect> Effects = new List<Effect>();

    public Creature(string name, CreatureType type) {
        Name = name;
        Type = type;
    }

    public virtual double DamageCalc() {return 0;}

    public virtual double DefenseCalc() {return 0;}

    public double GetDamage() {
        return BaseDamage + DamageCalc();
    }

    public double GetDefense() {
        return BaseDefense + DefenseCalc();
    }
    public List<Passive> OnHitPassives() {
        List<Passive> list = new List<Passive>();
        foreach (Passive p in Passives) {
            if (p.Type == PassiveType.OnHit) {
                list.Add(p);
            }
        }
        return list;
    }

    public string EffectList() {
        if (Effects.Count == 0) {
            return "";
        }
        if (Effects.Count == 1) {
            return Effects[0].ToString();
        }
        string list = "";
        foreach (int i in Range(0, Effects.Count)) {
            Effect effect = Effects[i];
            list += $"{effect.ToString()}, ";
        }
        return list;
    }
}

public class Inventory {
    public List<Item> Items = new List<Item>();
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
            RPG.Game.Log.Add($"Your inventory don't contains {item.Title}.");
            return;
        }
        Items.Remove(item);
    }

    public Item Get(int index) {
        return Items[index];
    }
}

public class Character : Creature
{
    public double NextLevel = 100;
    public double _experience = 0;

    public List<Spell> Spellbook = new List<Spell>();
    public double Experience {
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
    public Dictionary<ItemSlot, Item?> Body = new Dictionary<ItemSlot, Item?>();
    public Inventory Inventory = new Inventory();

    public Dictionary<string, int> Scores = new Dictionary<string, int>();
    public Character(string name) : base(name, CreatureType.Character) {
        Body[ItemSlot.Head] = null;
        Body[ItemSlot.Chest] = null;
        Body[ItemSlot.Legs] = null;
        Body[ItemSlot.Boots] = null;
        Body[ItemSlot.MainHand] = null;
        Body[ItemSlot.OffHand] = null;
    }

    public void LevelUP() {
        Level++;
        MaxHealth += 5;
        Health += 5;
    }

    public override double DamageCalc() {
        Item? item = Body.GetValueOrDefault(ItemSlot.MainHand, null);
        if (item != null) {
            Weapon weapon = (Weapon)item;
            return weapon.BaseDamage;
        }
        return 0;
    }

    public void Equip(Item item) {
        Item? equipped = Body.GetValueOrDefault(item.Slot, null);
        if (equipped == null) {
            Body[item.Slot] = item;
            Inventory.Remove(item);
            Game.Log.Add($"{item.Title} equipped.");
        }
        else {
            Unequip(equipped);
            Equip(item);
        }
    }

    public void Unequip(Item item) {
        Item? equipped = Body[item.Slot];
        if (equipped != null) {
            Body[item.Slot] = null;
            Inventory.Add(item);
            Game.Log.Add($"{item.Title} unequipped.");
            return;
        }
        RPG.Game.Log.Add("You have no item on this slot.");
    }

    public bool IsAlive() {
        if (Health > 0) {
            return true;
        }
        return false;
    }
}

public class Monster : Creature {

    public Tuple<int, int> DropGold;
    public int DropExp;
    public Monster(string name) : base(name, CreatureType.Monster) {
        DropGold = new Tuple<int, int>(0, 0);
        DropExp = 0;
    }

    public int GetGoldDrop() {
        int value = RPG.Game.RNG.Next(DropGold.Item1, DropGold.Item2);
        return value;
    }

    public virtual void StartTurn(Creature target) {
        Combat.Attack(this, target);
    }
}

public class Party : List<Creature> {
    public Party() {}
    public Party(Creature creature) {
        this.Add(creature);
    }
}